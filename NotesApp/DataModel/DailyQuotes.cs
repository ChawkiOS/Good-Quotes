using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.DataModel
{
    public class DailyQuotes
    {
        public string QuoteTitle { get; set; }
        public string QuoteText { get; set; }
        public string QuoteAuthor { get; set; }

        public static List<DailyQuotes> feedDataQuotesDay = new List<DailyQuotes>();
        public static List<DailyQuotes> feedDataPopularQuotes = new List<DailyQuotes>();

        public static List<DailyQuotes> SearchInQuotesDay(string queryText)
        {
            return feedDataQuotesDay.Where(p => p.QuoteText.ToLower().Contains(queryText.ToLower()) || p.QuoteAuthor.ToLower().Contains(queryText.ToLower())).ToList();
        }

        public static List<DailyQuotes> SearchInPopularQuotes(string queryText)
        {
            return feedDataPopularQuotes.Where(p => p.QuoteAuthor.ToLower().Contains(queryText.ToLower()) || p.QuoteAuthor.ToLower().Contains(queryText.ToLower())).ToList();
        }
    }
}
