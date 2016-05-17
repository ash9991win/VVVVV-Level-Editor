using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTest;

namespace VVVVVLevelEditor
{
    /// <summary>
    /// Interaction logic for SymbolTableEditor.xaml
    /// </summary>
    public partial class SymbolTableEditor : Window
    {
        //Tileinspector has all the symbols in ImageTable, we need to display the images from the image table, and the corresponding symbols from the symbol table
        public static Dictionary<ImageBrush, string> SymbolEditorTable;
        public static Dictionary<string,TextBox> SymbolEditor;
        public SymbolTableEditor()
        {
            InitializeComponent();
            SymbolEditorTable = new Dictionary<ImageBrush, string>();
            SymbolEditor = new Dictionary<string, TextBox>();
            foreach(var imk in TileInspector.ImageTable)
            {
                SymbolEditorTable.Add(imk.Value, TileInspector.SymbolTable[imk.Key]);
            }
            foreach(var sym in TileInspector.SymbolTable)
            {
                TextBox box = new TextBox();
                box.Width = 100;
                box.TextAlignment = TextAlignment.Center;
                box.Text = sym.Value;
                SymbolEditor.Add(sym.Key,box);
                
            }
            PopulateGrid();   
        }
        void PopulateGrid()
        {
            //Populate the grid in such a way that for each string in the symbol editor, 
            //draw the image on the first column, the textbox in the second
            SymbolTableEditorGrid.ColumnDefinitions.Add(new ColumnDefinition());
            SymbolTableEditorGrid.ColumnDefinitions.Add(new ColumnDefinition());
            foreach (var sym in SymbolEditor)
            {
                SymbolTableEditorGrid.RowDefinitions.Add(new RowDefinition());
                
                
            }
            int rowCount = 0;
            foreach (var Pair in TileInspector.ImageTable)
            {
                TileInspector i = new TileInspector();
                i.InspectorImage.Source = Pair.Value.ImageSource;
                i.InspectorImage.Width = i.InspectorImage.Height = MainWindow.InspectorImageHeight;
                Thickness t = i.Margin;
                t.Left = MainWindow.MarginLeft;
                t.Right = MainWindow.MarginRight;
                t.Bottom = MainWindow.MarginBottom;
                t.Top = MainWindow.MarginTop;
                i.InspectorImage.Margin = t;
                i.Id = Pair.Key;
                i.SetValue(Grid.RowProperty, rowCount);
                i.SetValue(Grid.ColumnProperty, 0);
                i.shouldMenuShow = false;
                SymbolTableEditorGrid.Children.Add(i);
                TextBox symbolBox = SymbolEditor[Pair.Key];
                symbolBox.SetValue(Grid.ColumnProperty, 1);
                symbolBox.SetValue(Grid.RowProperty, rowCount);
                SymbolTableEditorGrid.Children.Add(symbolBox);
                
                rowCount++;
            }
            //The number of rows would be the number of images in teh symbol table, the number of columns is two 
           
        }

        private void UpdateSymbolTable(object sender, RoutedEventArgs e)
        {
            foreach(var newSym in SymbolEditor)
            {
                TileInspector.SymbolTable[newSym.Key] = newSym.Value.Text;
            }
            TileInspector.SaveSymbolTable();
        }
    }
}
