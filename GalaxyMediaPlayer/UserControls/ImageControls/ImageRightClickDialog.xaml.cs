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
using System.Windows.Shapes;

namespace GalaxyMediaPlayer.UserControls.ImageControls
{
    /// <summary>
    /// Interaction logic for ImageRightClickDialog.xaml
    /// </summary>
    public partial class ImageRightClickDialog : UserControl
    {
        private Action onDeleteButtonClick;
        public ImageRightClickDialog(Action onDeleteButtonClick)
        {
            InitializeComponent();
            this.onDeleteButtonClick = onDeleteButtonClick;
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDeleteImageDialog dialog = new ConfirmDeleteImageDialog(
                "Delete image",
                "Are you sure to delete it from photos ?",
                onDeleteButtonClick);
            MainWindow.ClearAllMessageBox();
            dialog.Show();
        }
    }
}
