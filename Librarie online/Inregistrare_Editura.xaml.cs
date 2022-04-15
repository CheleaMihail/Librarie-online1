using Librarie_online.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Inregistrare_Editura.xaml
    /// </summary>
    public partial class Inregistrare_Editura : Window
    {
        int editId;
        ModelConnection db = new ModelConnection();
        bool denumire;
        public Inregistrare_Editura(int id)
        {
            InitializeComponent();
            editId = id;
            if (id > 0)
            {
                Editura edit = (from querry in db.Edituras where querry.Id == id select querry).FirstOrDefault();
                denumireEditura.Text = edit.denumire;
                denumire = true;
            }
            Closing += Inregistrare_Editura_Closing;
        }

        private void Inregistrare_Editura_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        private void registerEditura_Click(object sender, RoutedEventArgs e)
        {
            if (denumire)
            {
                Administrator administrator = Owner as Administrator;
                if (editId > 0)
                {
                    Editura editura = (from querry in administrator.db.Edituras where querry.Id == editId select querry).FirstOrDefault();
                    editura.denumire = denumireEditura.Text;
                    administrator.db.SaveChanges();
                }
                else
                {
                    administrator.db.Edituras.Add(new Editura
                    {
                        denumire = denumireEditura.Text,
                    });
                    administrator.db.SaveChanges();
                }
                Close();
            }
            else
            {
                MessageBox.Show("Nu ati completat corect campurile", "Atentie!");
            }
        }
        private void denumireEditura_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Regex.Match(denumireEditura.Text, @"^([A-Z][A-Za-z]+\s*)+$").Success)
            {
                denumireEditura.Background = Brushes.Red;
                denumireEditura.BorderBrush = Brushes.DarkRed;
                denumire = false;
            }
            else
            {
                denumireEditura.Background = Brushes.White;
                denumireEditura.BorderBrush = Brushes.BlueViolet;
                denumire = true;
            }
        }
    }
}
