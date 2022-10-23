using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GalaxyMediaPlayer.Pages.NavContentPages
{
    /// <summary>
    /// Interaction logic for ComputerBrowse.xaml
    /// </summary>
    public partial class Computer : Page
    {
        MediaPlayer mediaPlayer = new MediaPlayer();
        public static string currentBrowsingFolder = "";
        // Nam: which is used for navigating back
        private static Stack<string> pathStack = new Stack<string>();
        // Nam: this is for media file extension filter
        private List<string> musicExtension = new List<string> { "wma", "wax", "mp3", "m4a", "mpa", "mp2", "m3u", "mid", "midi", "rmi",
                                                                 "aif", "aifc", "aiff", "au", "snd", "wav", "cda", "aac", "adts", "m2ts", "flac" };
        private List<string> videoExtension = new List<string> { "asf", "wmv", "wm", "asx", "wvx", "wmx", "wpl", "dvr-ms",
                                                                 "wmd", "avi", "mpg", "mpeg", "m1v", "mpe", "ivf", "mov", 
                                                                 "m4v", "mp4v", "3g2", "3gp2", "3gp", "3gpp", "mp4" };
        private List<string> imageExtension = new List<string> { "jpg", "gif", "png" };

        private string dateFormat = "MM/dd/yyyy hh:mm tt";

        // Nam: change browse style (listbox and griddata)
        private bool isUsingGridStyle = false;

        // Nam: this is for playlist feature in MainPage and MyMediaPlayer.cs
        public List<string> allMusicPathsInFolder = new List<string>();
        // this binds to listbox in computer browse page
        private ObservableCollection<SystemEntityModel> systemEntities { get; set; }
        public Computer()
        {
            InitializeComponent();
            systemEntities = new();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // set up main folders and disks to browse
            if (pathStack.Count == 0)
            {
                IntializeBrowseFoldersAndDisksAndMediaControlButtonsView();
            }
            else
            {
                string folderPath = pathStack.Peek();
                OpenFolder(new DirectoryInfo(folderPath), false);
            }

            browseListBox.ItemsSource = systemEntities;
            browseDataGrid.ItemsSource = systemEntities;
        }
        private void IntializeBrowseFoldersAndDisksAndMediaControlButtonsView()
        {
            MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid("", true);
            MainPage.Instance.ChangeButtonsViewOnOpenFolder(true);

            systemEntities.Clear();
            List<string> rootFoldersAndDisksPath = new()
            {
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            };
            // This is for Downloads folder
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            rootFoldersAndDisksPath.Add($"{userPath}\\Downloads");
            // _____________

            foreach (string fullPath in rootFoldersAndDisksPath)
            {
                int lastIndexOfBackslash = fullPath.LastIndexOf(@"\");
                string name = fullPath.Substring(lastIndexOfBackslash + 1, fullPath.Length - 1 - lastIndexOfBackslash);
                // Nam: default screen doesnt have griddataview, so we put garbage in dateCreated, size, extension
                SystemEntityModel model = new SystemEntityModel(
                    name: name,
                    type: EntityType.Folder,
                    path: fullPath,
                    dateCreated: new DirectoryInfo(fullPath).CreationTime.ToString(dateFormat),
                    size: 0,
                    extension: "Folder");

                systemEntities.Add(model);
            }

            foreach (string diskName in Directory.GetLogicalDrives())
                systemEntities.Add(new SystemEntityModel(
                    name: diskName,
                    type: EntityType.Folder,
                    path: diskName,
                    dateCreated: "",
                    size: 0,
                    extension: "Folder"));
        }

        private void browseListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnBrowseItemDoubleClick(isUsingListBox: true);
        }

        private void DataGridRow_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnBrowseItemDoubleClick(isUsingListBox: false);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            pathStack.Pop();

            if (pathStack.Count == 0)
            {
                IntializeBrowseFoldersAndDisksAndMediaControlButtonsView();
                currentBrowsingFolder = "";
                currentFolderName.Text = "My Computer";
                BackBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                string pathToBack = pathStack.Peek();
                DirectoryInfo di = new DirectoryInfo(pathToBack);
                OpenFolder(di, true);
            }
        }

        // IsOnBackButtonPress is if the back button is press so that we wont push the path to the stack
        // (instead we pop it, but on the original function that do)
        private void OpenFolder(DirectoryInfo di, bool IsOnBackButtonPress)
        {
            currentBrowsingFolder = di.FullName;
            if (!MyMediaPlayer.isSongOpened) MyMediaPlayer.folderCurrentlyInUse = di.FullName;

            if (!IsOnBackButtonPress)
            {
                // this part is for navigating
                // if the new folderPath is the same at the peek pathStack, we won't add it
                if (pathStack.Count == 0 || pathStack.Peek() != di.FullName) pathStack.Push(di.FullName);
                if (pathStack.Count > 0) BackBtn.Visibility = Visibility.Visible;
                // done
            }

            currentFolderName.Text = di.Name;

            try
            {
                systemEntities.Clear();
                // add sub folders
                foreach (DirectoryInfo direcInfo in di.EnumerateDirectories())
                {
                    // check if the directory has read access
                    if (direcInfo.Exists)
                    {
                        SystemEntityModel newEntity = new SystemEntityModel(
                            name: direcInfo.Name,
                            type: EntityType.Folder,
                            path: direcInfo.FullName,
                            dateCreated: direcInfo.CreationTime.ToString(dateFormat),
                            size: 0,
                            extension: "Folder");
                        
                        systemEntities.Add(newEntity);
                    }
                }

                allMusicPathsInFolder.Clear();
                // add media files and pass every music to MyMediaPlayer
                foreach (FileInfo fi in di.EnumerateFiles("*.*"))
                {
                    if (fi.Exists)
                    {
                        var fileExtension = fi.Extension.TrimStart('.').ToLowerInvariant();

                        if (musicExtension.Contains(fileExtension))
                        {
                            allMusicPathsInFolder.Add(fi.FullName);
                            systemEntities.Add(new SystemEntityModel(
                                name: fi.Name,
                                type: EntityType.Music,
                                path: fi.FullName,
                                dateCreated: fi.CreationTime.ToString(dateFormat),
                                size: fi.Length,
                                extension: fi.Extension));
                        }
                        else if (imageExtension.Contains(fileExtension))
                        {
                            systemEntities.Add(new SystemEntityModel(
                                name: fi.Name,
                                type: EntityType.Image,
                                path: fi.FullName,
                                dateCreated: fi.CreationTime.ToString(dateFormat),
                                size: fi.Length,
                                extension: fi.Extension));
                        }
                        else if (videoExtension.Contains(fileExtension))
                        {
                            systemEntities.Add(new SystemEntityModel(
                                name: fi.Name,
                                type: EntityType.Video,
                                path: fi.FullName,
                                dateCreated: fi.CreationTime.ToString(""),
                                size: fi.Length,
                                extension: fi.Extension));
                        }
                    }
                }

                // Nam: mediaPlayer need to update first so ui can change accordingly
                MyMediaPlayer.SetTempPlaylist(allMusicPathsInFolder);
                MainPage.Instance.ChangeButtonsViewOnOpenFolder(forceShow: false);
                MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid(di.FullName, false);
            }
            catch(UnauthorizedAccessException) 
            {
                MessageBox.Show("You don't have the permission to access this folder");
            }
        }

        private void OnBrowseItemDoubleClick(bool isUsingListBox)
        {
            SystemEntityModel? entity;
            if (isUsingListBox)
                entity = browseListBox.SelectedItem as SystemEntityModel;
            else entity = browseDataGrid.SelectedItem as SystemEntityModel; 

            if (entity != null)
            {
                if (entity.Type == EntityType.Music)
                {
                    MyMediaPlayer.SetPlaylistFromTempPlaylist();
                    MyMediaPlayer.SetPositionInPlaylist(allMusicPathsInFolder.IndexOf(entity.Path));
                    MyMediaPlayer.PlayCurrentSong();
                }
                else if (entity.Type == EntityType.Folder)
                {
                    DirectoryInfo di = new DirectoryInfo(entity.Path);
                    OpenFolder(di, false);
                }
            }
        }

        private void BrowseStyleImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isUsingGridStyle = !isUsingGridStyle;
            if (isUsingGridStyle)
            {
                browseListBox.Visibility = Visibility.Collapsed;
                browseDataGrid.Visibility = Visibility.Visible;
                BrowseStyleImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/list_32.png"));
            }
            else
            {
                browseListBox.Visibility = Visibility.Visible;
                browseDataGrid.Visibility = Visibility.Collapsed;
                BrowseStyleImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/four_squares_32.png"));
            }
        }

        private sealed class SelectionAdorner : Adorner
        {
            // Initializes a new instance of the SelectionAdorner class.
            public SelectionAdorner(UIElement parent)
                : base(parent)
            {
                // Make sure the mouse doesn't see us.
                this.IsHitTestVisible = false;

                // We only draw a rectangle when we're enabled.
                this.IsEnabledChanged += delegate { this.InvalidateVisual(); };
            }

            // Gets or sets the area of the selection rectangle.
            public Rect SelectionArea { get; set; }

            // Participates in rendering operations that are directed by the layout system.
            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (this.IsEnabled)
                {
                    // Make the lines snap to pixels (add half the pen width [0.5])
                    double[] x = { this.SelectionArea.Left + 0.5, this.SelectionArea.Right + 0.5 };
                    double[] y = { this.SelectionArea.Top + 0.5, this.SelectionArea.Bottom + 0.5 };
                    drawingContext.PushGuidelineSet(new GuidelineSet(x, y));

                    Brush fill = SystemColors.HighlightBrush.Clone();
                    fill.Opacity = 0.4;
                    drawingContext.DrawRectangle(
                        fill,
                        new Pen(SystemColors.HighlightBrush, 1.0),
                        this.SelectionArea);
                }
            }
        }
    }
}
