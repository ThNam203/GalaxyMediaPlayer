﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GalaxyMediaPlayer.Pages.NavContentPages
{
    /// <summary>
    /// Interaction logic for ComputerBrowse.xaml
    /// </summary>
    public partial class Computer : Page
    {
        MediaPlayer mediaPlayer = new MediaPlayer();
        // which is used for navigating back
        private Stack<string> pathStack = new Stack<string>();
        // this is for media file extension filter
        private List<string> musicExtension = new List<string> { "mp3" };
        private List<string> videoExtension = new List<string> { };
        private List<string> imageExtension = new List<string> { "jpg", "gif", "png" };

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
            IntializeBrowseFoldersAndDisks();
            browseListBox.ItemsSource = systemEntities;
        }
        private void IntializeBrowseFoldersAndDisks()
        {
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
                SystemEntityModel model = new SystemEntityModel(
                    name: name,
                    type: EntityType.Folder,
                    path: fullPath,
                    imageSource: "folder.folder"); // folder.folder is dummy string, we use EntityType to resolve
                systemEntities.Add(model);
            }

            foreach (string diskName in Directory.GetLogicalDrives())
                systemEntities.Add(new SystemEntityModel(
                    name: diskName,
                    type: EntityType.Folder,
                    path: diskName,
                    imageSource: "folder.folder")); // folder.folder is dummy string, we use EntityType to resolve
        }

        private void browseListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SystemEntityModel? entity = browseListBox.SelectedItem as SystemEntityModel;

            if (entity != null)
            {
                if (entity.entityType == EntityType.Music)
                {
                    mediaPlayer.Open(new Uri(entity.entityPath));
                    mediaPlayer.Play();
                }
                else if (entity.entityType == EntityType.Folder)
                {
                    DirectoryInfo di = new DirectoryInfo(entity.entityPath);
                    OpenFolder(di, false);
                }
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            pathStack.Pop();

            if (pathStack.Count == 0)
            {
                IntializeBrowseFoldersAndDisks();
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
            if (!IsOnBackButtonPress)
            {
                // this part is for navigating
                pathStack.Push(di.FullName);
                if (pathStack.Count > 0) BackBtn.Visibility = Visibility.Visible;
                // done
            }

            currentFolderName.Text = di.Name;

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
                        imageSource: "folder.folder"); // folder.folder is dummy string, we use EntityType to resolve

                    systemEntities.Add(newEntity);
                }
            }

            // add media files
            foreach (FileInfo fi in di.EnumerateFiles("*.*"))
            {
                if (fi.Exists)
                {
                    var fileExtension = fi.Extension.TrimStart('.').ToLowerInvariant();

                    if (musicExtension.Contains(fileExtension))
                    {
                        systemEntities.Add(new SystemEntityModel(
                            name: fi.Name,
                            type: EntityType.Music,
                            path: fi.FullName,
                            imageSource: fi.FullName));
                    }
                    else if (imageExtension.Contains(fileExtension))
                    {
                        systemEntities.Add(new SystemEntityModel(
                            name: fi.Name,
                            type: EntityType.Image,
                            path: fi.FullName,
                            imageSource: fi.FullName));
                    }
                }
            }
        }
    }
}
