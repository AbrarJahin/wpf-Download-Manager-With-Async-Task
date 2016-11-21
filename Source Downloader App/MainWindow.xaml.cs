using System;
using System.Windows;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Source_Downloader_App
{
    public partial class MainWindow : Window
    {
        private readonly String downloadFileLocation = AppDomain.CurrentDomain.BaseDirectory + "download.txt";
        private String downloadedFileString = string.Empty;
        DateTime lastClicked;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void bDownloadSource(object sender, RoutedEventArgs e)
        {
            bDownload.IsEnabled = false;
            calculationResultTitle.Visibility   = Visibility.Visible;
            calculationResultValue.Visibility   = Visibility.Visible;
            downloadTime.Visibility             = Visibility.Visible;
            lastClicked = DateTime.Now;
            startDownloadFile(downloadUrl.Text);
        }

        private void startDownloadFile(string url)
        {
            try
            {
                downloadProgress.Visibility = Visibility.Visible;
                
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += wcDownloadStringDownloadCompleted;

                //Update UI with download progress
                webClient.DownloadProgressChanged += (s, e) =>
                {
                    downloadProgress.Value = e.ProgressPercentage;
                    downloadTime.Text = "Download Time - "+(DateTime.Now - lastClicked).TotalSeconds.ToString()+ " s";
                };

                webClient.DownloadStringAsync(new Uri(url), downloadFileLocation);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                bDownload.IsEnabled = true;
                downloadProgress.Visibility = Visibility.Hidden;
            }
        }

        void wcDownloadStringDownloadCompleted(object sender, DownloadStringCompletedEventArgs downloadArgument)
        {
            downloadedFileString = downloadArgument.Result;
            // Store the result
            File.WriteAllText(downloadFileLocation, downloadedFileString);

            //Now calculate total no of divs
            var matches = Regex.Matches(downloadedFileString.ToLower(), "</div>");
            calculationResultValue.Text = matches.Count.ToString();

            downloadProgress.Visibility = Visibility.Hidden;
            bDownload.IsEnabled = true;
            MessageBox.Show("Download Completed");
        }
    }
}
