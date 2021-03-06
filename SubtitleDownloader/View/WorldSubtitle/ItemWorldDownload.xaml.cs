﻿using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.ComponentModel;
using System.Net;
using System.Windows;

namespace SubtitleDownloader
{
    /// <summary>
    /// Interaction logic for ItemWorldDownload.xaml
    /// </summary>
    public partial class ItemWorldDownload
    {
        private readonly WebClient client = new WebClient();
        private string subName = string.Empty;
        private string location = string.Empty;

        public ItemWorldDownload()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(
            "DisplayName", typeof(string), typeof(ItemWorldDownload), new PropertyMetadata(default(string)));

        public string DisplayName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            "Status", typeof(string), typeof(ItemWorldDownload), new PropertyMetadata(default(string)));

        public string Status
        {
            get => (string)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public static readonly DependencyProperty LinkProperty = DependencyProperty.Register(
            "Link", typeof(string), typeof(ItemWorldDownload), new PropertyMetadata(default(string)));

        public string Link
        {
            get => (string)GetValue(LinkProperty);
            set => SetValue(LinkProperty, value);
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            tgDownload.Progress = int.Parse(Math.Truncate(percentage).ToString());
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            tgDownload.IsChecked = false;
            tgDownload.IsEnabled = true;
            tgDownload.Progress = 0;
            tgDownload.Content = Properties.Langs.Lang.OpenFolder;
            if (GlobalData.Config.IsShowNotification)
            {
                Growl.InfoGlobal(new GrowlInfo
                {
                    CancelStr = Properties.Langs.Lang.Cancel,
                    ConfirmStr = Properties.Langs.Lang.OpenFolder,
                    WaitTime = 5000,
                    Message = string.Format(Properties.Langs.Lang.DownloadCompleted, subName),
                    ActionBeforeClose = isConfirmed =>
                    {
                        if (!isConfirmed)
                        {
                            return true;
                        }

                        System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + location + "\"");
                        return true;
                    }
                });
            }
        }

        private void tgDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)tgDownload.Content != Properties.Langs.Lang.OpenFolder)
                {
                    tgDownload.IsEnabled = false;
                    tgDownload.IsChecked = true;
                    tgDownload.Content = Properties.Langs.Lang.Downloading;
                    tgDownload.Progress = 0;
                    subName = System.IO.Path.GetFileNameWithoutExtension(Link);


                    if (!string.IsNullOrEmpty(App.WindowsContextMenuArgument[1]))
                    {
                        location = App.WindowsContextMenuArgument[2] + System.IO.Path.GetFileName(Link);
                    }
                    else
                    {
                        location = GlobalData.Config.StoreLocation + System.IO.Path.GetFileName(Link);
                    }
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    client.DownloadFileAsync(new Uri(Link), location);
                }
                else
                {
                    System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + location + "\"");
                }
            }
            catch (UnauthorizedAccessException)
            {
                HandyControl.Controls.MessageBox.Error(Properties.Langs.Lang.AccessError, Properties.Langs.Lang.AccessErrorTitle);
            }
            catch (NotSupportedException) { }
        }
    }
}
