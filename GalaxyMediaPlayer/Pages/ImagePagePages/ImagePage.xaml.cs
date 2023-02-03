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
            get { return _Images; }
            set { _Images = value; }
        }

        public static ListView ListViewImage;
        private bool _isUsingGridStyle;
        public bool isUsingGridStyle
        {
            get { return _isUsingGridStyle; }
            set
            {
                _isUsingGridStyle = value;
                if (isUsingGridStyle)
                {
                    ShowBtnOfPage(2);
                    BrowseStyleImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/list_32.png"));
                }
                else
                {
                    ShowBtnOfPage(1);
                    BrowseStyleImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/four_squares_32.png"));
                }
            }
        }

        public ImagePage()
        {
            InitializeComponent();
            Images = new List<ImageModel>();
            ListViewImage = listViewImage;
            isUsingGridStyle = MainWindow.IsImagePageUsingGridStyle;
            LoadFromDB(isUsingGridStyle);
        }

        void ShowBtnOfPage(int num)
        {
            if (num == 0)
            {
                //Visible button
                BorderlistView.Visibility = Visibility.Visible;

                //Collasped button
                listViewImage.Visibility = Visibility.Collapsed;
                btn_Addmore.Visibility = Visibility.Collapsed;
                ComboboxFilter.Visibility = Visibility.Collapsed;
                BrowseStyleImage.Visibility = Visibility.Collapsed;
                browseDataGrid.Visibility = Visibility.Collapsed;
            }
            else if (num == 1)
            {
                //Visible button
                listViewImage.Visibility = Visibility.Visible;
                ComboboxFilter.Visibility = Visibility.Visible;
                btn_Addmore.Visibility = Visibility.Visible;
                BrowseStyleImage.Visibility = Visibility.Visible;

                //Collasped button
                BorderlistView.Visibility = Visibility.Collapsed;
                browseDataGrid.Visibility = Visibility.Collapsed;
            }
            else if (num == 2)
            {
                //Visible button
                ComboboxFilter.Visibility = Visibility.Visible;
                btn_Addmore.Visibility = Visibility.Visible;
                browseDataGrid.Visibility = Visibility.Visible;
                BrowseStyleImage.Visibility = Visibility.Visible;

                //Collasped button
                listViewImage.Visibility = Visibility.Collapsed;
                BorderlistView.Visibility = Visibility.Collapsed;
            }
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
                    string name = System.IO.Path.GetFileName(fi.FullName);
                    string size = fi.Length.ToString();
                    string id = Guid.NewGuid().ToString();
                    ImageModel imgModel = new ImageModel(id, name, fi.FullName, date, size);
                    var FindingResult = Images.Find(img => img.path == imgModel.path);
                    if (FindingResult == null)
                    {
                        //insert to database
                        int SavingResult = ImagesDBAccess.SaveImage(imgModel);
                        if (SavingResult != -1)
                        {
                            listViewImage.Items.Add(imgModel);
                            browseDataGrid.Items.Add(imgModel);
                            Images.Add(imgModel);
                        }
                    }
                    if (Images.Count > 0)
                    {
                        if (isUsingGridStyle) ShowBtnOfPage(2);
                        else ShowBtnOfPage(1);
                    }
                    else ShowBtnOfPage(0);
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



        private void LoadFromDB(bool isUsingGridStyle)
        {
            Images = ImagesDBAccess.LoadImageList();
            if (Images.Count > 0)
            {
                if (isUsingGridStyle)
                    ShowBtnOfPage(2);
                else
                    ShowBtnOfPage(1);
                foreach (ImageModel imageModel in Images)
                {
                    if (imageModel.path != "" && imageModel.path != null)
                    {
                        listViewImage.Items.Add(imageModel);
                        browseDataGrid.Items.Add(imageModel);
                    }
                }
            }
            else ShowBtnOfPage(0);

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
                browseDataGrid.Items.Clear();
                foreach (ImageModel model in list)
                {
                    listViewImage.Items.Add(model);
                    browseDataGrid.Items.Add(model);
                }
            }
            else if (sortIndex == 1)
            {
                List<ImageModel> list = new List<ImageModel>(Images);
                list.Sort((x, y) => x.CompareDate(y));
                listViewImage.Items.Clear();
                browseDataGrid.Items.Clear();
                foreach (ImageModel model in list)
                {
                    listViewImage.Items.Add(model);
                    browseDataGrid.Items.Add(model);
                }
            }
            else if (sortIndex == 2)
            {
                List<ImageModel> list = new List<ImageModel>(Images);
                list.Sort((x, y) => x.length.CompareTo(y.length));
                listViewImage.Items.Clear();
                browseDataGrid.Items.Clear();
                foreach (ImageModel model in list)
                {
                    listViewImage.Items.Add(model);
                    browseDataGrid.Items.Add(model);
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
            foreach (ImageModel item in listViewImage.SelectedItems)
            {
                Images.Remove(item);
                ImagesDBAccess.DeleteImage(item);
            }
            listViewImage.Items.Clear();
            browseDataGrid.Items.Clear();
            foreach (ImageModel item in Images)
            {
                listViewImage.Items.Add(item);
                browseDataGrid.Items.Add(item);
            }

            if (Images.Count == 0)
            {
                ShowBtnOfPage(0);
            }
        }

        private void DeleteImageInDataGridView()
        {
            foreach (ImageModel item in browseDataGrid.SelectedItems)
            {
                Images.Remove(item);
                ImagesDBAccess.DeleteImage(item);
            }
            listViewImage.Items.Clear();
            browseDataGrid.Items.Clear();
            foreach (ImageModel item in Images)
            {
                listViewImage.Items.Add(item);
                browseDataGrid.Items.Add(item);
            }

            if (Images.Count == 0)
            {
                ShowBtnOfPage(0);
            }
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            listViewImage.UnselectAll();
            browseDataGrid.UnselectAll();
            e.Handled = true;
        }



        private void BrowseStyleImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow.IsImagePageUsingGridStyle = !MainWindow.IsImagePageUsingGridStyle;
            isUsingGridStyle = MainWindow.IsImagePageUsingGridStyle;
        }

        private void browseDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                ImageModel imageModelSelected = (ImageModel)browseDataGrid.SelectedItem;
                OpenImagePage openImagePage = new OpenImagePage(imageModelSelected, Images);
                openImagePage.IsDoubleClick = true;

                MainWindow.Instance.MainFrame.Navigate(openImagePage);
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
    }
}
