using System;
using System.Collections.Generic;
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
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace onlyDesktop2 {


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public static int clientID;
        public static string clientName;
        public int amountOfThingsInCart = 0;


        public MainWindow() {
            InitializeComponent();
            User.user = Users.Watcher;
        }

        private void showAllProductsButton_Click(object sender, RoutedEventArgs e) {
            showProducts(
                "SELECT * FROM Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu");

        }

        private void printersButton_Click(object sender, RoutedEventArgs e) {
            showProducts(
                "select * from Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu WHERE Typ_produktu LIKE 'drukarka'");

        }

        private void laptopsButton_Click(object sender, RoutedEventArgs e) {
            showProducts(
                "select * from Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu WHERE Typ_produktu LIKE 'laptop'");

        }

        private void accesoriesButton_Click(object sender, RoutedEventArgs e) {
            showProducts(
                "select * from Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu WHERE Typ_produktu LIKE 'akcesoria'");

        }

        private void monitorsButton_Click(object sender, RoutedEventArgs e) {
            showProducts(
                "select * from Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu WHERE Typ_produktu LIKE 'monitor'");

        }

        public void showProducts(String cmd) {
            myListView.Items.Refresh();
            myListView.Items.Clear();
            Products.products.Clear();
            ListViewItem lvl = new ListViewItem();

            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command = new SqlCommand(cmd, conn);

            try {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    int ID = reader.GetInt32(0);
                    string name = reader["Nazwa_produktu"].ToString();
                    string type = reader["Typ_produktu"].ToString();
                    string description = reader["Opis_produktu"].ToString();
                    decimal price = reader.GetDecimal(4);
                    decimal buyPrice = reader.GetDecimal(5);
                    string codeID = reader["ID_kodu"].ToString();
                    int amount = reader["Ilosc_produktu"] as int? ?? default(int);
                    int piecesOfProduct = 1;

                    Products.products.Add(new Products() {
                        ID = ID,
                        name = name,
                        type = type,
                        description = description,
                        price = price,
                        buyPrice = buyPrice,
                        codeID = codeID,
                        amount = amount,
                        piecesOfProduct = piecesOfProduct
                    });
                }
            }
            catch (SqlException) {
            }

            foreach (Products p in Products.products) {
                myListView.Items.Add(p);
            }
        }

        private void signInButton_Click(object sender, RoutedEventArgs e) {

            LogIn logIn = new LogIn();
            logIn.Show();


        }

        private void help_Click(object sender, RoutedEventArgs e) {
            if (User.user == Users.Client) {
                cartButton.Visibility = Visibility.Visible;
                myAccountButton.Visibility = Visibility.Visible;
                signInButton.Visibility = Visibility.Collapsed;
                signUpButton.Visibility = Visibility.Collapsed;
                helloLabel.Content += clientName;
                helloLabel.Visibility = Visibility.Visible;
                logOutButton.Visibility = Visibility.Visible;
            }
            else if (User.user == Users.Worker) {
                helloLabel.Content += clientName;
                helloLabel.Visibility = Visibility.Visible;
                editAmountOfProductButton.Visibility = Visibility.Visible;
                addNewProductButton.Visibility = Visibility.Visible;
                amountOfProductTextBox.Visibility = Visibility.Visible;
                signInButton.Visibility = Visibility.Collapsed;
                signUpButton.Visibility = Visibility.Collapsed;
                checkComplaintsButton.Visibility = Visibility.Visible;
                logOutButton.Visibility = Visibility.Visible;
            }
            else if(User.user == Users.Watcher) {
                cartButton.Visibility = Visibility.Collapsed;
                myAccountButton.Visibility = Visibility.Collapsed;
                signInButton.Visibility = Visibility.Visible;
                signUpButton.Visibility = Visibility.Visible;
                helloLabel.Visibility = Visibility.Collapsed;
                logOutButton.Visibility = Visibility.Collapsed;
                editAmountOfProductButton.Visibility = Visibility.Collapsed;
                addNewProductButton.Visibility = Visibility.Collapsed;
                amountOfProductTextBox.Visibility = Visibility.Collapsed;
                checkComplaintsButton.Visibility = Visibility.Collapsed;
            }
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e) {
            Registration reg = new Registration();
            reg.Show();
        }

        private void addToCartButton_Click(object sender, RoutedEventArgs e) {
            dynamic selectedItem = myListView.SelectedItem;
            int IDOfSelectedProduct = selectedItem.ID;
            int amountOfProduct = selectedItem.amount;
            bool productWasChoodes = false;

            if (User.user == Users.Watcher) {
                MessageBox.Show("Zanim dodasz do koszyka zarejestruj się! ");
            }
            else if (amountOfProduct == 0) {
                MessageBox.Show("Brak produktu w magazynie, wkrótce dostawa!");
            }
            else {
                for (int i = 0; i < Order.giveMeProduct().Count(); i++) {
                    if (IDOfSelectedProduct == Order.giveMeProduct()[i][0]) {
                        MessageBox.Show("Ten produkt zostal juz wybrany, jego ilosc mozesz zmniejszyc w koszyku");
                        productWasChoodes = true;
                        break;
                    }
                }


                if (productWasChoodes == false) {
                    Products p = new Products();
                    p = selectedItem;

                    Order.addProduct(IDOfSelectedProduct, 1, p.price);
                    amountOfThingsInCart++;
                    cartTextBox.Text = "Koszyk " + amountOfThingsInCart.ToString();


                    p.amount--;
                    myListView.SelectedItem = p;
                    myListView.Items.Refresh();
                    //Order.setPiecesOfProduct(IDOfSelectedProduct,);
                }
            }

        }

        private void ButtonCart_Click(object sender, RoutedEventArgs e) {
            Cart cart = new Cart();
            cart.Show();
        }

        private void myAccountButton_Click(object sender, RoutedEventArgs e) {
            MyAccount acc = new MyAccount();
            acc.Show();

        }

        private void editAmountOfProductButton_Click(object sender, RoutedEventArgs e) {
            dynamic selectedItem = myListView.SelectedItem;
            int IDOfSelectedProduct = selectedItem.ID;
            int amount = int.Parse(amountOfProductTextBox.Text);

            if (amount >= 0) {

                SqlConnection conn =
                    new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
                SqlCommand command =
                    new SqlCommand(
                        "update Stan_magazynu set Ilosc_produktu = " + amount + " where ID_produktu = " +
                        IDOfSelectedProduct, conn);

                try {
                    conn.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Ilośc zostala zedytowana");

                    Products p = new Products();
                    p = selectedItem;
                    p.amount = amount;
                    myListView.SelectedItem = p;
                    myListView.Items.Refresh();

                }
                catch (SqlException) {
                }
            }
            else {
                MessageBox.Show("Ujemna ilosc produktu nie jest mozliwa");
            }
        }

        private void addNewProductButton_Click(object sender, RoutedEventArgs e) {
            NewProduct n = new NewProduct();
            n.Show();
        }

        private void amountOfProductTextBox_GotFocus(object sender, RoutedEventArgs e) {
            amountOfProductTextBox.Text = "";
        }

        private void checkComplaintsButton_Click(object sender, RoutedEventArgs e) {
            Complaint.complaints.Clear();
            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");

            SqlCommand command = new SqlCommand("select * from Reklamacje order by ID_pracownika", conn);
            try {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    int ID = reader.GetInt32(0);
                    string status = reader["Status"].ToString();
                    string date = reader["Data_zgloszenia"].ToString();
                    string description = reader["Opis_problemu"].ToString();
                    int addressID = reader.GetInt32(4);
                    int workerID = reader.GetInt32(5);
                    int clientID = reader.GetInt32(6);

                    Complaint.complaints.Add(new Complaint() {
                        ID = ID,
                        status = status,
                        date = date,
                        description = description,
                        addressID = addressID,
                        workerID = workerID,
                        clinetID = clientID
                    });
                }
                ComplaintsWindow cw = new ComplaintsWindow();
                cw.Show();

            }
            catch (SqlException) {

            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e) {
            string phraseToFind = searchTextBox.Text;
            showProducts("select * from Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu WHERE Nazwa_produktu like '%" + phraseToFind + "%'");
        }

        private void logOutButton_Click(object sender, RoutedEventArgs e) {
            User.user = Users.Watcher;
        }
    }
}
