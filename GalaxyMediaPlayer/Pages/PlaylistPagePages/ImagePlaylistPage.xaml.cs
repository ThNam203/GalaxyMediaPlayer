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
using static GalaxyMediaPlayer.Pages.NavContentPages.PlaylistPage;

namespace GalaxyMediaPlayer.Pages.PlaylistPagePages
{
    /// <summary>
    /// Interaction logic for ImagePlaylistPage.xaml
    /// </summary>
    public partial class ImagePlaylistPage : Page
    {
        public static ObservableCollection<ImagePlaylistModel> ImagePlaylists;
        public static int SelectedPlaylistIndex;
        public static ListView ListViewImage;
        public static ListBox listBoxImagePlaylist;
        public static Border BorderListView;
        public static DataGrid BrowseDataGrid;

        private bool _IsRuningImagePlaylist;
        public bool IsRuningImagePlaylist
        {
            get { return _IsRuningImagePlaylist; }
            set
            {
                _IsRuningImagePlaylist = value;
                if (_IsRuningImagePlaylist)
                {
                    ShowBtnOfPage2();
                    MainWindow.IsRuningImagePlaylist = false;
                    PreviewlistBoxItem_MouseLeftButtonDown();
                }
            }
        }


        public ImagePlaylistPage()
        {
            InitializeComponent();
            listBoxImagePlaylist = ListBoxImagePlaylist;
            ListViewImage = listViewImage;
            BorderListView = BorderlistView;
            BrowseDataGrid = browseDataGrid;
            ImagePlaylists = new ObservableCollection<ImagePlaylistModel>(ImagesPlaylistDBAccess.LoadImagePlayList());

            foreach (ImagePlaylistModel imagePlaylistModel in ImagePlaylists)
            {
                ListBoxImagePlaylist.Items.Add(imagePlaylistModel);
            }

            if (ImagePlaylists.Count > 0)
            {
                ShowBtnOfPage(1);
            }
            else
            {
                ShowBtnOfPage(0);
            }

            IsRuningImagePlaylist = MainWindow.IsRuningImagePlaylist;
        }

        public static void ShowBtnOfPage1()
        {
            //Visible button
            listBoxImagePlaylist.Visibility = Visibility.Visible;
            PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Visible;
            PlaylistPage.CbSortPlaylistBy.Visibility = Visibility.Visible;

            //Collasped button
            BorderListView.Visibility = Visibility.Collapsed;
            ListViewImage.Visibility = Visibility.Collapsed;
            BrowseDataGrid.Visibility = Visibility.Collapsed;
            PlaylistPage.AddNewImageToPlaylistBtn.Visibility = Visibility.Collapsed;
            PlaylistPage.BackBtn.Visibility = Visibility.Collapsed;
            PlaylistPage.browseStyleImage.Visibility = Visibility.Hidden;
            PlaylistPage.CbSortPlaylistInImagePlaylist.Visibility = Visibility.Collapsed;

            MainPage.Instance.btnPlayPause.Background.Opacity = 0.5f;
            MainPage.Instance.btnPlayPause.IsEnabled = false;
        }

        public static void ShowBtnOfPage2()
        {
            //Visible button
            ListViewImage.Visibility = Visibility.Visible;
            PlaylistPage.BackBtn.Visibility = Visibility.Visible;
            PlaylistPage.AddNewImageToPlaylistBtn.Visibility = Visibility.Visible;
            PlaylistPage.browseStyleImage.Visibility = Visibility.Visible;
            PlaylistPage.CbSortPlaylistInImagePlaylist.Visibility = Visibility.Visible;

            MainPage.Instance.btnPlayPause.Background.Opacity = 1;
            MainPage.Instance.btnPlayPause.IsEnabled = true;

            //Collasped button
            BrowseDataGrid.Visibility = Visibility.Collapsed;
            BorderListView.Visibility = Visibility.Collapsed;
            PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
            listBoxImagePlaylist.Visibility = Visibility.Collapsed;
            PlaylistPage.CbSortPlaylistBy.Visibility = Visibility.Collapsed;
        }

