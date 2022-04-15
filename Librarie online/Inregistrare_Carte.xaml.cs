using Librarie_online.Models;
using Microsoft.Win32;
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
    /// Interaction logic for Inregistrare_Carte.xaml
    /// </summary>
    public partial class Inregistrare_Carte : Window
    {
        int carteId;
        ModelConnection db = new ModelConnection();
  
        public Inregistrare_Carte(int id)
        {
            InitializeComponent();
            loadAutori();
            loadEdituri();
            carteId = id;
            if (id > 0)
            {   
                Carte carte = (from querry in db.Cartes where querry.Id == id select querry).FirstOrDefault();
                Editura editura = (from querry in db.Edituras where querry.Id == carte.editura select querry).FirstOrDefault();
                Autor autor = (from querry in db.Autors where querry.Id == carte.autor select querry).FirstOrDefault();
                denumireCarte.Text = carte.denumire;
                autorCarte.Text= autor.nume+" "+autor.prenume;
                edituraCombo.Text= editura.denumire;
                limba.Text = carte.limba;
                pret.Text = carte.pret.ToString();
                reducere.Text = carte.reducere.ToString();
                stoc.Text = carte.stoc.ToString();
                descriere.Text = carte.descriere;
                if (carte.imagine != null)
                {
                    imageLocation = carte.imagine;
                    image.Source = new BitmapImage(new Uri(carte.imagine));
                }
                LoadCategoriiUpdateOption();
            }
            else
            {
                LoadCategorii();
            }
            Closing += Inregistrare_Carte_Closing;
        }

        private void LoadCategoriiUpdateOption()
        {
            List<int> lista1 = (from q in db.Carte_Categories where q.carte == carteId select (int)q.categorie).ToList();
            List<int> lista2 = new();
            foreach(int idCategorie in lista1)
            {
                lista2.Add((from q in db.Categories where q.Id == idCategorie select q.Id).FirstOrDefault());
            }

            List<Categorie> categoriiList = db.Categories.ToList();
            foreach (Categorie categorie in categoriiList)
            {
                CheckBox check = new CheckBox
                {
                    Content = categorie.denumire,
                    Name = "check" + categorie.Id.ToString(),
                };
                foreach(int idCat in lista2)
                {
                    if(categorie.Id == idCat)
                    {
                        check.IsChecked = true;
                    }
                }
                categoriiContainer.Children.Add(check);
            }
        }
        private void LoadCategorii()
        {
            foreach (Categorie categorie in db.Categories)
            {
                CheckBox check = new CheckBox
                {
                    Content = categorie.denumire,
                    Name = "check" + categorie.Id.ToString(),
                };
                categoriiContainer.Children.Add(check);
            }
        }
        private void Inregistrare_Carte_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        private void loadEdituri()
        {
            List<Editura> edituriList = db.Edituras.ToList();
            edituraCombo.Items.Add("Selecteaza editura");
            foreach (Editura editura in edituriList)
            {
                Label lab = new();
                lab.Content = editura.denumire;
                lab.Name = "le" + editura.Id;
                edituraCombo.Items.Add(lab);
            }
        }
        private void loadAutori()
        {
            List<Autor> autorList = db.Autors.ToList();
            autorCarte.Items.Add("Selecteaza autorul");
            foreach(Autor autor in autorList)
            {
                Label lab = new();
                lab.Content = autor.nume + " " + autor.prenume;
                lab.Name = "la" + autor.Id;
                autorCarte.Items.Add(lab);
            }
        }

        private string imageLocation;
        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            if (op.ShowDialog() == true)
            {
                imageLocation = op.FileName;
                image.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void OpenStandartImage_Click(object sender, RoutedEventArgs e)
        {
            image.Source = new BitmapImage(new Uri(@"\Images\incognito.jfif", UriKind.Relative));
        }
        private bool isChecked()
        {
            bool isChecked = false;
            foreach(CheckBox check in categoriiContainer.Children)
            {
                if (check.IsChecked == true)
                {
                    isChecked = true;
                }
            }
            return isChecked;
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (denumireCarte.Text != "" && autorCarte.Text != "Selecteaza autorul" && edituraCombo.Text != "Selecteaza editura" && limba.Text != "" && pret.Text != "" && reducere.Text != "" && stoc.Text != "" && descriere.Text != "" && imageLocation != "" && isChecked())
            {
                Administrator administrator = Owner as Administrator;
                if (carteId > 0)
                {
                    Carte book = (from querry in administrator.db.Cartes where querry.Id == carteId select querry).FirstOrDefault();
                    book.denumire = denumireCarte.Text;
                    book.autor = Convert.ToInt32((autorCarte.SelectedItem as Label).Name.Replace("la", ""));
                    book.editura = Convert.ToInt32((edituraCombo.SelectedItem as Label).Name.Replace("le", ""));
                    book.limba = limba.Text;
                    book.pret = (float)Convert.ToDouble(pret.Text);
                    book.reducere = Convert.ToInt32(reducere.Text);
                    book.stoc = Convert.ToInt32(stoc.Text);
                    book.descriere = descriere.Text;
                    book.imagine = imageLocation;

                    foreach (Carte_Categorie categorie in from q in administrator.db.Carte_Categories where q.carte == carteId select q)
                    {
                        administrator.db.Carte_Categories.Remove(categorie);
                    }
                    foreach (CheckBox ch in categoriiContainer.Children)
                    {
                        if (ch.IsChecked == true)
                        {
                            Carte_Categorie carte_Categorie = new Carte_Categorie()
                            {
                                carte = book.Id,
                                categorie = Convert.ToInt32(ch.Name.Replace("check", "")),
                            };
                            administrator.db.Carte_Categories.Add(carte_Categorie);
                        }
                    }
                    administrator.db.SaveChanges();
                }
                else
                {
                    Carte carte = new Carte()
                    {
                        denumire = denumireCarte.Text,
                        autor = Convert.ToInt32((autorCarte.SelectedItem as Label).Name.Replace("la", "")),
                        editura = Convert.ToInt32((edituraCombo.SelectedItem as Label).Name.Replace("le", "")),
                        limba = limba.Text,
                        pret = (float)Convert.ToDouble(pret.Text),
                        reducere = Convert.ToInt32(reducere.Text),
                        stoc = Convert.ToInt32(stoc.Text),
                        descriere = descriere.Text,
                        imagine = imageLocation,
                    };
                    _ = administrator.db.Cartes.Add(carte);
                    _ = administrator.db.SaveChanges();
                    foreach (CheckBox ch in categoriiContainer.Children)
                    {
                        if (ch.IsChecked == true)
                        {
                            Carte_Categorie carte_Categorie = new Carte_Categorie()
                            {
                                carte = carte.Id,
                                categorie = Convert.ToInt32(ch.Name.Replace("check", "")),
                            };
                            administrator.db.Carte_Categories.Add(carte_Categorie);
                        }
                    }
                }
                _ = administrator.db.SaveChanges();
                Close();
            }
            else { MessageBox.Show("Introduceti toate datele", "Atentie!"); }
        }

        private void pret_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void reducere_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]+");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void reducere_LostFocus(object sender, RoutedEventArgs e)
        {
            if (reducere.Text=="")
            {
                reducere.Background = Brushes.Red;
                reducere.BorderBrush= Brushes.DarkRed;
            }
            else if (Convert.ToInt32(reducere.Text) > 100)
            {
                reducere.Background = Brushes.Red;
                reducere.BorderBrush = Brushes.DarkRed;
            }
            else
            {
                reducere.Background = Brushes.White;
                reducere.BorderBrush = Brushes.BlueViolet;
            }
        }

        private void stoc_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]+");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void denumireCarte_LostFocus(object sender, RoutedEventArgs e)
        {
            if (denumireCarte.Text=="")
            {
                denumireCarte.Background = Brushes.Red;
                denumireCarte.BorderBrush = Brushes.DarkRed;
            }
            else
            {
                denumireCarte.Background = Brushes.White;
                denumireCarte.BorderBrush = Brushes.BlueViolet;
            }
        }

        private void limba_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Regex.Match(limba.Text, "^[A-Z][a-z]+$").Success)
            {
                limba.Background = Brushes.Red;
                limba.BorderBrush = Brushes.DarkRed;
            }
            else
            {
                limba.Background = Brushes.White;
                limba.BorderBrush = Brushes.BlueViolet;
            }
        }
    }
}
