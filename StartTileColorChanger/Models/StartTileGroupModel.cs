using StartTileColorChanger.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StartTileColorChanger.Models {
    class StartTileGroupModel {

        private BindingList<StartTileModel> m_Tiles = new BindingList<StartTileModel>();

        public BindingList<StartTileModel> Tiles {
            get {
                return m_Tiles;
            }
            set {
                Tiles.Clear();
                foreach (StartTileModel Tile in value) {
                    Tiles.Add(Tile);
                }
            }
        }

        public string Name {
            get;
            set;
        }

        public StartTileGroupModel() {
        }

        public StartTileGroupModel(List<StartTileModel> StartTiles) {
            Tiles.ListChanged += NotifyListChanged;
            Tiles = new BindingList<StartTileModel>(StartTiles);
        }

        public void NotifyListChanged(object sender, ListChangedEventArgs e) {
            NotifyPropertyChanged("List changed");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string Obj) {
            if (PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(Obj));
            }
        }
    }
}
