using GoodQuotes.DataModel;
using GoodQuotes.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;


namespace GoodQuotes.Views
{

    public sealed partial class Loading : GoodQuotes.Common.LayoutAwarePage
    {
        XmlDocument dom = new XmlDocument();
        XmlElement x;

        public Loading()
        {
            this.InitializeComponent();

            XmlComment dec = dom.CreateComment("This is data of all Quotes of the day");
            dom.AppendChild(dec);

            x = dom.CreateElement("Quotes");
            dom.AppendChild(x);
        }

        

        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            await Initialise();
            //await getQuotes();
            //await getAuthors();
            //await getQuoteTopics();

            //Statique._HubPageViewModel.setWidth(Window.Current.Bounds.Width);
            
            //this.Frame.Navigate(typeof(HubPage));
        }

        private async Task Initialise()
        {
                Windows.Storage.StorageFolder sf = await ApplicationData.Current.LocalFolder.CreateFolderAsync("GoodQuotesFolder", CreationCollisionOption.OpenIfExists);
                StorageFile stf = await sf.CreateFileAsync("DailyQuoteFile.xml", CreationCollisionOption.OpenIfExists);
                StorageFile stfs = await sf.CreateFileAsync("FavoriteQuotesFile.xml", CreationCollisionOption.OpenIfExists);
                StorageFile st = await sf.CreateFileAsync("MyQuotesFile.xml", CreationCollisionOption.OpenIfExists);

                await getQuotes();
        }

