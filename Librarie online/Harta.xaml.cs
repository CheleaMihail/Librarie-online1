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

namespace Librarie_online
{
    /// <summary>
    /// Interaction logic for Harta.xaml
    /// </summary>
    public partial class Harta : Window
    {
        public Harta()
        {
            InitializeComponent();
            this.TestWebBrowser.Navigate("https://www.google.com/maps/d/u/0/embed?mid=1lh5Px4z-6qIz4cyZPGvx-Y9AcFI&msa=0&ll=47.02295968996409%2C28.857762461870077&spn=17.813056%2C36.870117&t=m&z=13&output=embed");
        }
    }
}
