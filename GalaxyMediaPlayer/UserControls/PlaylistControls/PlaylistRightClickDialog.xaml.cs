using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GalaxyMediaPlayer.UserControls.PlaylistControls
{
    /// <summary>
    /// Interaction logic for PlaylistRightClickDialog.xaml
    /// </summary>
    public partial class PlaylistRightClickDialog : UserControl
    {
        private Action onDeleteButtonClick;
        private Action<string> onRenameButtonClick;
        private string currentName; // which is used for renaming

        public PlaylistRightClickDialog(
            Action<string> onRenameButtonClick,
            Action onDeleteButtonClick,
            string currentName)
        {
            InitializeComponent();
            this.onDeleteButtonClick = onDeleteButtonClick;
            this.onRenameButtonClick = onRenameButtonClick;
            this.currentName = currentName;
        }

        private void renameBtn_Click(object sender, RoutedEventArgs e)
        {
            RenameDialog renameDialog = new RenameDialog(onRenameButtonClick, currentName);
            MainWindow.ClearAllMessageBox();
            MainWindow.ShowCustomMessageBoxInMiddle(renameDialog);
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDialog confirmDeleteDialog = new ConfirmDialog(
                "Delete playlist", 
                "Are you sure to delete this playlist?", 
                onDeleteButtonClick);

            MainWindow.ClearAllMessageBox();
            confirmDeleteDialog.Show();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
