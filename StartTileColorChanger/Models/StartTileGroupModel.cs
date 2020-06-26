using System.Collections.Generic;
using System.ComponentModel;

namespace StartTileColorChanger.Models {
    internal class StartTileGroupModel {
        private readonly BindingList<StartTileModel> _mTiles = new BindingList<StartTileModel>();

        public BindingList<StartTileModel> Tiles {
            get => _mTiles;
            set {
                Tiles.Clear();
                foreach (StartTileModel tile in value) {
                    Tiles.Add(tile);
                }
            }
        }

        public string Name { get; set; }

        public StartTileGroupModel() { }

        public StartTileGroupModel(IList<StartTileModel> startTiles) {
            Tiles.ListChanged += NotifyListChanged;
            Tiles = new BindingList<StartTileModel>(startTiles);
        }

        public void NotifyListChanged(object sender, ListChangedEventArgs e) {
            NotifyPropertyChanged("List changed");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string obj) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));
        }
    }
}
