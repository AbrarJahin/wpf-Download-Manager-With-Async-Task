using System;
using System.Windows;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;

namespace Source_Downloader_App
{
    public partial class MainWindow : Window
    {
        private readonly String downloadFileLocation = AppDomain.CurrentDomain.BaseDirectory + "download.txt";
        private String downloadedFileString = string.Empty;
        private DateTime lastClicked;
        private Timer timer = new Timer();

        public MainWindow()
        {
            InitializeComponent();
            // Set the Interval to 100 ms
            timer.Interval = 100;

            // Hook up the Elapsed event for the timer
            timer.Elapsed += updateDownloadTimer;
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
                downloadProgress.Value = 0;
                downloadProgress.Visibility = Visibility.Visible;

                // Start the timer.
                timer.Enabled = true;

                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += wcDownloadStringDownloadCompleted;

                //Update UI with download progress
                webClient.DownloadProgressChanged += wcDownloadProgressChanged;

                webClient.DownloadStringAsync(new Uri(url), downloadFileLocation);
            }
            catch (Exception e)
            {
                // Stop the timer.
                timer.Enabled = false;

                MessageBox.Show(e.ToString());
                bDownload.IsEnabled = true;
                downloadProgress.Visibility = Visibility.Hidden;
            }
        }

        private void updateDownloadTimer(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                downloadTime.Text = "Download Time - " + (DateTime.Now - lastClicked).TotalSeconds.ToString() + " s";
            });
        }

        private void wcDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadProgress.Value = e.ProgressPercentage;
            //downloadTime.Text = "Download Time - "+(DateTime.Now - lastClicked).TotalSeconds.ToString()+ " s";
        }

        void wcDownloadStringDownloadCompleted(object sender, DownloadStringCompletedEventArgs downloadArgument)
        {
            // Stop the timer.
            timer.Enabled = false;

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
