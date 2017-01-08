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
    /// Interaction logic for Cart.xaml
    /// </summary>
    public partial class Cart : Window {

        public Cart() {
            
            InitializeComponent();

            decimal totalPrice = 0;
            List<Products> products = new List<Products>();

            for (int i = 0 ; i < Order.giveMeTHisFuckingID().Count; i++){
                totalPrice = showProducts(Order.giveMeTHisFuckingID()[i], products, totalPrice);
                
            }

            foreach (Products p in products) {
                myListView.Items.Add(p);
            }

            summary.Text = totalPrice.ToString();

        }

        public decimal showProducts(int productID, List<Products> products, decimal totalPrice) {
            myListView.Items.Clear();
            ListViewItem lvl = new ListViewItem();
            
            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command = new SqlCommand("select * from Produkty FULL OUTER JOIN Stan_magazynu ON Produkty.ID_produktu = Stan_magazynu.ID_produktu WHERE Produkty.ID_produktu = " + productID, conn);

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

                    totalPrice += price;
                    

                    products.Add(new Products() {
                        ID = ID,
                        name = name,
                        type = type,
                        description = description,
                        price = price,
                        buyPrice = buyPrice,
                        codeID = codeID,
                        amount = amount
                    });
                }
            }
            catch (SqlException) {
            }

            return totalPrice;
        }

        private void pay_Click(object sender, RoutedEventArgs e) {

        }

        private void codeTextBlock_GotFocus(object sender, RoutedEventArgs e){
            codeTextBlock.Text = "";
        }
    }
}
