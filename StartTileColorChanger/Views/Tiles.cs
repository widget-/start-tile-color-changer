using StartTileColorChanger.Actions;
using StartTileColorChanger.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace StartTileColorChanger.Views {
    class Tiles {
        private BindingList<StartTileGroupModel> m_Groups;
        public BindingList<StartTileGroupModel> Groups {
            get { return m_Groups; }
            set { m_Groups = value; }
        }

        public Tiles() {
            Initialize();
            Groups = new BindingList<StartTileGroupModel>();
        }

        public async void Initialize() {
            System.Diagnostics.Debug.WriteLine("Loading layout");
            ExportLayout layout = new ExportLayout();
            System.Diagnostics.Debug.WriteLine("Layout exporter created");
            string path = await layout.Export();
            System.Diagnostics.Debug.WriteLine($"Path is {path}");
            List<StartTileGroupModel> Groups = await layout.ParseExportedLayout(path);

            System.Diagnostics.Debug.WriteLine("Done loading tiles");

            this.Groups.Clear();
            System.Diagnostics.Debug.WriteLine("Done clearing tiles");
            foreach (StartTileGroupModel Group in Groups) {
                System.Diagnostics.Debug.WriteLine($"Adding group {Group.Name}");
                this.Groups.Add(Group);
            }
        }
    }

}
