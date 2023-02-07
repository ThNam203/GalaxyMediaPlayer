using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media;
using System.Collections.ObjectModel;
using GalaxyMediaPlayer.Databases.MusicPage;
using GalaxyMediaPlayer.Databases.VideoPage;

namespace GalaxyMediaPlayer.Windows
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        private ObservableCollection<string> musicFolderPaths = new ObservableCollection<string>();
        private ObservableCollection<string> videoFolderPaths = new ObservableCollection<string>();
        private ObservableCollection<string> imageFolderPaths = new ObservableCollection<string>();

        public enum SettingPage
        {
            Music,
            Video,
            Image
        }

        public SettingWindow(SettingPage whatPage)
        {
            InitializeComponent();

            SetUpMusicSetting();
            SetUpVideoSetting();
            SetUpImageSetting();

            if (whatPage == SettingPage.Music)
            {
                musicSettingPanel.Visibility = Visibility.Visible;
                musicLabel.BorderBrush = Brushes.White;
            }
            else if (whatPage == SettingPage.Video)
            {
                videoSettingPanel.Visibility = Visibility.Visible;
                videoLabel.BorderBrush = Brushes.White;
            }
            else
            {
                videoSettingPanel.Visibility = Visibility.Visible;
                videoLabel.BorderBrush = Brushes.White;
            }
        }

        private void SetUpMusicSetting()
        {
            foreach (string path in MusicPageDatabaseAccess.GetAllData())
            {
                if (path == null || path.Length == 0) continue;
                musicFolderPaths.Add(path);
            }

            musicListView.ItemsSource = musicFolderPaths;
            if (musicFolderPaths.Count > 0) emptyMusicTb.Visibility = Visibility.Collapsed;
        }

        private void SetUpVideoSetting()
        {
            foreach (string path in VideoPageDatabaseAccess.GetAllData())
            {
                if (path == null || path.Length == 0) continue;
                videoFolderPaths.Add(path);
            }

            videoListView.ItemsSource = videoFolderPaths;
            if (videoFolderPaths.Count > 0) emptyVideoTb.Visibility = Visibility.Collapsed;
        }

        private void SetUpImageSetting()
        {
            foreach (string path in VideoPageDatabaseAccess.GetAllData())
            {
                if (path == null || path.Length == 0) continue;
                imageFolderPaths.Add(path);
            }

            imageListView.ItemsSource = imageFolderPaths;
            if (imageFolderPaths.Count > 0) emptyMusicTb.Visibility = Visibility.Collapsed;
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void addMusicFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MusicPageDatabaseAccess.SaveFolderToDatabase(dialog.SelectedPath);
                musicFolderPaths.Clear();
                foreach (string path in MusicPageDatabaseAccess.GetAllData())
                    musicFolderPaths.Add(path);
            }

            if (musicFolderPaths.Count > 0)
            {
                emptyMusicTb.Visibility = Visibility.Collapsed;
            }
        }

        private void musicLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            musicLabel.BorderBrush = Brushes.White;
            videoLabel.BorderBrush = Brushes.Transparent;
            imageLabel.BorderBrush = Brushes.Transparent;

            videoSettingPanel.Visibility = Visibility.Collapsed;
            musicSettingPanel.Visibility = Visibility.Visible;
            imageSettingPanel.Visibility = Visibility.Collapsed;
        }

        private void videoLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            musicLabel.BorderBrush = Brushes.Transparent;
            videoLabel.BorderBrush = Brushes.White;
            imageLabel.BorderBrush = Brushes.Transparent;

            videoSettingPanel.Visibility = Visibility.Visible;
            musicSettingPanel.Visibility = Visibility.Collapsed;
            imageSettingPanel.Visibility = Visibility.Collapsed;
        }

        private void imageLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            musicLabel.BorderBrush = Brushes.Transparent;
            videoLabel.BorderBrush = Brushes.Transparent;
            imageLabel.BorderBrush = Brushes.White;

            videoSettingPanel.Visibility = Visibility.Collapsed;
            musicSettingPanel.Visibility = Visibility.Collapsed;
            imageSettingPanel.Visibility = Visibility.Visible;
        }

        private void musicDelBtn_Click(object sender, RoutedEventArgs e)
        {
            string? deletePath = null;
            try
            {
                var parent = (sender as Button).TemplatedParent;
                deletePath = ((ContentControl)parent).Content.ToString();
            }
            catch (Exception) { }

            if (deletePath != null)
            {
                MusicPageDatabaseAccess.RemoveFolder(deletePath);
                musicFolderPaths.Clear();
                foreach (string path in MusicPageDatabaseAccess.GetAllData()) musicFolderPaths.Add(path);

                if (musicFolderPaths.Count == 0) emptyMusicTb.Visibility = Visibility.Visible;
            }
        }

        private void videoDelBtn_Click(object sender, RoutedEventArgs e)
        {
            string? deletePath = null;
            try
            {
                var parent = (sender as Button).TemplatedParent;
                deletePath = ((ContentControl)parent).Content.ToString();
            }
            catch (Exception) { }

            if (deletePath != null)
            {
                VideoPageDatabaseAccess.RemoveFolder(deletePath);
                videoFolderPaths.Clear();
                foreach (string path in VideoPageDatabaseAccess.GetAllData()) videoFolderPaths.Add(path);

                if (videoFolderPaths.Count == 0) emptyVideoTb.Visibility = Visibility.Visible;
            }
        }

        private void addVideoFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                VideoPageDatabaseAccess.SaveFolderToDatabase(dialog.SelectedPath);
                videoFolderPaths.Clear();
                foreach (string path in VideoPageDatabaseAccess.GetAllData())
                    videoFolderPaths.Add(path);
            }

            if (videoFolderPaths.Count > 0) emptyVideoTb.Visibility = Visibility.Collapsed;
        }

        private void imageDelBtn_Click(object sender, RoutedEventArgs e)
        {
            string? deletePath = null;
            try
            {
                var parent = (sender as Button).TemplatedParent;
                deletePath = ((ContentControl)parent).Content.ToString();
            }
            catch (Exception) { }

            if (deletePath != null)
            {
                Databases.ImagePage.SettingDatabase.ImageSettingDatabaseAccess.RemoveFolder(deletePath);
                imageFolderPaths.Clear();
                foreach (string path in Databases.ImagePage.SettingDatabase.ImageSettingDatabaseAccess.GetAllData()) imageFolderPaths.Add(path);

                if (imageFolderPaths.Count == 0) emptyVideoTb.Visibility = Visibility.Visible;
            }
        }

        private void addImageFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Databases.ImagePage.SettingDatabase.ImageSettingDatabaseAccess.SaveFolderToDatabase(dialog.SelectedPath);
                imageFolderPaths.Clear();
                foreach (string path in Databases.ImagePage.SettingDatabase.ImageSettingDatabaseAccess.GetAllData()) imageFolderPaths.Add(path);
            }

            if (imageFolderPaths.Count > 0) emptyImageTb.Visibility = Visibility.Collapsed;
        }
    }
}