using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GalaxyMediaPlayer.UserControls
{
    /// <summary>
    /// Interaction logic for NewPlaylistControl.xaml
    /// </summary>
    public partial class NewPlaylistControl : UserControl
    {
        private Action<string> onOkButtonClick;

        public NewPlaylistControl(Action<string> onOkButtonClick)
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
            string playlistName = playlistNameTextbox.Text;

            if (playlistName == "" || playlistName == null)
            {
                playlistNameTextbox.Background = Brushes.DarkRed;
            } else
            {
                onOkButtonClick(playlistName);
                MainWindow.ClearAllMessageBox();
            }
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
