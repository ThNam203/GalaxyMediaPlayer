﻿using GalaxyMediaPlayer.Databases.HomePage;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace GalaxyMediaPlayer.Pages.HomePage
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public class MostWatchEntity
        {
            public string Path { get; set; }
            public int Count { get; set; }
            public string Title
            {
                get { return System.IO.Path.GetFileNameWithoutExtension(Path); }
                set { Title = value; }
            }

            public MostWatchEntity() { }

            public MostWatchEntity(string path, int count)
            {
                Path = path;
                Count = count;
            }
        }

        private ObservableCollection<MostWatchEntity> mostWatchedEntities = new ObservableCollection<MostWatchEntity>();
        private ObservableCollection<MostWatchEntity> mostListenedEntities = new ObservableCollection<MostWatchEntity>();

        public MainPage()
        {
            InitializeComponent();

            if (DateTime.Now.ToString("tt") == "AM") greetingTb.Text = "Good Morning,";
            else if (DateTime.Now.ToString("tt") == "PM") greetingTb.Text = "Good Afternoon,";

            mostWatchListbox.ItemsSource = mostWatchedEntities;
            mostListenedListbox.ItemsSource = mostListenedEntities;

            foreach (var item in HomePageDatabaseAccess.GetMostWatchedEntities()) mostWatchedEntities.Add(item);
            foreach (var item in HomePageDatabaseAccess.GetMostListenedEntities()) mostListenedEntities.Add(item);

            if (mostWatchedEntities.Count == 0) mostWatchedTb.Visibility = System.Windows.Visibility.Collapsed;
            if (mostListenedEntities.Count == 0) mostListenedTb.Visibility = System.Windows.Visibility.Collapsed;

            if (mostWatchedEntities.Count == 0 && mostListenedEntities.Count == 0)
            {
                welcomeText.Visibility = System.Windows.Visibility.Visible; 
                contentGrid.Visibility= System.Windows.Visibility.Collapsed;
            }
        }

        private void mostWatchedItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mostListenedListbox.UnselectAll();

            if (e.ClickCount >= 2)
            {
                MainWindow.Instance.MainFrame.Navigate(new VideoMediaPLayer(new System.Collections.Generic.List<string> { mostWatchedEntities[mostWatchListbox.SelectedIndex].Path }));
            }
        }

        private void mostListenedItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mostWatchListbox.UnselectAll();

            if (e.ClickCount >= 2)
            {
                MyMusicMediaPlayer.SetNewPlaylist(new System.Collections.Generic.List<string> { mostListenedEntities[mostListenedListbox.SelectedIndex].Path });
                MyMusicMediaPlayer.SetPositionInPlaylist(0);
                MyMusicMediaPlayer.PlayCurrentSong();
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (e.Delta < 0)
            {
                scrollViewer.LineRight();
            }
            else
            {
                scrollViewer.LineLeft();
            }
            e.Handled = true;
        }
    }
}
