using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media;
using System.Collections.ObjectModel;

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
                if (SaveFileToDatabase(dialog.SelectedPath))
                    folderPaths.Add(dialog.SelectedPath);
            }

            if (folderPaths.Count > 0) emptyTb.Visibility = Visibility.Collapsed;
        }

        private bool SaveFileToDatabase(string newFolderPath)
        {
            // Nam: should check if file and folder exists (WARNING)

            // Nam: check if it existed, if yes then exit, not saving
            string[] folders = File.ReadAllLines(DATABASE_PATH);
            foreach (string folderPath in folders) if (folderPath == newFolderPath) return false;

            File.AppendAllText(DATABASE_PATH, newFolderPath + Environment.NewLine);
            return true;
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
                string[] paths = File.ReadAllLines(DATABASE_PATH);
                string tempFile = "";

                foreach (string path in paths)
                {
                    if (path == deletePath)
                    {
                        continue;
                    }

                    tempFile += path + Environment.NewLine;
                }

                File.WriteAllText(DATABASE_PATH, String.Empty);

                if (tempFile.LastIndexOf(Environment.NewLine) != -1)
                    tempFile = tempFile.Remove(tempFile.LastIndexOf(Environment.NewLine), Environment.NewLine.Length);

                SaveFileToDatabase(tempFile);

                folderPaths.Remove(deletePath);
            }
        }
    }
}