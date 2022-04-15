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
    /// Interaction logic for Register_user.xaml
    /// </summary>
    public partial class Register_user : Window
    { 
        ModelConnection db = new ModelConnection();
        int userId;
        string ActualEmail = "";
        bool closing;
        public Register_user(int id)
        {
            InitializeComponent();
            
            userId = id;
            if (id > 0)
            {
                User user = (from querry in db.Users where querry.Id == id select querry).FirstOrDefault();
                nume.Text = user.nume;
                prenume.Text = user.prenume;
                dataNasterii.SelectedDate = user.dataNasterii;
                telefon.Text = user.telefon;
                ActualEmail = email.Text = user.email;
                adresa.Text = user.adresa;
                parola.Password = user.parola;
                confirmareParola.Password = user.parola;
                if (user.isAdmin) { adminType.IsChecked = true; } else {userType.IsChecked = true; }
                if (user.image != "")
                {
                    imageLocation = user.image;
                    image.Source = new BitmapImage(new Uri(user.image));
                }
                validNume = true;
                validPrenume = true;
                validEmail = true;
                validAdress = true;
                validDate = true;
                validPassword = true;
                validTelefon = true;
            }
            Closing += Register_user_Closing;
            Loaded += Register_user_Loaded;
        }

        private void Register_user_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner is Administrator)
            {
                typePanel.Visibility = Visibility.Visible;
            }
        }

        private void Register_user_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
            db.Dispose();
        }

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

        private string imageLocation;
        private void OpenStandartImage_Click(object sender, RoutedEventArgs e)
        {

            image.Source = new BitmapImage(new Uri(@"\Images\incognito.jfif", UriKind.Relative));
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            bool a;
            if (adminType.IsChecked == true) {
                a = true;
            }
            else
            {
                a = false;
            }
            if (validNume == true && validPrenume == true && validDate == true && validTelefon == true && validEmail == true && validAdress == true && validPassword == true)
            { if (Owner is Administrator administrator)
                {
                    if (userId > 0)
                    {
                        User user = (from querry in administrator.db.Users where querry.Id == userId select querry).FirstOrDefault();
                        user.nume = nume.Text;
                        user.prenume = prenume.Text;
                        user.dataNasterii = dataNasterii.DisplayDate;
                        user.telefon = telefon.Text;
                        user.email = email.Text;
                        user.adresa = adresa.Text;
                        user.parola = confirmareParola.Password;
                        user.image = imageLocation;
                        user.isAdmin = a;
                        _ = administrator.db.SaveChanges();

                        if (administrator.idUser == user.Id)
                        {
                            administrator.image.Source = new BitmapImage(new Uri(imageLocation));
                            administrator.nume.Content = nume.Text;
                            administrator.prenume.Content = prenume.Text;
                            administrator.data.Content = dataNasterii.Text;
                            administrator.telefon.Content = telefon.Text;
                            administrator.email.Content = email.Text;
                        }
                    }
                    else
                    {
                        _ = administrator.db.Users.Add(new User()
                        {
                            nume = nume.Text,
                            prenume = prenume.Text,
                            dataNasterii = dataNasterii.DisplayDate,
                            telefon = telefon.Text,
                            email = email.Text,
                            adresa = adresa.Text,
                            parola = confirmareParola.Password,
                            image = imageLocation,
                            isAdmin = a,
                        });
                        _ = administrator.db.SaveChanges();
                    }
                }
                else if (Owner is Utilizatori utilizatori)
                {
                    User user = (from querry in utilizatori.db.Users where querry.Id == userId select querry).FirstOrDefault();
                    user.nume = nume.Text;
                    user.prenume = prenume.Text;
                    user.dataNasterii = dataNasterii.DisplayDate;
                    user.telefon = telefon.Text;
                    user.email = email.Text;
                    user.adresa = adresa.Text;
                    user.parola = confirmareParola.Password;
                    user.image = imageLocation;
                    user.isAdmin = a;
                    utilizatori.image.Source = new BitmapImage(new Uri(imageLocation));
                    utilizatori.nume.Content = nume.Text;
                    utilizatori.prenume.Content = prenume.Text;
                    utilizatori.data.Content = dataNasterii.Text;
                    utilizatori.telefon.Content = telefon.Text;
                    utilizatori.email.Content = email.Text;
                }
                else
                {
                    db.Users.Add(new User()
                    {
                        nume = nume.Text,
                        prenume = prenume.Text,
                        dataNasterii = dataNasterii.DisplayDate,
                        telefon = telefon.Text,
                        email = email.Text,
                        adresa = adresa.Text,
                        parola = confirmareParola.Password,
                        image = imageLocation,
                        isAdmin = a,
                    });
                    db.SaveChanges();
                    MessageBox.Show("Acum urmeaza sa va logati", "Inregistrare reusita");
                    MainWindow main = new MainWindow();
                    main.Show();
                }
                Close();
            }
            else { MessageBox.Show("Nu ati completat corect campurile", "Atentie!"); }
        }
        private bool validNume;
        private bool validPrenume;
        private bool validDate;
        private bool validTelefon;
        private bool validEmail;
        private bool validPassword;
        private bool validAdress;
        private void nume_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Regex.Match(nume.Text, @"^[A-Z][a-z]+$").Success)
            {
                nume.Background = Brushes.Red;
                nume.BorderBrush = Brushes.DarkRed;
                validNume = false;
            }
            else
            {
                nume.Background = Brushes.White;
                nume.BorderBrush = Brushes.BlueViolet;
                validNume = true;
            }
        }

        private void prenume_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Regex.Match(prenume.Text, @"^[A-Z][a-z]+$").Success)
            {
                prenume.Background = Brushes.Red;
                prenume.BorderBrush = Brushes.DarkRed;
                validPrenume = false;
            }
            else
            {
                prenume.Background = Brushes.White;
                prenume.BorderBrush = Brushes.BlueViolet;
                validPrenume = true;
            }
        }

        private void dataNasterii_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dataNasterii.SelectedDate == null)
            {
                dataNasterii.Background = Brushes.Red;
                dataNasterii.BorderBrush = Brushes.DarkRed;
                validDate = false;
            }
            else
            {
                dataNasterii.Background = Brushes.White;
                dataNasterii.BorderBrush = Brushes.BlueViolet;
                validDate = true;
            }
        }

        private void telefon_LostFocus(object sender, RoutedEventArgs e)
        {
            if (telefon.Text == "")
            {
                telefon.Background = Brushes.Red;
                telefon.BorderBrush = Brushes.DarkRed;
                validTelefon = false;
            }
            else if (Convert.ToInt32(telefon.Text.Length) != 9)
            {
                telefon.Background = Brushes.Red;
                telefon.BorderBrush = Brushes.DarkRed;
                validTelefon = false;
            }
            else
            {
                telefon.Background = Brushes.White;
                telefon.BorderBrush = Brushes.BlueViolet;
                validTelefon = true;
            }
        }

        private void telefon_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]+");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Regex.Match(email.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
            {
                email.Background = Brushes.Red;
                email.BorderBrush = Brushes.DarkRed;
                validEmail = false;
            }
            else
            {
                if (!closing)
                {
                    if (db.Users.FirstOrDefault(q => q.email == email.Text) == null || (userId > 0 && email.Text == ActualEmail))
                    {
                        email.Background = Brushes.White;
                        email.BorderBrush = Brushes.BlueViolet;
                        validEmail = true;
                    }
                    else
                    {
                        MessageBox.Show("Email ocupat");
                        email.Background = Brushes.Red;
                        email.BorderBrush = Brushes.DarkRed;
                        validEmail = false;
                    }
                }
            }
        }

        private void parola_LostFocus(object sender, RoutedEventArgs e)
        {
            if (parola.Password.Length < 8)
            {
                parola.Background = Brushes.Red;
                parola.BorderBrush = Brushes.DarkRed;
                MessageBox.Show("Parola trebuie sa contina minim 8 caractere", "Atentie!");
            }
            else
            {
                parola.Background = Brushes.White;
                parola.BorderBrush = Brushes.BlueViolet;
            }
        }

        private void confirmareParola_LostFocus(object sender, RoutedEventArgs e)
        {
            if (confirmareParola.Password != parola.Password)
            {
                confirmareParola.Background = Brushes.Red;
                confirmareParola.BorderBrush = Brushes.DarkRed;
                MessageBox.Show("Parolele nu coincid", "Atentie!");
                validPassword = false;
            }
            else
            {
                confirmareParola.Background = Brushes.White;
                confirmareParola.BorderBrush = Brushes.BlueViolet;
                validPassword = true;
            }
        }

        private void adresa_LostFocus(object sender, RoutedEventArgs e)
        {
            if (adresa.Text.Length < 10)
            {
                adresa.Background = Brushes.Red;
                adresa.BorderBrush = Brushes.DarkRed;
                validAdress = false;
            }
            else
            {
                adresa.Background = Brushes.White;
                adresa.BorderBrush = Brushes.BlueViolet;
                validAdress = true;
            }
        }
    }
}
