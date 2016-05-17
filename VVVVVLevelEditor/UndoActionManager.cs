using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfTest
{
    class UndoActionManager
    {
        public static Stack<ActionPerformed> ListOfActionsInLevel = new Stack<ActionPerformed>();
        public static MainWindow window;
        UndoActionManager(MainWindow w)
        {
            window = w;
        }
        public static void Undo()
        {
            if (ListOfActionsInLevel.Count == 0)
                return;
            ListOfActionsInLevel.Pop().UndoAction(window);
        }
    }
    abstract class  ActionPerformed 
    {
        public abstract void UndoAction(MainWindow window);
    }
    class ClearLevelAction : ActionPerformed
    {
        //This action occurs when the leveel is cleared, which means undoing it will restore the whole level 
        public static Tile[,] oldTiles;
        public ClearLevelAction(Tile[,] ot)
        {
            oldTiles = ot;

        }
        public override void UndoAction(MainWindow window)
        {
            MainWindow.Tiles = oldTiles;
        }
    }
    class SaveLevelAction : ActionPerformed
    {
        //Action performed when level is saved, undoing it will do nothing
        public override void UndoAction(MainWindow window)
        {
            throw new NotImplementedException();
        }
    }
    class EditorSelectAction : ActionPerformed
    {
        //Action performed when an option is chosen at the editor screen. Undoing it will make it go to the old selection
        public static Stack<TileInspector> OldInspectors = new Stack<TileInspector>();
        public EditorSelectAction(TileInspector ins)
        {
            OldInspectors.Push(ins);
        }
        public override void UndoAction(MainWindow window)
        {
            if (OldInspectors.Count == 0)
                return;
            TileInspector oldI = OldInspectors.Pop();
            if (OldInspectors.Count > 0)
                oldI = OldInspectors.Pop();
            TileInspector.SetImages(oldI);
        }
    }
    class ChangedTileAction : ActionPerformed
    {
        //The action performed when a tile is actively changed
       public  struct TileData
        {
            public int row;
            public int col;
            public ImageBrush brush;
            public string oldId;
            public bool isSet;
        }
        public ChangedTileAction(int r, int c, ImageBrush brush, string oldId)
        {
            TileData d = new TileData();
            d.row = r;
            d.col = c;
            d.brush = brush;
            d.oldId = oldId;
            oldTileData.Push(d);
        }
        public static Stack<TileData> oldTileData = new Stack<TileData>();
        public override void UndoAction(MainWindow window)
        {
            if (oldTileData.Count == 0)
                return;
             
            TileData oldTile= oldTileData.Pop();
            if (oldTileData.Count > 0)
               oldTile  = oldTileData.Pop();
            MainWindow.Tiles[oldTile.row, oldTile.col].SetTileData(oldTile.brush, oldTile.oldId);
        }
    }
}
