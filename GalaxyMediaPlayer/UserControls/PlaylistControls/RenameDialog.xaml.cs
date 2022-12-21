using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GalaxyMediaPlayer.UserControls.PlaylistControls
{
    /// <summary>
    /// Interaction logic for RenameDialog.xaml
    /// </summary>
    public partial class RenameDialog : UserControl
    {
        private Action<string> onRenameButtonClick;

        public RenameDialog(Action<string> onRenameButtonClick, string currentName)
        {
            InitializeComponent();
            this.onRenameButtonClick = onRenameButtonClick;
            playlistNameTextbox.Text = currentName;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ClearAllMessageBox();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            string playlistName = playlistNameTextbox.Text;

            if (playlistName == "" || playlistName == null)
            {
                playlistNameTextbox.Background = Brushes.DarkRed;
            }
            else
            {
                onRenameButtonClick(playlistName);
                MainWindow.ClearAllMessageBox();
            }
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}