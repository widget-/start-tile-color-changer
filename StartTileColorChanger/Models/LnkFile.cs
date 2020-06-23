using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ShellLink;
using ShellLink.Flags;

namespace StartTileColorChanger.Models
{
    class LnkFileModel : INotifyPropertyChanged
    {
        private string m_exePath;
        private string m_lnkPath;
        private string m_name;

        public LnkFileModel()
        {

        }

        public string LnkPath
        {
            get
            {
                return m_lnkPath;
            }
            set
            {
                m_lnkPath = value;
                loadExe(value);
            }
        }

        public string ExePath
        {
            get
            {
                return m_exePath;
            }
            set
            {
                m_exePath = value;
                System.Diagnostics.Debug.WriteLine($"Exepath set to {value}");
                NotifyPropertyChanged("ExePath");
            }
        }

        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
                System.Diagnostics.Debug.WriteLine($"Name set to {value}");
                NotifyPropertyChanged("Name");
            }
        }

        private async void loadExe(string path)
        {
            await Task.Run(() =>
            {
                string FullPath = Environment.ExpandEnvironmentVariables(path);
                System.Diagnostics.Debug.WriteLine($"Full path is {FullPath}");
                Shortcut lnk = Shortcut.ReadFromFile(FullPath);
                ExePath = lnk.LinkTargetIDList?.Path;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string Obj)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(Obj));
            }
        }
    }
}
