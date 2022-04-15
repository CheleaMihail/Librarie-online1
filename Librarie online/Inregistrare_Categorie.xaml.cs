using Librarie_online.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Inregistrare_Categorie.xaml
    /// </summary>
    public partial class Inregistrare_Categorie : Window
    {
        int catId;
        ModelConnection db = new ModelConnection();
        bool validDenumire;
        public Inregistrare_Categorie(int id)
        {
            InitializeComponent();
            catId = id;
            if (id > 0)
            {
                Categorie categorie = (from querry in db.Categories where querry.Id == id select querry).FirstOrDefault();
                denumireCategorie.Text = categorie.denumire;
                validDenumire = true;
            }
            Closing += Inregistrare_Categorie_Closing;
        }

        private void Inregistrare_Categorie_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        private void registerCategorie_Click(object sender, RoutedEventArgs e)
        {
            if (validDenumire)
            {
                Administrator administrator = Owner as Administrator;
                if (catId > 0)
                {
                    Categorie categ = (from querry in administrator.db.Categories where querry.Id == catId select querry).FirstOrDefault();
                    categ.denumire = denumireCategorie.Text;
                    administrator.db.SaveChanges();
                }
                else
                {
                    administrator.db.Categories.Add(new Categorie
                    {
                        denumire = denumireCategorie.Text,
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
        private void denumireCategorie_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Regex.Match(denumireCategorie.Text, @"^([A-Z][a-z]+\s*)+$").Success)
            {
                denumireCategorie.Background = Brushes.Red;
                denumireCategorie.BorderBrush = Brushes.DarkRed;
                validDenumire = false;
            }
            else
            {
                denumireCategorie.Background = Brushes.White;
                denumireCategorie.BorderBrush = Brushes.BlueViolet;
                validDenumire = true;
            }
        }
    }
}
