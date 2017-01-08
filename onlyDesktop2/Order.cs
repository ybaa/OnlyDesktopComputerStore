using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace onlyDesktop2 {
    class Order
    {
        //public static List<int> productsID { get; set; }
        //private static List<int> productsID;

        //public void setProductsID(int i)
        //{
        //    productsID.Add(i);
        //}

        //public List<int> getProductsID()
        //{
        //    return productsID;

        //}

        private static List<int> productID = new List<int>();

        public static void addProduct(int p) {
            productID.Add(p);
        }

        public static List<int> giveMeTHisFuckingID() {
            return productID;
        }

        //ublic int ID { get; set; }



    }
}
