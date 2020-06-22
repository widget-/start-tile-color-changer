using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace StartTileColorChanger.Models
{
    class StartTileModel : INotifyPropertyChanged
    {
        public StartTileModel(string Name = "", string LnkPath = "", string ExePath = "")
        {
            m_Name = Name;
            m_LnkPath = new LnkFileModel();
            m_ExePath = ExePath;

            m_LnkPath.PropertyChanged += M_LnkPath_PropertyChanged;
        }
        public StartTileModel(StartMenuTileItem tile)
        {
            m_Name = tile.name;
            m_LnkPath.LnkPath = tile.path;
            m_ExePath = ExePath;

            m_LnkPath.PropertyChanged += M_LnkPath_PropertyChanged;
        }

        private void M_LnkPath_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
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

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                NotifyPropertyChanged("Name");
            }
        }
        public string LnkPath
        {
            get
            {
                return m_LnkPath.LnkPath;
            }
            set
            {
                m_LnkPath.LnkPath = value;
                NotifyPropertyChanged("LnkPath");
            }
        }

        public string ExePath
        {
            get
            {
                return m_ExePath;
            }
            set
            {
                m_ExePath = value;
                NotifyPropertyChanged("ExePath");
            }
        }

        public Image Icon
        {
            get
            {
                return m_Icon;
            }
            set
            {
                m_Icon = value;
                NotifyPropertyChanged("Icon");
            }
        }

        public Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                m_Color = value;
                NotifyPropertyChanged("Color");
            }
        }

        public int Row
        {
            get;
            set;
        }

        public int Column
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public string Size
        {
            get
            {
                return Width + "x" + Height;
            }
            set
            {
                Regex regex = new Regex(@"(\d+)x(\d+)");
                Match match = regex.Match(value);
                Width = Int32.Parse(match.Groups[1].Captures[0].Value);
                Height = Int32.Parse(match.Groups[2].Captures[0].Value);
            }
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
