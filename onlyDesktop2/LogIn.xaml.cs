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

        public LogIn() {
            InitializeComponent();
        }

        private void mailTextBox_GotFocus(object sender, RoutedEventArgs e) {
            mailTextBox.Text = "";
        }



        private void logInButton_Click(object sender, RoutedEventArgs e) {
            
            string password = passwordTextBox.Password;
            string username = mailTextBox.Text;
            searchInDatabase(username, password);
            this.Close();
        }

        public void searchInDatabase(string username, string password) {
            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");

            SqlCommand command = new SqlCommand("SELECT * FROM Klienci WHERE Mail = '" + username + "'", conn);
            SqlCommand command2 = new SqlCommand("SELECT * FROM Pracownicy WHERE Mail = '" + username + "'", conn);

            int checkIfWorkerOrClientFound = 0;
            try {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    string usernameRead = reader["Mail"].ToString();
                    if (usernameRead == username) {
                        string pass = reader["Haslo"].ToString();
                        if (pass == password) {
                            MainWindow.clientID = reader.GetInt32(0);
                            MainWindow.clientName = reader["Imie"].ToString();
                            User.user = Users.Client;
                            checkIfWorkerOrClientFound = 1;
                        }
                        else {
                            
                            checkIfWorkerOrClientFound = 2;
                        }
                    }
                }
                if (checkIfWorkerOrClientFound == 0) {
                    reader.Close();
                    SqlDataReader reader2 = command2.ExecuteReader();

                    while (reader2.Read()) {

                        string usernameRead2 = reader2["Mail"].ToString();
                        if (usernameRead2 == username) {
                            string pass = reader2["Haslo"].ToString();
                            if (pass == password)
                            {
                                MainWindow.clientID = reader2.GetInt32(0);
                                MainWindow.clientName = reader2["Imie"].ToString();
                                User.user = Users.Worker;
                                checkIfWorkerOrClientFound = 1;

                            }
                            else
                            {
                                
                                checkIfWorkerOrClientFound = 2;
                            }
                        }
                    }
                    reader2.Close();
                }
                
            }
            catch (SqlException) {
            }

            if (checkIfWorkerOrClientFound == 0)
                MessageBox.Show("Nie ma takiego użytkownika lub danie nieporpawne");
            else if (checkIfWorkerOrClientFound == 2)
            {
                MessageBox.Show("Hasło niepoprawne");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {            

        }
    }
}
