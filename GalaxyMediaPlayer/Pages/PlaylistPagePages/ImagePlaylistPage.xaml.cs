using GalaxyMediaPlayer.Databases.ImagePage;
using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.Pages.ImagePagePages;
using GalaxyMediaPlayer.Pages.NavContentPages;
using GalaxyMediaPlayer.UserControls;
using GalaxyMediaPlayer.UserControls.ImageControls;
using GalaxyMediaPlayer.UserControls.PlaylistControls;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GalaxyMediaPlayer.Pages.PlaylistPagePages
{
    /// <summary>
    /// Interaction logic for ImagePlaylistPage.xaml
    /// </summary>
    public partial class ImagePlaylistPage : Page
    {
        public static ObservableCollection<ImagePlaylistModel> ImagePlaylists;
        public static ListView ListViewImage;
        public static ListBox listBoxImagePlaylist;
        public static Border BorderListView;

        public ImagePlaylistPage()
        {
            InitializeComponent();
            listBoxImagePlaylist = ListBoxImagePlaylist;
            ListViewImage = listViewImage;
            BorderListView = BorderlistView;
            ImagePlaylists = new ObservableCollection<ImagePlaylistModel>(ImagesPlaylistDBAccess.LoadImagePlayList());

            foreach(ImagePlaylistModel imagePlaylistModel in ImagePlaylists)
            {
                ListBoxImagePlaylist.Items.Add(imagePlaylistModel);
            }

            if(ImagePlaylists.Count > 0)
            {
                ShowButtonWhenHaveImagePlaylist();
            }
            else
            {
                ShowButtonWhenDoNotHaveImagePlaylist();
            }
        }

        void ShowButtonWhenDoNotHaveImagePlaylist()
        {
            BorderlistView.Visibility = Visibility.Visible;
            PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
            ListBoxImagePlaylist.Visibility = Visibility.Collapsed;
        }

        void ShowButtonWhenHaveImagePlaylist()
        {
            BorderlistView.Visibility = Visibility.Collapsed;
            PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Visible;
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
            ImagePlaylists.Add(imagePlaylistModel);
            ListBoxImagePlaylist.Items.Add(imagePlaylistModel);

            ImagesPlaylistDBAccess.SaveImagePlaylist(imagePlaylistModel);
            MainWindow.ClearAllMessageBox();
            if (ImagePlaylists.Count > 0)
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
                    {
                        imagePlaylistModel.Images.Clear();
                        ListViewImage.Items.Clear();
                    }
                    foreach(ImageModel imageModel in ImagesInPlaylistDBAccess.LoadImageInPlayList(imagePlaylistModel.Id))
                    {
                        imagePlaylistModel.Images.Add(imageModel);
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
            ImagePlaylistModel? playlist;
            playlist = ListBoxImagePlaylist.SelectedItem as ImagePlaylistModel;

            if (playlist != null)
            {
                PlaylistRightClickDialog dialog = new PlaylistRightClickDialog(
                    onRenameButtonClick: RenameImagePlaylist,
                    onDeleteButtonClick: RemoveImagePlaylist,
                    currentName: playlist.PlaylistName);

                int left = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).X);
                int top = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).Y);
                MainWindow.ShowCustomMessageBox(dialog, left: left, top: top);
                e.Handled = true;
            }
        }

        private void RemoveImagePlaylist()
        {
            ImagePlaylistModel? playlist;
            playlist = ListBoxImagePlaylist.SelectedItem as ImagePlaylistModel;

            if (playlist != null)
            {
                ImagePlaylists.Remove(playlist);
                ListBoxImagePlaylist.Items.Remove(playlist);
                ImagesPlaylistDBAccess.DeleteImagePlaylist(playlist);
            }

            if (ImagePlaylists.Count <= 0)
            {
                ShowButtonWhenDoNotHaveImagePlaylist();
            }
        }

        private void RenameImagePlaylist(string newName)
        {
            ImagePlaylistModel? playlist;
            playlist = ListBoxImagePlaylist.SelectedItem as ImagePlaylistModel;

            if (playlist != null)
            {
                ImagePlaylistModel renamedPlaylist = new ImagePlaylistModel(playlist);
                renamedPlaylist.PlaylistName = newName;


                ImagesPlaylistDBAccess.RenameImagePlaylist(renamedPlaylist);

                for (int i = 0; i < ImagePlaylists.Count; i++)
                {
                    if (playlist.Id == ImagePlaylists[i].Id)
                    {
                        ImagePlaylists.RemoveAt(i);
                        ListBoxImagePlaylist.Items.RemoveAt(i);
                        ImagePlaylists.Insert(i, renamedPlaylist);
                        ListBoxImagePlaylist.Items.Insert(i, ImagePlaylists[i]);
                        break;
                    }
                }
            }
        }

        void getSizeOfNormalWindow(
            ref double normalWidth,
            ref double normalHeight,
            ref double normalLeft,
            ref double normalTop)
        {
            if(MainWindow.Instance.WindowState == WindowState.Maximized)
            {
                MainWindow.Instance.WindowState = WindowState.Normal;

                normalWidth = MainWindow.Instance.ActualWidth;
                normalHeight = MainWindow.Instance.ActualHeight;
                normalLeft = MainWindow.Instance.Left;
                normalTop = MainWindow.Instance.Top;

                MainWindow.Instance.WindowState = WindowState.Maximized;
            }
            else
            {
                normalWidth = MainWindow.Instance.ActualWidth;
                normalHeight = MainWindow.Instance.ActualHeight;
                normalLeft = MainWindow.Instance.Left;
                normalTop = MainWindow.Instance.Top;
            }
        }

        private void img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ImagePlaylistModel imagePlaylistModel = ListBoxImagePlaylist.SelectedItem as ImagePlaylistModel;
            if (e.ClickCount >= 2)
            {
                if (imagePlaylistModel != null)
                {
                    ImageModel imageModelSelected = (ImageModel)listViewImage.SelectedItem;
                    if (imageModelSelected != null)
                    {
                        Application.Current.MainWindow.Visibility = Visibility.Hidden;

                        double normalWidth = 0, normalHeight = 0, normalLeft = 0,normalTop = 0 ;
                        getSizeOfNormalWindow(ref normalWidth,ref normalHeight, ref normalLeft, ref normalTop);
                        ShowImagePlaylistPage showImagePlaylistPage = new ShowImagePlaylistPage(imageModelSelected, imagePlaylistModel.Images, normalWidth, normalHeight, normalLeft, normalTop);

                        showImagePlaylistPage.WindowState = MainWindow.Instance.WindowState;
                        showImagePlaylistPage.Show();
                    }
                }
            }
        }

        private void img_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageModel? image;
            image = listViewImage.SelectedItem as ImageModel;
            if (image != null)
            {
                ImageRightClickDialog dialog = new ImageRightClickDialog(
                    onDeleteButtonClick: DeleteImage);
                int left = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).X);
                int top = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).Y);
                MainWindow.ShowCustomMessageBox(dialog, left: left, top: top);

                e.Handled = true;
            }
        }

        private void DeleteImage()
        {
            ImagePlaylistModel? playlist;
            playlist = ListBoxImagePlaylist.SelectedItem as ImagePlaylistModel;
            if (playlist != null)
            {
                foreach (ImageModel item in listViewImage.SelectedItems)
                {
                    ImagesInPlaylistDBAccess.DeleteImagePlaylist(item);
                }
                listViewImage.Items.Clear();
                foreach (ImageModel imageModel in ImagesInPlaylistDBAccess.LoadImageInPlayList(playlist.Id))
                {
                    listViewImage.Items.Add(imageModel);
                }
            }
        }
    }
}
