using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using Windows.UI.ViewManagement;

namespace StartTileColorChanger.Models {
    class StartTileModel : INotifyPropertyChanged {
        public StartTileModel(string Name = "", string LnkPath = "", string ExePath = "") {
            m_Name = Name;
            m_LnkPath = new LnkFileModel();
            m_ExePath = ExePath;

            m_LnkPath.PropertyChanged += M_LnkPath_PropertyChanged;
        }

        private void M_LnkPath_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "ExePath")
                ExePath = m_LnkPath.ExePath;
            if (e.PropertyName == "Name")
                Name = m_LnkPath.Name;
        }

        private string m_Name;
        private string m_ExePath;
        private LnkFileModel m_LnkPath;
        private Color m_Color;
        private Image m_Icon;
        private bool m_Editable = false;

        private Color defaultColor = Color.FromArgb(0,0,0,0);

        public string Name {
            get {
                return m_Name;
            }
            set {
                m_Name = value;
                NotifyPropertyChanged("Name");
            }
        }
        public string LnkPath {
            get {
                return m_LnkPath.LnkPath;
            }
            set {
                m_LnkPath.LnkPath = value;
                Name = GetName(value);
                NotifyPropertyChanged("LnkPath");
            }
        }

        public string ExePath {
            get {
                return m_ExePath;
            }
            set {
                m_ExePath = value;
                Editable = GetIsEditable(value);
                Task.Run(async () => Color = await GetColor(value));
                NotifyPropertyChanged("ExePath");
            }
        }

        public Image Icon {
            get {
                return m_Icon;
            }
            set {
                m_Icon = value;
                NotifyPropertyChanged("Icon");
            }
        }

        public Color Color {
            get {
                return m_Color;
            }
            set {
                m_Color = value;
                NotifyPropertyChanged("Color");
            }
        }
        public bool Editable {
            get {
                return m_Editable;
            }
            set {
                m_Editable = value;
                NotifyPropertyChanged("Editable");
            }
        }

        public int Row {
            get;
            set;
        }

        public int Column {
            get;
            set;
        }

        public int Width {
            get;
            set;
        }

        public int Height {
            get;
            set;
        }

        public string Size {
            get {
                return Width + "x" + Height;
            }
            set {
                Regex regex = new Regex(@"(\d+)x(\d+)");
                Match match = regex.Match(value);
                Width = Int32.Parse(match.Groups[1].Captures[0].Value);
                Height = Int32.Parse(match.Groups[2].Captures[0].Value);
            }
        }

        public SolidColorBrush BackgroundColor {
            get {
                return new SolidColorBrush(Color);
            }
        }
        public int DisplayWidth {
            get {
                return Width * 50;
            }
        }
        public int DisplayHeight {
            get {
                return Height * 50;
            }
        }

        private string GetName(string lnkPath) {
            if (lnkPath != "") {
                return Path.GetFileNameWithoutExtension(LnkPath);
            } else {
                return "";
            }
        }

        private bool GetIsEditable(string exePath) {
            if (exePath == null)
                return false;
            if (exePath == "")
                return false;
            if (exePath.StartsWith(@"C:\Windows\Installer"))
                return false;

            return true;
        }

        private async Task<Color> GetColor(string exePath) {
            string Folder = Path.GetDirectoryName(exePath);
            string ExeName = Path.GetFileNameWithoutExtension(exePath);
            string ManifestPath = $"{Folder}\\{ExeName}.visualelementsmanifest.xml";

            try {
                CancellationToken Token = new CancellationToken();
                XDocument xml = await XDocument.LoadAsync(File.OpenRead(ManifestPath), LoadOptions.None, Token);

                var query = from item in xml.Root.Descendants("VisualElements")
                            select item;

                string ColorString = query.First()?.Attribute("BackgroundColor")?.Value?.ToString();
                if (ColorString != null || ColorString == "") {
                    //ColorConverter Converter = new ColorConverter();
                    Color RetColor = (Color) ColorConverter.ConvertFromString(ColorString);
                    return RetColor != Color.FromArgb(0,0,0,0) ? (Color)RetColor : getDefaultColor();
                } else {
                    return getDefaultColor();
                }
            } catch (FileNotFoundException) {
                return getDefaultColor();
            }
        }

        private Color getDefaultColor() {
            if (defaultColor.Equals(Color.FromArgb(0,0,0,0))) {
                UISettings uiSettings = new UISettings();
                Windows.UI.Color DefaultColorUI = uiSettings.GetColorValue(UIColorType.Accent);
                defaultColor = Color.FromArgb(DefaultColorUI.A, DefaultColorUI.R, DefaultColorUI.G, DefaultColorUI.B);
            }

            return defaultColor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string Obj) {
            if (PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(Obj));
            }
        }
    }
}
