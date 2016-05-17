using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest
{
    class LevelLoader
    {
        public static void LoadLevel(string path)
        {
            StreamReader reader = new StreamReader(path);
            MainWindow.NumberOfRows = File.ReadLines(path).Count();
            MainWindow.NumberOfColumns = reader.ReadLine().Length;
            MainWindow.PopulateTiles();
            StreamReader readerLevel = new StreamReader(path);
            string fileData = readerLevel.ReadToEnd();
            int RowIndex = 0;
            int ColIndex = 0;
            foreach(var ch in fileData)
            {
                if(ch.ToString() == Environment.NewLine || ch.ToString() == "\n")
                {
                    RowIndex++;
                    ColIndex = 0;
                    continue;
                }
                if (RowIndex > MainWindow.NumberOfRows)
                {
                    RowIndex = 0;
                }
                if(ColIndex > MainWindow.NumberOfColumns)
                {
                    ColIndex = 0;
                }
                if ( TileInspector.InverseSymbolTable.ContainsKey(ch.ToString()))
                {
                    string ImageName = TileInspector.InverseSymbolTable[ch.ToString()];
                    MainWindow.Tiles[RowIndex, ColIndex].SetTileData(TileInspector.ImageTable[ImageName], TileInspector.SymbolTable[ImageName]);
                }
   
                    ++ColIndex;
            }
        }
    }
}
