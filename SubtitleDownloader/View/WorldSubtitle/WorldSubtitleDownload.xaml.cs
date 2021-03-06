﻿using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SubtitleDownloader
{
    /// <summary>
    /// Interaction logic for WorldSubtitleDownload.xaml
    /// </summary>
    public partial class WorldSubtitleDownload : INotifyPropertyChanged
    {
        internal static WorldSubtitleDownload WorldSubtitle;
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        private ObservableCollection<WorldModel> _DataList;
        public ObservableCollection<WorldModel> DataList
        {
            get => _DataList;
            set
            {
                if (_DataList != value)
                {
                    _DataList = value;
                    NotifyPropertyChanged("DataList");
                }
            }
        }

        public static string Link = string.Empty;

        public WorldSubtitleDownload()
        {
            InitializeComponent();
            DataContext = WorldSubtitle = this;
        }

        public async void Load()
        {
            busyIndicator.IsBusy = true;
            DataList = new ObservableCollection<WorldModel>();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = await web.LoadFromWebAsync(Link);

            foreach (HtmlNode item in doc.DocumentNode.SelectNodes(".//li"))
            {
                try
                {
                    string displayName = item.SelectSingleNode(".//div[@class='new-link-1']").InnerText;
                    string status = item.SelectSingleNode(".//div[@class='new-link-2']").InnerText;
                    string link = item.SelectSingleNode(".//a").Attributes["href"].Value;
                    if (status.Contains("&nbsp;"))
                    {
                        status = status.Replace("&nbsp;", "");
                    }

                    DataList.Add(new WorldModel { DisplayName = displayName, Status = status, Link = link });
                }
                catch { }
            }
            busyIndicator.IsBusy = false;
        }
    }
}
