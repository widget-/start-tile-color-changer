using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using Windows.UI.ViewManagement;

namespace StartTileColorChanger.Models {
    internal class StartTileModel : INotifyPropertyChanged {
        public StartTileModel(string name = "", string exePath = "") {
            _mName = name;
            _mLnkPath = new LnkFileModel();
            _mExePath = exePath;

            _mLnkPath.PropertyChanged += M_LnkPath_PropertyChanged;
        }

        private void M_LnkPath_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "ExePath")
                ExePath = _mLnkPath.ExePath;
            if (e.PropertyName == "Name")
                Name = _mLnkPath.Name;
        }

        private string _mName;
        private string _mExePath;
        private LnkFileModel _mLnkPath;
        private Color _mColor;
        private Image _mIcon;
        private bool _mEditable;

        private Color _defaultColor = Color.FromArgb(0, 0, 0, 0);
        private const double DisabledTileContrast = 0.6;

        public string Name {
            get => _mName;
            set {
                _mName = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string LnkPath {
            get => _mLnkPath.LnkPath;
            set {
                _mLnkPath.LnkPath = value;
                Name = GetName(value);
                NotifyPropertyChanged("LnkPath");
            }
        }

        public string ExePath {
            get => _mExePath;
            set {
                _mExePath = value;
                Editable = GetIsEditable(value);
                Task.Run(async () => BackgroundColor = await GetColor(value));
                NotifyPropertyChanged("ExePath");
            }
        }

        public Image Icon {
            get => _mIcon;
            set {
                _mIcon = value;
                NotifyPropertyChanged("Icon");
            }
        }

        public Color BackgroundColor {
            get => _mColor;
            set {
                _mColor = value;
                NotifyPropertyChanged("Color");
            }
        }

        public Brush ForegroundColor {
            get {
                Color fgColor = Editable ? Color.FromArgb(255, 255, 255, 255) : Color.FromArgb(255, 192, 192, 192);
                return new SolidColorBrush(fgColor);
            }
        }

        public bool Editable {
            get => _mEditable;
            set {
                _mEditable = value;
                NotifyPropertyChanged("Editable");
            }
        }

        public string EditableStr => _mEditable ? "Editable" : "Not editable";

        public int Row { get; set; }

        public int Column { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Size {
            get => Width + "x" + Height;
            set {
                Regex sizeStringRegex = new Regex(@"(\d+)x(\d+)");
                Match regexMatches = sizeStringRegex.Match(value);
                Width = int.Parse(regexMatches.Groups[1].Captures[0].Value);
                Height = int.Parse(regexMatches.Groups[2].Captures[0].Value);
            }
        }

        public SolidColorBrush BackgroundColorBrush =>
            !Editable
                ? new SolidColorBrush(AdjustContrast(BackgroundColor, DisabledTileContrast))
                : new SolidColorBrush(BackgroundColor);

        private static string GetName(string lnkPath) {
            try {
                return Path.GetFileNameWithoutExtension(lnkPath);
            } catch (ArgumentException) {
                return "";
            }
        }

        private static bool GetIsEditable(string exePath) {
            if (exePath == null)
                return false;
            if (exePath == "")
                return false;
            if (exePath.StartsWith(@"C:\Windows\Installer"))
                return false;

            return true;
        }

        private static Color AdjustContrast(Color color, double contrast) {
            Color retColor = Color.FromArgb(color.A, color.R, color.G, color.B);
            retColor.R = (byte) (color.R * contrast + 255 * contrast / 2);
            retColor.G = (byte) (color.G * contrast + 255 * contrast / 2);
            retColor.B = (byte) (color.B * contrast + 255 * contrast / 2);
            return retColor;
        }

        private async Task<Color> GetColor(string exePath) {
            string folder = Path.GetDirectoryName(exePath);
            string exeName = Path.GetFileNameWithoutExtension(exePath);
            string manifestPath = $"{folder}\\{exeName}.visualelementsmanifest.xml";

            try {
                CancellationToken token = new CancellationToken();
                XDocument xml = await XDocument.LoadAsync(File.OpenRead(manifestPath), LoadOptions.None, token);

                var query = from item in xml.Root?.Descendants("VisualElements")
                    select item;

                string colorString = query.First()?.Attribute("BackgroundColor")?.Value;
                try {
                    return (Color) ColorConverter.ConvertFromString(colorString);
                } catch (Exception e) when (e is NullReferenceException || e is FormatException) {
                    return GetDefaultColor();
                }
            } catch (FileNotFoundException e) {
                return GetDefaultColor();
            }
        }

        private Color GetDefaultColor() {
            if (_defaultColor.Equals(Color.FromArgb(0, 0, 0, 0))) {
                UISettings uiSettings = new UISettings();
                Windows.UI.Color defaultColorUi = uiSettings.GetColorValue(UIColorType.Accent);
                _defaultColor = Color.FromArgb(defaultColorUi.A, defaultColorUi.R, defaultColorUi.G, defaultColorUi.B);
            }

            return _defaultColor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string obj) {
            if (PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(obj));
            }
        }
    }
}
