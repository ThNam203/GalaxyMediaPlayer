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

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for ImagePage.xaml
    /// </summary>
    public partial class ImagePage : Page
    {

        private List<ImageModel> Images;
        DataTable dtImagePath;
        public ImagePage()
        {
            InitializeComponent();
            Images = new List<ImageModel>();
            LoadFromDB();
            ItemBarImages.BorderBrush = Brushes.White;
        }

        private void btn_Addmore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Title = "Open Image";
            dialog.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|jpeg files (*.jpeg)|*.jpeg";
            if (dialog.ShowDialog() == true)
            {
                BorderlistView.Visibility = Visibility.Collapsed;
                listViewImage.Visibility = Visibility.Visible;
                btn_Addmore.Visibility = Visibility.Visible;
                btn_DeleteImage.Visibility = Visibility.Visible;
                foreach (string file in dialog.FileNames)
                {
                    //add filePath to listview
                    FileInfo fi = new FileInfo(file);
                    string date = fi.CreationTime.ToString();
                    ImageModel imgModel = new ImageModel(file,date);
                    Images.Add(imgModel);
                    listViewImage.Items.Add(imgModel);

                    //insert to database
                    ImagesDBAccess.SaveImage(imgModel);
                }
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                ImageModel imageModelSelected = Images[listViewImage.SelectedIndex];
                string ImagePath = imageModelSelected.path;
                OpenImagePage openImagePage = new OpenImagePage(ImagePath);
                openImagePage.IsDoubleClick = true;
                MainWindow.Instance.MainFrame.Navigate(openImagePage);
            }
        }



        private void btn_DeleteImage_Click(object sender, RoutedEventArgs e)
        {
            foreach (ImageModel item in Images.ToList())
            {
                if (item.imgIsSelected)
                {
                    DeleteFromDB(item.path);
                    Images.Remove(item);
                }
            }
            if(Images.Count == 0)
            {
                BorderlistView.Visibility = Visibility.Visible;
                listViewImage.Visibility = Visibility.Collapsed;
                btn_Addmore.Visibility = Visibility.Collapsed;
                btn_DeleteImage.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadFromDB()
        {
            Images = ImagesDBAccess.LoadImageList();
            if (Images.Count > 0)
            {
                BorderlistView.Visibility = Visibility.Collapsed;
                listViewImage.Visibility = Visibility.Visible;
                btn_Addmore.Visibility = Visibility.Visible;
                btn_DeleteImage.Visibility = Visibility.Visible;
            }
            else
            {
                BorderlistView.Visibility = Visibility.Visible;
                listViewImage.Visibility = Visibility.Collapsed;
                btn_Addmore.Visibility = Visibility.Collapsed;
                btn_DeleteImage.Visibility = Visibility.Collapsed;
            }

        }

        private void DeleteFromDB(string imagePath)
        {
            DataConfig.DataExecution(DeleteImagePathFromDatatable(imagePath).ToString());
        }

        private StringBuilder InsertImagePathToDatatable(string imagePath)
        {
            StringBuilder sSQL = new StringBuilder();

            sSQL.Append("INSERT INTO ImageList (");
            sSQL.Append("ImagePath");
            sSQL.Append(") VALUES (");
            sSQL.Append(ConvertToSQL(imagePath) + ")");

            return sSQL;
        }

        private StringBuilder DeleteImagePathFromDatatable(string imagePath)
        {
            StringBuilder sSQL = new StringBuilder();

            sSQL.Append("DELETE FROM ImageList WHERE ImagePath = ");
            sSQL.Append(ConvertToSQL(imagePath));
            return sSQL;
        }

        private string ConvertToSQL(string sValue)
        {
            return "'" + sValue + "'";
        }

        private void img_MouseLeave(object sender, MouseEventArgs e)
        {
            foreach(ImageModel item in listViewImage.Items)
            {
                int index = listViewImage.Items.IndexOf(item);
                Images[index].imgIsSelected = false;
            }
            foreach(ImageModel item in listViewImage.SelectedItems)
            {
                int index = listViewImage.Items.IndexOf(item);
                Images[index].imgIsSelected = true;
            }
        }

        private void ItemBarImages_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ItemBarImages.BorderBrush = Brushes.White;
        }

        private void ComboboxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sortIndex = ComboboxFilter.SelectedIndex;
            if (sortIndex == 0)
            {
                List<ImageModel> list = new List<ImageModel>(Images);
                list.Sort((x, y) => x.path.CompareTo(y.path));
                Images.Clear();
                foreach (ImageModel model in list)
                {
                    Images.Add(model);
                }
            }
            else if(sortIndex == 1)
            {

            }
        }
    }
}
