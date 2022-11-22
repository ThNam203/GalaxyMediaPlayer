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
            ConfirmDeleteDialog confirmDeleteDialog = new ConfirmDeleteDialog(onDeleteButtonClick);
            MainWindow.ClearAllMessageBox();
            MainWindow.ShowCustomMessageBoxInMiddle(confirmDeleteDialog);
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
