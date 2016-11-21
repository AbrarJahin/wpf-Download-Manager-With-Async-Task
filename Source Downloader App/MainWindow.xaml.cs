using System;
using System.Windows;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

namespace Source_Downloader_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly String downloadFileLocation = AppDomain.CurrentDomain.BaseDirectory + "download.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void bDownloadSource(object sender, RoutedEventArgs e)
        {
            downloadProgress.Visibility = Visibility.Visible;
            calculationResultTitle.Visibility = Visibility.Visible;
            calculationResultValue.Visibility = Visibility.Visible;

            startDownloadFile(downloadUrl.Text);

            downloadProgress.Visibility = Visibility.Hidden;
        }

        private void startDownloadFile(string url)
        {
            var downloadedFileString = string.Empty;
            try
            {
                //client.DownloadFileAsync(new Uri(url), downloadFileLocation);
                using (var webClient = new WebClient())
                {
                    downloadedFileString = webClient.DownloadString(url);
                    File.WriteAllText(downloadFileLocation, downloadedFileString);
                }

                //Now calculate total no of divs
                using (StreamReader sr = new StreamReader(downloadFileLocation))
                {
                    var matches = Regex.Matches(downloadedFileString.ToLower(), "</div>");
                    calculationResultValue.Text = matches.Count.ToString();
                }

                MessageBox.Show("Download Completed");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
