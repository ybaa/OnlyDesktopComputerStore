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
    /// Interaction logic for NewProduct.xaml
    /// </summary>
    public partial class NewProduct : Window {
        public NewProduct() {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e) {
            string name = nameTextBox.Text;
            string type = typeTextBox.Text;
            string desc = descriptionTextBox.Text;
            string price = priceTextBox.Text;
            string buyprice = buypriceTextBox.Text;

            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command2 = new SqlCommand("select IDENT_CURRENT('Produkty')", conn);
            SqlCommand command =
                new SqlCommand("insert into Produkty(Nazwa_produktu, Typ_produktu, Opis_produktu, Cena, Cena_kupna) values('" + name + "','" + type + "','" + desc + "'," + price + "," + buyprice + ")", conn);

            try {
                conn.Open();
                int x = Convert.ToInt32(command2.ExecuteScalar()) + 1;
                command.ExecuteNonQuery();
                SqlCommand command3 = new SqlCommand("insert into Stan_magazynu(Ilosc_produktu,ID_produktu) values(0," + x + ")", conn);
                command3.ExecuteNonQuery();
                MessageBox.Show("Produkt dodany poprawnie");
                price = price.Replace(".", ",");
                buyprice = buyprice.Replace(".", ",");
                Products.products.Add(new Products() {
                    ID = x,
                    name = name,
                    type = type,
                    description = desc,
                    price = decimal.Parse(price),
                    buyPrice = decimal.Parse(buyprice),
                    codeID = "0",
                    amount = 0,
                    piecesOfProduct = 0
                });
            }
            catch (SqlException) {
            }
            this.Close();

        }
    }
}
