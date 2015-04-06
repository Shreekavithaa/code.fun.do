using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diya.Azure
{
    public class items
    {
        public string id { get; set; }
        public string description { get; set; }
        public string emailid { get; set; }
        public string image { get; set; }


        private string _status = "open";
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }
        public string landmark { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public string type { get; set; }

        public string title { get; set; }
    }
}
