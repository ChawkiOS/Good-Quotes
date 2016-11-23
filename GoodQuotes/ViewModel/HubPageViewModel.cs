using GoodQuotes.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodQuotes.ViewModel
{
    public class HubPageViewModel
    {
        public ObservableCollection<DailyQuotes> QuoteDayCollectionItem { get; set; }
        public ObservableCollection<DailyQuotes> PolularQuoteCollectionItem { get; set; }
        public ObservableCollection<QuotesTopic> TopicsCollectionItem { get; set; }

        public double windowWidth { get; set; }
        public double windowWidthNegative { get; set; }

        public HubPageViewModel()
        {
            QuoteDayCollectionItem = new ObservableCollection<DailyQuotes>();
            PolularQuoteCollectionItem = new ObservableCollection<DailyQuotes>();
            TopicsCollectionItem = new ObservableCollection<QuotesTopic>();
        }

        public void InsertList(List<DailyQuotes> listQuotes , int i)
        {
            if (i == 0)
            {
                QuoteDayCollectionItem.Clear();

                foreach (var item in listQuotes)
                {
                    QuoteDayCollectionItem.Add(item);
                }
            }
            else if (i == 1)
            {
                PolularQuoteCollectionItem.Clear();

                foreach (var item in listQuotes)
                {
                    PolularQuoteCollectionItem.Add(item);
                }
            }
        }

        public void InsertTopicsList(List<QuotesTopic> listTopic)
        {
            TopicsCollectionItem.Clear();

            foreach (var item in listTopic)
            {
                TopicsCollectionItem.Add(item);
            }
        }

        public void setWidth(double width)
        {
            width += 100;
            windowWidth = width;
            windowWidthNegative = width * (-1);
        }



        public void InsertListDailyQuotes(List<DailyQuotes> listQuotes)
        {
                QuoteDayCollectionItem.Clear();

                foreach (var item in listQuotes)
                {
                    QuoteDayCollectionItem.Add(item);
                }
        }

        public void InsertListPopularQuotes(List<DailyQuotes> listQuotes)
        {
            PolularQuoteCollectionItem.Clear();

            foreach (var item in listQuotes)
            {
                PolularQuoteCollectionItem.Add(item);
            }
        }
    }
}
