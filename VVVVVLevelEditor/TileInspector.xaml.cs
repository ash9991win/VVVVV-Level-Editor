using System;
using System.Collections.Generic;
using System.IO;
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
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for TileInspector.xaml
    /// </summary>
    public partial class TileInspector : UserControl
    {
        public static Dictionary<string, ImageBrush> ImageTable = new Dictionary<string, ImageBrush>();
        public static Dictionary<string, string> SymbolTable = new Dictionary<string, string>();
        public static Dictionary<string, string> InverseSymbolTable = new Dictionary<string, string>();
        public static Dictionary<string, List<MenuItem>> ContextMenus = new Dictionary<string, List<MenuItem>>();
        public static Dictionary<string, string> SymbolInFile = new Dictionary<string, string>();
        public string Id ;
        public static string CurrentId;
        public static ImageBrush CurrentImage;
        public bool shouldMenuShow;
        public TileInspector()
        {
            
            InitializeComponent();
            shouldMenuShow = true;
        }
        public static void PopulateTables()
        {
            LoadImageTable();
            LoadSymbolTable();
            LoadContextMenus();
         }
        private static void LoadContextMenus()
        {
            MenuItem UpSpawn;
            List<MenuItem> CheckPointMenu = new List<MenuItem>();
            UpSpawn = new MenuItem();
            UpSpawn.Header = "Spawn Up";
            UpSpawn.IsCheckable = true;
            UpSpawn.Click += CheckPointMenuHandler;
            MenuItem downSpawn = new MenuItem();
            downSpawn.Header = "Spawn Upside Down";
            downSpawn.IsCheckable = true;
            downSpawn.Click += CheckPointMenuHandler;
            CheckPointMenu.Add(UpSpawn);
            CheckPointMenu.Add(downSpawn);
            ContextMenus["CheckPoint"] = CheckPointMenu;

            List<MenuItem> phDMenu = new List<MenuItem>();
            MenuItem leftPhd = new MenuItem();
            leftPhd.IsCheckable = true;
            leftPhd.Header = "Start Left";
            leftPhd.Click += PhdMenuHandler;
            MenuItem rightPhd = new MenuItem();
            rightPhd.IsCheckable = true;
            rightPhd.Header = "Start Right";
            rightPhd.Click += PhdMenuHandler;
            phDMenu.Add(leftPhd);
            phDMenu.Add(rightPhd);
            ContextMenus["Phd"] = phDMenu;

            List<MenuItem> platformMenu = new List<MenuItem>();
            MenuItem upPlatform = new MenuItem();
            upPlatform.IsCheckable = true;
            upPlatform.Header = "Move Up";
            upPlatform.Click += PlatformMenuHandler;
            MenuItem downPlatform = new MenuItem();
            downPlatform.IsCheckable = true;
            downPlatform.Header = "Move Down";
            downPlatform.Click += PlatformMenuHandler;
            MenuItem leftPlatform = new MenuItem();
            leftPlatform.IsCheckable = true;
            leftPlatform.Header = "Move Left";
            leftPlatform.Click += PlatformMenuHandler;
            MenuItem rightPlatform = new MenuItem();
            rightPlatform.IsCheckable = true;
            rightPlatform.Header = "Move Right";
            rightPlatform.Click += PlatformMenuHandler;
            platformMenu.Add(upPlatform);
            platformMenu.Add(downPlatform);
            platformMenu.Add(leftPlatform);
            platformMenu.Add(rightPlatform);
            ContextMenus["Platform0"] = platformMenu;

            List<MenuItem> shooterMenu = new List<MenuItem>();
            MenuItem upShoot = new MenuItem();
            upShoot.Header = "Spawn Up";
            upShoot.Click += ShooterMenuHandler;
            upShoot.IsCheckable = true;
            MenuItem downShoot = new MenuItem();
            downShoot.Header = "Spawn Down";
            downShoot.Click += ShooterMenuHandler;
            downShoot.IsCheckable = true;
            MenuItem leftShoot = new MenuItem();
            leftShoot.Header = "Spawn Left";
            leftShoot.Click += ShooterMenuHandler;
            leftShoot.IsCheckable = true;
            MenuItem rightShoot = new MenuItem();
            rightShoot.Header = "Spawn Right";
            rightShoot.Click += ShooterMenuHandler;
            rightShoot.IsCheckable = true;
            shooterMenu.Add(upShoot);
            shooterMenu.Add(downShoot);
            shooterMenu.Add(leftShoot);
            shooterMenu.Add(rightShoot);
            ContextMenus["Shooter"] = shooterMenu;
        }
        private static void PlatformMenuHandler(object sender, RoutedEventArgs e)
        {
            MenuItem itemSelected = e.OriginalSource as MenuItem;
            foreach (var item in ContextMenus["Platform0"])
            {
                item.IsChecked = false;
            }
            itemSelected.IsChecked = true;
            string option = itemSelected.Header as string;
            if (option == "Move Up")
            {
                CurrentId = "^";
            }
            else if (option == "Move Down")
            {
                CurrentId = "v";
            }
            else if(option == "Move Left")
            {
                CurrentId = "}";
            }
            else if(option == "Move Right")
            {
                CurrentId = "{";
            }
        }
        public static void LoadSymbolTable()
        {
            SymbolTable.Clear();
            SymbolInFile = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Resources/SymbolTable.json"));
            SymbolTable = SymbolInFile;
            LoadInverseSymbolTable();
        }
        private static void LoadImageTable()
        {
            ImageTable["Block0"] = new ImageBrush(new BitmapImage(new Uri("Resources/Block0.png", UriKind.Relative)));
            ImageTable["Block1"] = new ImageBrush(new BitmapImage(new Uri("Resources/Block1.png", UriKind.Relative)));
            ImageTable["Block2"] = new ImageBrush(new BitmapImage(new Uri("Resources/Block2.png", UriKind.Relative)));
            ImageTable["Block3"] = new ImageBrush(new BitmapImage(new Uri("Resources/Block3.png", UriKind.Relative)));
            ImageTable["CheckPoint"] = new ImageBrush(new BitmapImage(new Uri("Resources/CheckPoint.png", UriKind.Relative)));
            ImageTable["HorizontalPipe"] = new ImageBrush(new BitmapImage(new Uri("Resources/HorizontalPipe.png", UriKind.Relative)));
            ImageTable["Phd"] = new ImageBrush(new BitmapImage(new Uri("Resources/Phd.png", UriKind.Relative)));
            ImageTable["Platform0"] = new ImageBrush(new BitmapImage(new Uri("Resources/Platform0.png", UriKind.Relative)));
            ImageTable["Platform1"] = new ImageBrush(new BitmapImage(new Uri("Resources/Platform1.png", UriKind.Relative)));
            ImageTable["PlatformEnd"] = new ImageBrush(new BitmapImage(new Uri("Resources/platformEnd.png", UriKind.Relative)));
            ImageTable["PlayerStart"] = new ImageBrush(new BitmapImage(new Uri("Resources/PlayerStart.png", UriKind.Relative)));
            ImageTable["Shooter"] = new ImageBrush(new BitmapImage(new Uri("Resources/Shooter.png", UriKind.Relative)));
            ImageTable["SpikeDown"] = new ImageBrush(new BitmapImage(new Uri("Resources/SpikeDown.png", UriKind.Relative)));
            ImageTable["SpikeLeft"] = new ImageBrush(new BitmapImage(new Uri("Resources/SpikeLeft.png", UriKind.Relative)));
            ImageTable["SpikeRight"] = new ImageBrush(new BitmapImage(new Uri("Resources/SpikeRight.png", UriKind.Relative)));
            ImageTable["SpikeUp"] = new ImageBrush(new BitmapImage(new Uri("Resources/SpikeUp.png", UriKind.Relative)));
            ImageTable["VerticalPipe"] = new ImageBrush(new BitmapImage(new Uri("Resources/VerticalPipe.png", UriKind.Relative)));
            ImageTable["Wall0"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall0.png", UriKind.Relative)));
            ImageTable["Wall1"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall1.png", UriKind.Relative)));
            ImageTable["Wall2"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall2.png", UriKind.Relative)));
            ImageTable["Wall3"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall3.png", UriKind.Relative)));
            ImageTable["Wall4"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall4.png", UriKind.Relative)));
            ImageTable["Wall5"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall5.png", UriKind.Relative)));
            ImageTable["Wall6"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall6.png", UriKind.Relative)));
            ImageTable["Wall7"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall7.png", UriKind.Relative)));
            ImageTable["Wall8"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall8.png", UriKind.Relative)));
            ImageTable["Wall9"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall9.png", UriKind.Relative)));
            ImageTable["Wall10"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall10.png", UriKind.Relative)));
            ImageTable["Wall11"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall11.png", UriKind.Relative)));
            ImageTable["Wall12"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall12.png", UriKind.Relative)));
            ImageTable["Wall13"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall13.png", UriKind.Relative)));
            ImageTable["Wall14"] = new ImageBrush(new BitmapImage(new Uri("Resources/Wall14.png", UriKind.Relative)));
            ImageTable["Exit"] = new ImageBrush(new BitmapImage(new Uri("Resources/ExitSign.jpg", UriKind.Relative)));
        }
        public static void SaveSymbolTable()
        {
            File.WriteAllText("Resources/SymbolTable.json", new JavaScriptSerializer().Serialize(SymbolTable));
            LoadInverseSymbolTable();
        }
        private static void LoadInverseSymbolTable()
        {
            InverseSymbolTable.Clear();
            foreach (var sym in SymbolTable)
            {
                InverseSymbolTable.Add(sym.Value, sym.Key);
            }
            InverseSymbolTable["3"] = "CheckPoint";
            InverseSymbolTable["5"] = "Phd";
            InverseSymbolTable["v"] = "Platform0";
            InverseSymbolTable["7"] = "Shooter";
            InverseSymbolTable["8"] = "Shooter";
            InverseSymbolTable["9"] = "Shooter";
        }
        private static void ShooterMenuHandler(object sender, RoutedEventArgs e)
        {
            //Uncheck all except this
            MenuItem itemSelected = e.OriginalSource as MenuItem;
            foreach(var item in ContextMenus["Shooter"])
            {
                item.IsChecked = false;
            }
            itemSelected.IsChecked = true;
            string option = itemSelected.Header as string;
            if(option == "Spawn Up")
            {
                CurrentId = "6";
            }
            else if(option == "Spawn Left")
            {
                CurrentId = "8";
            }
            else if (option == "Spawn Down")
            {
                CurrentId = "7";
            }
            else if (option == "Spawn Right")
            {
                CurrentId = "9";
            }
        }

        private static void PhdMenuHandler(object sender, RoutedEventArgs e)
        {
            MenuItem itemSelected = e.OriginalSource as MenuItem;
            foreach (var item in ContextMenus["Phd"])
            {
                item.IsChecked = false;
            }
            itemSelected.IsChecked = true;
            string option = itemSelected.Header as string;
            if (option == "Start Left")
            {
                CurrentId = "4";
            }
            else if (option == "Start Right")
            {
                CurrentId = "5";
            }
        }

        private static void CheckPointMenuHandler(object sender, RoutedEventArgs e)
        {
            MenuItem itemSelected = e.OriginalSource as MenuItem;
            foreach (var item in ContextMenus["CheckPoint"])
            {
                item.IsChecked = false;
            }
            itemSelected.IsChecked = true;
            string option = itemSelected.Header as string;
            if (option == "Spawn Up")
            {
                CurrentId = "2";

            }
            else if (option == "Spawn Upside Down")
            {
                CurrentId = "3";
            }
        }
        private void LeftClick(object sender, MouseButtonEventArgs e)
        {
            if (shouldMenuShow)
            {
                TileInspector inspector = e.Source as TileInspector;
                if (inspector == null)
                    return;
                SetImages(inspector);
                e.Handled = true;
            }
        }
        public  static void SetImages(TileInspector inspector)
        {
            MainWindow window = System.Windows.Application.Current.MainWindow as MainWindow;
            if (SymbolTable.ContainsKey(inspector.Id))
            {
                CurrentId = SymbolTable[inspector.Id];
            }
            else
            {
                CurrentId = Tile.DefaultTileString;
            }
            CurrentImage = ImageTable[inspector.Id];

        }
        private void RightClick(object sender, MouseButtonEventArgs e)
        {
            if (shouldMenuShow)
            {
                TileInspector inspector = e.Source as TileInspector;
                if (inspector == null)
                    return;
                ContextMenu m = new ContextMenu();
                MenuItem emptyItem = new MenuItem();
                emptyItem.Header = "No options available...";
                if (inspector != null)
                {
                    if (ContextMenus.ContainsKey(inspector.Id))
                    {
                        foreach (var subMenu in ContextMenus[inspector.Id])
                        {
                            var Parent = LogicalTreeHelper.GetParent(subMenu) as ContextMenu;
                            if (Parent != null)
                            {
                                Parent.Items.Remove(subMenu);
                            }
                            m.Items.Add(subMenu);
                        }
                    }
                    else
                    {
                        m.Items.Add(emptyItem);
                    }
                    m.IsOpen = true;

                }
                SetImages(inspector);
                e.Handled = true;
            }
        }
    }
}
