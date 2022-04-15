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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Librarie_online
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ModelConnection db;
        public MainWindow()
        {
            InitializeComponent();
            db = new ModelConnection();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Register_user register_User = new Register_user(0);
            register_User.Show();
            this.Close();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            User user = db.Users.FirstOrDefault(x => x.email == email.Text && x.parola == password.Password);
            if (user != null)
            {
                if (user.isAdmin)
                {
                    Administrator administrator = new Administrator(user.Id);
                    administrator.Show();
                    this.Close();
                }
                else
                {
                    Utilizatori utilizatori = new Utilizatori(user.Id);
                    utilizatori.Show();
                    this.Close();
                }
            }
            else MessageBox.Show("Email sau parola invalida", "Atentie!"); 
        }
    }
}
