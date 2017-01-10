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
        }

        private void myOrdersButton_Click(object sender, RoutedEventArgs e) {
            myListView.Visibility = Visibility.Visible;
            showMyOrders();
        }

        public void showMyOrders() {
            List<MyOrders> myOrders = new List<MyOrders>();
            SqlConnection conn = new SqlConnection("Data Source=MARTYNA-PC;Initial Catalog=SklepKomputerowy;Integrated Security=True");
            SqlCommand command = new SqlCommand("SELECT DISTINCT paki.ID_paczki, Zamowienia.Data_zlozenia, Zamowienia.Status_zamowienia, paki.Cena_calkowita   FROM Zamowienia FULL OUTER JOIN paki ON Zamowienia.ID_paczki = paki.ID_paczki full outer join Klienci on Klienci.ID_klienta = Zamowienia.ID_klienta   WHERE Klienci.ID_klienta = " + MainWindow.clientID, conn);

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

            foreach (MyOrders p in myOrders) {
                myListView.Items.Add(p);
            }
        }
    }
}
