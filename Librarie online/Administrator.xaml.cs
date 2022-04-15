using Librarie_online.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        public int idUser;
        public ModelConnection db;
        public Administrator(int ID)
        {
            InitializeComponent();
            db = new ModelConnection();
            idUser = ID;
            User user = (from querry in db.Users where querry.Id == ID select querry).FirstOrDefault();
            nume.Content = user.nume;
            prenume.Content = user.prenume;
            data.Content = user.dataNasterii;
            telefon.Content = user.telefon;
            email.Content = user.email;
            if (user.image != "")
            {
                string imageLocation = user.image;
                image.Source = new BitmapImage(new Uri(user.image));
            }
            db.Users.Load();
            utilizatoriDataGrid.ItemsSource = db.Users.Local.ToBindingList();
            LoadCategorii();
            LoadLimba();
            LoadAutori();
            LoadEdituri();
        }

        private void AddUsers_Click(object sender, RoutedEventArgs e)
        {
            Register_user register_User = new(0)
            {
                Owner = this,
            };
            register_User.Owner = this;
            register_User.Show();
        }

        private void vizualizareUtilizatori_Click(object sender, RoutedEventArgs e)
        {
            TabUtilizatori.SelectedIndex = 0;
            titluVizualizare.Content = "Utilizatori";
            db.Users.Load();
            utilizatoriDataGrid.ItemsSource = db.Users.Local.ToBindingList();
        }

        private void vizualizareCarti_Click(object sender, RoutedEventArgs e)
        {
            TabUtilizatori.SelectedIndex = 1;
            titluVizualizare.Content = "Carti";
            db.Cartes.Load();
            cartiDataGrid.ItemsSource = db.Cartes.Local.ToBindingList();
        }

        private void vizualizareAutori_Click(object sender, RoutedEventArgs e)
        {
            TabUtilizatori.SelectedIndex = 2;
            titluVizualizare.Content = "Autori";
            db.Autors.Load();
            autoriDataGrid.ItemsSource = db.Autors.Local.ToBindingList();
        }

        private void addCarti_Click(object sender, RoutedEventArgs e)
        {
            Inregistrare_Carte inregistrare_Carte = new(0)
            {
                Owner = this,
            };
            inregistrare_Carte.Owner = this;
            inregistrare_Carte.Show();
        }

        private void addAutori_Click(object sender, RoutedEventArgs e)
        {
            Inregistrare_Autor f = new Inregistrare_Autor(0)
            {
                Owner = this,
            };
            f.Owner = this;
            f.Show();
        }

        private void addCategorii_Click(object sender, RoutedEventArgs e)
        {
            Inregistrare_Categorie inregistrare_Categorie = new Inregistrare_Categorie(0)
            {
                Owner = this,
            };
            inregistrare_Categorie.Owner = this;
            inregistrare_Categorie.Show();
        }

        private void addEdituri_Click(object sender, RoutedEventArgs e)
        {
            Inregistrare_Editura inregistrare_Editura = new Inregistrare_Editura(0)
            {
                Owner = this,
            };
            inregistrare_Editura.Owner = this;
            inregistrare_Editura.Show();
        }

        private void vizualizareCategorii_Click(object sender, RoutedEventArgs e)
        {
            TabUtilizatori.SelectedIndex = 3;
            titluVizualizare.Content = "Categorii";
            db.Categories.Load();
            categoriiDataGrid.ItemsSource = db.Categories.Local.ToBindingList();
        }

        private void vizualizareEdituri_Click(object sender, RoutedEventArgs e)
        {
            TabUtilizatori.SelectedIndex = 4;
            titluVizualizare.Content = "Edituri";
            db.Edituras.Load();
            edituriDataGrid.ItemsSource = db.Edituras.Local.ToBindingList();
        }
        private void deleteCartes_Click(object sender, RoutedEventArgs e)
        {
            if (cartiDataGrid.SelectedItems.Count > 0)
            {
                for (int i = cartiDataGrid.SelectedItems.Count - 1; i >= 0; i--)
                {
                    Carte carte = cartiDataGrid.SelectedItems[i] as Carte;
                    if (carte != null)
                    {
                        db.Cartes.Remove(carte);
                    }
                }
            }
            db.SaveChanges();
        }

        private void stergeUtilizatori_Click(object sender, RoutedEventArgs e)
        {
            if (utilizatoriDataGrid.SelectedItems.Count > 0)
            {
                for (int i = utilizatoriDataGrid.SelectedItems.Count - 1; i >= 0; i--)
                {
                    User utilizatori = utilizatoriDataGrid.SelectedItems[i] as User;
                    if (utilizatori != null)
                    {
                        db.Users.Remove(utilizatori);
                    }
                }
            }
            db.SaveChanges();
        }

        private void stergeAutori_Click(object sender, RoutedEventArgs e)
        {
            if (utilizatoriDataGrid.SelectedItems.Count > 0)
            {
                for (int i = autoriDataGrid.SelectedItems.Count - 1; i >= 0; i--)
                {
                    Autor autor = autoriDataGrid.SelectedItems[i] as Autor;
                    if (autor != null)
                    {
                        db.Autors.Remove(autor);
                    }
                }
            }
            db.SaveChanges();
        }

        private void stergeCategorii_Click(object sender, RoutedEventArgs e)
        {
            if (categoriiDataGrid.SelectedItems.Count > 0)
            {
                for (int i = categoriiDataGrid.SelectedItems.Count - 1; i >= 0; i--)
                {
                    Categorie categorie = categoriiDataGrid.SelectedItems[i] as Categorie;
                    if (categorie != null)
                    {
                        db.Categories.Remove(categorie);
                    }
                }
            }
            db.SaveChanges();
        }

        private void stergeEditura_Click(object sender, RoutedEventArgs e)
        {
            if (edituriDataGrid.SelectedItems.Count > 0)
            {
                for (int i = edituriDataGrid.SelectedItems.Count - 1; i >= 0; i--)
                {
                    Editura editura = edituriDataGrid.SelectedItems[i] as Editura;
                    if (editura != null)
                    {
                        db.Edituras.Remove(editura);
                    }
                }
            }
            db.SaveChanges();
        }

        private void modificaCartes_Click(object sender, RoutedEventArgs e)
        {
            if (cartiDataGrid.SelectedItems.Count == 1)
            {

                if (cartiDataGrid.SelectedItems[0] is Carte carte)
                {
                    Inregistrare_Carte book = new Inregistrare_Carte(carte.Id)
                    {
                        Owner = this,
                    };
                    book.Show();
                    book.Closed += Book_Closed; ;
                }
            }
            db.SaveChanges();
        }

        private void Book_Closed(object sender, EventArgs e)
        {
            cartiDataGrid.ItemsSource = null;
            cartiDataGrid.ItemsSource = db.Cartes.Local.ToBindingList();
        }

        private void modificaEditura_Click(object sender, RoutedEventArgs e)
        {
            if (edituriDataGrid.SelectedItems.Count == 1)
            {

                if (edituriDataGrid.SelectedItems[0] is Editura editura)
                {
                    Inregistrare_Editura edit = new Inregistrare_Editura(editura.Id)
                    {
                        Owner = this,
                    };
                    edit.Show();
                    edit.Closed += Edit_Closed;
                }
            }
            db.SaveChanges();
        }

        private void Edit_Closed(object sender, EventArgs e)
        {
            edituriDataGrid.ItemsSource = null;
            edituriDataGrid.ItemsSource = db.Edituras.Local.ToBindingList();
        }

        private void modificaCategorie_Click(object sender, RoutedEventArgs e)
        {

            if (categoriiDataGrid.SelectedItems.Count == 1)
            {

                if (categoriiDataGrid.SelectedItems[0] is Categorie categorie)
                {
                    Inregistrare_Categorie categ = new Inregistrare_Categorie(categorie.Id)
                    {
                        Owner = this,
                    };
                    categ.Show();
                    categ.Closed += Categorie_Closed;
                }
            }
            db.SaveChanges();
        }

        private void Categorie_Closed(object sender, EventArgs e)
        {
            categoriiDataGrid.ItemsSource = null;
            categoriiDataGrid.ItemsSource = db.Categories.Local.ToBindingList();
        }

        private void modificaAutori_Click(object sender, RoutedEventArgs e)
        {
            if (autoriDataGrid.SelectedItems[0] is Autor autor)
            {
                Inregistrare_Autor inregistrareAutor = new Inregistrare_Autor(autor.Id)
                {
                    Owner = this,
                };
                inregistrareAutor.Show();
                inregistrareAutor.Closed += InregistrareAutor_Closed;
            }
        }

        private void InregistrareAutor_Closed(object sender, EventArgs e)
        {
            autoriDataGrid.ItemsSource = null;
            autoriDataGrid.ItemsSource = db.Autors.Local.ToBindingList();
        }

        private void filterAutorButton_Click(object sender, RoutedEventArgs e)
        {
            autoriDataGrid.ItemsSource = db.Autors.Local.ToBindingList().Where
               (
                   filtru =>
                   filtru.nume.Contains(filterAutorName.Text) &&
                  filtru.prenume.Contains(filterAutorPrename.Text)
               );
        }

        private void resetAutorFilter_Click(object sender, RoutedEventArgs e)
        {
            autoriDataGrid.ItemsSource = db.Autors.Local.ToBindingList();
            filterAutorName.Text = "";
            filterAutorPrename.Text = "";
        }

        private void filterCategprieButton_Click(object sender, RoutedEventArgs e)
        {
            categoriiDataGrid.ItemsSource = db.Categories.Local.ToBindingList().Where(
                filtru =>
                filtru.denumire.Contains(filterCategorieDenumire.Text)
                );
        }

        private void resetCategorieFilter_Click(object sender, RoutedEventArgs e)
        {
            categoriiDataGrid.ItemsSource = db.Categories.Local.ToBindingList();
            filterCategorieDenumire.Text = "";
        }

        private void resetEdituraFilter_Click(object sender, RoutedEventArgs e)
        {
            edituriDataGrid.ItemsSource = db.Edituras.Local.ToBindingList();
            filterEditureNameInput.Text = "";
        }

        private void filterEdituraButton_Click(object sender, RoutedEventArgs e)
        {
            edituriDataGrid.ItemsSource = db.Edituras.Local.ToBindingList().Where(
                 filtru =>
                 filtru.denumire.Contains(filterEditureNameInput.Text)
                 );
        }
        private bool existLaguage(string language)
        {
            bool result = false;
            foreach (CheckBox check in languageContainer.Children)
            {
                if (check.Content.ToString() == language)
                {
                    result = true;
                }
            }
            return result;
        }
        private void LoadCategorii()
        {
            List<Categorie> categoriiList = db.Categories.ToList();
            foreach (Categorie categorie in categoriiList)
            {
                CheckBox lab = new CheckBox
                {
                    Content = categorie.denumire,
                    Name = "cat" + categorie.Id.ToString(),
                };
                categoryContainerForFiltration.Children.Add(lab);
                lab.Background = Brushes.LightGray;
                lab.Margin = new Thickness(0, 1, 0, 1);
                lab.FontSize = 14;
            }
        }
        private void LoadLimba()
        {
            List<Carte> List = db.Cartes.ToList();
            foreach (Carte carte in List)
            {
                if (!existLaguage(carte.limba))
                {
                    CheckBox check = new CheckBox
                    {
                        Name = "limba" + carte.Id.ToString(),
                        Content = carte.limba,
                        Margin = new Thickness(0, 1, 0, 1),
                        FontSize = 12,
                    };
                    languageContainer.Children.Add(check);
                }
            }
        }
        private void LoadAutori()
        {
            List<Autor> autorList = db.Autors.ToList();
            foreach (Autor autor in autorList)
            {
                CheckBox check = new CheckBox
                {
                    Content = autor.nume + " " + autor.prenume,
                    Name = "autor" + autor.Id.ToString(),
                };
                autorContainer.Children.Add(check);
                check.Margin = new Thickness(0, 1, 0, 1);
                check.FontSize = 12;
            }
        }
        private void LoadEdituri()
        {
            List<Editura> edituraList = db.Edituras.ToList();
            foreach (Editura editura in edituraList)
            {
                CheckBox check = new CheckBox
                {
                    Content = editura.denumire,
                    Name = "check" + editura.Id.ToString(),
                };
                editureContainer.Children.Add(check);
                check.Margin = new Thickness(0, 1, 0, 1);
                check.FontSize = 12;
            }
        }
        private void resetBookFilter_Click(object sender, RoutedEventArgs e)
        {
            cartiDataGrid.ItemsSource = db.Cartes.Local.ToBindingList();
        }
        private bool CategoryFiltre(int bookId)
        {
            List<int> categoryId = (from q in db.Carte_Categories where q.carte == bookId select (int)q.categorie).ToList();
            List<int> selectedCategoryId = new();
            for (int i = 0; i < categoryContainerForFiltration.Children.Count; i++)
            {
                CheckBox ch = categoryContainerForFiltration.Children[i] as CheckBox;
                if (ch.IsChecked == true)
                {
                    selectedCategoryId.Add(Convert.ToInt32(ch.Name.Replace("cat", "")));
                }
            }
            bool r = false;
            foreach (int i in categoryId)
            {
                foreach (int j in selectedCategoryId)
                {
                    if (i == j) r = true;
                }
            }
            return r;
        }
        private bool EditureFiltre(int bookId)
        {
            List<int> editureId = (from q in db.Cartes where q.Id == bookId select (int)q.editura).ToList();
            List<int> selectedEditureId = new();
            for (int i = 0; i < editureContainer.Children.Count; i++)
            {
                CheckBox ch = editureContainer.Children[i] as CheckBox;
                if (ch.IsChecked == true)
                {
                    selectedEditureId.Add(Convert.ToInt32(ch.Name.Replace("check", "")));
                }
            }
            bool r = false;
            foreach (int i in editureId)
            {
                foreach (int j in selectedEditureId)
                {
                    if (i == j) r = true;
                }
            }
            return r;
        }
        private bool AuthorFiltre(int bookId)
        {
            List<int> autorId = (from q in db.Cartes where q.Id == bookId select (int)q.autor).ToList();
            List<int> selectedAuthorId = new();
            for (int i = 0; i < autorContainer.Children.Count; i++)
            {
                CheckBox ch = autorContainer.Children[i] as CheckBox;
                if (ch.IsChecked == true)
                {
                    selectedAuthorId.Add(Convert.ToInt32(ch.Name.Replace("autor", "")));
                }
            }
            bool r = false;
            foreach (int i in autorId)
            {
                foreach (int j in selectedAuthorId)
                {
                    if (i == j) r = true;
                }
            }
            return r;
        }
        private bool LanguageFiltre(string book)
        {
            List<string> selectedLanguage = new();

            for (int i = 0; i < languageContainer.Children.Count; i++)
            {
                CheckBox ch = languageContainer.Children[i] as CheckBox;
                if (ch.IsChecked == true)
                {
                    selectedLanguage.Add(ch.Content.ToString());
                }
            }
            for (int i = 0; i < selectedLanguage.Count; i++)
            {
                if (selectedLanguage[i] == book)
                {
                    return true;
                }
            }
            return false;
        }
        private bool PriceFiltre(float pret)
        {
            if (minPriceInput.Text != "" && Convert.ToDouble(minPriceInput.Text) <= pret && maxPriceInput.Text == "")
            {
                return true;
            }
            if (maxPriceInput.Text != "" && Convert.ToDouble(maxPriceInput.Text) >= pret && minPriceInput.Text == "")
            {
                return true;
            }
            if (maxPriceInput.Text != "" && minPriceInput.Text != "" && Convert.ToDouble(maxPriceInput.Text) >= pret && Convert.ToDouble(minPriceInput.Text) <= pret)
            {
                return true;
            }
            return false;
        }
        private bool AgeFiltre(float pret)
        {
            if (minPriceInput.Text != "" && Convert.ToDouble(minPriceInput.Text) <= pret && maxPriceInput.Text == "")
            {
                return true;
            }
            if (maxPriceInput.Text != "" && Convert.ToDouble(maxPriceInput.Text) >= pret && minPriceInput.Text == "")
            {
                return true;
            }
            if (maxPriceInput.Text != "" && minPriceInput.Text != "" && Convert.ToDouble(maxPriceInput.Text) >= pret && Convert.ToDouble(minPriceInput.Text) <= pret)
            {
                return true;
            }
            return false;
        }
        private void filterBookButton_Click(object sender, RoutedEventArgs e)
        {
            List<Carte> bookList = db.Cartes.Local.ToBindingList().ToList();
            for (int i = bookList.Count - 1; i >= 0; i--)
            {
                if (!CategoryFiltre(bookList[i].Id) && !EditureFiltre(bookList[i].Id) && !AuthorFiltre(bookList[i].Id) && !LanguageFiltre(bookList[i].limba) && !PriceFiltre((float)bookList[i].pret))
                {
                    bookList.Remove(bookList[i]);
                }
            }
            cartiDataGrid.ItemsSource = bookList;
        }

        private void filterUserButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime dataMin = DateTime.Now;
            DateTime dataMax = new DateTime(1, 1, 1);
            if (int.TryParse(minInputAge.Text, out int age))
            {
                dataMin = DateTime.Now.AddYears(-age);
            }
            if (int.TryParse(maxInputAge.Text, out age))
            {
                dataMax = DateTime.Now.AddYears(-age);
            }
            utilizatoriDataGrid.ItemsSource = db.Users.Local.ToBindingList().Where(
             filtru =>
             filtru.nume.Contains(filterUserNameInput.Text) &&
             filtru.prenume.Contains(filterUserPrenameInput.Text) &&
             filtru.dataNasterii <= dataMin &&
             filtru.dataNasterii >= dataMax
             );
        }

        private void resetUserFilter_Click(object sender, RoutedEventArgs e)
        {
            utilizatoriDataGrid.ItemsSource = db.Users.Local.ToBindingList();
            filterUserNameInput.Text = "";
            filterUserPrenameInput.Text = "";
            minInputAge.Text = "";
            maxInputAge.Text = "";
        }

        private void modificaUtilizatori_Click(object sender, RoutedEventArgs e)
        {
            if (utilizatoriDataGrid.SelectedItems.Count == 1)
            {

                if (utilizatoriDataGrid.SelectedItems[0] is User user)
                {
                    Register_user register_User = new Register_user(user.Id)
                    {
                        Owner = this,
                    };
                    register_User.Show();
                    register_User.Closed += Register_User_Closed; ;
                }
            }
            db.SaveChanges();
        }

        private void Register_User_Closed(object sender, EventArgs e)
        {
            utilizatoriDataGrid.ItemsSource = null;
            utilizatoriDataGrid.ItemsSource = db.Users.Local.ToBindingList();
        }

        private void contulMeu_Click(object sender, RoutedEventArgs e)
        {
            if (acountPanel.Visibility == Visibility.Collapsed)
            {
                accountStackPanel.Margin = new Thickness(0, 0, 0, -300);
                acountPanel.Visibility = Visibility.Visible;
                cartArrow.Source = new BitmapImage(new Uri(@"Images/up-arrow.png", UriKind.Relative));
            }
            else
            {
                accountStackPanel.Margin = new Thickness(0);
                acountPanel.Visibility = Visibility.Collapsed;
                cartArrow.Source = new BitmapImage(new Uri(@"Images/down-arrow-white.png", UriKind.Relative));
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void searchEngineButton_Click(object sender, RoutedEventArgs e)
        {
            lupaImg.Margin = new Thickness(0, 10, 0, 10);
            switch (TabUtilizatori.SelectedIndex)
            {
                case 0:
                    {
                        List<User> User = db.Users.ToList();
                        utilizatoriDataGrid.ItemsSource = User.Where(q => q.nume.Contains(searchEngine.Text) || q.prenume.Contains(searchEngine.Text));
                    }
                    break;
                case 1:
                    {
                        List<Carte> cartes = db.Cartes.ToList();
                        cartiDataGrid.ItemsSource = cartes.Where(q => q.denumire.Contains(searchEngine.Text));
                    }
                    break;
                case 2:
                    {
                        List<Autor> autors = db.Autors.ToList();
                        autoriDataGrid.ItemsSource = autors.Where(q => q.nume.Contains(searchEngine.Text) || q.prenume.Contains(searchEngine.Text));
                    }
                    break;
                case 3:
                    {
                        List<Categorie> categories = db.Categories.ToList();
                        categoriiDataGrid.ItemsSource = categories.Where(q => q.denumire.Contains(searchEngine.Text));
                    }
                    break;
                case 4:
                    {
                        List<Editura> edituras = db.Edituras.ToList();
                        edituriDataGrid.ItemsSource = edituras.Where(q => q.denumire.Contains(searchEngine.Text));
                    }
                    break;
            }
        }
        private void lupaImg_MouseEnter(object sender, MouseEventArgs e)
        {
            lupaImg.Margin = new Thickness(0, 8, 0, 8);
        }

        private void lupaImg_MouseLeave(object sender, MouseEventArgs e)
        {
            lupaImg.Margin = new Thickness(0, 10, 0, 10);
        }

        private void selfChangeData_Click(object sender, RoutedEventArgs e)
        {
            Register_user register_User = new Register_user(idUser)
            {
                Owner = this,
            };
            register_User.Show();
        }

        private void acountPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            accountStackPanel.Margin = new Thickness(0);
            acountPanel.Visibility = Visibility.Collapsed;
            cartArrow.Source = new BitmapImage(new Uri(@"Images/down-arrow-white.png", UriKind.Relative));
        }
    }
}

