using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace onlyDesktop2 {
    class Order {
 
        private static List<int> productID = new List<int>();

        public static void addProduct(int p) {
            productID.Add(p);
        }

        public static List<int> giveMeTHisFuckingID() {
            return productID;
        }


    }
}
