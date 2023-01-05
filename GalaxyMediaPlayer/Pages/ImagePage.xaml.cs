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

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for ImagePage.xaml
    /// </summary>
    public partial class ImagePage : Page
    {

        private ObservableCollection<ImageModel> Images;
        DataTable dtImagePath;
        public ImagePage()
        {
            InitializeComponent();
            Images = new ObservableCollection<ImageModel>();
            listViewImage.ItemsSource = Images;
            LoadFromDB();
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
                foreach (string file in dialog.FileNames)
                {
                    //add filePath to listview
                    ImageModel imgModel = new ImageModel(file);
                    Images.Add(imgModel);

                    //add filePath to database

                }
                InsertToDB();
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ImageModel imageModelSelected = Images[listViewImage.SelectedIndex];
                string ImagePath = imageModelSelected.path;
                OpenImageWindow openImageWindow = new OpenImageWindow(ImagePath);

                //MainWindow main = Application.Current.MainWindow as MainWindow;
                //if (main != null)
                //{
                //    main.WindowState = WindowState.Minimized;
                //}

                openImageWindow.WindowState = WindowState.Maximized;
                openImageWindow.Show();
            }
            else if (e.ClickCount == 1)
            {
                if (sender != null)
                {
                    if (e.OriginalSource is not CheckBox)
                    {
                        FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
                        ImageModel model = (ImageModel)frameworkElement.DataContext;
                        if (model != null)
                        {
                            if (model.imgIsSelected) model.imgIsSelected = false;
                            else model.imgIsSelected = true;
                        }
                    }
                }
            }
        }

        private void btn_DeleteImage_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            foreach (ImageModel item in Images.ToList())
            {
                index++;
                if (item.imgIsSelected)
                {
                    DeleteFromDB(item.path);
                    Images.RemoveAt(index);
                }
            }
        }

        private void LoadFromDB()
        {
            dtImagePath = new DataTable();
            dtImagePath = DataConfig.DataTransport("SELECT * FROM ImageList WHERE ImagePath IS NOT NULL");
            if(dtImagePath != null)
            {
                BorderlistView.Visibility = Visibility.Collapsed;
                listViewImage.Visibility = Visibility.Visible;
            }
            foreach (DataRow row in dtImagePath.Rows)
            {
                if (row != null)
                {
                    ImageModel model = new ImageModel(row[0].ToString());
                    Images.Add(model);
                }
            }
        }

        private void InsertToDB()
        {
            foreach (ImageModel imageModel in Images.ToList())
            {
                DataConfig.DataExecution(InsertImagePathToDatatable(imageModel.path).ToString());
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
    }
}
