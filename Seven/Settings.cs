using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Seven.Annotations;

namespace Seven
{
    [Serializable]
    public class Settings : ISerializable, INotifyPropertyChanged
    {
        public static string ConfigName = "launcher.cfg";

        private string _steamFolder;

        public string SteamFolder
        {
            get { return _steamFolder; }
            set
            {
                _steamFolder = value;
                OnPropertyChanged();
            }
        }

        private string _addonFolder;

        public string AddonFolder
        {
            get { return _addonFolder; }
            set
            {
                _addonFolder = value;
                OnPropertyChanged();
            }
        }

        public bool NoSplash { get; set; }
        public bool EmptyWorld { get; set; }
        public bool ShowScriptErrors { get; set; }

        public Settings()
        {
            _steamFolder = ".";
            _addonFolder = ".";
            ShowScriptErrors = false;
            EmptyWorld = false;
            NoSplash = false;
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public Settings(SerializationInfo info, StreamingContext context)
        {
            _steamFolder = info.GetString("SteamFolder");
            _addonFolder = info.GetString("AddonFolder");
            ShowScriptErrors = info.GetBoolean("ShowScriptErrors");
            EmptyWorld = info.GetBoolean("EmptyWorld");
            NoSplash = info.GetBoolean("NoSplash");           
        }

        /// <summary>
        /// Serialization data grabber
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SteamFolder", SteamFolder);
            info.AddValue("AddonFolder", AddonFolder);
            info.AddValue("ShowScriptErrors", ShowScriptErrors);
            info.AddValue("EmptyWorld", EmptyWorld);
            info.AddValue("NoSplash", NoSplash);
        }

        /// <summary>
        /// Serialization method
        /// </summary>
        public void Serialize()
        {
            using (StreamWriter writer = new StreamWriter(ConfigName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(writer, this);
            }
        }

        /// <summary>
        /// Deserialization method
        /// </summary>
        /// <returns>A Settings instance</returns>
        public static Settings Deserialize()
        {
            using (StreamReader reader = new StreamReader(ConfigName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                return serializer.Deserialize(reader) as Settings;
            }
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
