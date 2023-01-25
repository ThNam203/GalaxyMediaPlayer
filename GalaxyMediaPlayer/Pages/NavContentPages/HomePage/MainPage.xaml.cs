using GalaxyMediaPlayer.Databases.HomePage;
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

            if (DateTime.Now.ToString("tt") == "AM") greetingTb.Text = "Good Afternoon,";
            else if (DateTime.Now.ToString("tt") == "PM") greetingTb.Text = "Good Morning,";

            mostWatchListbox.ItemsSource = mostWatchedEntities;
            mostListenedListbox.ItemsSource = mostListenedEntities;

            foreach (var item in HomePageDatabaseAccess.GetMostWatchedEntities()) mostWatchedEntities.Add(item);
            foreach (var item in HomePageDatabaseAccess.GetMostListenedEntities()) mostListenedEntities.Add(item);

            if (mostWatchedEntities.Count == 0) mostWatchedTb.Visibility = System.Windows.Visibility.Collapsed;
            if (mostListenedEntities.Count == 0) mostListenedTb.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void listboxItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }
    }
}
