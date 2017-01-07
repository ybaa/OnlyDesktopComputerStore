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
    public partial class MainWindow : Window
    {
        public User currentUser = new User();

        public MainWindow() {
            InitializeComponent();
            currentUser.user = Users.Watcher;

            
        }

        

        private void showAllProductsButton_Click(object sender, RoutedEventArgs e){
            
            showProducts("SELECT * FROM Produkty");

        }

        private void printersButton_Click(object sender, RoutedEventArgs e){
            showProducts("SELECT * FROM Produkty WHERE Typ_produktu LIKE 'drukarka'");
        }

        private void laptopsButton_Click(object sender, RoutedEventArgs e){
            showProducts("SELECT * FROM Produkty WHERE Typ_produktu LIKE 'laptop'");
        }

        private void accesoriesButton_Click(object sender, RoutedEventArgs e){
            showProducts("SELECT * FROM Produkty WHERE Typ_produktu LIKE 'akcesoria'");
        }

        private void monitorsButton_Click(object sender, RoutedEventArgs e){
            showProducts("SELECT * FROM Produkty WHERE Typ_produktu LIKE 'monitor'");
            
        }

        public void showProducts(String cmd) {
            myListView.Items.Clear();
            ListViewItem lvl = new ListViewItem();
            List<Products> products = new List<Products>();
            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command = new SqlCommand(cmd, conn);
            
            try {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read()) {
                    string ID = reader["ID_produktu"].ToString();
                    string name = reader["Nazwa_produktu"].ToString();
                    string type = reader["Typ_produktu"].ToString();
                    string description = reader["Opis_produktu"].ToString();
                    decimal price = reader.GetDecimal(4);
                    decimal buyPrice = reader.GetDecimal(5);
                    string codeID = reader["ID_kodu"].ToString();

                    products.Add(new Products() { ID = ID, name = name, type = type, description = description, price = price, buyPrice = buyPrice, codeID = codeID });

                }
            }
            catch (SqlException) {
            }

            foreach (Products p in products) {
                myListView.Items.Add(p);
            }
        }
        
        private void signInButton_Click(object sender, RoutedEventArgs e){

            LogIn logIn = new LogIn();
            logIn.Show();
           
            //MessageBox.Show(logIn.IsVisible.ToString());

            //if (logIn.IsVisible == false) {
            //    MessageBox.Show("asda");
            //    currentUser = LogIn.u;
            //    logIn.Close();

            //    if (currentUser.user == Users.Client) {
            //        signInButton.Visibility = Visibility.Collapsed;
            //        signUpButton.Visibility = Visibility.Collapsed;
            //    }
            //}

            
        }

        private void help_Click(object sender, RoutedEventArgs e) {

            MessageBox.Show("asda");
            currentUser = LogIn.u;
            

            if (currentUser.user == Users.Client) {
                signInButton.Visibility = Visibility.Collapsed;
                signUpButton.Visibility = Visibility.Collapsed;
            }

        }
        }
    
}
