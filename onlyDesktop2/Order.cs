using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace onlyDesktop2 {
    class Order {
 
        //private static List<int> productID = new List<int>();

        private static List<List<int>> product = new List<List<int>>();
        private static List<decimal> prices = new List<decimal>();

        public static void addProduct(int p,int i, decimal d) {
            List<int> IDAndAmount = new List<int>();
            IDAndAmount.Add(p);
            IDAndAmount.Add(i);
            prices.Add(d);
            product.Add(IDAndAmount);
        }

        public static void removeProduct(int id)
        {
            for (int i = 0; i < product.Count(); i++)
            {
                if (product[i][0] == id)
                {
                    product.RemoveAt(i);
                }
            }
        }

        public static List<List<int>> giveMeProduct() {
            return product;
        }

        public static void setPiecesOfProduct(int ID, int value) {
            for (int i = 0; i < product.Count(); i++) {
                if (product[i][0] == ID) {
                    product[i][1] = value;
                }
            }
        }

        public static int giveMeAmountOfProductsWithThisID(int ID) {
            int ret = 1;
            for (int i = 0; i < product.Count(); i++) {
                if (product[i][0] == ID) {
                    ret = product[i][1];
                }
            }
            return ret;
        }

        public static decimal getPrice(int i)
        {
            decimal price;
            price = prices[i] * product[i][1];
            return price;
        }

    }
}
