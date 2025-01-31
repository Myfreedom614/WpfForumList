﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfForumList
{
    /// <summary>
    /// Interaction logic for DataDownload.xaml
    /// </summary>
    public partial class DataDownload : Window
    {
        string baseDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public DataDownload()
        {
            InitializeComponent();
        }

        public DataDownload(bool p1, bool p2) : base()
        {
            InitializeComponent();

            if(p1 ==true && p2 == false)
            {
                grid.RowDefinitions.RemoveAt(1);
            }

            if (p1 == false && p2 == true)
            {
                grid.RowDefinitions.RemoveAt(1);
                Grid.SetRow(lbTechNet, 0);
                Grid.SetRow(btnTechNet, 0);
                Grid.SetRow(pbTechNet, 0);
            }

            if (p1 == false)
            { 
                btnMSDN.IsEnabled = false;
                lbMSDN.Visibility = Visibility.Collapsed;
                btnMSDN.Visibility = Visibility.Collapsed;
                pbMSDN.Visibility = Visibility.Collapsed;
            }

            if (p2 == false)
            { 
                btnTechNet.IsEnabled = false;
                lbTechNet.Visibility = Visibility.Collapsed;
                btnTechNet.Visibility = Visibility.Collapsed;
                pbTechNet.Visibility = Visibility.Collapsed;
            }
        }

        private void btnMSDN_Click(object sender, RoutedEventArgs e)
        {
            btnMSDN.IsEnabled = false;
            
            try
            {
                string url = "http://openszone.com/ForumData/MSDNForum.xml";
                using(WebClient client = new WebClient())
                {
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_MSDNDownloadFileCompleted);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_MSDNDownloadProgressChanged);
                    client.DownloadFileAsync(new Uri(url), baseDir + "\\MSDNForum.xml");
                }
            }
            catch(Exception ex)
            {
                btnMSDN.IsEnabled = true;
                MessageBox.Show(ex.ToString());
            }
        }

        int msdn_increament = 0;

        void client_MSDNDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            pbMSDN.Value=50;

            string url = "http://openszone.com/ForumData/MSDNCHSForum.xml";
            using (WebClient client = new WebClient())
            {
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_MSDNCHSDownloadFileCompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_MSDNDownloadProgressChanged);
                client.DownloadFileAsync(new Uri(url), baseDir + "\\MSDNCHSForum.xml");
            }
        }

        void client_MSDNCHSDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            pbMSDN.Value = 100;
            btnMSDN.Visibility = Visibility.Collapsed;
        }

        void client_MSDNDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            msdn_increament = msdn_increament + 20;
            pbMSDN.Value=msdn_increament;
        }

        int technet_increament = 0;

        void client_TechNetDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            pbTechNet.Value = 50;

            string url = "http://openszone.com/ForumData/TechNetCHSForum.xml";
            using (WebClient client = new WebClient())
            {
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_TechNetCHSDownloadFileCompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_TechNetDownloadProgressChanged);
                client.DownloadFileAsync(new Uri(url), baseDir + "\\TechNetCHSForum.xml");
            }
        }

        void client_TechNetCHSDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            pbTechNet.Value = 100;
            btnTechNet.Visibility = Visibility.Collapsed;
        }

        void client_TechNetDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            technet_increament = technet_increament + 20;
            pbTechNet.Value = technet_increament;
        }

        private void btnTechNet_Click(object sender, RoutedEventArgs e)
        {
            btnTechNet.IsEnabled = false;
            
            try
            {
                string url = "http://openszone.com/ForumData/TechNetForum.xml";
                using (WebClient client = new WebClient())
                {
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_TechNetDownloadFileCompleted);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_TechNetDownloadProgressChanged);
                    client.DownloadFileAsync(new Uri(url), baseDir + "\\TechNetForum.xml");
                }
            }
            catch (Exception ex)
            {
                btnTechNet.IsEnabled = true;
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
