using System.Windows;
using System.Windows.Controls;

namespace GalaxyMediaPlayer.UserControls
{
    /// <summary>
    /// Interaction logic for ShowMessageControl.xaml
    /// </summary>
    public partial class ShowMessageControl : UserControl
    {
        public ShowMessageControl(string messageTitle, string messageBody)
        {
            InitializeComponent();
            messageTitleTb.Content = messageTitle;
            messageBodyTb.Text = messageBody;
        }

        public void Show()
        {
            MainWindow.ShowCustomMessageBoxInMiddle(this);
            double x = Height;
        }

        private void understandBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ClearAllMessageBox();
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
