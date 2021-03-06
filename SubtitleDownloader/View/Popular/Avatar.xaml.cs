﻿using System.Windows;
using System.Windows.Media.Imaging;

namespace SubtitleDownloader
{
    /// <summary>
    /// Interaction logic for Avatar.xaml
    /// </summary>
    public partial class Avatar
    {
        public Avatar()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
           "Source", typeof(BitmapFrame), typeof(Avatar), new PropertyMetadata(default(BitmapFrame)));

        public BitmapFrame Source
        {
            get => (BitmapFrame)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(
            "DisplayName", typeof(string), typeof(Avatar), new PropertyMetadata(default(string)));

        public string DisplayName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }

        public static readonly DependencyProperty LinkProperty = DependencyProperty.Register(
            "Link", typeof(string), typeof(Avatar), new PropertyMetadata(default(string)));

        public string Link
        {
            get => (string)GetValue(LinkProperty);
            set => SetValue(LinkProperty, value);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Because the word "sex" is blocked in Iran We need to remove it to prevent error
            string display = txtDisplay.Text;
            if (txtDisplay.Text.Contains("Sex "))
            {
                display = display.Replace("Sex ", "");
            }

            MainWindow.mainWindow.getTabHome(display);
        }
    }
}
