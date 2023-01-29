using GalaxyMediaPlayer.Helpers;
using LrcParser.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        // Nam: which is used for navigating back
        private static Stack<string> pathStack = new Stack<string>();

        private const string dateFormat = "MM/dd/yyyy hh:mm tt";

        // Nam: change browse style (listbox and griddata)
        private bool isUsingGridStyle = false;

        // Nam: this is for playlist feature in MainPage and MyMediaPlayer.cs
        private List<string> allMusicPathsInFolder
        {
            get { return GetAllMusicPathsInFolderEvenSorted(); }
        }

        // this binds to listbox in computer browse page
        public ObservableCollection<SystemEntityModel> systemEntities { get; set; }
        public Computer()
        {
            InitializeComponent();
            systemEntities = new();
            DataContext = this;

            browseListBox.ItemsSource = systemEntities;
            browseDataGrid.ItemsSource = systemEntities;
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

                Stack<string> temp = new Stack<string>(pathStack);
                temp.Pop();
                Stack<string> reverseTemp = new Stack<string>(temp);
                while (temp.Count > 0) reverseTemp.Push(temp.Pop());
                while (reverseTemp.Count > 0) MainPage.currentMusicBrowsingFolder += reverseTemp.Pop();

                OpenFolder(new DirectoryInfo(folderPath), false);
            }

            cbSortByOptions.ItemsSource = new List<SortType> { SortType.Name, SortType.Date, SortType.Size, SortType.Type };
        }
        private void IntializeBrowseFoldersAndDisksAndMediaControlButtonsView()
        {
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

        // Nam: Music list (allMusicPathsInFolder) must be sorted to the coresponding position in systemEntities
        private void sortSystemEntities(SortType type, bool isSortAscending)
        {
            List<SystemEntityModel> systemEntitiesSort = new List<SystemEntityModel>(systemEntities);
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

            systemEntities.Clear();
            foreach (SystemEntityModel entity in systemEntitiesSort) systemEntities.Add(entity);
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
            int idx = MainPage.currentMusicBrowsingFolder.LastIndexOf(pathStack.Peek());
            MainPage.currentMusicBrowsingFolder = MainPage.currentMusicBrowsingFolder.Remove(idx, pathStack.Peek().Length);
            pathStack.Pop();

            if (pathStack.Count == 0)
            {
                IntializeBrowseFoldersAndDisksAndMediaControlButtonsView();
                MainPage.currentMusicBrowsingFolder = "ComputerBrowse";
                currentFolderName.Text = "My Computer";
                BackBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                string pathToBack = pathStack.Peek();

                try
                {
                    idx = MainPage.currentMusicBrowsingFolder.LastIndexOf(pathToBack);
                    MainPage.currentMusicBrowsingFolder = MainPage.currentMusicBrowsingFolder.Remove(idx, pathToBack.Length);
                }
                catch (Exception)
                {
                    MainWindow.ShowCustomMessageBoxInMiddle(new UserControls.ShowMessageControl("Error", "Something is wrong, you can restart the software"));
                }

                DirectoryInfo di = new DirectoryInfo(pathToBack);
                OpenFolder(di, true);
            }
        }

        // IsOnBackButtonPress is if the back button is press so that we wont push the path to the stack
        // (instead we pop it, but on the original function that do)
        private void OpenFolder(DirectoryInfo di, bool IsOnBackButtonPress)
        {
            MainPage.currentMusicBrowsingFolder += di.FullName;
            //if (!MyMediaPlayer.isSongOpened) MyMediaPlayer.pathCurrentlyInUse = MainPage.currentMusicBrowsingFolder;

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

                if (cbSortByOptions.SelectedIndex != -1)
                {
                    SortType type = (SortType)cbSortByOptions.SelectedItem;
                    sortSystemEntities(type, true);
                }

                MyMusicMediaPlayer.SetTempPlaylist(allMusicPathsInFolder);
                // Nam: mediaPlayer need to update first so ui can change accordingly
                MainPage.Instance.ChangeButtonsViewOnOpenFolder(forceDisable: false);
                MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid(false);
            }
            catch(UnauthorizedAccessException) 
            {
                MainWindow.ShowCustomMessageBoxInMiddle(new UserControls.ShowMessageControl("Error", "You don't have the permission to do this action"));
            }
        }

        // Nam: get musicPaths accordinglly to the sorted (or not) systemEntities
        private List<string> GetAllMusicPathsInFolderEvenSorted()
        {
            List<string> result = new List<string>();

            if (isUsingGridStyle)
            {
                foreach (SystemEntityModel model in browseDataGrid.Items) 
                    if (model.Type == EntityType.Music) result.Add(model.Path);
            }
            else
            {
                foreach (SystemEntityModel model in systemEntities)
                    if (model.Type == EntityType.Music) result.Add(model.Path);
            }

            return result;
        }

        private void OnBrowseItemDoubleClick(bool isUsingListBox)
        {
            SystemEntityModel? entity;
            if (isUsingListBox) entity = browseListBox.SelectedItem as SystemEntityModel;
            else entity = browseDataGrid.SelectedItem as SystemEntityModel; 

            if (entity != null)
            {
                if (entity.Type == EntityType.Music)
                {
                    MyMusicMediaPlayer.SetNewPlaylist(new List<string> { entity.Path });
                    MyMusicMediaPlayer.SetPositionInPlaylist(0);
                    MyMusicMediaPlayer.PlayCurrentSong();
                }
                else if (entity.Type == EntityType.Image)
                {

                }
                else if (entity.Type == EntityType.Video)
                {
                    MainWindow.Instance.MainFrame.Navigate(new VideoMediaPLayer(new List<string> { entity.Path }));
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
                cbSortByOptions.Visibility = Visibility.Collapsed;
                browseListBox.Visibility = Visibility.Collapsed;
                browseDataGrid.Visibility = Visibility.Visible;
                BrowseStyleImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/list_32.png"));
            }
            else
            {
                List<SystemEntityModel> tempEntities = new List<SystemEntityModel>();
                foreach (SystemEntityModel model in browseDataGrid.Items) tempEntities.Add(model);
                systemEntities.Clear();
                foreach (SystemEntityModel model in tempEntities) { systemEntities.Add(model); break; };

                cbSortByOptions.SelectedIndex = -1;
                cbSortByOptions.Visibility = Visibility.Visible;
                browseListBox.Visibility = Visibility.Visible;
                browseDataGrid.Visibility = Visibility.Collapsed;
                BrowseStyleImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/four_squares_32.png"));
            }
        }

        private void cbSortByOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSortByOptions.SelectedIndex == -1) return;
            SortType type = (SortType)((ComboBox)sender).SelectedItem;
            sortSystemEntities(type, true);
            MyMusicMediaPlayer.SetTempPlaylist(allMusicPathsInFolder);
        }

        private void browseDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)delegate ()
            {
                //runs after sorting is done
                MyMusicMediaPlayer.SetTempPlaylist(allMusicPathsInFolder);
            }, null);
        }
    }
}