using GalaxyMediaPlayer.Models;
using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;

namespace GalaxyMediaPlayer.Pages.NavContentPages
{
    /// <summary>
    /// Interaction logic for MusicDetailPage.xaml
    /// </summary>
    /// 
    public partial class MusicDetailPage : Page
    {
        public MusicModel musicModel;
        public MusicDetailPage()
        {
            musicModel = new MusicModel(Uri.UnescapeDataString(MyMediaPlayer.GetSource.AbsolutePath));
            InitializeComponent();
            this.DataContext = musicModel;
            SetFontSize();
        }

        private void SetFontSize()
        {
            SizeF size = new SizeF();
            uint fontSize = 24;
            uint textBlockWidth = 500;
            uint textBlockHeight = 180;
            while (size.Width < textBlockWidth && size.Height < textBlockHeight)
                size = TextRenderer.MeasureText(musicModel.SongTitle, new Font("Segoe UI", fontSize++));

            double padding = (textBlockHeight - size.Height) / 2;
            tbSongTitle.Padding = new System.Windows.Thickness(0, padding * 3 / 2, 0, padding / 2);
            tbSongTitle.FontSize = fontSize;
        }

        private void btnGoBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MainPage.frameStack.Count > 1)
            {
                MainPage.frameStack.Pop();
                Uri lastestPage = MainPage.frameStack.Peek();
                MainPage.Instance.ContentFrame.Navigate(lastestPage);
            }
        }
    }
}
