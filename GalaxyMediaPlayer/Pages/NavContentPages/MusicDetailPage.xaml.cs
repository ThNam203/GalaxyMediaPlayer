using GalaxyMediaPlayer.Models;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System;
using System.Drawing;
using System.IO;
using System.Text;
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
        }

        private async Task<string> SetLyricsOnline()
        {
            string DEFAULT_RETURN_VALUE = "Sorry we couldn't fetch lyrics for your song.";

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
                        if (lyrics.Name == "#text") resultSongLyrics += HtmlEntity.DeEntitize(lyrics.InnerText.CleanInnerHtmlAscii());
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

        private async Task<string> GetLyricsFromFile(string filePath)
        {
            string resultLyrics = "";

            if (Path.GetExtension(filePath) == ".lrc")
            {
                await Task.Run(() =>
                {
                    try
                    {
                        var parser = new LrcParser.Parser.Lrc.LrcParser();
                        const Int32 BufferSize = 128;
                        string rawLyrics = "";
                        using (var fileStream = File.OpenRead(filePath))
                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                        {
                            String line;
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                rawLyrics += line + "\n";
                            }
                        }

                        var result = parser.Decode(rawLyrics);
                        foreach (var item in result.Lyrics)
                        {
                            resultLyrics += item.Text + "\n";
                        }
                    }
                    catch (Exception)
                    {
                        resultLyrics = "Can't fetch lyrics from your file";
                    }
                });
            }
            else await Task.Run(() =>
            {
                try
                {
                    var parser = new SubtitlesParser.Classes.Parsers.SubParser();
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        var items = parser.ParseStream(fileStream);
                        foreach (var item in items)
                        {
                            foreach (var line in item.Lines)
                            {
                                resultLyrics += line + " ";
                            }
                            resultLyrics += "\n";
                        }
                    }
                }
                catch (Exception)
                {
                    resultLyrics = "Can't fetch lyrics from your file";
                }
            });

            return resultLyrics;
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
                MainPage.Instance.SongInfoDisplayGrid.Visibility = System.Windows.Visibility.Visible;
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

        private void btnFetchLyrics_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetLyricsOnline().ContinueWith(result =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    tbSongLyrics.Text = result.Result; 
                    spFetchLyricsOptions.Visibility = System.Windows.Visibility.Collapsed;
                });
            });
        }

        private void btnOpenLyricsFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            spFetchLyricsOptions.Visibility = System.Windows.Visibility.Collapsed;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Subtitles|*.srt;*.lrc;*.vtt;*.sub;*.ssa;*.ttml";
            dialog.Multiselect = false;

            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                GetLyricsFromFile(dialog.FileName).ContinueWith(result =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        tbSongLyrics.Text = result.Result;
                    });
                });
            }
        }

        private void btnFetchLyricsOptions_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (spFetchLyricsOptions.Visibility == System.Windows.Visibility.Collapsed)
                spFetchLyricsOptions.Visibility = System.Windows.Visibility.Visible;
            else spFetchLyricsOptions.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
