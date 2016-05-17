﻿using System;
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

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    /// 
    public struct UndoStack
    {
        public UndoStack(int r, int c, ImageBrush b, string i)
        {
            Row = r;
            Col = c;
            brush = b;
            id = i;
        }
        public int Row;
        public int Col;
        public ImageBrush brush;
        public string id;
    }
    public partial class Tile : UserControl
    {
        public string CurrentIdToWrite;
        private ImageBrush CurrentBrushProperty;
        private ContextMenu TileMenu;
        public ImageBrush CurrentBrush
        {
            get { return CurrentBrushProperty; }
            set
            {
                CurrentBrushProperty = value; TileImage.Source = value.ImageSource;

            }
        }
        public bool isSet;
        public int RowIndex;
        public int ColIndex;
        public static ImageBrush DefaultBrush = new ImageBrush(new BitmapImage(new Uri("Resources/DefaultTile.bmp", UriKind.Relative)));
        public static string DefaultTileString = ".";
        public static Stack<UndoStack> UndoElements = new Stack<UndoStack>();
        public Tile()
            : this(0, 0)
        {

        }
        public Tile(int r, int c)
        {
            RowIndex = r;
            ColIndex = c;
            InitializeComponent();
            CurrentBrush = DefaultBrush;
            
            isSet = false;
            MouseEnter += MouseEnterHandler;
            MouseLeave += MouseExitEvent;
            TileMenu = new ContextMenu();
            MenuItem clearTileItem = new MenuItem();
            clearTileItem.Header = "Clear Tile";
            clearTileItem.Click += (object e,RoutedEventArgs re) => { SetTileData(DefaultBrush, DefaultTileString); };
            TileMenu.Items.Add(clearTileItem);
        }

        private void MouseExitEvent(object sender, MouseEventArgs e)
        {
            Tile tile = e.Source as Tile;
            if (tile != null && !isSet)
            {
                CurrentBrush = DefaultBrush;
            }
        }

        private void MouseEnterHandler(object sender, MouseEventArgs e)
        {
            Tile tile = e.Source as Tile;
            if (tile != null && !isSet)
            {
                tile.ToolTip = "Row: " + RowIndex + "\nCol: " + ColIndex;
                if (TileInspector.CurrentImage == null)
                    return;
                CurrentBrush = TileInspector.CurrentImage;
                e.Handled = true;
            }
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                if (!isSet)
                {
                    UndoActionManager.ListOfActionsInLevel.Push(new ChangedTileAction(RowIndex, ColIndex, DefaultBrush, Tile.DefaultTileString));
                }
                UndoActionManager.ListOfActionsInLevel.Push(new ChangedTileAction(RowIndex, ColIndex, CurrentBrush, CurrentIdToWrite));
                SetTileData(TileInspector.CurrentImage, TileInspector.CurrentId);
                e.Handled = true;
            }
        }

        private void TileClick(object sender, MouseButtonEventArgs e)
        {
            Tile tile = e.Source as Tile;
            if (tile != null)
            {
                if (TileInspector.CurrentImage == null)
                    return;
                if (!isSet)
                {
                    UndoActionManager.ListOfActionsInLevel.Push(new ChangedTileAction(RowIndex, ColIndex, DefaultBrush, Tile.DefaultTileString));
                }
                UndoActionManager.ListOfActionsInLevel.Push(new ChangedTileAction(RowIndex, ColIndex, CurrentBrush, CurrentIdToWrite));
                SetTileData(TileInspector.CurrentImage, TileInspector.CurrentId);
                e.Handled = true;
            }
        }
        public void SetTileData(ImageBrush brush, string Id)
        {
            CurrentBrush = brush;
            CurrentIdToWrite = Id;
            isSet = true;
        }

        private void TileOptions(object sender, MouseButtonEventArgs e)
        {
            if(isSet)
            {
                TileMenu.IsOpen = true;
            }
        }
    }
}
