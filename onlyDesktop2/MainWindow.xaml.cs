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
        public static int clientID;
        public static string clientName;

        public MainWindow() {
            InitializeComponent();            
            User.user = Users.Watcher;
        }

        

        private void showAllProductsButton_Click(object sender, RoutedEventArgs e){            
            showProducts("SELECT * FROM Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu");

        }

        private void printersButton_Click(object sender, RoutedEventArgs e){
            showProducts("select * from Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu WHERE Typ_produktu LIKE 'drukarka'");
        }

        private void laptopsButton_Click(object sender, RoutedEventArgs e){
            showProducts("select * from Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu WHERE Typ_produktu LIKE 'laptop'");
        }

        private void accesoriesButton_Click(object sender, RoutedEventArgs e){
            showProducts("select * from Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu WHERE Typ_produktu LIKE 'akcesoria'");
        }

        private void monitorsButton_Click(object sender, RoutedEventArgs e){
            showProducts("select * from Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu WHERE Typ_produktu LIKE 'monitor'");
            
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

                while (reader.Read()){
                    int ID = reader.GetInt32(0);
                    string name = reader["Nazwa_produktu"].ToString();
                    string type = reader["Typ_produktu"].ToString();
                    string description = reader["Opis_produktu"].ToString();
                    decimal price = reader.GetDecimal(4);
                    decimal buyPrice = reader.GetDecimal(5);
                    string codeID = reader["ID_kodu"].ToString();                   
                    int amount = reader["Ilosc_produktu"] as int? ?? default(int);                   

                    products.Add(new Products() { ID = ID, name = name, type = type, description = description, price = price, buyPrice = buyPrice, codeID = codeID, amount = amount });
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
            
        }

        private void help_Click(object sender, RoutedEventArgs e) {
            //currentUser = LogIn.u;
            if (User.user == Users.Client) {
                signInButton.Visibility = Visibility.Collapsed;
                signUpButton.Visibility = Visibility.Collapsed;
                helloLabel.Content += clientName;
                helloLabel.Visibility = Visibility.Visible;
            }
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e){
            Registration reg = new Registration();
            reg.Show();
        }
    }
    
}
