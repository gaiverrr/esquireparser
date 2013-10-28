using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace esquire {
    class Program {
        static void Main(string[] args) {
            string url = "http://esquire.ru/wil/?adv_types=all";
            var rules = EsquireService.GetRules(url);
            return;
        }
       
    }
}
