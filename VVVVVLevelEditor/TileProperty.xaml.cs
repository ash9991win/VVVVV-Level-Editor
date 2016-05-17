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

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for TileProperty.xaml
    /// </summary>
    public partial class TileProperty : UserControl
    {
        public static DependencyProperty ImageIdProperty = DependencyProperty.Register("Image Id", typeof(string), typeof(TileProperty));
        public string ImageId
        {
            get { return (string)GetValue(ImageIdProperty); }
            set { SetValue(ImageIdProperty, value); }
        }
        public TileProperty()
        {
            InitializeComponent();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
