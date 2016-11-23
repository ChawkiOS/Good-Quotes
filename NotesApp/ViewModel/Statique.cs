using NotesApp.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    static class Statique
    {
        public static HubPageViewModel _HubPageViewModel = new HubPageViewModel();

        public static AuthorsViewModel _AuthorsViewModel = new AuthorsViewModel();

        public static QuotesViewModel _QuotesViewModel = new QuotesViewModel();

        public static FavoriteQuotesViewModel _FavoriteQuotesViewModel = new FavoriteQuotesViewModel();

        public static MyQuotesViewModel _MyQuotesViewModel = new MyQuotesViewModel();

        public static AuthorQuotesViewModel _AuthorQuotesViewModel = new AuthorQuotesViewModel();

        public static QuotesTopicViewModel _QuotesTopicViewModel = new QuotesTopicViewModel();
    }
}
