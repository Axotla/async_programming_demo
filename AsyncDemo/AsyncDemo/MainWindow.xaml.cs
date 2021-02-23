using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string URL = "https://jsonplaceholder.typicode.com/photos";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void executeSync_Click(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            RunDownloadSync();
            watch.Stop();
            long elapseMs = watch.ElapsedMilliseconds;
            results.Text += $"Total execution time: {elapseMs}";
        }

        /// <summary>
        /// Run download synchronous
        /// </summary>
        private void RunDownloadSync()
        {
            results.Text = "";
            WebClient client = new WebClient();
            List<string> websites = WebsiteList();
            foreach (string url in websites)
            {
                string response = client.DownloadString(url);
                results.Text += String.Format("Download from {0}: total {1} characters\r", url, response.Length);
            }                        
        }
        private async void executeASync_Click(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // await RunDownloadASync();
            await RunDownloadParallelASync();
            watch.Stop();
            long elapseMs = watch.ElapsedMilliseconds;
            results.Text += $"Total execution time: {elapseMs}";
        }

        /// <summary>
        /// Run download asynchronous
        /// </summary>
        private async Task RunDownloadASync()
        {
            results.Text = "";
            WebClient client = new WebClient();
            List<string> websites = WebsiteList();
            foreach (string url in websites)
            {
                string response = await Task.Run(() => client.DownloadString(url));
                results.Text += String.Format("Download from {0}: total {1} characters\r", url, response.Length);
            }
        }

        /// <summary>
        /// Run download asynchronous
        /// </summary>
        private async Task RunDownloadParallelASync()
        {
            results.Text = "";
            // List of the task
            List<Task<string>> tasks = new List<Task<string>>();
            
            List<string> websites = WebsiteList();
            foreach (string url in websites)
            {
                tasks.Add(Task.Run(() => {
                    WebClient client = new WebClient();
                    return String.Format("Download from {0}: total {1} characters\r", url, client.DownloadString(url).Length);                    
                }));                
            }
            string[] responses = await Task.WhenAll(tasks);
            foreach(string response in responses)
            {
                results.Text += response;
            }            
        }

        private List<string> WebsiteList ()
        {
            List<string> websites = new List<string>();
            websites.Add("https://www.microsoft.com");
            websites.Add("https://www.cnn.com");
            websites.Add("https://www.yahoo.com");
            websites.Add("https://www.amazon.com");
            websites.Add("https://www.ebay.com");
            websites.Add("https://www.stackoverflow.com");
            websites.Add("https://www.codeproject.com");
            return websites;
        }
    }
}
