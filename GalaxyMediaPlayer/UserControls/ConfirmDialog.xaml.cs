using System;
using System.Windows;
using System.Windows.Controls;

namespace GalaxyMediaPlayer.UserControls.PlaylistControls
{
    /// <summary>
    /// Interaction logic for ConfirmDeleteDialog.xaml
    /// </summary>
    public partial class ConfirmDialog : UserControl
    {
        private Action onOkButtonClick;
        private Action? onCancelButtonClick;

        public ConfirmDialog(string title, string message, Action onOkButtonClick, Action? onCancelButtonClick = null)
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