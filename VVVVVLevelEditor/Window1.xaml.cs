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

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            RowField.Text = MainWindow.NumberOfRows.ToString();
            ColField.Text = MainWindow.NumberOfColumns.ToString();
        }

        private void RowValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            if(mainWindow != null)
            {
                MainWindow.NumberOfRows = int.Parse(RowField.Text);
              MainWindow.PopulateTiles();
            }
        }

        private void ColumnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                MainWindow.NumberOfColumns = int.Parse(ColField.Text);
                MainWindow.PopulateTiles();
            }
        }
    }
}
