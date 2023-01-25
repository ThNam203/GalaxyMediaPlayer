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
        private class MostWatchEntity
        {
            public string Path { get; set; }
            public int Count { get; set; }
            public string Title
            {
                get { return System.IO.Path.GetFileNameWithoutExtension(Path); }
            }

            public MostWatchEntity(string path, int count)
            {
                Path = path;
                Count = count;
            }
        }

        private ObservableCollection<MostWatchEntity> mostWatchedEntities = new ObservableCollection<MostWatchEntity>();

        public MainPage()
        {
            InitializeComponent();

            if (DateTime.Now.ToString("tt") == "AM") greetingTb.Text = "Good Afternoon,";
            else if (DateTime.Now.ToString("tt") == "PM") greetingTb.Text = "Good Morning,";

            mostWatchListbox.ItemsSource = mostWatchedEntities;
            mostWatchedEntities.Add(new MostWatchEntity(@"C:\Users\hthna\Downloads\Die For You - VALORANT Champions 2021.mp3", 0));
            mostWatchedEntities.Add(new MostWatchEntity(@"C:\Users\hthna\Desktop\24kGoldn - Mood.mp4", 0));

            
        }

        private void listboxItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
