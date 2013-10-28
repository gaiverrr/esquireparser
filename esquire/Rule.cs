using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;


namespace esquire {
    class Rule {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Signature { get; set; }
        public List<string> ImgUrls {
            get { return imgUrls; }
            set { imgUrls = value; }
        }
        public List<byte[]> Imgs {
            get { return imgs; }
            set { imgs = value; }
        }
        private List<string> imgUrls = new List<string>();
        private List<byte[]> imgs = new List<byte[]>();
          
        
               
    }
}
