using GalaxyMediaPlayer.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GalaxyMediaPlayer.Pages.NavContentPages
{
    public enum SortType
    {
        Name,
        Date,
        Size,
        Type
    }
    public partial class Computer : Page
    {
        MediaPlayer mediaPlayer = new MediaPlayer();
        public static bool isUserBrowsing = false;
        // Nam: which is used for navigating back
        private static Stack<string> pathStack = new Stack<string>();

        private const string dateFormat = "MM/dd/yyyy hh:mm tt";

        // Nam: change browse style (listbox and griddata)
        private bool isUsingGridStyle = false;

        // Nam: this is for playlist feature in MainPage and MyMediaPlayer.cs
        private List<string> allMusicPathsInFolder = new List<string>();

        // this binds to listbox in computer browse page
        public ObservableCollection<SystemEntityModel> systemEntities { get; set; }
        // Nam: this is to sort the list by SortType, then systemEntities point to it to show the sorted
        private List<SystemEntityModel> systemEntitiesSort { get; set; }
        public Computer()
        {
            InitializeComponent();
            systemEntities = new();
            systemEntitiesSort = new();
            DataContext = this;

            DispatcherTimer debugTimer = new DispatcherTimer();
            debugTimer.Interval = TimeSpan.FromSeconds(10);
            debugTimer.Tick += DebugTimer_Tick;
            debugTimer.Start();
        }

        private void DebugTimer_Tick(object? sender, EventArgs e)
        {
            return;
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

            cbSortByOptions.ItemsSource = new List<SortType> { SortType.Name, SortType.Date, SortType.Size, SortType.Type };
        }
        private void IntializeBrowseFoldersAndDisksAndMediaControlButtonsView()
        {
            MainPage.currentMusicBrowsingFolder = "";
            MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid(true);
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

        private void sortList(SortType type, bool isSortAscending)
        {
            systemEntitiesSort = new List<SystemEntityModel>(systemEntities);
            if (type == SortType.Name)
            {
                systemEntitiesSort.Sort((x, y) => x.Name.CompareTo(y.Name));
            } 
            else if (type == SortType.Date)
            {
                systemEntitiesSort.Sort((x, y) => x.DateCreated.CompareTo(y.DateCreated));
            }
            else if (type == SortType.Size)
            {
                systemEntitiesSort.Sort((x, y) => x.Size.CompareTo(y.Size));
            }
            else if (type == SortType.Type)
            {
                systemEntitiesSort.Sort((x, y) => x.Type.CompareTo(y.Type));
            }

            // Nam: THIS IS NOT A GOOD IDEA, SHOULD CHANGE IF POSSIBLE
            // systemEntities.Clear();
            systemEntities = new ObservableCollection<SystemEntityModel>(systemEntitiesSort);
            browseListBox.ItemsSource = systemEntities;
            browseDataGrid.ItemsSource = systemEntities;
        }

        private void browseListBoxItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                OnBrowseItemDoubleClick(isUsingListBox: true);
            }
            // Nam: remove all other IsSelected when click on an item (not checkbox)
            // the reason is virtualizing make it broken, we make it manual by using IsSelected property
            else if (e.ClickCount == 1)
            {
                if (sender != null)
                {
                    if (e.OriginalSource is not CheckBox)
                    {
                        FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
                        SystemEntityModel model = (SystemEntityModel)frameworkElement.DataContext;
                        if (model != null && model.Type != EntityType.Folder)
                        {
                            foreach (SystemEntityModel item in systemEntities)
                            {
                                if (item.IsSelected == true && item.Name != model.Name) item.IsSelected = false;
                            }
                        }
                    }
                }
            }
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
                MainPage.currentMusicBrowsingFolder = "";
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
            MainPage.currentMusicBrowsingFolder = di.FullName;
            if (!MyMediaPlayer.isSongOpened) MyMediaPlayer.pathCurrentlyInUse = di.FullName;

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

                        if (SupportedExtensions.MUSIC_EXTENSION.Contains(fileExtension))
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
                        else if (SupportedExtensions.IMAGE_EXTENSION.Contains(fileExtension))
                        {
                            systemEntities.Add(new SystemEntityModel(
                                name: fi.Name,
                                type: EntityType.Image,
                                path: fi.FullName,
                                dateCreated: fi.CreationTime.ToString(dateFormat),
                                size: fi.Length,
                                extension: fi.Extension));
                        }
                        else if (SupportedExtensions.VIDEO_EXTENSION.Contains(fileExtension))
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
                MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid(false);
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
                    MyMediaPlayer.pathCurrentlyInUse = MainPage.currentMusicBrowsingFolder;
                    MyMediaPlayer.SetPlaylistFromTempPlaylist();
                    MyMediaPlayer.SetPositionInPlaylist(allMusicPathsInFolder.IndexOf(entity.Path));
                    MyMediaPlayer.PlayCurrentSong();
                }
                else if (entity.Type == EntityType.Image)
                {

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

        private void cbSortByOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortType type = (SortType)((ComboBox)sender).SelectedItem;
            sortList(type, true);
        }
    }
}