        public static void ShowBtnOfPage3()
        {
            //Visible button
            PlaylistPage.BackBtn.Visibility = Visibility.Visible;
            PlaylistPage.AddNewImageToPlaylistBtn.Visibility = Visibility.Visible;
            BrowseDataGrid.Visibility = Visibility.Visible;
            PlaylistPage.browseStyleImage.Visibility = Visibility.Visible;
            PlaylistPage.CbSortPlaylistInImagePlaylist.Visibility = Visibility.Visible;

            MainPage.Instance.btnPlayPause.Background.Opacity = 1;
            MainPage.Instance.btnPlayPause.IsEnabled = true;

            //Collasped button
            ListViewImage.Visibility = Visibility.Collapsed;
            BorderListView.Visibility = Visibility.Collapsed;
            PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
            listBoxImagePlaylist.Visibility = Visibility.Collapsed;
            PlaylistPage.CbSortPlaylistBy.Visibility = Visibility.Collapsed;
        }

        void ShowBtnOfPage(int num)
        {
            if (num == 0)
            {
                //Visible button
                BorderlistView.Visibility = Visibility.Visible;

                //Collasped button
                ListBoxImagePlaylist.Visibility = Visibility.Collapsed;
                PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
                listViewImage.Visibility = Visibility.Collapsed;
                browseDataGrid.Visibility = Visibility.Collapsed;
                PlaylistPage.AddNewImageToPlaylistBtn.Visibility = Visibility.Collapsed;
                PlaylistPage.BackBtn.Visibility = Visibility.Collapsed;
                PlaylistPage.browseStyleImage.Visibility = Visibility.Hidden;
                PlaylistPage.CbSortPlaylistBy.Visibility = Visibility.Collapsed;
                PlaylistPage.CbSortPlaylistInImagePlaylist.Visibility = Visibility.Collapsed;

                MainPage.Instance.btnPlayPause.Background.Opacity = 0.5f;
                MainPage.Instance.btnPlayPause.IsEnabled = false;
            }
            else if (num == 1)
            {
                //Visible button
                ListBoxImagePlaylist.Visibility = Visibility.Visible;
                PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Visible;
                PlaylistPage.CbSortPlaylistBy.Visibility = Visibility.Visible;

                //Collasped button
                BorderlistView.Visibility = Visibility.Collapsed;
                listViewImage.Visibility = Visibility.Collapsed;
                browseDataGrid.Visibility = Visibility.Collapsed;
                PlaylistPage.AddNewImageToPlaylistBtn.Visibility = Visibility.Collapsed;
                PlaylistPage.BackBtn.Visibility = Visibility.Collapsed;
                PlaylistPage.browseStyleImage.Visibility = Visibility.Hidden;
                PlaylistPage.CbSortPlaylistInImagePlaylist.Visibility = Visibility.Collapsed;

                MainPage.Instance.btnPlayPause.Background.Opacity = 0.5f;
                MainPage.Instance.btnPlayPause.IsEnabled = false;
            }
            else if (num == 2)
            {
                //Visible button
                listViewImage.Visibility = Visibility.Visible;
                PlaylistPage.BackBtn.Visibility = Visibility.Visible;
                PlaylistPage.AddNewImageToPlaylistBtn.Visibility = Visibility.Visible;
                PlaylistPage.browseStyleImage.Visibility = Visibility.Visible;
                PlaylistPage.CbSortPlaylistInImagePlaylist.Visibility = Visibility.Visible;

                MainPage.Instance.btnPlayPause.Background.Opacity = 1;
                MainPage.Instance.btnPlayPause.IsEnabled = true;

                //Collasped button
                browseDataGrid.Visibility = Visibility.Collapsed;
                BorderlistView.Visibility = Visibility.Collapsed;
                PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
                ListBoxImagePlaylist.Visibility = Visibility.Collapsed;
                PlaylistPage.CbSortPlaylistBy.Visibility = Visibility.Collapsed;
            }
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
            if (ImagePlaylists.Count > 0) ShowBtnOfPage(1);
            else ShowBtnOfPage(0);
        }

