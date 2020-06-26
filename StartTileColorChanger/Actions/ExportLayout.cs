using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using StartTileColorChanger.Models;
using System.ComponentModel;

namespace StartTileColorChanger.Actions {
    internal static class ExportLayout {
        public static async Task<string> Export() {
            string path = Path.GetTempFileName();

            ProcessStartInfo startInfo = new ProcessStartInfo() {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy unrestricted -Command \"Export-StartLayout \\\"{path}\\\"\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process process = Process.Start(startInfo);
            await Task.Run(() => process?.WaitForExit());

            return path;
        }

        public static async Task<BindingList<StartTileGroupModel>> ParseExportedLayout(string path) {
            XDocument xml = await Task.Run(() => XDocument.Load(path));
            XNamespace start = "http://schemas.microsoft.com/Start/2014/StartLayout";
            IEnumerable<XElement> xmlGroups = xml.Root?.Descendants(start + "Group");
            BindingList<StartTileGroupModel> groups = new BindingList<StartTileGroupModel>();

            foreach (XElement @group in xmlGroups) {
                IEnumerable<StartTileModel> tiles = from item in @group.Descendants(start + "DesktopApplicationTile")
                    select new StartTileModel {
                        Row = int.Parse(item.Attribute("Row")?.Value ?? "0"),
                        Column = int.Parse(item.Attribute("Column")?.Value ?? "0"),
                        Name = "",
                        LnkPath = item.Attribute("DesktopApplicationLinkPath")?.Value ?? "",
                        Size = item.Attribute("Size")?.Value ?? "2x2",
                    };
                groups.Add(new StartTileGroupModel {
                    Name = group.Attribute("Name")?.Value,
                    Tiles = new BindingList<StartTileModel>(new List<StartTileModel>(tiles))
                });
            }

            return groups;
        }
    }
}
