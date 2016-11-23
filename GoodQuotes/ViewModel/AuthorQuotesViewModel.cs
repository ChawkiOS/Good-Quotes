using GoodQuotes.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GoodQuotes.ViewModel
{
    class AuthorQuotesViewModel
    {
        public ObservableCollection<Quotes> AuthorsCollectionItem { get; set; }
        //public string visible { get; set; }
        

        string str;
        List<HtmlAgilityPack.HtmlNode> liste, liste2;

        public AuthorQuotesViewModel()
        {
            //visible = "Visible";
            AuthorsCollectionItem = new ObservableCollection<Quotes>();
            //getList(auth);
        }

        public void InsertAuthorsList(List<Quotes> listQuotes)
        {
            AuthorsCollectionItem.Clear();
            
            foreach (var item in listQuotes)
            {
                AuthorsCollectionItem.Add(item);
            }
        }

        public async Task getList(Authors auth)
        {
            AuthorsCollectionItem.Clear();

            //visible = "Visible";

            List<Quotes> lsQ = new List<Quotes>();
            HttpClient http = new System.Net.Http.HttpClient();
            var reponse = await http.GetByteArrayAsync("http://www.brainyquote.com" + auth.AuthorLink);
            str = Encoding.UTF8.GetString(reponse, 0, reponse.Length - 1);
            var strdecodet = System.Net.WebUtility.HtmlDecode(str);
            HtmlAgilityPack.HtmlDocument document1 = new HtmlAgilityPack.HtmlDocument();
            document1.LoadHtml(str);

            liste = document1.DocumentNode.Descendants("div").Where(x => (x.Attributes.Contains("class") && x.GetAttributeValue("class", "null").Contains("masonryitem boxy bqQt"))).ToList();

            string link2 = auth.AuthorLink.Remove(auth.AuthorLink.Length - 5, 5);
            var reponse2 = await http.GetByteArrayAsync("http://www.brainyquote.com" + link2 +"_2.html");
            str = Encoding.UTF8.GetString(reponse2, 0, reponse2.Length - 1);
            var strdecodet2 = System.Net.WebUtility.HtmlDecode(str);
            HtmlAgilityPack.HtmlDocument document2 = new HtmlAgilityPack.HtmlDocument();
            document2.LoadHtml(str);

            liste2 = document2.DocumentNode.Descendants("div").Where(x => (x.Attributes.Contains("class") && x.GetAttributeValue("class", "null").Contains("masonryitem boxy bqQt"))).ToList();

            foreach (var item in liste2)
            {
                liste.Add(item);
            }

            for (int i = 0; i < liste.Count; i++)
            {
                Quotes quote = new Quotes();

                if (!liste[i].Descendants("a").ToList()[0].InnerText.ToString().Equals(""))
                {
                    quote.QuoteText = liste[i].Descendants("a").ToList()[0].InnerText.ToString();
                    //quote.QuoteAuthor = auth.AuthorName;
                    quote.QuoteAuthor = liste[i].Descendants("a").ToList()[1].InnerText.ToString();

                    lsQ.Add(quote);
                }
            }

            InsertAuthorsList(lsQ);
        }


    }
}
