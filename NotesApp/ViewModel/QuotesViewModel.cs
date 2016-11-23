using NotesApp.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace NotesApp.ViewModel
{
    public class QuotesViewModel
    {
        public ObservableCollection<Quotes> QuotesCollectionItem { get; set; }

        public QuotesViewModel()
        {
            QuotesCollectionItem = new ObservableCollection<Quotes>();
            getQuotes();
        }

        public void InsertQuotesList(List<Quotes> listquotes)
        {
            QuotesCollectionItem.Clear();

            foreach (var item in listquotes)
            {
                QuotesCollectionItem.Add(item);
            }
        }

        private async void getQuotes()
        {
            StorageFolder storageFldr = Package.Current.InstalledLocation;
            storageFldr = await storageFldr.GetFolderAsync("File");
            StorageFile storageFile = await storageFldr.GetFileAsync("QuotesFullFile.xml");
            var xml = await FileIO.ReadTextAsync(storageFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            XmlNodeList ListItems = xDoc.SelectNodes("/Quotes/quote");
            foreach (var item in ListItems)
            {
                Quotes quote = new Quotes();
                quote.QuoteText = item.SelectSingleNode("text").InnerText.ToString();
                quote.QuoteAuthor = item.SelectSingleNode("author").InnerText.ToString();
                quote.QuotesTags = item.SelectSingleNode("tags").InnerText.ToString();
                Quotes.feedDataQuotes.Add(quote);
            }

            InsertQuotesList(Quotes.feedDataQuotes);
        }

    }
}
