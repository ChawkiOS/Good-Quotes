using GoodQuotes.DataModel;
using GoodQuotes.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace GoodQuotes.Views
{

    public sealed partial class QuotesView : Page
    {
        Quotes selectedQuotes = new Quotes();
        XmlDocument dom = new XmlDocument();
        XmlElement x;

        public QuotesView()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Window_SizeChanged;
            DataContext = Statique._QuotesViewModel;

            XmlComment dec = dom.CreateComment("This is data of all Favorite Quotes");
            dom.AppendChild(dec);

            x = dom.CreateElement("Quotes");
            dom.AppendChild(x);
        }


        private void imgSearch_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgSearch.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri("ms-appx:///Assets/Search2.png"));
        }

        private void imgSearch_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgSearch.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri("ms-appx:///Assets/Search1.png"));
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            var txtSearch = txtboxSearch.Text;
            if (DataModel.Quotes.SearchInQuotes(txtSearch.ToString()).Count == 0)
            {
               
            }
            else
            {
                Statique._QuotesViewModel.InsertQuotesList(DataModel.Quotes.SearchInQuotes(txtSearch.ToString()));
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._QuotesViewModel.InsertQuotesList(DataModel.Quotes.feedDataQuotes);
            }
           
        }

        private void imgSearch_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var txtSearch = txtboxSearch.Text;
            if (DataModel.Quotes.SearchInQuotes(txtSearch.ToString()).Count == 0)
            {

            }
            else
            {
                Statique._QuotesViewModel.InsertQuotesList(DataModel.Quotes.SearchInQuotes(txtSearch.ToString()));
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._QuotesViewModel.InsertQuotesList(DataModel.Quotes.feedDataQuotes);
            }
        }

        private async void gridViewQuotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedQuotes = gridViewQuotes.SelectedItem as DataModel.Quotes;

            try
            {
                MessageDialog msgDialog = new MessageDialog(selectedQuotes.QuoteText, selectedQuotes.QuoteAuthor);
                msgDialog.Commands.Add(new UICommand("Ok"));

                UICommand ShareBtn = new UICommand("Share");
                ShareBtn.Invoked = ShareBtnClick;
                msgDialog.Commands.Add(ShareBtn);


                //UICommand MakePucBtn = new UICommand("Make Picture");
                //MakePucBtn.Invoked = MakePucBtnClick;
                //msgDialog.Commands.Add(MakePucBtn);


                if (!Statique._FavoriteQuotesViewModel.ExistQuote(selectedQuotes))
                {
                    UICommand FavoriteBtn = new UICommand("Favorite");
                    FavoriteBtn.Invoked = FavoriteBtnBtnClick;
                    msgDialog.Commands.Add(FavoriteBtn);
                }


                await msgDialog.ShowAsync();
            }
            catch {}
            
        }

        private void MakePucBtnClick(IUICommand command)
        {
            throw new NotImplementedException();
        }



        //private async Task addToFavorites()
        //{
        //    XmlElement x1 = dom.CreateElement("Quote");

        //    XmlElement x11 = dom.CreateElement("Tags");
        //    x11.InnerText = selectedQuotes.QuotesTags;
        //    x1.AppendChild(x11);

        //    XmlElement x12 = dom.CreateElement("Text");
        //    x12.InnerText = selectedQuotes.QuoteText;
        //    x1.AppendChild(x12);

        //    XmlElement x13 = dom.CreateElement("Author");
        //    x13.InnerText = selectedQuotes.QuoteAuthor;
        //    x1.AppendChild(x13);

        //    x.AppendChild(x1);

        //    Windows.Storage.StorageFolder sf = await ApplicationData.Current.LocalFolder.CreateFolderAsync("GoodQuotesFolder", CreationCollisionOption.OpenIfExists);
        //    StorageFile st = await sf.CreateFileAsync("FavoriteQuotesFile.xml", CreationCollisionOption.OpenIfExists);
        //    await dom.SaveToFileAsync(st);
        //}

        private void FavoriteBtnBtnClick(IUICommand command)
        {
            Statique._FavoriteQuotesViewModel.addQuotes(selectedQuotes);

            //await addToFavorites();
        }

        private void ShareBtnClick(IUICommand command)
        {
            RegisterForShare();
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareTextHandler);
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = selectedQuotes.QuoteAuthor;
            request.Data.Properties.Description = selectedQuotes.QuoteText;
            request.Data.SetText("#GoodQuotes #App" + Environment.NewLine + selectedQuotes.QuoteText + Environment.NewLine + selectedQuotes.QuoteAuthor);
        }

        //Window.Current.SizeChanged += Window_SizeChanged;
        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            DetermineVisualState();
        }
        private void DetermineVisualState()
        {
            var state = string.Empty;
            var applicationView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            var size = Window.Current.Bounds;

            if (applicationView.IsFullScreen)
            {
                state = "FullScreenPortrait";
            }
            else
            {
                state = "Narrow";
            }

            System.Diagnostics.Debug.WriteLine("Width: {0}, New VisulState: {1}",
                size.Width, state);

            VisualStateManager.GoToState(this, state, true);
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HubPage), "0");
        }

        private void lostfocis(object sender, RoutedEventArgs e)
        {
            if (txtboxSearch.Text.Equals(""))
            {
                Statique._QuotesViewModel.InsertQuotesList(DataModel.Quotes.feedDataQuotes);
            }
        }

        private void textchanged(object sender, TextChangedEventArgs e)
        {
            var txtSearch = txtboxSearch.Text;
            if (DataModel.Quotes.SearchInQuotes(txtSearch.ToString()).Count == 0)
            {

            }
            else
            {
                Statique._QuotesViewModel.InsertQuotesList(DataModel.Quotes.SearchInQuotes(txtSearch.ToString()));
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._QuotesViewModel.InsertQuotesList(DataModel.Quotes.feedDataQuotes);
            }
        }

        private void unloaded(object sender, RoutedEventArgs e)
        {
            if (txtboxSearch.Text.Equals(""))
            {
                Statique._QuotesViewModel.InsertQuotesList(DataModel.Quotes.feedDataQuotes);
            }
        }

    }
}
