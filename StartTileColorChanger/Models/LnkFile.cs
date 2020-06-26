using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using ShellLink;

namespace StartTileColorChanger.Models {
    internal class LnkFileModel : INotifyPropertyChanged {
        private string _mExePath;
        private string _mLnkPath;
        private string _mName;

        public string LnkPath {
            get => _mLnkPath;
            set {
                _mLnkPath = value;
                LoadExePath(value);
            }
        }

        public string ExePath {
            get => _mExePath;
            set {
                _mExePath = value;
                System.Diagnostics.Debug.WriteLine($"Exepath set to {value}");
                NotifyPropertyChanged("ExePath");
            }
        }

        public string Name {
            get => _mName;
            set {
                _mName = value;
                System.Diagnostics.Debug.WriteLine($"Name set to {value}");
                NotifyPropertyChanged("Name");
            }
        }

        private async void LoadExePath(string path) {
            await Task.Run(() => {
                string fullPath = Environment.ExpandEnvironmentVariables(path);
                System.Diagnostics.Debug.WriteLine($"Full path is {fullPath}");
                try {
                    Shortcut lnk = Shortcut.ReadFromFile(fullPath);
                    ExePath = lnk.LinkTargetIDList?.Path;
                } catch (FileNotFoundException) { }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string obj) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));
        }
    }
}
