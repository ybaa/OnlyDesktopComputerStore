using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlyDesktop2 {
    public enum Users {
        Client,
        Worker,
        Watcher
    };

   public class User {
       public static Users user { get; set; }
    }
}
