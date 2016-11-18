using System;
using System.Windows;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

        private async void startDownloadFile(string url)
        {
            //var html = new System.Net.WebClient().DownloadString(url);
            //Console.WriteLine("Done");
            WebClient client = new WebClient();

            try
            {
                client.DownloadFileAsync(new Uri(url), downloadFileLocation);
                await viewTotalDivCount(downloadFileLocation);
                MessageBox.Show("Download Completed");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private Task viewTotalDivCount(string htmlFileLocation)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(htmlFileLocation))
                {
                    // Read the stream to a string, and write the string to the console.
                    String downloadedHtml = sr.ReadToEnd();
                    var matches = Regex.Matches(downloadedHtml, "<div>");
                    calculationResultValue.Text = matches.Count.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return Task.FromResult<object>(null);
        }
    }
}
