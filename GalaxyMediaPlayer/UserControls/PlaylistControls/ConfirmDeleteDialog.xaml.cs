using System;
using System.Windows;
using System.Windows.Controls;

namespace GalaxyMediaPlayer.UserControls.PlaylistControls
{
    /// <summary>
    /// Interaction logic for ConfirmDeleteDialog.xaml
    /// </summary>
    public partial class ConfirmDeleteDialog : UserControl
    {
        private Action onOkButtonClick;

        public ConfirmDeleteDialog(Action onOkButtonClick)
        {
            InitializeComponent();
            this.onOkButtonClick = onOkButtonClick;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ClearAllMessageBox();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            onOkButtonClick();
            MainWindow.ClearAllMessageBox();
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}