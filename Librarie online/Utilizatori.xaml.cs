using Librarie_online.Models;
using Librarie_online.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Librarie_online
{
    /// <summary>
    /// Interaction logic for Utilizatori.xaml
    /// </summary>
    public partial class Utilizatori : Window
    {
        public ModelConnection db;
        public ObservableCollection<BookList> Books { get; set; }
        public ObservableCollection<BookList> BooksCart { get; set; }
        public ObservableCollection<Purchased> PurchasedBooks { get; set; }
        public int idUser;
        public Utilizatori(int ID)
        {
            InitializeComponent();
            tabControl.SelectedIndex = 1;
            db = new ModelConnection();
            idUser = ID;
            User user = (from querry in db.Users where querry.Id == ID select querry).FirstOrDefault();
            nume.Content = user.nume;
            prenume.Content = user.prenume;
            data.Content = user.dataNasterii;
            telefon.Content = user.telefon;
            email.Content = user.email;
            if (user.image != null)
            {
                string imageLocation = user.image;
                image.Source = new BitmapImage(new Uri(user.image));
            }
            LoadCategorii();
            LoadLimba();
            LoadAutori();
            LoadEdituri();
            Books = new ObservableCollection<BookList>();
            BooksCart = new ObservableCollection<BookList>();
            PurchasedBooks = new ObservableCollection<Purchased>();
            ModelConnection bookReader = new ModelConnection();
            bookReader.Cartes.Load();
            foreach (Carte book in bookReader.Cartes)
            {
                string predInitial = "";
                string pretRedus = "";
                if (book.reducere > 0)
                {
                    predInitial = book.pret.ToString() + " MDL";
                    pretRedus = Math.Round((decimal)(book.pret - (book.pret * book.reducere / 100)), 2).ToString() + " MDL";
                }
                else
                {
                    predInitial = "";
                    pretRedus = Math.Round((decimal)book.pret, 2).ToString() + " MDL";
                }

                List<Rating> rat = db.Ratings.Where(q => q.CarteId == book.Id).ToList();
                bool[] ratigStars = new bool[5];
                if (rat.Count > 0)
                {
                    int allPoints = 0;
                    foreach (Rating r in rat)
                    {
                        allPoints += r.Puncte;
                    }
                    int rating = Convert.ToInt32((double)allPoints / rat.Count);

                   
                    for (int i = 1; i <= 5; i++)
                    {
                        if (i <= rating) ratigStars[i - 1] = true;
                        else ratigStars[i - 1] = false;
                    }
                }
                Books.Add(new BookList
                {
                    id = book.Id,
                    denumire = book.denumire,
                    autor = (from q in db.Autors where q.Id == book.autor select q.nume + " " + q.prenume).FirstOrDefault(),
                    editura = (from q in db.Edituras where q.Id == book.editura select q.denumire).FirstOrDefault(),
                    categorii = returnCategories(book.Id),
                    categoriiId = returnCategoriesId(book.Id),
                    limba = book.limba,
                    pretAnterior = predInitial,
                    pretCurent = pretRedus,
                    stoc = (int)book.stoc,
                    descriere = book.descriere,
                    imagine = book.imagine,
                    count = 1,
                    star1 = ratigStars[0],
                    star2 = ratigStars[1],
                    star3 = ratigStars[2],
                    star4 = ratigStars[3],
                    star5 = ratigStars[4],
                });
            }
            listaLibrariei.ItemsSource = Books;
            bookReader.Dispose();
            bookCartList.ItemsSource = BooksCart;
            LoadCumparaturi();
        }
        private void LoadCumparaturi()
        {
            List<Cos> booksInCart = db.Cos.Where(q => q.UserId == idUser).ToList();
            foreach (Cos cos in booksInCart)
            {
                BookList b = Books.FirstOrDefault(q => q.id == cos.CarteId);
                b.count = cos.Count;
                BooksCart.Add(b);
                sumaBooks.Content = Convert.ToDecimal(sumaBooks.Content) + Convert.ToDecimal(b.count * Convert.ToDecimal(b.pretCurent.Replace(" MDL", "")));
            }
        }
        private string returnCategories(int idBook)
        {
            List<int> bookCategory = (from q in db.Carte_Categories where q.carte == idBook select (int)q.categorie).ToList();
            string categoryName = "";
            foreach (int id in bookCategory)
            {
                categoryName += db.Categories.Where(q => q.Id == id).Select(q => q.denumire).FirstOrDefault() + ", ";
            }
            categoryName = categoryName.Substring(0, categoryName.Length - 2);

            return categoryName;
        }
        private List<int> returnCategoriesId(int idBook)
        {
            List<int> bookCategoryIdList = (from q in db.Carte_Categories where q.carte == idBook select (int)q.categorie).ToList();
            List<int> categoriesIdToReturn = new();
            foreach (int id in bookCategoryIdList)
            {
                categoriesIdToReturn.Add(db.Categories.Where(q => q.Id == id).Select(q => q.Id).FirstOrDefault());
            }
            return categoriesIdToReturn;
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
            foreach (Autor autor in db.Autors)
            {
                CheckBox check = new()
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
            listaLibrariei.ItemsSource = Books;
            foreach (CheckBox ch in categoryContainerForFiltration.Children)
            {
                ch.IsChecked = false;
            }
            foreach (CheckBox ch in languageContainer.Children)
            {
                ch.IsChecked = false;
            }
            foreach (CheckBox ch in autorContainer.Children)
            {
                ch.IsChecked = false;
            }
            foreach (CheckBox ch in editureContainer.Children)
            {
                ch.IsChecked = false;
            }
            minPriceInput.Text = "";
            maxPriceInput.Text = "";
        }
        private bool EditureFiltre(string editura)
        {
            bool rez = true;
            foreach (CheckBox check in editureContainer.Children)
            {
                if (check.IsChecked == true)
                {
                    rez = false;
                    if (check.Content.ToString() == editura)
                    {
                        return true;
                    }
                }
            }
            return rez;
        }
        private bool LanguageFiltre(string limba)
        {
            bool rez = true;
            foreach (CheckBox check in languageContainer.Children)
            {
                if (check.IsChecked == true)
                {
                    rez = false;
                    if (check.Content.ToString() == limba)
                    {
                        return true;
                    }
                }
            }
            return rez;
        }
        private bool AuthorFiltre(string autor)
        {
            bool rez = true;
            foreach (CheckBox check in autorContainer.Children)
            {
                if (check.IsChecked == true)
                {
                    rez = false;
                    if (check.Content.ToString() == autor) return true;
                }
            }
            return rez;
        }
        private bool PretFiltre(string pret)
        {
            string price = pret.Replace(" MDL", "");
            if (minPriceInput.Text != "" && Convert.ToDouble(minPriceInput.Text) <= Convert.ToDouble(price) && maxPriceInput.Text == "")
            {
                return true;
            }
            if (maxPriceInput.Text != "" && Convert.ToDouble(maxPriceInput.Text) >= Convert.ToDouble(price) && minPriceInput.Text == "")
            {
                return true;
            }
            if (maxPriceInput.Text != "" && minPriceInput.Text != "" && Convert.ToDouble(maxPriceInput.Text) >= Convert.ToDouble(price) && Convert.ToDouble(minPriceInput.Text) <= Convert.ToDouble(price))
            {
                return true;
            }
            if (maxPriceInput.Text == "" && minPriceInput.Text == "")
            {
                return true;
            }
            return false;
        }

        List<int> SelectedCategoryIdFiltre = new List<int>();
        private bool CategorieFiltre(List<int> BookCategories)
        {
            bool rez = true;
            foreach (int catId in SelectedCategoryIdFiltre)
            {
                rez = false;
                if (BookCategories.Contains(catId))
                {
                    return true;
                }
            }
            return rez;
        }
        private void filterBookButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedCategoryIdFiltre.Clear();
            foreach (CheckBox check in categoryContainerForFiltration.Children)
            {
                if (check.IsChecked == true)
                {
                    SelectedCategoryIdFiltre.Add(Convert.ToInt32(check.Name.Replace("cat", "")));
                }
            }
            listaLibrariei.ItemsSource = Books.Where(q => EditureFiltre(q.editura) && LanguageFiltre(q.limba) && AuthorFiltre(q.autor) && PretFiltre(q.pretCurent) && CategorieFiltre(q.categoriiId));
        }
        private void searchEngineButton_Click(object sender, RoutedEventArgs e)
        {
            lupaImg.Margin = new Thickness(0, 10, 0, 10);
            listaLibrariei.ItemsSource = Books.Where(q => q.denumire.Contains(searchEngine.Text) || q.autor.Contains(searchEngine.Text) || q.categorii.Contains(searchEngine.Text));
        }
        private void cosClick_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {
                tabControl.SelectedIndex = 1;
                cartArrow.Source = new BitmapImage(new Uri(@"Images/down-arrow-white.png", UriKind.Relative));
            }
            else
            {
                tabControl.SelectedIndex = 0;
                cartArrow.Source = new BitmapImage(new Uri(@"Images/up-arrow.png", UriKind.Relative));
            }

        }

        private void cotulMeu_Click(object sender, RoutedEventArgs e)
        {
            if (acountPanel.Visibility == Visibility.Collapsed)
            {
                accountStackPanel.Margin = new Thickness(0, 0, 0, -300);
                acountPanel.Visibility = Visibility.Visible;
                downImg.Source = new BitmapImage(new Uri(@"Images/up-arrow.png", UriKind.Relative));

            }
            else
            {
                accountStackPanel.Margin = new Thickness(0);
                acountPanel.Visibility = Visibility.Collapsed;
                downImg.Source = new BitmapImage(new Uri(@"Images/down-arrow-white.png", UriKind.Relative));
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = returIdOfBook((Button)sender);
            var b = Books.FirstOrDefault(q => q.id == id);
            if (b.stoc > 0)
            {
                if (db.Cos.FirstOrDefault(q => q.CarteId == b.id) == null)
                {
                    //=============================================================================================
                    db.Cos.Add(new Cos {
                        CarteId = b.id,
                        UserId = idUser,
                        Count = b.count,
                    });
                    db.SaveChanges();
                    BooksCart.Add(b);
                    sumaBooks.Content = Convert.ToDecimal(sumaBooks.Content) + Convert.ToDecimal(b.count * Convert.ToDecimal(b.pretCurent.Replace(" MDL", "")));
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int id = returIdOfBook((Button)sender);
            var b = BooksCart.FirstOrDefault(q => q.id == id);
            sumaBooks.Content = Convert.ToDecimal(sumaBooks.Content) - Convert.ToDecimal(b.pretCurent.Replace(" MDL", "")) * b.count;
            b.count = 1;
            BooksCart.Remove(b);
            Cos bookInCart = db.Cos.FirstOrDefault(q => q.CarteId == id && q.UserId == idUser);
            db.Cos.Remove(bookInCart);
            db.SaveChanges();
        }
        int returIdOfBook(Button sender)
        {
            return Convert.ToInt32((sender.Content as StackPanel).Children.OfType<TextBlock>().FirstOrDefault().Text);
        }
        private void CartPlus_Click(object sender, RoutedEventArgs e)
        {
            int id = returIdOfBook((Button)sender);
            var b = BooksCart.FirstOrDefault(q => q.id == id);
            b.count++;
            ((((Button)sender).Parent as StackPanel).Parent as StackPanel).Children.OfType<TextBox>().FirstOrDefault().Text = BooksCart.Where(q => q.id == id).FirstOrDefault().count.ToString();
            sumaBooks.Content = Convert.ToDecimal(sumaBooks.Content) + Convert.ToDecimal(b.pretCurent.Replace(" MDL", ""));
        }

        private void CartMinus_Click(object sender, RoutedEventArgs e)
        {
            int id = returIdOfBook((Button)sender);
            var b = BooksCart.FirstOrDefault(q => q.id == id);
            if (b.count > 1)
            {
                b.count--;
                ((((Button)sender).Parent as StackPanel).Parent as StackPanel).Children.OfType<TextBox>().FirstOrDefault().Text = BooksCart.Where(q => q.id == id).FirstOrDefault().count.ToString();
                sumaBooks.Content = Convert.ToDecimal(sumaBooks.Content) - Convert.ToDecimal(b.pretCurent.Replace(" MDL", ""));
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "") textBox.Text = "1";
            int id = Convert.ToInt32((((((textBox.Parent as StackPanel).Children[1] as StackPanel).Children[0] as Button).Content as StackPanel).Children[0] as TextBlock).Text);              
            
            BookList b = BooksCart.FirstOrDefault(q => q.id == id);
            if (int.TryParse(textBox.Text, out int nr))
            {
                if (Convert.ToInt32(textBox.Text) > b.stoc) { textBox.Text = b.stoc.ToString(); nr = b.stoc; }

                if (nr > 0)
                {
                    if (nr < b.count)
                    {
                        sumaBooks.Content = Convert.ToDecimal(sumaBooks.Content) - Convert.ToDecimal(b.pretCurent.Replace(" MDL", "")) * (b.count - nr);
                    }
                    else
                    {
                        sumaBooks.Content = Convert.ToDecimal(sumaBooks.Content) + Convert.ToDecimal(b.pretCurent.Replace(" MDL", "")) * (nr - b.count);
                    }
                    b.count = nr;
                }
                else
                {
                    sumaBooks.Content = Convert.ToDecimal(sumaBooks.Content) - Convert.ToDecimal(b.pretCurent.Replace(" MDL", "")) * (b.count - 1);
                    b.count = 1;
                    textBox.Text = "1";
                }
            }
            else
            {
                textBox.Text = b.count.ToString();
            }
        }
        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            Regex justNumber = new(@"^[0-9]$");
            e.Handled = !justNumber.IsMatch(e.Text);
        }

        private void Harta_Click(object sender, RoutedEventArgs e)
        {
            Harta button = new Harta();
            button.Show();
        }
        private void starBut1_MouseEnter(object sender, MouseEventArgs e)
        {
            (((Button)sender).Content as Image).Source = new BitmapImage(new Uri(@"Images\star.png", UriKind.Relative));
        }

        private void starBut1_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            int idStar = Convert.ToInt32(button.Name.Replace("starBut", ""));

            if (button.Name == "starBut5")
            {
                (button.Content as Image).Source = new BitmapImage(new Uri(@"Images\starprime.png", UriKind.Relative));
            }
            else
            {
                Binding binding = new Binding();
                binding.ElementName = "starImage" + (idStar + 1).ToString();
                binding.Path = new PropertyPath("Source");
                (button.Content as Image).SetBinding(Image.SourceProperty, binding);
            }
        }    

        private int expandedBookId;

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(cantitatea.Text) < Books.Where(q => q.id == expandedBookId).Select(q => q.stoc).FirstOrDefault())
            {
                cantitatea.Text = (Convert.ToInt32(cantitatea.Text) + 1).ToString();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(cantitatea.Text) > 1)
            {
                cantitatea.Text = (Convert.ToInt32(cantitatea.Text) - 1).ToString();
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            tabListaCarti.IsSelected = true;
        }

        private void placeInCart_Click(object sender, RoutedEventArgs e)
        {
            BookList b = Books.FirstOrDefault(q => q.id == expandedBookId);
            if (b.stoc > 0)
            {
                b.count = Convert.ToInt32(cantitatea.Text) > b.stoc ? b.stoc : Convert.ToInt32(cantitatea.Text);

                if (db.Cos.FirstOrDefault(q => q.CarteId == b.id) == null)
                {
                    //=============================================================================================
                    db.Cos.Add(new Cos
                    {
                        CarteId = b.id,
                        UserId = idUser,
                        Count = b.count,
                    });
                    db.SaveChanges();
                    BooksCart.Add(b);
                    sumaBooks.Content = Convert.ToDecimal(sumaBooks.Content) + Convert.ToDecimal(b.count * Convert.ToDecimal(b.pretCurent.Replace(" MDL", "")));
                }
            }
        }

        private void golireCos_Click(object sender, RoutedEventArgs e)
        {
            foreach (BookList b in BooksCart)
            {
                b.count = 1;
            }
            BooksCart.Clear();
            sumaBooks.Content = "0";
            List<Cos> golire = db.Cos.Where(q => q.UserId == idUser).ToList();
            foreach (Cos c in golire)
            {
                db.Cos.Remove(c);
            }
            db.SaveChanges();
        }

        private void achitareCumparaturi_Click(object sender, RoutedEventArgs e)
        {
            if (BooksCart.Count > 0)
            {
                for (int i = BooksCart.Count - 1; i >= 0; i--)
                {
                    Vanzari vanzare = new Vanzari
                    {
                        utilizator = idUser,
                        carte = BooksCart[i].id,
                        cantitate = BooksCart[i].count,
                        data = DateTime.Now,
                    };
                    db.Vanzaris.Add(vanzare);


                    Carte book = db.Cartes.FirstOrDefault(q => q.Id == vanzare.carte);
                    book.stoc -= BooksCart[i].count;
                    BooksCart[i].stoc -= BooksCart[i].count;

                    if (BooksCart[i].stoc == 0)
                    {
                        int idBookTodeleteFormCart = BooksCart[i].id;
                        Cos cos = db.Cos.FirstOrDefault(q => q.CarteId == idBookTodeleteFormCart && q.UserId == idUser);
                        db.Cos.Remove(cos);
                        sumaBooks.Content = Convert.ToDecimal(sumaBooks.Content) - Convert.ToDecimal(BooksCart[i].count * Convert.ToDecimal(BooksCart[i].pretCurent.Replace(" MDL", "")));
                        BooksCart.Remove(BooksCart[i]); 
                    }
                    else
                    {
                        ListBoxItem myListBoxItem = (ListBoxItem)bookCartList.ItemContainerGenerator.ContainerFromItem(bookCartList.Items[i]);
                        ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);
                        DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                        TextBox count = (TextBox)myDataTemplate.FindName("textBox", myContentPresenter);
                        if (Convert.ToInt32(count.Text) > book.stoc)
                        {
                            count.Text = book.stoc.ToString();
                        }
                    }
                }
            }
            MessageBoxResult d = MessageBox.Show("Doriti sa cumparati cartile din cos?", "Cumparare", MessageBoxButton.YesNo);
            if (d == MessageBoxResult.Yes)
            {
                MessageBox.Show("Cumparare reusita", "Cumparare");
                db.SaveChanges();
                listaLibrariei.ItemsSource = null;
                listaLibrariei.ItemsSource = Books;
            }

        }
        
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            expandedBookId = Convert.ToInt32(((sender as Button).Content as StackPanel).Children.OfType<TextBlock>().FirstOrDefault().Text);
            BookList book = Books.FirstOrDefault(q => q.id == expandedBookId);
            imagineCarte.Source = new BitmapImage(new Uri(book.imagine, UriKind.Absolute));
            denumireCarte.Text = book.denumire;
            autorCarte.Text = "Autor: " + book.autor;
            categoriiCarte.Text = "Categorii: " + book.categorii;
            edituraCarte.Text = "Editura: " + book.editura;
            limbaCarte.Text = "Limba: " + book.limba;
            pretInitialCarte.Text = book.pretAnterior;
            pretActualCarte.Text = book.pretCurent;
            reducereCarte.Text = "Reducere:" + (Convert.ToDecimal(book.pretCurent.Replace("MDL", "")) - Convert.ToDecimal(book.pretAnterior.Replace("MDL", ""))).ToString() + " MDL";
            descriereCarte.Text = book.descriere;
            stocCarte.Text = "In stoc: " + book.stoc + " buc";
            cantitatea.Text = "1";
            tabExpandedBook.IsSelected = true;
            expandStar1.Source = book.star1
                ? new BitmapImage(new Uri("Images/star.png", UriKind.Relative))
                : new BitmapImage(new Uri("Images/starprime.png", UriKind.Relative));
            expandStar2.Source = book.star2
               ? new BitmapImage(new Uri("Images/star.png", UriKind.Relative))
               : new BitmapImage(new Uri("Images/starprime.png", UriKind.Relative));
            expandStar3.Source = book.star3
               ? new BitmapImage(new Uri("Images/star.png", UriKind.Relative))
               : new BitmapImage(new Uri("Images/starprime.png", UriKind.Relative));
            expandStar4.Source = book.star4
               ? new BitmapImage(new Uri("Images/star.png", UriKind.Relative))
               : new BitmapImage(new Uri("Images/starprime.png", UriKind.Relative));
            expandStar5.Source = book.star5
               ? new BitmapImage(new Uri("Images/star.png", UriKind.Relative))
               : new BitmapImage(new Uri("Images/starprime.png", UriKind.Relative));
        }

        private void cantitatea_LostFocus(object sender, RoutedEventArgs e)
        {
            if(!int.TryParse(cantitatea.Text, out int nr))
            {
                cantitatea.Text = "1";
            }
        }
        private childItem FindVisualChild<childItem>(DependencyObject obj)
    where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        private void selfChangeData_Click(object sender, RoutedEventArgs e)
        {
            Register_user register_User = new Register_user(idUser)
            {
                Owner = this,
            };
            register_User.Show();
        }
        private void mySales_Click(object sender, RoutedEventArgs e)
        {
            Cumparaturi.IsSelected = true;
            PurchasedBooks.Clear();
            cartiCumparate.ItemsSource = null;
            ModelConnection vanzariReader = new ModelConnection();
            foreach (Vanzari vanz in db.Vanzaris.Where(q => q.utilizator == idUser))
            {
                BookList book = Books.FirstOrDefault(q => q.id == vanz.carte);
                PurchasedBooks.Add(new Purchased()
                {
                    denumire = book.denumire,
                    cantitate = vanz.cantitate.ToString(),
                    pret = book.pretCurent.ToString(),
                    data = vanz.data.ToString().Substring(0, vanz.data.ToString().Length - 3),
                });
            }
            cartiCumparate.ItemsSource = PurchasedBooks;
            vanzariReader.Dispose();
        }
        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            Panel.SetZIndex(starButtonContainer, 1);
            Panel.SetZIndex(starIamgeContainer, 0);
        }

        private void starButtonContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            Panel.SetZIndex(starButtonContainer, 0);
            Panel.SetZIndex(starIamgeContainer, 1);
        }
        private void starBut1_Click(object sender, RoutedEventArgs e)
        {
            int p = Convert.ToInt32(((Button)sender).Name.Replace("starBut", ""));     
            if (db.Ratings.FirstOrDefault(q => q.UserId == idUser && q.CarteId == expandedBookId) == null)
            {
                db.Ratings.Add(new Rating
                {
                    CarteId = expandedBookId,
                    UserId = idUser,
                    Puncte = p,
                });
            }
            else
            {
                Rating rat = db.Ratings.FirstOrDefault(q => q.UserId == idUser && q.CarteId == expandedBookId);
                rat.Puncte = p;
            }
            db.SaveChanges();
            List<Rating> ratList = db.Ratings.Where(q => q.CarteId == expandedBookId).ToList();
            int allPoints = 0;
            foreach (Rating r in ratList)
            {
                allPoints += r.Puncte;
            }
            int rating = Convert.ToInt32((double)allPoints / ratList.Count);

            bool[] ratigStars = new bool[5];
            for (int i = 1; i <= 5; i++)
            {
                if (i <= rating) ratigStars[i - 1] = true;
                else ratigStars[i - 1] = false;
            }
            BookList book = Books.FirstOrDefault(q => q.id == expandedBookId);
            book.star1 = ratigStars[0];
            book.star2 = ratigStars[1];
            book.star3 = ratigStars[2];
            book.star4 = ratigStars[3];
            book.star5 = ratigStars[4];
            expandStar1.Source = book.star1
                ? new BitmapImage(new Uri("Images/star.png", UriKind.Relative))
                : new BitmapImage(new Uri("Images/starprime.png", UriKind.Relative));
            expandStar2.Source = book.star2
               ? new BitmapImage(new Uri("Images/star.png", UriKind.Relative))
               : new BitmapImage(new Uri("Images/starprime.png", UriKind.Relative));
            expandStar3.Source = book.star3
               ? new BitmapImage(new Uri("Images/star.png", UriKind.Relative))
               : new BitmapImage(new Uri("Images/starprime.png", UriKind.Relative));
            expandStar4.Source = book.star4
               ? new BitmapImage(new Uri("Images/star.png", UriKind.Relative))
               : new BitmapImage(new Uri("Images/starprime.png", UriKind.Relative));
            expandStar5.Source = book.star5
               ? new BitmapImage(new Uri("Images/star.png", UriKind.Relative))
               : new BitmapImage(new Uri("Images/starprime.png", UriKind.Relative));
            listaLibrariei.ItemsSource = null;
            listaLibrariei.ItemsSource = Books;
        }

        private void lupaImg_MouseEnter(object sender, MouseEventArgs e)
        {
            lupaImg.Margin = new Thickness(0, 8, 0, 8);
        }
        private void lupaImg_MouseLeave(object sender, MouseEventArgs e)
        {
            lupaImg.Margin = new Thickness(0, 10, 0, 10);
        }

        private void acountPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            accountStackPanel.Margin = new Thickness(0);
            acountPanel.Visibility = Visibility.Collapsed;
            downImg.Source = new BitmapImage(new Uri(@"Images/down-arrow-white.png", UriKind.Relative));
        }
    }
}
