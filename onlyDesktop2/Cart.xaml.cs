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

            for (int i = 0; i < Order.giveMeProduct().Count; i++) {
                totalPrice = showProducts(Order.giveMeProduct()[i][0], products, totalPrice);

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
                    int piecesOfProduct = Order.giveMeAmountOfProductsWithThisID(ID);
                    totalPrice += Order.getPriceByID(ID);

                    products.Add(new Products() {
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

            return totalPrice;
        }

        private void pay_Click(object sender, RoutedEventArgs e)
        {
            decimal pricee = decimal.Parse(summary.Text);


            MessageBox.Show("Twoje zamowienie zostało złożone, w celu dokonania zapłaty przejdź do zamówień");
            for (int i = 0; i < Order.giveMeProduct().Count; i++) {
                MessageBox.Show(pricee.ToString());
                addOrderToDatabase(Order.giveMeProduct()[i][0], Order.giveMeProduct()[i][1], pricee );
                MessageBox.Show(pricee.ToString());
            }

        }

        private void addOrderToDatabase(int productID, int piecesOfProducts, decimal price)
        {
            
            int discount = 0;
            decimal deliveryPrice = 0;
            if (price < 100)
                deliveryPrice = 10;

            string priceString = price.ToString();
            priceString = priceString.Replace(",", ".");
            string deliveryPriceString = deliveryPrice.ToString();
            deliveryPriceString = deliveryPriceString.Replace(",", ".");
            


            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command = new SqlCommand("update Stan_magazynu set Ilosc_produktu = Ilosc_produktu - " + piecesOfProducts + " where ID_produktu = " + productID, conn);
            SqlCommand command2 = new SqlCommand("insert into paki(Cena_przesylki, Cena_calkowita) Values(" +deliveryPriceString + ", " + priceString +")" , conn);
            SqlCommand command3 = new SqlCommand("select IDENT_CURRENT('Zamowienia')", conn);
            SqlCommand command4 = new SqlCommand("select COUNT(*) from Pracownicy ", conn);
            SqlCommand command5 = new SqlCommand("select Adresy.ID_Adresu from Adresy full outer join Klienci_Adresy on Adresy.ID_Adresu = Klienci_Adresy.ID_Adresu full outer join Klienci on Klienci.ID_klienta = Klienci_Adresy.ID_klienta where Klienci.ID_klienta = " + MainWindow.clientID, conn);
            try {
               
                conn.Open();
                command.ExecuteNonQuery();                
                command2.ExecuteNonQuery();
                
                int x = Convert.ToInt32(command3.ExecuteScalar());

                int numberOfWorkers = Convert.ToInt32(command4.ExecuteScalar());
                
                Random rand = new Random();
                int worker = rand.Next(1,numberOfWorkers);

                int addressID = Convert.ToInt32(command5.ExecuteScalar());

                DateTime thisDay = DateTime.Today;
                for (int i = 0; i < Order.giveMeProduct().Count; i++)
                {
                    string priceX = Order.getPrice(i).ToString();
                    priceX = priceX.Replace(",", ".");
                    SqlCommand commandHelp = new SqlCommand("insert into Zamowienia(Data_zlozenia, Status_zamowienia,Ilosc_produktu,  Cena_produktu, Rabat,ID_Pracownika, ID_klienta, ID_Adresu, ID_paczki, ID_produktu) " +
                                                            "values('" + thisDay + "'," + "'zlozono',"+ Order.giveMeProduct()[i][1] +","+ priceX +","+discount+","+worker+","+MainWindow.clientID+","+addressID+","+ x+ "," + Order.giveMeProduct()[i][0] + ")", conn);
                    commandHelp.ExecuteNonQuery();
                }
                //MessageBox.Show(thisDay.ToString());
            }
            catch (SqlException) {
            }
        }


        private void codeTextBlock_GotFocus(object sender, RoutedEventArgs e) {
            codeTextBox.Text = "";
        }

        private void incrementAmountOfProductButton_Click(object sender, RoutedEventArgs e) {
            dynamic selectedItem = myListView.SelectedItem;

            if (selectedItem == null) { // no items selected
            }
            else {
                Products p = new Products();
                p = selectedItem;
                if (p.piecesOfProduct >= p.amount) {
                    MessageBox.Show("Nie możesz kupic wiecej tego produktu");
                }
                else {
                    p.piecesOfProduct++;
                    myListView.SelectedItem = p;
                    Order.setPiecesOfProduct(p.ID, p.piecesOfProduct);
                    decimal totalPrice = decimal.Parse(summary.Text);
                    totalPrice += p.price;
                    summary.Text = totalPrice.ToString();
                    myListView.Items.Refresh();
                }
            }
        }

        private void decrementAmountOfProductButton_Click(object sender, RoutedEventArgs e) {
            dynamic selectedItem = myListView.SelectedItem;

            if (selectedItem == null) { // no items selected
            }
            else {
                Products p = new Products();
                p = selectedItem;
                if (p.piecesOfProduct == 1) {
                    myListView.Items.Remove(p);
                    Order.removeProduct(p.ID);

                }
                else {
                    p.piecesOfProduct--;
                    myListView.SelectedItem = p;

                }
                decimal totalPrice = decimal.Parse(summary.Text);
                totalPrice -= p.price;
                summary.Text = totalPrice.ToString();
                myListView.Items.Refresh();
            }
        }

        private void discountCodButton_Click(object sender, RoutedEventArgs e) {
            dynamic selectedItem = myListView.SelectedItem;
            int IDOfSelectedProduct = selectedItem.ID;
            string code = codeTextBox.Text;

            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command = new SqlCommand("select Wielkosc_znizki from Kody_promocyjne full outer join Produkty  on Produkty.ID_Kodu = Kody_promocyjne.ID_kodu where Haslo_dostepu = '" + code + "' and Produkty.ID_produktu = " + IDOfSelectedProduct, conn);
            

            try {
                conn.Open();
                int discount = Convert.ToInt32(command.ExecuteScalar()) as int? ?? default(int);    //jak jest null to zwróci piękne 0

                //for (int i = 0; i < Order.giveMeProduct().Count; i++) {
                //    string priceX = Order.getPrice(i).ToString();
                //    priceX = priceX.Replace(",", ".");

                //}


                double  price = Convert.ToDouble( Order.getPriceByID(IDOfSelectedProduct));
                double oldPrice = price;
                price = price * (1 - discount*0.01);
                string priceS = price.ToString();
                priceS = priceS.Replace(",", ".");
                MessageBox.Show(priceS);

                //Order.updatePrice(IDOfSelectedProduct, Convert.ToDecimal(price));
                MessageBox.Show(Order.updatePrice(IDOfSelectedProduct, Convert.ToDecimal(price)).ToString());

                oldPrice = oldPrice - price;

                decimal totalPrice = decimal.Parse(summary.Text);
                totalPrice -= Convert.ToDecimal(oldPrice);
                
                summary.Text = totalPrice.ToString();


                myListView.Items.Refresh();


            }
            catch (SqlException) {

                
            }
        }
    }

}
