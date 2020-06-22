using StartTileColorChanger.Actions;
using StartTileColorChanger.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace StartTileColorChanger.Views
{
    class Tiles
    {
        private BindingList<StartTileModel> m_Tiles;
        public BindingList<StartTileModel> Items
        {
            get { return m_Tiles; }
            set { m_Tiles = value; }
        }

        public Tiles()
        {
            Items = new BindingList<StartTileModel>();
            //    Items.Add(new StartTileModel() {
            //        Column = 0,
            //        Row = 0,
            //        Height = 2,
            //        Width = 2,
            //        Name = "Foo",
            //        Color = Color.SkyBlue
            //    });

            //    Items.Add(new StartTileModel() {
            //        Column = 2,
            //        Row = 0,
            //        Height = 2,
            //        Width = 2,
            //        Name = "Bar",
            //        Color = Color.Red
            //    });

            //    Items.Add(new StartTileModel() {
            //        Column = 0,
            //        Row = 2,
            //        Height = 2,
            //        Width = 2,
            //        Name = "Baz",
            //        Color = Color.Green
            //    });
            //}

            Initialize();
        }

        public async void Initialize()
        {
            System.Diagnostics.Debug.WriteLine("Loading layout");
            ExportLayout layout = new ExportLayout();
            System.Diagnostics.Debug.WriteLine("Layout exporter created");
            string path = await layout.Export();
            System.Diagnostics.Debug.WriteLine($"Path is {path}");
            List<StartTileModel> tiles = await layout.ParseExportedLayout(path);

            System.Diagnostics.Debug.WriteLine("Done loading tiles");

            Items.Clear();
            System.Diagnostics.Debug.WriteLine("Done clearing tiles");
            foreach (StartTileModel tile in tiles)
            {
                System.Diagnostics.Debug.WriteLine($"Adding tile {tile.Name}");
                Items.Add(tile);
            }
        }
    }

}
