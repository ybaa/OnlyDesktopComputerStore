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
    /// Interaction logic for MyAccount.xaml
    /// </summary>
    public partial class MyAccount : Window {
        public MyAccount() {
            InitializeComponent();

            SqlConnection conn =
                new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command =
                new SqlCommand("select * from Adresy full outer join Klienci_Adresy on Adresy.ID_Adresu = Klienci_Adresy.ID_Adresu full outer join Klienci on Klienci.ID_klienta = Klienci_Adresy.ID_klienta where Klienci.ID_klienta =" + MainWindow.clientID, conn);

            try {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    nameTextBox.Text = reader["Imie"].ToString();
                    surnameTextBox.Text = reader["Nazwisko"].ToString();
                    mailTextBox.Text = reader["Mail"].ToString();
                    cityTextBox.Text = reader["Miejscowosc"].ToString();
                    zipCodeTextBox.Text = reader["Kod_pocztowy"].ToString();
                    streetTextBox.Text = reader["Ulica"].ToString();
                    houseNumberTextBox.Text = reader["Nr_domu"].ToString();
                    localNumberTextBox.Text = reader["Nr_mieszkania"].ToString();
                }

            }
            catch (SqlException) {
            }




        }

        List<MyOrders> myOrders = new List<MyOrders>();
        bool areMyOrdersShown = false;

        private void myOrdersButton_Click(object sender, RoutedEventArgs e) {
            showOrHideMyData(Visibility.Collapsed);
            myListView.Visibility = Visibility.Visible;
            showMyOrders(myOrders);
        }

        public void showMyOrders(List<MyOrders> myOrders) {
            SqlConnection conn =
                new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command =
                new SqlCommand(
                    "SELECT DISTINCT paki.ID_paczki, Zamowienia.Data_zlozenia, Zamowienia.Status_zamowienia, paki.Cena_calkowita   FROM Zamowienia FULL OUTER JOIN paki ON Zamowienia.ID_paczki = paki.ID_paczki full outer join Klienci on Klienci.ID_klienta = Zamowienia.ID_klienta   WHERE Klienci.ID_klienta = " +
                    MainWindow.clientID, conn);

            try {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    int ID = reader.GetInt32(0);
                    string orderDate = reader["Data_zlozenia"].ToString();
                    string status = reader["Status_zamowienia"].ToString();
                    decimal price = reader.GetDecimal(3);

                    myOrders.Add(new MyOrders() { ID = ID, orderDate = orderDate, status = status, price = price });
                }

            }
            catch (SqlException) {
            }

            if (areMyOrdersShown == false)
                showMyOrdersInListView(myOrders);
        }

        private void showMyOrdersInListView(List<MyOrders> myOrders) {
            foreach (MyOrders p in myOrders) {
                myListView.Items.Add(p);
            }
            areMyOrdersShown = true;

        }

        private void editDataButton_Click(object sender, RoutedEventArgs e) {
            endableEditing(true);
        }

        private void finishEditionButton_Click(object sender, RoutedEventArgs e) {
            endableEditing(false);

            string name = nameTextBox.Text;
            string surname = surnameTextBox.Text;
            string mail = mailTextBox.Text;
            string city = cityTextBox.Text;
            string zipCode = zipCodeTextBox.Text;
            string street = streetTextBox.Text;
            string houseNumber = houseNumberTextBox.Text;
            string localNumber = localNumberTextBox.Text;

            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command = new SqlCommand("update Klienci set Imie = '" + name + "', Nazwisko = '" + surname + "', Mail = '" + mail + "' where ID_klienta = " + MainWindow.clientID, conn);
            SqlCommand command2 = new SqlCommand("update Adresy set Miejscowosc = '" + city + "',Kod_pocztowy='" + zipCode + "',Ulica='" + street + "',Nr_domu=" + houseNumber + " ,Nr_mieszkania=" + localNumber, conn);
            try {
                conn.Open();
                command.ExecuteNonQuery();
                command2.ExecuteNonQuery();
            }
            catch (SqlException) {

            }


        }

        private void myDataButton_Click(object sender, RoutedEventArgs e) {
            showOrHideMyData(Visibility.Visible);
            endableEditing(false);
            myListView.Visibility = Visibility.Collapsed;
        }

        public void showOrHideMyData(Visibility x) {
            nameTextBox.Visibility = x;
            surnameTextBox.Visibility = x;
            mailTextBox.Visibility = x;
            cityTextBox.Visibility = x;
            zipCodeTextBox.Visibility = x;
            streetTextBox.Visibility = x;
            houseNumberTextBox.Visibility = x;
            localNumberTextBox.Visibility = x;
            editDataButton.Visibility = x;
            finishEditionButton.Visibility = x;
            editPasswordButton.Visibility = x;
            if (x == Visibility.Collapsed)
                x = Visibility.Visible;
            else
                x = Visibility.Collapsed;

            transferDataButton.Visibility = x;
        }

        public void endableEditing(bool enable) {
            nameTextBox.IsEnabled = enable;
            surnameTextBox.IsEnabled = enable;
            mailTextBox.IsEnabled = enable;
            cityTextBox.IsEnabled = enable;
            zipCodeTextBox.IsEnabled = enable;
            streetTextBox.IsEnabled = enable;
            houseNumberTextBox.IsEnabled = enable;
            localNumberTextBox.IsEnabled = enable;
        }

        private void editPasswordButton_Click(object sender, RoutedEventArgs e) {
            passwordTextBox.Visibility = Visibility.Visible;
            passwordTextBox.IsEnabled = true;
            savePasswordButton.Visibility = Visibility.Visible;

        }

        private void savePasswordButton_Click(object sender, RoutedEventArgs e) {
            string pass = passwordTextBox.Text;
            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command = new SqlCommand("update Klienci set Haslo = '" + pass + "' where ID_klienta = " + MainWindow.clientID, conn);
            try {
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException) {

            }

            passwordTextBox.Visibility = Visibility.Collapsed;
            savePasswordButton.Visibility = Visibility.Collapsed;
        }

        private void passwordTextBox_GotFocus(object sender, RoutedEventArgs e) {
            passwordTextBox.Text = "";
        }

        private void transferDataButton_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("dupa_");
            dynamic selectedItem = myListView.SelectedItem;
            int IDOfSelectedProduct = selectedItem.ID;
            decimal price = selectedItem.price;

            MessageBox.Show(price.ToString());

            MessageBox.Show("Proszę wykonac przelew na numer konta:\n 101010230000261395100000252183\n" +
                 "Cena: " + price.ToString() + "\nJako tytuł przelewu proszę wpisać " + "'" + IDOfSelectedProduct.ToString() + "'");
        }

        private void complaintButton_Click(object sender, RoutedEventArgs e) {
            complaintTextBox.IsEnabled = true;
            saveComplaintButton.Visibility = Visibility.Visible;
            

        }

        private void saveComplaintButton_Click(object sender, RoutedEventArgs e) {          
            string complaint = complaintTextBox.Text;
            saveComplaintButton.Visibility = Visibility.Collapsed;
            DateTime thisDay = DateTime.Today;

            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command2 = new SqlCommand("select ID_Adresu from Klienci, Klienci_Adresy where Klienci.ID_klienta = Klienci_Adresy.ID_klienta and Klienci.ID_klienta = " + MainWindow.clientID, conn);
            SqlCommand command3 = new SqlCommand("select COUNT(*) from Pracownicy ", conn);

            try {
                conn.Open();
                int addressID = Convert.ToInt32(command2.ExecuteScalar());
                int numberOfWorkers = Convert.ToInt32(command3.ExecuteScalar());
                Random rand = new Random();
                int worker = rand.Next(1, numberOfWorkers);

                SqlCommand command = new SqlCommand("insert into Reklamacje(Status, Data_zgloszenia, Opis_problemu,ID_Adresu,ID_Pracownika,ID_klienta) values('nierozpatrzona','" + thisDay + "','" + complaint + "'," + addressID + "," + worker + "," + MainWindow.clientID + ")" , conn);
                command.ExecuteNonQuery();
                complaintTextBox.IsEnabled = false;
                complaintTextBox.Text = "Reklamacja została wysłana";
                saveComplaintButton.Visibility = Visibility.Collapsed;

            }
            catch (SqlException) {

            }
        }
    }

}
