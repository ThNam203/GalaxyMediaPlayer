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
    /// Interaction logic for ConfirmDeleteImageDialog.xaml
    /// </summary>
    public partial class ConfirmDeleteImageDialog : UserControl
    {
        private Action onOkButtonClick;
        private Action? onCancelButtonClick;
        public ConfirmDeleteImageDialog(string title, string message, Action onOkButtonClick, Action? onCancelButtonClick = null)
        {
            InitializeComponent();
            this.onOkButtonClick = onOkButtonClick;
            this.onCancelButtonClick = onCancelButtonClick;
            this.titleLabel.Content = title;
            this.messageBodyTb.Text = message;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (onCancelButtonClick == null) MainWindow.ClearAllMessageBox();
            else onCancelButtonClick();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            onOkButtonClick();
            MainWindow.ClearAllMessageBox();
        }

        public void Show()
        {
            MainWindow.ShowCustomMessageBoxInMiddle(this);
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
