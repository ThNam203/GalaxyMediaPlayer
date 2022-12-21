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

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for ImagePage.xaml
    /// </summary>
    public partial class ImagePage : Page
    {

        private ObservableCollection<ImageModel> Images;
        public ImagePage()
        {
            InitializeComponent();
            Images = new ObservableCollection<ImageModel>();
            listViewImage.ItemsSource = Images;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
            }
        }


        //void ConnectDB()
        //{
        //    string cn_String = Properties.Settings.Default.connectionString;
        //}
        //void InsertToDB()
        //{
            
        //}

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

                openImageWindow.Show();
            }
        }

        
    }
}
