﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using StartTileColorChanger.Models;
using Microsoft.VisualBasic.CompilerServices;

namespace StartTileColorChanger.Actions
{
    class ExportLayout
    {
        public ExportLayout()
        {}

        public async Task<string> Export()
        {
            string path = Path.GetTempFileName();

            var startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy unrestricted -Command \"Export-StartLayout \\\"{path}\\\"\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            };
            Process process = Process.Start(startInfo);
            process.WaitForExit();

            return path;
        }

        public async Task<List<StartTileModel>> ParseExportedLayout(string path)
        {
            XDocument xml = await Task.Run(() => XDocument.Load(path));
            XNamespace start = "http://schemas.microsoft.com/Start/2014/StartLayout";
            var query = from item in xml.Root.Descendants(start+"DesktopApplicationTile") select
                        new StartTileModel() {
                            Row = int.Parse(item.Attribute("Row").Value),
                            Column = int.Parse(item.Attribute("Column").Value),
                            Name = "",
                            LnkPath = item.Attribute("DesktopApplicationLinkPath")?.Value,
                            Size = item.Attribute("Size").Value,
                        };

            return new List<StartTileModel>(query);
        }
    }
}