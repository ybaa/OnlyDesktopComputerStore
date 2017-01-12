using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlyDesktop2 {
    public class Complaint {
        public static List<Complaint> complaints = new List<Complaint>();
        public int ID { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public int addressID { get; set; }
        public int workerID { get; set; }
        public int clinetID { get; set; }
    }
}
