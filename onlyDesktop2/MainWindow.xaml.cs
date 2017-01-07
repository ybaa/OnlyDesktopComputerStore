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



        public MainWindow() {
            InitializeComponent();
        }



        private void showAllProductsButton_Click(object sender, RoutedEventArgs e) {
            ListViewItem lvl = new ListViewItem();
            List<Products> products = new List<Products>();


            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");

            SqlCommand command = new SqlCommand("SELECT * FROM Produkty", conn);
            try {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read()) {
                    string ID = reader["ID_produktu"].ToString();
                    string name = reader["Nazwa_produktu"].ToString();
                    string type = reader["Typ_produktu"].ToString();
                    string description = reader["Opis_produktu"].ToString();
                    decimal price = 0;

                    products.Add(new Products() { ID = ID, name = name, type = type, description = description, price = price });

                }

            }
            catch (SqlException) {

            }

            foreach (Products p in products) {
                myListView.Items.Add(p);
            }

        }
    }
    
}
