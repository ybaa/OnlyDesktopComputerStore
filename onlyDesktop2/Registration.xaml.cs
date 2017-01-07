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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window {
        public Registration() {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string surname = surnameTextBox.Text;
            string mail = mailTextBox.Text;
            string password = passwordTextBox.Text;
            string city = cityTextBox.Text;
            string zipCode = zipCodeTextBox.Text;
            string street = streetTextBox.Text;
            int homeNumber = int.Parse(homeNumberTextBox.Text);
            int localNumber = int.Parse(localNumberTextBox.Text);

            addNewClient(name, surname, mail, password, city, zipCode, street, homeNumber, localNumber);
            this.Close();

        }

        public void addNewClient(string name, string surname, string mail, string password, string city, string zipCode,
            string street, int homeNumber, int localNumber)
        {
            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command = new SqlCommand("INSERT INTO Klienci(Imie, Nazwisko, Mail, Haslo) VALUES('" + name + "','" +surname +"','" + mail + "','" + password +"');", conn);

            SqlCommand command2 = new SqlCommand("INSERT INTO Adresy(Miejscowosc, Kod_pocztowy, Ulica, Nr_domu, Nr_mieszkania) VALUES('" + city + "','" + zipCode + "','" + street + "','" + homeNumber + "','" + localNumber + "');", conn);
            try {
                conn.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Twoje konto zostalo utowrzone");
            }
            catch (SqlException) {
            }
        }
        
    }
}
