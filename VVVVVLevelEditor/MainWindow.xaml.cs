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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using Xceed.Wpf.Toolkit;
using  VVVVVLevelEditor;
namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public static int MarginLeft = 10;
        public static int MarginRight = 10;
        public static int MarginTop = 10;
        public static int MarginBottom = 10;
        public static int TileMargin = 50;
        public static int InspectorImageHeight = 50;
        public static int InspectorImageWidth = 50;
        public static int TileWidth = 50;
        public static int TileHeight = 50;
        public static int NumberOfRows;
        public static int NumberOfColumns;
        public static Tile[,] Tiles;
        double windowWidth;
        double windowHeight;
        public static RoutedCommand UndoCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        static Grid TileGrid;
        public static bool isFileLoaded;
        public static string loadedFilePath;
        public MainWindow()
        {

            TileGrid = new Grid();
            InitializeComponent();
            windowHeight = this.Height;
            windowWidth = this.Width;
            TileInspector.PopulateTables();


            TileScroller.Width = InspectorImageWidth * 2;
            EditorPanel.Width = TileScroller.Width - 10;
            NumberOfColumns = (int)(this.Width - EditorPanel.Width - TileScroller.Width) / TileWidth;
            NumberOfRows = (int)(this.Height) / TileHeight - 1;
            TileMapScroller.Width = this.Width;
            GridWrap.Children.Add(TileGrid);
            foreach (var Pair in TileInspector.ImageTable)
            {
                TileInspector i = new TileInspector();
                i.InspectorImage.Source = Pair.Value.ImageSource;
                i.InspectorImage.Width = i.InspectorImage.Height = InspectorImageHeight;
                Thickness t = i.Margin;
                t.Left = MarginLeft;
                t.Right = MarginRight;
                t.Bottom = MarginBottom;
                t.Top = MarginTop;
                i.InspectorImage.Margin = t;
                i.Id = Pair.Key;
                EditorPanel.Children.Add(i);
            }
            PopulateTiles();
            UndoActionManager.window = this;
            UndoCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(UndoCommand, UndoCommandHandler));
            this.CommandBindings.Add(new CommandBinding(SaveCommand, SaveCommandHandler));
        }
        public static void PopulateTiles()
        {
            TileGrid.RowDefinitions.Clear();
            TileGrid.ColumnDefinitions.Clear();
            TileGrid.Children.Clear();
            Tiles = new Tile[NumberOfRows, NumberOfColumns];
            for (int i = 0; i <NumberOfRows; i++)
            {
                for (int j = 0; j <NumberOfColumns; j++)
                {
                    Tiles[i, j] = new Tile(i, j);
                    var currentTile = Tiles[i, j];
                    currentTile.Width = TileWidth ;
                    currentTile.Height = TileHeight;
                    currentTile.SetValue(Grid.ColumnProperty, j);
                    currentTile.SetValue(Grid.RowProperty, i);
                    ColumnDefinition col = new ColumnDefinition();
                    col.Width = new GridLength(TileWidth);
                    TileGrid.ColumnDefinitions.Add(col);
                    TileGrid.Children.Add(currentTile);
                }
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(TileHeight);
                TileGrid.RowDefinitions.Add(row);
            }

        }
        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            SaveLevel();
        }

        private void UndoCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            UndoActionManager.Undo();
        }

        private void ClearLevelHandler(object sender, RoutedEventArgs e)
        {
            UndoActionManager.ListOfActionsInLevel.Push(new ClearLevelAction(Tiles));
            ClearLevel();
        }
        public void ClearLevel()
        {
            if (Tiles != null)
            {
                foreach (var tile in Tiles)
                {
                    tile.CurrentBrush = Tile.DefaultBrush;
                    tile.isSet = false;
                }
            }
        }
        private void LoadLevelHandler(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileLoader = new OpenFileDialog();
            fileLoader.Filter = "Text Files(*.txt) | *.txt | Level Files(*.lvl) | *.lvl | All Files (*.*) | *.*";
            if (fileLoader.ShowDialog() == true)
            {
                ClearLevel();
                LevelLoader.LoadLevel(fileLoader.FileName);
                isFileLoaded = true;
                loadedFilePath = fileLoader.FileName;
            }
        }

        private void SaveLevelHandler(object sender, RoutedEventArgs e)
        {
            SaveLevel();
        }
        void SaveLevel()
        {
           
            SaveFileDialog filesaver = new SaveFileDialog();
            filesaver.Filter = "Text File (*.txt) | *.txt | Level Files (*.lvl) | *.lvl";
            StringBuilder outputString = new StringBuilder();
            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfColumns; j++)
                {
                    var tile = Tiles[i, j];
                    if (tile.CurrentBrush == Tile.DefaultBrush || tile.CurrentIdToWrite == null)
                    {
                        outputString.Append(Tile.DefaultTileString);
                    }
                    else
                    {
                        outputString.Append(tile.CurrentIdToWrite);

                    }
                }
                outputString.Append(Environment.NewLine);
            }
            StreamWriter write = null;
            if(!isFileLoaded)
            {
                if (filesaver.ShowDialog() == true)
                {
                     write = new StreamWriter(filesaver.FileName);
                }
            }
            else
            {
                write = new StreamWriter(loadedFilePath);
            }
            if (write != null)
            {
                write.Write(outputString.ToString());
                write.Close();
            }
        }
        private void UndoHandler(object sender, RoutedEventArgs e)
        {

            UndoActionManager.Undo();
        }

        private void WindowResize(object sender, SizeChangedEventArgs e)
        {
            TileMapScroller.Width = this.Width - EditorPanel.Width - TileScroller.Width;

        }

        private void OpenRowColumnChanger(object sender, RoutedEventArgs e)
        {
            ShowRowColumnChanger();
        }
   
        private void ShowRowColumnChanger()
        {
            Window1 rowColChanger = new Window1();
            rowColChanger.ShowInTaskbar = false;
            rowColChanger.Owner = this;
            rowColChanger.Show();
        }

        private void OpenSymbolTableEditor(object sender, RoutedEventArgs e)
        {
            SymbolTableEditor editor = new SymbolTableEditor();
            editor.ShowInTaskbar = false;
            editor.Owner = this;
            editor.Show();
        }

        private void OnFileDrop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try {
                    string[] filePath =(string[]) e.Data.GetData(DataFormats.FileDrop);
                    ClearLevel();
                    LevelLoader.LoadLevel(filePath[0]);
                isFileLoaded = true;
                    loadedFilePath = filePath[0];
                }
                catch(Exception ex)
                {
                    System.Windows.MessageBox.Show("Invalid file");
                }
            }
        }

        private void SaveAsHandler(object sender, RoutedEventArgs e)
        {
            isFileLoaded = false;
            SaveLevel();
        }
    }

}
