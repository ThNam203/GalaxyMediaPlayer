using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using GalaxyMediaPlayer.Databases.MusicPage;

namespace GalaxyMediaPlayer.Windows
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        private readonly string DATABASE_PATH = AppDomain.CurrentDomain.BaseDirectory + "Databases\\MusicPage\\Database.txt";
        private ObservableCollection<string> folderPaths = new ObservableCollection<string>();

        public SettingWindow()
        {
            InitializeComponent();

            if (File.Exists(DATABASE_PATH))
            {
                foreach (string path in File.ReadAllLines(DATABASE_PATH))
                {
                    if (path == null || path.Length == 0) continue;
                    folderPaths.Add(path);
                }
            }

            musicListView.ItemsSource = folderPaths;
            if (folderPaths.Count > 0) emptyTb.Visibility = Visibility.Collapsed;
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
                folderPaths.Clear();
                foreach (string path in MusicPageDatabaseAccess.GetAllData())
                    folderPaths.Add(path);
            }

            if (folderPaths.Count > 0) emptyTb.Visibility = Visibility.Collapsed;
        }

        private void musicLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            musicLabel.BorderBrush = Brushes.White;
            videoLabel.BorderBrush = Brushes.Transparent;
            imageLabel.BorderBrush = Brushes.Transparent;
        }

        private void videoLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            musicLabel.BorderBrush = Brushes.Transparent;
            videoLabel.BorderBrush = Brushes.White;
            imageLabel.BorderBrush = Brushes.Transparent;
        }

        private void imageLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            musicLabel.BorderBrush = Brushes.Transparent;
            videoLabel.BorderBrush = Brushes.Transparent;
            imageLabel.BorderBrush = Brushes.White;
        }

        private void delBtn_Click(object sender, RoutedEventArgs e)
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
                folderPaths.Clear();
                foreach (string path in MusicPageDatabaseAccess.GetAllData()) folderPaths.Add(path);
            }
        }
    }
}