        private async Task getQuotes()
        {
            if (!ConnectedToInternet())
            {
                await getLocalQuotes();
            }
            else
            {
                await getDailyQuotesRSS("http://feeds.feedburner.com/brainyquote/QUOTEBR");
                await getDailyQuotesRSS("http://feeds.feedburner.com/brainyquote/QUOTEAR");
                await getDailyQuotesRSS("http://feeds.feedburner.com/brainyquote/QUOTEFU");
                await getDailyQuotesRSS("http://feeds.feedburner.com/brainyquote/QUOTELO");
                await getDailyQuotesRSS("http://feeds.feedburner.com/brainyquote/QUOTENA");

                Statique._HubPageViewModel.InsertListDailyQuotes(DataModel.DailyQuotes.feedDataQuotesDay);
                Statique._HubPageViewModel.InsertListPopularQuotes(DataModel.DailyQuotes.feedDataPopularQuotes);
            }
            await getAuthors();
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        //Local Quotes (local)
        private async Task getLocalQuotes()
        {
            StorageFolder storageFldr = Windows.Storage.ApplicationData.Current.LocalFolder;
            storageFldr = await storageFldr.GetFolderAsync("GoodQuotesFolder");
            StorageFile storageFile = await storageFldr.GetFileAsync("DailyQuoteFile.xml");
            var xml = await FileIO.ReadTextAsync(storageFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            try
            {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
                    XmlNodeList ListItemsQuotesDay = xDoc.SelectNodes("/Quotes/QuotesDay");
                    XmlNodeList ListItemsPolumarQuotes = xDoc.SelectNodes("/Quotes/PopularQuotes");

                    foreach (var item in ListItemsQuotesDay)
                    {
                        DailyQuotes feedItem = new DailyQuotes();

                        feedItem.QuoteTitle = item.SelectSingleNode("Title").InnerText.ToString();
                        feedItem.QuoteText = item.SelectSingleNode("Text").InnerText.ToString();
                        feedItem.QuoteAuthor = item.SelectSingleNode("Author").InnerText.ToString();

                        DailyQuotes.feedDataQuotesDay.Add(feedItem);
                    }

                    foreach (var item in ListItemsPolumarQuotes)
                    {
                        DailyQuotes feedItem = new DailyQuotes();

                        feedItem.QuoteTitle = item.SelectSingleNode("Title").InnerText.ToString();
                        feedItem.QuoteText = item.SelectSingleNode("Text").InnerText.ToString();
                        feedItem.QuoteAuthor = item.SelectSingleNode("Author").InnerText.ToString();

                        DailyQuotes.feedDataPopularQuotes.Add(feedItem);
                    }

                    Statique._HubPageViewModel.InsertListDailyQuotes(DataModel.DailyQuotes.feedDataQuotesDay);
                    Statique._HubPageViewModel.InsertListPopularQuotes(DataModel.DailyQuotes.feedDataPopularQuotes);
                }
            catch {
                MessageDialog msgDialog = new MessageDialog("Unable to load data. Please check your connection and try again.", "No Internet connection!");
                UICommand CloseBtn = new UICommand("Close");
                CloseBtn.Invoked = CloseBtnClick;
                msgDialog.Commands.Add(CloseBtn);
                msgDialog.ShowAsync();
            }
        }

        private void CloseBtnClick(IUICommand command)
        {
            App.Current.Exit();
        }

        //Quote Topics (local)
        private async Task getQuoteTopics()
        {
            StorageFolder storageFldr = Package.Current.InstalledLocation;
            storageFldr = await storageFldr.GetFolderAsync("File");
            StorageFile storageFile = await storageFldr.GetFileAsync("QuoteTopicsFile.xml");
            var xml = await FileIO.ReadTextAsync(storageFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            
            XmlNodeList ListItems = xDoc.SelectNodes("/topics/topic");
            foreach (var item in ListItems)
            {
                QuotesTopic quoteTopic = new QuotesTopic();
                quoteTopic.TopicText = item.SelectSingleNode("name").InnerText.ToString();
                quoteTopic.TopicLink = item.SelectSingleNode("link").InnerText.ToString();
                QuotesTopic.feedDataQuotesTopic.Add(quoteTopic);
            }
            Statique._HubPageViewModel.InsertTopicsList(QuotesTopic.feedDataQuotesTopic);

            //await Task.Delay(TimeSpan.FromSeconds(0.5));
            Statique._HubPageViewModel.setWidth(Window.Current.Bounds.Width);

            this.Frame.Navigate(typeof(HubPage), "0");
        }

        //Authors (local)
        private async Task getAuthors()
        {
            StorageFolder storageFldr = Package.Current.InstalledLocation;
            storageFldr = await storageFldr.GetFolderAsync("File");
            StorageFile storageFile = await storageFldr.GetFileAsync("AuthorsFile.xml");
            var xml = await FileIO.ReadTextAsync(storageFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            XmlNodeList ListItems = xDoc.SelectNodes("/authors/lettre");
            foreach (var item in ListItems)
            {
                Authors authre = new Authors();
                authre.AuthorName = item.Attributes.GetNamedItem("text").NodeValue.ToString();
                Authors.feedDataAuthors.Add(authre);
                XmlNodeList ListXMLCote = item.SelectNodes("author");
                foreach (var row in ListXMLCote)
                {
                    Authors auth = new Authors();
                    auth.AuthorName = row.SelectSingleNode("name").InnerText.ToString().Replace("\n", "").TrimStart(' ');
                    auth.AuthorLink = row.SelectSingleNode("link").InnerText.ToString().Replace("\n", "").TrimStart(' ').TrimEnd(' ');
                    Authors.feedDataAuthors.Add(auth);
                }
            }
            Statique._AuthorsViewModel.InsertAuthorsList(Authors.feedDataAuthors);

            await getQuoteTopics();
        }

        //Quotes of the day
        private async Task getDailyQuotesRSS(string link)
        {
            XDocument rss = XDocument.Load(link);
            IEnumerable<XElement> items = rss.Element("rss").Element("channel").Elements("item");
            XElement TitleElement = rss.Element("rss").Element("channel");

            for (int i = 0; i < items.Count(); i++)
            {
                DailyQuotes feedItem = new DailyQuotes();

                feedItem.QuoteTitle = TitleElement.Element("title").Value;
                feedItem.QuoteText = items.ElementAt(i).Element("description").Value;
                feedItem.QuoteAuthor = items.ElementAt(i).Element("title").Value;

                XmlElement x1;
                if (i == 0)
                {
                    DailyQuotes.feedDataQuotesDay.Add(feedItem);
                    x1 = dom.CreateElement("QuotesDay");
                }
                else
                {
                    DailyQuotes.feedDataPopularQuotes.Add(feedItem);
                    x1 = dom.CreateElement("PopularQuotes");
                }
                    
                    XmlElement x11 = dom.CreateElement("Title");
                    x11.InnerText = feedItem.QuoteTitle;
                    x1.AppendChild(x11);

                    XmlElement x12 = dom.CreateElement("Text");
                    x12.InnerText = feedItem.QuoteText;
                    x1.AppendChild(x12);

                    XmlElement x13 = dom.CreateElement("Author");
                    x13.InnerText = feedItem.QuoteAuthor;
                    x1.AppendChild(x13);

                    x.AppendChild(x1);
            }
            try
            {
                Windows.Storage.StorageFolder sf = await ApplicationData.Current.LocalFolder.CreateFolderAsync("GoodQuotesFolder", CreationCollisionOption.OpenIfExists);
                StorageFile st = await sf.CreateFileAsync("DailyQuoteFile.xml", CreationCollisionOption.OpenIfExists);
                await dom.SaveToFileAsync(st);
            }
            catch { }
        }

        //Verify Internet Connection
        private bool ConnectedToInternet()
        {
            ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

            if (InternetConnectionProfile == null)
            {
                return false;
            }

            var level =
            InternetConnectionProfile.GetNetworkConnectivityLevel();

            return level == NetworkConnectivityLevel.InternetAccess;
        }
    }
}
