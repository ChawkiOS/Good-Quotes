using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.DataModel
{
    public class QuotesTopic
    {
        public string TopicText { get; set; }
        public string TopicLink { get; set; }

        public static List<QuotesTopic> feedDataQuotesTopic = new List<QuotesTopic>();

        public static List<QuotesTopic> SearchInQuotesTopic(string queryText)
        {
            return feedDataQuotesTopic.Where(p => p.TopicText.ToLower().Contains(queryText.ToLower())).ToList();
        }
    }
}
