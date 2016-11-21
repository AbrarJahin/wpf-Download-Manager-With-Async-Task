using System;
using System.Windows;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace Source_Downloader_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly String downloadFileLocation = AppDomain.CurrentDomain.BaseDirectory + "download.txt";
        private String downloadedFileString = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void bDownloadSource(object sender, RoutedEventArgs e)
        {
            bDownload.IsEnabled = false;
            calculationResultTitle.Visibility = Visibility.Visible;
            calculationResultValue.Visibility = Visibility.Visible;

            startDownloadFile(downloadUrl.Text);
            bDownload.IsEnabled = true;
        }

        private void startDownloadFile(string url)
        {
            try
            {
                downloadProgress.Visibility = Visibility.Visible;

                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcDownloadStringCompleted);

                //Update UI with download progress
                webClient.DownloadProgressChanged += (s, e) =>
                {
                    downloadProgress.Value = e.ProgressPercentage;
                };

                //webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(wcDownloadFileCompleted);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(wcDownloadFileCompleted);

                webClient.DownloadStringAsync(new Uri(url), downloadFileLocation);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void wcDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            downloadProgress.Visibility = Visibility.Hidden;
            ////Now calculate total no of divs
            //var matches = Regex.Matches(downloadedFileString.ToLower(), "</div>");
            //calculationResultValue.Text = matches.Count.ToString();

            MessageBox.Show("Download Completed");
        }

        void wcDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs downloadArgument)
        {
            downloadedFileString = downloadArgument.Result;
            // Store the result
            File.WriteAllText(downloadFileLocation, downloadedFileString);

            //Now calculate total no of divs
            var matches = Regex.Matches(downloadedFileString.ToLower(), "</div>");
            calculationResultValue.Text = matches.Count.ToString();
        }
    }
}
