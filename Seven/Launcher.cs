using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Seven.Annotations;

namespace Seven
{
    public class Launcher : INotifyPropertyChanged
    {
        public ObservableCollection<Addon> Addons { get; set; }

        public string Modline
        {
            get { return _modline; }
            set { _modline = value; OnPropertyChanged(); }
        }

        private string _modline;
        private readonly Settings _settings;

        public Launcher(Settings settings)
        {
            _settings = settings;

            Addons = new ObservableCollection<Addon>();
            UpdateAddons();
            HandleCheckBoxChanged();    // really just a way to update the modline at the start
        }

        public void ButtonRefreshAddons()
        {
            UpdateAddons();
            HandleCheckBoxChanged();
        }

        public void ButtonLaunchGame()
        {
            if (!File.Exists(String.Format("{0}\\steam.exe", _settings.SteamFolder)))
            {
                MessageBox.Show("You haven't set a valid steam folder yet");
            }
            else
            {
                string launch = String.Format("{0}\\steam.exe", _settings.SteamFolder);
                string parameters = String.Format("-applaunch 107410 {0}", Modline);
                Process.Start(launch, parameters);
            }
        }

        public void HandleCheckBoxChanged()
        {
            Modline = String.Format("{1} {0}", BuildParameterLine(), BuildModLine());
        }

        public string BuildModLine()
        {
            string modline = "\"-mod=";
            foreach (Addon addon in Addons)
            {
                if (addon.Active)
                    modline += String.Format("{0};", addon.Name);
            }
            if (modline != "\"-mod=")   // when mods were added, cap it off with a trailing "
            {
                modline = modline.Substring(0, modline.Length - 1);
                modline += "\"";
            }
            else // when no mods were added, remove the whole modline
            {
                modline = "";
            }
            return modline;
        }

        public string BuildParameterLine()
        {
            string parameters = "";
            if (_settings.NoSplash)
                parameters += "\"-nosplash\" ";
            if (_settings.EmptyWorld)
                parameters += "\"-world=empty\" ";
            if (_settings.ShowScriptErrors)
                parameters += "\"-showScriptErrors\" ";
            // remove last space as it is not needed when a parameter list actually exists
            if (parameters != "")
                parameters = parameters.Substring(0, parameters.Length - 1);
            return parameters;
        }

        public void UpdateAddons()
        {
            DirectoryInfo addonFolder = new DirectoryInfo(_settings.AddonFolder);
            List<DirectoryInfo> candidates = addonFolder.EnumerateDirectories().ToList();

            Addons.Clear();

            foreach (DirectoryInfo folder in candidates)
            {
                foreach (DirectoryInfo subfolder in folder.EnumerateDirectories())
                {
                    if (subfolder.Name.ToLower() == "addons" && folder.Name.ToLower().StartsWith("@"))
                    {
                        Addons.Add(new Addon(folder.Name, folder.FullName));
                        break;
                    }
                }
            }
        }

        public void ButtonSetSteamFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (File.Exists(String.Format("{0}\\steam.exe", dialog.SelectedPath)))
                {
                    _settings.SteamFolder = dialog.SelectedPath;
                }
                else
                {
                    MessageBox.Show("The selected folder is not a steam folder. Please try again");
                }
            }
        }

        public void ButtonSetAddonFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _settings.AddonFolder = dialog.SelectedPath;
            }
            UpdateAddons();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
