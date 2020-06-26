using StartTileColorChanger.Actions;
using StartTileColorChanger.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace StartTileColorChanger.Views {
    internal class Tiles {
        private BindingList<StartTileGroupModel> _mGroups;

        public BindingList<StartTileGroupModel> Groups {
            get => _mGroups;
            set => _mGroups = value;
        }

        public Tiles() {
            Groups = new BindingList<StartTileGroupModel>();
            Initialize();
        }

        public async Task Initialize() {
            System.Diagnostics.Debug.WriteLine("Loading layout");
            string path = await ExportLayout.Export();
            System.Diagnostics.Debug.WriteLine($"Path is {path}");
            BindingList<StartTileGroupModel> groups = await ExportLayout.ParseExportedLayout(path);

            System.Diagnostics.Debug.WriteLine("Done loading tiles");

            Groups.Clear();
            System.Diagnostics.Debug.WriteLine("Done clearing tiles");
            foreach (StartTileGroupModel group in groups) {
                System.Diagnostics.Debug.WriteLine($"Adding group {group.Name}");
                Groups.Add(group);
            }
        }
    }
}
