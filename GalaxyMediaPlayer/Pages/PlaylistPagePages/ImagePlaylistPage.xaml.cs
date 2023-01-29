using GalaxyMediaPlayer.Databases.ImagePage;
using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.Pages.NavContentPages;
using GalaxyMediaPlayer.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GalaxyMediaPlayer.Pages.PlaylistPagePages
{
    /// <summary>
    /// Interaction logic for ImagePlaylistPage.xaml
    /// </summary>
    public partial class ImagePlaylistPage : Page
    {
        public static ObservableCollection<ImagePlaylistModel> ImagesPlaylist;
        public static ListView ListViewImage;
        public static ListBox listBoxImagePlaylist;
        public static Border BorderListView;

        public ImagePlaylistPage()
        {
            InitializeComponent();
            listBoxImagePlaylist = ListBoxImagePlaylist;
            ListViewImage = listViewImage;
            BorderListView = BorderlistView;
            ImagesPlaylist = new ObservableCollection<ImagePlaylistModel>(ImagesPlaylistDBAccess.LoadImagePlayList());

            foreach(ImagePlaylistModel imagePlaylistModel in ImagesPlaylist)
            {
                ListBoxImagePlaylist.Items.Add(imagePlaylistModel);
            }

            if(ImagesPlaylist.Count > 0)
            {
                ShowButtonWhenHaveImagePlaylist();
            }
        }

        void ShowButtonWhenDoNotHaveImagePlaylist()
        {
            BorderlistView.Visibility = Visibility.Visible;
            ListBoxImagePlaylist.Visibility = Visibility.Collapsed;
        }

        void ShowButtonWhenHaveImagePlaylist()
        {
            BorderlistView.Visibility = Visibility.Collapsed;
            ListBoxImagePlaylist.Visibility = Visibility.Visible;
        }

        void ShowButtonWhenInImagePlaylist()
        {
            listViewImage.Visibility = Visibility.Visible;
            ListBoxImagePlaylist.Visibility = Visibility.Collapsed;
        }

        void ShowButtonWhenNotInImagePlaylist()
        {
            listViewImage.Visibility= Visibility.Collapsed;
            ListBoxImagePlaylist.Visibility = Visibility.Visible;
        }

        private void Btn_NewPlaylist_Click(object sender, RoutedEventArgs e)
        {
            NewPlaylistControl newPlaylistControl = new NewPlaylistControl(AddNewImagePlaylist);
            MainWindow.ShowCustomMessageBoxInMiddle(newPlaylistControl);
        }

        private void AddNewImagePlaylist(string PlaylistName)
        {
            ImagePlaylistModel imagePlaylistModel = new ImagePlaylistModel(PlaylistName);
            ImagesPlaylist.Add(imagePlaylistModel);
            ListBoxImagePlaylist.Items.Add(imagePlaylistModel);

            ImagesPlaylistDBAccess.SaveImagePlaylist(imagePlaylistModel);
            MainWindow.ClearAllMessageBox();
            if (ImagesPlaylist.Count > 0)
            {
                ShowButtonWhenHaveImagePlaylist();
            }
        }

        private void listBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount >=2)
            {
                ImagePlaylistModel imagePlaylistModel = ListBoxImagePlaylist.SelectedItem as ImagePlaylistModel;
                if (imagePlaylistModel != null)
                {
                    if(listViewImage.Items.Count > 0)
                        ListViewImage.Items.Clear();
                    foreach(ImageModel imageModel in ImagesInPlaylistDBAccess.LoadImageInPlayList(imagePlaylistModel.Id))
                    {
                        listViewImage.Items.Add(imageModel);
                    }

                    ShowButtonWhenInImagePlaylist();
                    PlaylistPage.PlaylistNameHeader.Text = imagePlaylistModel.PlaylistName;
                    PlaylistPage.ChooseCategoryPanel.Visibility = Visibility.Collapsed;
                    PlaylistPage.BackBtn.Visibility = Visibility.Visible;
                    PlaylistPage.AddNewImageToPlaylistBtn.Visibility = Visibility.Visible;
                    PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void listBoxItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void img_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void img_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