        private void listBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                ImagePlaylistModel imagePlaylistModel = ListBoxImagePlaylist.SelectedItem as ImagePlaylistModel;
                if (imagePlaylistModel != null)
                {
                    imagePlaylistModel.Images.Clear();
                    ListViewImage.Items.Clear();
                    browseDataGrid.Items.Clear();

                    foreach (ImageModel imageModel in ImagesInPlaylistDBAccess.LoadImageInPlayList(imagePlaylistModel.Id))
                    {
                        imagePlaylistModel.Images.Add(imageModel);
                        listViewImage.Items.Add(imageModel);
                        browseDataGrid.Items.Add(imageModel);
                    }

                    // Nam: use to control btnPlayPause on click
                    if (listViewImage.Items.Count > 0)
                    {
                        MainPage.Instance.btnPlayPause.Background.Opacity = 1;
                        MainPage.Instance.btnPlayPause.IsEnabled = true;
                    }
                    // Nam: end

                    ShowBtnOfPage(2);
                    PlaylistPage.CbSortPlaylistBy.SelectedIndex = -1;
                    PlaylistPage.PlaylistNameHeader.Text = imagePlaylistModel.PlaylistName;
                    PlaylistPage.ChooseCategoryPanel.Visibility = Visibility.Collapsed;
                    PlaylistPage.BackBtn.Visibility = Visibility.Visible;
                    PlaylistPage.AddNewImageToPlaylistBtn.Visibility = Visibility.Visible;
                    PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;

                    MainWindow.IdPlaylistRunning = imagePlaylistModel.Id;
                    MainWindow.IndexPlaylistRunning = listBoxImagePlaylist.SelectedIndex;
                }
            }
        }

        void PreviewlistBoxItem_MouseLeftButtonDown()
        {
            ImagePlaylistModel? imagePlaylistModel = listBoxImagePlaylist.Items[MainWindow.IndexPlaylistRunning] as ImagePlaylistModel;
            if (imagePlaylistModel != null)
            {
                imagePlaylistModel.Images.Clear();
                ListViewImage.Items.Clear();
                browseDataGrid.Items.Clear();

                foreach (ImageModel imageModel in ImagesInPlaylistDBAccess.LoadImageInPlayList(imagePlaylistModel.Id))
                {
                    imagePlaylistModel.Images.Add(imageModel);
                    listViewImage.Items.Add(imageModel);
                    browseDataGrid.Items.Add(imageModel);
                }

                // Nam: use to control btnPlayPause on click
                if (listViewImage.Items.Count > 0)
                {
                    MainPage.Instance.btnPlayPause.Background.Opacity = 1;
                    MainPage.Instance.btnPlayPause.IsEnabled = true;
                }
                // Nam: end

                ShowBtnOfPage(2);
                PlaylistPage.CbSortPlaylistBy.SelectedIndex = -1;
                PlaylistPage.PlaylistNameHeader.Text = imagePlaylistModel.PlaylistName;
                PlaylistPage.ChooseCategoryPanel.Visibility = Visibility.Collapsed;
                PlaylistPage.BackBtn.Visibility = Visibility.Visible;
                PlaylistPage.AddNewImageToPlaylistBtn.Visibility = Visibility.Visible;
                PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
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
            foreach (ImagePlaylistModel playlist in ListBoxImagePlaylist.SelectedItems)
            {
                ImagePlaylists.Remove(playlist);
                ImagesPlaylistDBAccess.DeleteImagePlaylist(playlist);
            }
            ListBoxImagePlaylist.Items.Clear();
            foreach (ImagePlaylistModel imagePlaylistModel in ImagePlaylists)
            {
                ListBoxImagePlaylist.Items.Add(imagePlaylistModel);
            }

            if (ImagePlaylists.Count <= 0)
            {
                ShowBtnOfPage(0);
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

        private void img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                ImagePlaylistModel? imagePlaylistModel = listBoxImagePlaylist.Items[MainWindow.IndexPlaylistRunning] as ImagePlaylistModel;
                if (imagePlaylistModel != null)
                {
                    ImageModel imageModelSelected = (ImageModel)listViewImage.SelectedItem;
                    if (imageModelSelected != null)
                    {
                        OpenImagePage openImagePage = new OpenImagePage(imageModelSelected, imagePlaylistModel.Images);
                        MainWindow.Instance.MainFrame.NavigationService.Navigate(openImagePage);
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
                    onDeleteButtonClick: DeleteImageInListView);
                int left = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).X);
                int top = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).Y);
                MainWindow.ShowCustomMessageBox(dialog, left: left, top: top);

                e.Handled = true;
            }
        }

        private void DeleteImageInListView()
        {
            ImagePlaylistModel? playlist;
            playlist = ListBoxImagePlaylist.SelectedItem as ImagePlaylistModel;
            if (playlist != null)
            {
                foreach (ImageModel item in listViewImage.SelectedItems)
                {
                    ImagesInPlaylistDBAccess.DeleteImageInPlaylist(item);
                }
                listViewImage.Items.Clear();
                browseDataGrid.Items.Clear();
                playlist.Images.Clear();
                foreach (ImageModel imageModel in ImagesInPlaylistDBAccess.LoadImageInPlayList(playlist.Id))
                {
                    listViewImage.Items.Add(imageModel);
                    browseDataGrid.Items.Add(imageModel);
                    playlist.Images.Add(imageModel);
                }
            }
        }

        private void DeleteImageInDataGridView()
        {
            ImagePlaylistModel? playlist;
            playlist = ListBoxImagePlaylist.SelectedItem as ImagePlaylistModel;
            if (playlist != null)
            {
                foreach (ImageModel item in browseDataGrid.SelectedItems)
                {
                    ImagesInPlaylistDBAccess.DeleteImageInPlaylist(item);
                }
                listViewImage.Items.Clear();
                browseDataGrid.Items.Clear();
                playlist.Images.Clear();
                foreach (ImageModel imageModel in ImagesInPlaylistDBAccess.LoadImageInPlayList(playlist.Id))
                {
                    listViewImage.Items.Add(imageModel);
                    browseDataGrid.Items.Add(imageModel);
                    playlist.Images.Add(imageModel);
                }
            }
        }

        private void browseDataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                ImagePlaylistModel? imagePlaylistModel = listBoxImagePlaylist.Items[MainWindow.IndexPlaylistRunning] as ImagePlaylistModel;
                if (imagePlaylistModel != null)
                {
                    ImageModel imageModelSelected = (ImageModel)browseDataGrid.SelectedItem;
                    if (imageModelSelected != null)
                    {
                        OpenImagePage openImagePage = new OpenImagePage(imageModelSelected, imagePlaylistModel.Images);
                        MainWindow.Instance.MainFrame.NavigationService.Navigate(openImagePage);
                    }
                }
            }
        }

        private void browseDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageModel? image;
            image = browseDataGrid.SelectedItem as ImageModel;
            if (image != null)
            {
                ImageRightClickDialog dialog = new ImageRightClickDialog(
                    onDeleteButtonClick: DeleteImageInDataGridView);
                int left = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).X);
                int top = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).Y);
                MainWindow.ShowCustomMessageBox(dialog, left: left, top: top);

                e.Handled = true;
            }
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            listViewImage.UnselectAll();
            browseDataGrid.UnselectAll();
            e.Handled = true;
        }

        
    }
}
