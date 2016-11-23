using NotesApp.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    public class AuthorsViewModel
    {
        public ObservableCollection<string> AuthorsCollectionItem { get; set; }

        public AuthorsViewModel()
        {
            AuthorsCollectionItem = new ObservableCollection<string>();
        }

        public void InsertAuthorsList(List<Authors> listAuthors)
        {
            AuthorsCollectionItem.Clear();

            foreach (var item in listAuthors)
            {
                AuthorsCollectionItem.Add(item.AuthorName);
                
            }
        }
    }
}
