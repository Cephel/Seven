using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Seven
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Settings Settings { get; set; }
        public Launcher Launcher { get; set; }
        public Downloader Downloader { get; set; }

        public MainWindow()
        {
            Init();
            InitializeComponent();
        }

        public void Init()
        {
            Settings = File.Exists(Settings.ConfigName) ? Settings.Deserialize() : new Settings();
            Launcher = new Launcher(Settings);
            Downloader = new Downloader();
        }

        #region Button Handlers

        private void ButtonSaveConfig(object sender, RoutedEventArgs e)
        {
            Settings.Serialize();
        }

        private void ButtonLoadConfig(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Settings.ConfigName))
                Settings = Settings.Deserialize();
            else
                MessageBox.Show("No config found");
        }

        private void ButtonSetSteamFolder(object sender, RoutedEventArgs e)
        {
            Launcher.ButtonSetSteamFolder();
        }

        private void ButtonSetAddonFolder(object sender, RoutedEventArgs e)
        {
            Launcher.ButtonSetAddonFolder();
        }

        private void ButtonCheckboxChecked(object sender, RoutedEventArgs e)
        {
            Launcher.HandleCheckBoxChanged();
        }

        private void ButtonCheckboxUnchecked(object sender, RoutedEventArgs e)
        {
            Launcher.HandleCheckBoxChanged();
        }

        private void ButtonLaunchGame(object sender, RoutedEventArgs e)
        {
            Launcher.ButtonLaunchGame();
        }

        private void ButtonStartSync(object sender, RoutedEventArgs e)
        {
            Downloader.ButtonStartSync();
        }
        private void ButtonRefresh(object sender, RoutedEventArgs e)
        {
            Launcher.ButtonRefreshAddons();
        }

        #endregion
    }
}
