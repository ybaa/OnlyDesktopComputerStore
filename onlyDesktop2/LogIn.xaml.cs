using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace onlyDesktop2 {
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window {

        public static User u { get; set; }

     
        public LogIn() {
            InitializeComponent();
            //u.user = Users.Watcher;
   
        }

        private void mailTextBox_GotFocus(object sender, RoutedEventArgs e){
            mailTextBox.Text = "";
        }

        private void passwordTextBox_GotFocus(object sender, RoutedEventArgs e){
            passwordTextBox.Text = "";
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordTextBox.Text;
            string username = mailTextBox.Text;
            u = searchInDatabase(username, password);
            this.Close();

        }

        public User searchInDatabase(string username, string password) {
            User user = new User();
            user.user = Users.Watcher;

            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command = new SqlCommand("SELECT * FROM Klienci WHERE Mail = '" + username + "'", conn);

            SqlCommand command2 = new SqlCommand("SELECT * FROM Pracownicy WHERE Mail = '" + username + "'"  , conn);
            try {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    string usernameRead = reader["Mail"].ToString();
                    if (usernameRead == username) {
                        string pass = reader["Haslo"].ToString();
                        if (pass == password) {
                            user.user = Users.Client;                        
                            return user;
                        }
                    }
                }

                reader = command2.ExecuteReader();
                while (reader.Read()) {
                    string usernameRead = reader["Mail"].ToString();
                    if (usernameRead == username) {
                        string pass = reader["Haslo"].ToString();
                        if (pass == password) {
                            user.user = Users.Worker;
                            return user;
                        }
                    }
                }


            }
            catch (SqlException) {
            }

            MessageBox.Show("Nie ma takiego użytkownika lub danie nieporpawne");


            return user;
        }

    }
}
