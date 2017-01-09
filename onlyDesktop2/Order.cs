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


        public static void addProduct(int p,int i) {
            List<int> IDAndAmount = new List<int>();
            IDAndAmount.Add(p);
            IDAndAmount.Add(i);
            product.Add(IDAndAmount);
        }

        public static List<List<int>> giveMeProduct() {
            return product;
        }


    }
}
