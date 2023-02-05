using GalaxyMediaPlayer.Databases.SongPlaylist;
using GalaxyMediaPlayer.Databases.VideoPage;
using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.Pages.NavContentPages;
using GalaxyMediaPlayer.UserControls.PlaylistControls;
using System;
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
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GalaxyMediaPlayer.Pages.PlaylistPagePages
{
    /// <summary>
    /// Interaction logic for VideoPlaylistPage.xaml
    /// </summary>
    public partial class VideoPlaylistPage : Page
    {
        public static ObservableCollection<VideoDisplay> source;
        public static ObservableCollection<VideoPaths> playlistSource;
        public static ListBox PlaylistListBox;
        public static DataGrid PlaylistVideosDataGrid;
        public static Border EmptyPlaylistBorder;
        private List<int> deleteIndices = new List<int>();


        VideoPaths videoPaths;

        public VideoPlaylistPage()
        {
            InitializeComponent();
            PlaylistListBox = playlistListBox;
            PlaylistVideosDataGrid = playlistVideosDataGrid;
            EmptyPlaylistBorder = emptyPlaylistBorder;
            DataBaseInit();
        }
        private void DataBaseInit()
        {
            if (!IsDataBaseExists())//H.Nam if the database is not exists, create new one
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlElement root = xmlDocument.CreateElement("root");
                xmlDocument.AppendChild(root);
                string path = AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPlaylistPage";
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                xmlDocument.Save(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPlayListPage\\VideoPlayListPath.xml");

            }
            videoPaths = new VideoPaths(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage\\VideoPath.xml");
            playlistSource = new ObservableCollection<VideoPaths>();
            playlistSource = videoPaths.GetAllPlaylistPaths();
            source = new ObservableCollection<VideoDisplay>(); 
            playlistListBox.ItemsSource = playlistSource;
            if (playlistListBox.Items.Count == 0) {
                PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
               emptyPlaylistBorder.Visibility= Visibility.Visible;
            }
        }
        public bool IsDataBaseExists()
        {
            return (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPlayListPage\\VideoPlayListPath.xml"));
        }


        private void secondaryNewPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            PlaylistPage.NewPlaylistBtn_Click(sender,e);            
            e.Handled = true;
        }
        public void change_Btn_Visibility()
        {

        }
        private void listBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
                if (playlistListBox.SelectedItems.Count > 0 && e.ClickCount >= 2)
                {

                    var x = playlistListBox.SelectedItem as VideoPaths;
                    source = x.GetAllPathsObs();
                    playlistVideosDataGrid.ItemsSource = source;//Video
                    NavContentPages.PlaylistPage.CbSortPlaylistBy.Visibility = Visibility.Collapsed;
                    NavContentPages.PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
                    NavContentPages.PlaylistPage.ChooseCategoryPanel.Visibility = Visibility.Collapsed;
                    NavContentPages.PlaylistPage.BackBtn.Visibility = Visibility.Visible;
                    NavContentPages.PlaylistPage.AddNewVideoToPlaylistBtn.Visibility = Visibility.Visible;
                    playlistVideosDataGrid.Visibility = Visibility.Visible;
                    playlistListBox.Visibility = Visibility.Hidden;
                    PlaylistPage.BackBtn.Visibility = Visibility.Visible;
                }
            

        }
        private void listBoxItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (playlistListBox.SelectedItems.Count > 0)
            {
                var x = playlistListBox.SelectedItem as VideoPaths;
                PlaylistRightClickDialog dialog = new PlaylistRightClickDialog(
                    onRenameButtonClick: RenamePlaylist,
                    onDeleteButtonClick: RemovePlaylist,
                    currentName: x.playlistName);

                int left = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).X);
                int top = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).Y);
                MainWindow.ShowCustomMessageBox(dialog, left: left, top: top);
                e.Handled = true;
            }



        }
        private void RemovePlaylist()
        {
            string[] oDirectories = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage", "*.xml", SearchOption.AllDirectories);
            var x = playlistListBox.SelectedItem as VideoPaths;
            foreach (string oDirectory in oDirectories)
            {
                if (System.IO.Path.GetFileNameWithoutExtension(oDirectory) == x.playlistName)
                {
                    File.Delete(oDirectory);
                }
            }
            playlistSource.Remove(x);
            if (playlistListBox.Items.Count == 0)
            {
                emptyPlaylistBorder.Visibility = Visibility.Visible;
                PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void RenamePlaylist(string newName)
        {
            string[] oDirectories = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage", "*.xml", SearchOption.AllDirectories);
            var x = playlistListBox.SelectedItem as VideoPaths;
            string renamePlaylist = string.Empty;
            foreach (string oDirectory in oDirectories)
            {
                if (System.IO.Path.GetFileNameWithoutExtension(oDirectory) == x.playlistName)
                {
                    renamePlaylist = System.IO.Path.GetDirectoryName(oDirectory) + "\\" + newName + ".xml";
                    File.Move(oDirectory, renamePlaylist);
                }
            }
            playlistSource.Remove(x);
            VideoPaths videoPaths = new VideoPaths(renamePlaylist);
            VideoPlaylistPage.playlistSource.Add(videoPaths);
        }
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            List<String> path = new List<string>();
            var x = playlistVideosDataGrid.SelectedItem as VideoDisplay;
            path.Add(x.pathToVideo);
            MainWindow.Instance.MainFrame.Navigate(new VideoMediaPLayer(path));
        }

        private void deleteIconHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var x = playlistVideosDataGrid.SelectedItems as VideoDisplay;
            if (deleteIndices.Count > 0)
            {
                ConfirmDialog dialog = new ConfirmDialog("Delete Songs", "Are you sure to delete " + deleteIndices.Count + " songs", deleteVideosInPlaylist);
                dialog.Show();
            }
        }
        private void DeleteCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;

            CheckBox cb = (CheckBox)sender;
            if ((bool)cb.IsChecked) deleteIndices.Add(playlistVideosDataGrid.SelectedIndex);
            else deleteIndices.Remove(playlistVideosDataGrid.SelectedIndex);
            e.Handled = true;
        }
        private void deleteVideosInPlaylist()
        {
            List<SongInfor> deleteSongs = new List<SongInfor>();
            foreach (int idx in deleteIndices.OrderByDescending(v => v))
            {
                var videoPaths = playlistListBox.SelectedItem as VideoPaths;
                videoPaths.DeletePath(source[idx].pathToVideo);
                source.RemoveAt(idx);
            }

            deleteIndices.Clear();
            PlaylistSongsDatabaseAccess.DeleteSongs(deleteSongs);
        }
    }
}
