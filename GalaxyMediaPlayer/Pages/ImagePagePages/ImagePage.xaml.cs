using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;
using GalaxyMediaPlayer.Models;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using GalaxyMediaPlayer.ConnectDB;
using GalaxyMediaPlayer.Databases.ImagePage;
using GalaxyMediaPlayer.Helpers;
using GalaxyMediaPlayer.UserControls.ImageControls;
using System.Runtime.Serialization;
using System.Net.Mime;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for ImagePage.xaml
    /// </summary>
    public partial class ImagePage : Page
    {

        private static List<ImageModel> _Images;
        public static List<ImageModel> Images
        {
            get { return _Images;}
            set { _Images = value; }
        }

        public static ListView ListViewImage;
        public ImagePage()
        {
            InitializeComponent();
            Images = new List<ImageModel>();
            ListViewImage = listViewImage;
            LoadFromDB();
        }

        void ShowButtonWhenDoNotHaveImage()
        {
            BorderlistView.Visibility = Visibility.Visible;
            listViewImage.Visibility = Visibility.Collapsed;
            btn_Addmore.Visibility = Visibility.Collapsed;
            ComboboxFilter.Visibility = Visibility.Collapsed;
        }

        void ShowButtonWhenHaveImage()
        {
            BorderlistView.Visibility = Visibility.Collapsed;
            listViewImage.Visibility = Visibility.Visible;
            btn_Addmore.Visibility = Visibility.Visible;
            ComboboxFilter.Visibility = Visibility.Visible;
        }

        private void btn_Addmore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string filter = "SupportedFormat|";
            foreach (string extenstion in SupportedExtensions.IMAGE_EXTENSION)
            {
                filter += "*." + extenstion + ";";
            }
            dialog.Filter = filter;
            dialog.Multiselect = true;
            dialog.Title = "Open Image";
            if (dialog.ShowDialog() == true)
            {
                foreach (string file in dialog.FileNames)
                {
                    //add filePath to listview
                    FileInfo fi = new FileInfo(file);
                    string date = fi.CreationTime.ToString();
                    ImageModel imgModel = new ImageModel(file, date);
                    var FindingResult = Images.Find(img => img.path == imgModel.path);
                    if (FindingResult == null)
                    {
                        Images.Add(imgModel);
                        //insert to database
                        int SavingResult = ImagesDBAccess.SaveImage(imgModel);
                        if (SavingResult == 1) listViewImage.Items.Add(imgModel);
                    }
                    ShowButtonWhenHaveImage();
                }
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                ImageModel imageModelSelected = (ImageModel)listViewImage.SelectedItem;
                OpenImagePage openImagePage = new OpenImagePage(imageModelSelected, Images);
                openImagePage.IsDoubleClick = true;
                
                MainWindow.Instance.MainFrame.Navigate(openImagePage);
            }
        }

        private void LoadFromDB()
        {
            Images = ImagesDBAccess.LoadImageList();
            if (Images.Count > 0)
            {
                ShowButtonWhenHaveImage();

                foreach (ImageModel imageModel in Images)
                {
                    if (imageModel.path != "" && imageModel.path != null)
                        listViewImage.Items.Add(imageModel);
                }
            }
            else ShowButtonWhenDoNotHaveImage();

        }

        private void ItemBarImages_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //ItemBarImages.BorderBrush = Brushes.White;
        }
        private void ComboboxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sortIndex = ComboboxFilter.SelectedIndex;
            if (sortIndex == -1)
                DefaultContentCombobox.Content = "Filter";
            else
                DefaultContentCombobox.Content = "";

            if (sortIndex == 0)
            {
                List<ImageModel> list = new List<ImageModel>(Images);
                list.Sort((x, y) => Path.GetFileName(x.path).CompareTo(Path.GetFileName(y.path)));
                listViewImage.Items.Clear();
                foreach (ImageModel model in list)
                {
                    listViewImage.Items.Add(model);

                }
            }
            else if (sortIndex == 1)
            {
                List<ImageModel> list = new List<ImageModel>(Images);
                list.Sort((x, y) => x.CompareDate(y));
                listViewImage.Items.Clear();
                foreach (ImageModel model in list)
                {
                    listViewImage.Items.Add(model);
                }
            }
        }

        private void img_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageModel? image;
            image = listViewImage.SelectedItem as ImageModel;
            if(image != null)
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
            foreach (ImageModel item in Images)
            {
                item.IsSelected = false;
            }
            foreach (ImageModel item in listViewImage.SelectedItems)
            {
                int index = listViewImage.Items.IndexOf(item);
                Images[index].IsSelected = true;
            }
            foreach (ImageModel item in Images.ToList())
            {
                if (item.IsSelected)
                {
                    Images.Remove(item);
                    listViewImage.Items.Remove(item);
                    ImagesDBAccess.DeleteImage(item);
                }
            }
            if (Images.Count == 0)
            {
                ShowButtonWhenDoNotHaveImage();
            }
        }
    }
}
