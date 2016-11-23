using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodQuotes.DataModel
{
    public class Authors
    {
        public string AuthorName { get; set; }
        public string AuthorLink { get; set; }

        public static List<Authors> feedDataAuthors = new List<Authors>();
    }
}
