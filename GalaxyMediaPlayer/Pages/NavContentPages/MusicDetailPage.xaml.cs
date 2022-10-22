using GalaxyMediaPlayer.Models;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System;
using System.Drawing;
using System.Threading.Tasks;
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
            SetLyrics().ContinueWith(result =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    tbSongLyrics.Text = result.Result;
                });
            });
        }

        private async Task<string> SetLyrics()
        {
            string DEFAULT_RETURN_VALUE = "No lyrics found";

            var baseUrl = "https://www.azlyrics.com/lyrics";
            string paramArtist = FormatStringToGetLyrics(musicModel.SongFirstArtist);
            string paramTitle = FormatStringToGetLyrics(musicModel.SongTitle);

            if (paramArtist == "" && paramTitle == "") return DEFAULT_RETURN_VALUE;

            string finalUrl = baseUrl + "/" + paramArtist + "/" + paramTitle + ".html";

            string resultSongLyrics = "";
            await Task.Run(() =>
            {
                try
                {
                    var web = new HtmlWeb();

                    var page = web.Load(finalUrl);

                    var lyricsNode = page.DocumentNode.SelectSingleNode("//div[@id='azmxmbanner']/preceding::div[1]");

                    foreach (var lyrics in lyricsNode.Descendants())
                    {
                        if (lyrics.Name == "#text") resultSongLyrics += lyrics.InnerText.CleanInnerHtmlAscii();
                    }

                    resultSongLyrics.TrimStart('\r', '\n');
                }
                catch (Exception)
                {
                    resultSongLyrics = DEFAULT_RETURN_VALUE;
                }
            });

            return resultSongLyrics.TrimStart('\r', '\n');
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

        private string FormatStringToGetLyrics(string _str)
        {
            if (string.IsNullOrEmpty(_str)) return "";

            string str = _str.ToLower();
            string rs = "";
            foreach (char ch in str)
            {
                if (char.IsLetter(ch)) rs += ch;
            }

            return rs;
        }
    }
}
