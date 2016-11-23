using GoodQuotes.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Popups;

namespace GoodQuotes.ViewModel
{
    class MyQuotesViewModel
    {
         public ObservableCollection<Quotes> QuotesCollectionItem { get; set; }

         public MyQuotesViewModel()
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
            Windows.Storage.StorageFolder storageFldr = await ApplicationData.Current.LocalFolder.GetFolderAsync("GoodQuotesFolder");
            StorageFile storageFile = await storageFldr.GetFileAsync("MyQuotesFile.xml");
                var xml = await FileIO.ReadTextAsync(storageFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                try
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xml);

                    XmlNodeList ListItems = xDoc.SelectNodes("/Quotes/Quote");
                    //Quotes.feedDataFavoriteQuotes.Clear();
                    List<Quotes> ListQuote = new List<Quotes>();
                    ListQuote.Clear();
                    foreach (var item in ListItems)
                    {
                        Quotes quote = new Quotes();
                        quote.QuoteText = item.SelectSingleNode("Text").InnerText.ToString();
                        quote.QuoteAuthor = item.SelectSingleNode("Author").InnerText.ToString();
                        quote.QuotesTags = item.SelectSingleNode("Tags").InnerText.ToString();
                        //Quotes.feedDataFavoriteQuotes.Add(quote);
                        ListQuote.Add(quote);
                    }
                    //InsertQuotesList(Quotes.feedDataFavoriteQuotes);
                    InsertQuotesList(ListQuote);
                }
                catch
                {
                    MessageDialog msgDialog = new MessageDialog("Unable to load data. Please check your connection and try again.", "Oups!");
                    UICommand CloseBtn = new UICommand("Close");
                    CloseBtn.Invoked = CloseBtnClick;
                    msgDialog.Commands.Add(CloseBtn);
                    //msgDialog.ShowAsync();
                }
        }

        private void CloseBtnClick(IUICommand command)
        {
            App.Current.Exit();
        }


        public bool ExistQuote(Quotes selectedQuotes)
        {
            foreach (var item in QuotesCollectionItem)
            {
                if (item.QuoteText.Equals(selectedQuotes.QuoteText))
                    return true;
            }
            return false;
        }




        public async void addQuotes(Quotes selectedQuotes)
        {
            Windows.Storage.StorageFolder storageFldr = await ApplicationData.Current.LocalFolder.GetFolderAsync("GoodQuotesFolder");
            StorageFile storageFile = await storageFldr.GetFileAsync("MyQuotesFile.xml");
            var xml = await FileIO.ReadTextAsync(storageFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);

            List<Quotes> ListQuote = new List<Quotes>();

            XmlDocument dom = new XmlDocument();
            XmlElement x;

            XmlComment dec = dom.CreateComment("This is data of all My Quotes");
            dom.AppendChild(dec);

            x = dom.CreateElement("Quotes");
            dom.AppendChild(x);

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(xml);
                ListQuote.Clear();
                XmlNodeList ListItems = xDoc.SelectNodes("/Quotes/Quote");
                foreach (var item in ListItems)
                {
                    Quotes quote = new Quotes();
                    quote.QuoteText = item.SelectSingleNode("Text").InnerText.ToString();
                    quote.QuoteAuthor = item.SelectSingleNode("Author").InnerText.ToString();
                    quote.QuotesTags = item.SelectSingleNode("Tags").InnerText.ToString();
                    ListQuote.Add(quote);
                }
            }
            catch { }
            
            ListQuote.Add(selectedQuotes);

            foreach (var item in ListQuote)
            {
                    XmlElement x1 = dom.CreateElement("Quote");

                    XmlElement x11 = dom.CreateElement("Tags");
                    x11.InnerText = item.QuotesTags;
                    x1.AppendChild(x11);

                    XmlElement x12 = dom.CreateElement("Text");
                    x12.InnerText = item.QuoteText;
                    x1.AppendChild(x12);

                    XmlElement x13 = dom.CreateElement("Author");
                    x13.InnerText = item.QuoteAuthor;
                    x1.AppendChild(x13);

                    x.AppendChild(x1);
                }

            InsertQuotesList(ListQuote);

                    Windows.Storage.StorageFolder sf = await ApplicationData.Current.LocalFolder.CreateFolderAsync("GoodQuotesFolder", CreationCollisionOption.OpenIfExists);
                    StorageFile st = await sf.CreateFileAsync("MyQuotesFile.xml", CreationCollisionOption.ReplaceExisting);
                    await dom.SaveToFileAsync(st);
                    //dom.RemoveChild(x);
                    
        }


    }
}
