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
    /// Interaction logic for InregistrareAutor.xaml
    /// </summary>
    public partial class Inregistrare_Autor : Window
    {
        ModelConnection db = new ModelConnection();
        int autorId;
        bool Validnume;
        bool Validprenume;
        public Inregistrare_Autor(int id)
        {
            InitializeComponent();
            autorId = id;
            if (id > 0)
            {
                Autor author = (from querry in db.Autors where querry.Id == id select querry).FirstOrDefault();
                numeAutor.Text = author.nume;
                prenumeAutor.Text = author.prenume;
                Validnume = true;
                Validprenume = true;
            }
            Closing += Inregistrare_Autor_Closing; ;
        }

        private void Inregistrare_Autor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        private void autorRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validnume || Validprenume)
            {
                Administrator administrator = Owner as Administrator;
                if (autorId > 0)
                {
                    Autor autor = (from querry in administrator.db.Autors where querry.Id == autorId select querry).FirstOrDefault();
                    autor.nume = numeAutor.Text;
                    autor.prenume = prenumeAutor.Text;
                    administrator.db.SaveChanges();
                }
                else
                {
                    administrator.db.Autors.Add(new Autor
                    {
                        nume = numeAutor.Text,
                        prenume = prenumeAutor.Text,
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
        private void numeAutor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Regex.Match(numeAutor.Text, "^[A-Z][a-zA-Z]*$").Success)
            {
                numeAutor.Background = Brushes.Red;
                numeAutor.BorderBrush = Brushes.DarkRed;
                Validnume = false;
            }
            else
            {
                numeAutor.Background = Brushes.White;
                numeAutor.BorderBrush = Brushes.BlueViolet;
                Validnume = true;
            }
        }
        private void prenumeAutor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Regex.Match(numeAutor.Text, "^[A-Z][a-zA-Z]*$").Success)
            {
                prenumeAutor.Background = Brushes.Red;
                prenumeAutor.BorderBrush = Brushes.DarkRed;
                Validprenume = false;

            }
            else
            {
                prenumeAutor.Background = Brushes.White;
                prenumeAutor.BorderBrush = Brushes.BlueViolet;
                Validprenume = true;
            }
        }
    }
}
