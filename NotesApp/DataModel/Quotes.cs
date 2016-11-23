using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.DataModel
{
    public class Quotes
    {
        public string QuoteText { get; set; }
        public string QuoteAuthor { get; set; }
        public string QuotesTags { get; set; }

        public static List<Quotes> feedDataQuotes = new List<Quotes>();
        public static List<Quotes> feedDataFavoriteQuotes = new List<Quotes>();

        public static List<Quotes> SearchInQuotes(string queryText)
        {
            return feedDataQuotes.Where(p => p.QuotesTags.ToLower().Contains(queryText.ToLower()) || p.QuoteAuthor.ToLower().Contains(queryText.ToLower())).ToList();
        }
    }
}
