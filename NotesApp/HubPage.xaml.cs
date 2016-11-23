using NotesApp.DataModel;
using NotesApp.SettingsFlyouts;
using NotesApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace NotesApp
{

    public sealed partial class HubPage : Page
    {
        DataModel.QuotesTopic selectedTopic = new DataModel.QuotesTopic();
        DataModel.DailyQuotes selectedQuotes;
        DataModel.Quotes quote = new Quotes();

        public HubPage()
        {
            this.InitializeComponent();

            Window.Current.SizeChanged += Window_SizeChanged;

            DataContext = Statique._HubPageViewModel;

            storyAnim.Begin();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as string;
            if (param.ToString().Equals("1"))
            {
                storyPageNavigation.Begin();
                btnNext.Visibility = Visibility.Collapsed;
                btnPrevious.Visibility = Visibility.Visible;
            }

            selectedQuotes = new DataModel.DailyQuotes();
        }

        private void Icon1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //scrollViewer.IsHorizontalScrollChainingEnabled = true;
        }

        private void Icon2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AuthorsView));
        }

        private void Icon3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.QuotesView));
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

        private void btnMoreAppBar_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AuthorsView));
        }

        private void Icon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.PictureQuotes));
        }

        private void imgSearch_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgSearch.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri("ms-appx:///Assets/Search2.png"));
        }

        private void imgSearch_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgSearch.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri("ms-appx:///Assets/Search1.png"));
        }

        private async Task ShowMsgDialog()
        {
            MessageDialog msgDialog = new MessageDialog(selectedQuotes.QuoteText, selectedQuotes.QuoteAuthor);
            msgDialog.Commands.Add(new UICommand("Ok"));

            UICommand ShareBtn = new UICommand("Share");
            ShareBtn.Invoked = ShareBtnClick;
            msgDialog.Commands.Add(ShareBtn);

            UICommand MakePucBtn = new UICommand("Make Picture");
            MakePucBtn.Invoked = MakePucBtnClick;
            msgDialog.Commands.Add(MakePucBtn);

            if (!Statique._FavoriteQuotesViewModel.ExistQuote(quote))
            {
                UICommand FavoriteBtn = new UICommand("Favorite");
                FavoriteBtn.Invoked = FavoriteBtnBtnClick;
                msgDialog.Commands.Add(FavoriteBtn);
            }

            

            await msgDialog.ShowAsync();
        }

        private void MakePucBtnClick(IUICommand command)
        {
            throw new NotImplementedException();
        }

        private void FavoriteBtnBtnClick(IUICommand command)
        {
            Statique._FavoriteQuotesViewModel.addQuotes(quote);
        }

        private void ShareBtnClick(IUICommand command)
        {
            RegisterForShare();
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

        private void Icon5_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.FavoriteQuotesView));
        }

        private void Icon6_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.myQuotes));
        }

        private void btnNext_Tapped(object sender, TappedRoutedEventArgs e)
        {
            storyPageNavigation.Begin();
            btnNext.Visibility = Visibility.Collapsed;
            btnPrevious.Visibility = Visibility.Visible;
        }

        private void btnPrevious_Tapped(object sender, TappedRoutedEventArgs e)
        {
            storyPageNavigation2.Begin();
            btnPrevious.Visibility = Visibility.Collapsed;
            btnNext.Visibility = Visibility.Visible;
        }

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
                if (size.Width == 320)
                    state = "Narrow";
                else if (size.Width <= 600)
                    state = "Narrow";
                else
                    state = "Narrow";
            }

            System.Diagnostics.Debug.WriteLine("Width: {0}, New VisulState: {1}",
                size.Width, state);

            VisualStateManager.GoToState(this, state, true);
        }

        private void unloaded(object sender, RoutedEventArgs e)
        {
            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertListDailyQuotes(DataModel.DailyQuotes.feedDataQuotesDay);
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertListPopularQuotes(DataModel.DailyQuotes.feedDataPopularQuotes);
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertTopicsList(DataModel.QuotesTopic.feedDataQuotesTopic);
            }
        }

        private void textchanged(object sender, TextChangedEventArgs e)
        {
            var txtSearch = txtboxSearch.Text;

            // Quotes Day
            if (DataModel.DailyQuotes.SearchInQuotesDay(txtSearch.ToString()).Count == 0)
            { }
            else
            {
                Statique._HubPageViewModel.InsertListDailyQuotes(DataModel.DailyQuotes.SearchInQuotesDay(txtSearch.ToString()));
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertListDailyQuotes(DataModel.DailyQuotes.feedDataQuotesDay);
            }

            // Popular Quotes
            if (DataModel.DailyQuotes.SearchInPopularQuotes(txtSearch.ToString()).Count == 0)
            { }
            else
            {
                Statique._HubPageViewModel.InsertListPopularQuotes(DataModel.DailyQuotes.SearchInPopularQuotes(txtSearch.ToString()));
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertListPopularQuotes(DataModel.DailyQuotes.feedDataPopularQuotes);
            }

            // Quotes Topic

            if (DataModel.QuotesTopic.SearchInQuotesTopic(txtSearch.ToString()).Count == 0)
            { }
            else
            {
                Statique._HubPageViewModel.InsertTopicsList(DataModel.QuotesTopic.SearchInQuotesTopic(txtSearch.ToString()));
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertTopicsList(DataModel.QuotesTopic.feedDataQuotesTopic);
            }
        }

        private void lostfocis(object sender, RoutedEventArgs e)
        {
            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertListDailyQuotes(DataModel.DailyQuotes.feedDataQuotesDay);
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertListPopularQuotes(DataModel.DailyQuotes.feedDataPopularQuotes);
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertTopicsList(DataModel.QuotesTopic.feedDataQuotesTopic);
            }

        }

        private void imgSearch_Tapped(object sender, TappedRoutedEventArgs e)
        {

            var txtSearch = txtboxSearch.Text;

            // Quotes Day
            if (DataModel.DailyQuotes.SearchInQuotesDay(txtSearch.ToString()).Count == 0)
            { }
            else
            {
                Statique._HubPageViewModel.InsertListDailyQuotes(DataModel.DailyQuotes.SearchInQuotesDay(txtSearch.ToString()));
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertListDailyQuotes(DataModel.DailyQuotes.feedDataQuotesDay);
            }

            // Popular Quotes
            if (DataModel.DailyQuotes.SearchInPopularQuotes(txtSearch.ToString()).Count == 0)
            { }
            else
            {
                Statique._HubPageViewModel.InsertListPopularQuotes(DataModel.DailyQuotes.SearchInPopularQuotes(txtSearch.ToString()));
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertListPopularQuotes(DataModel.DailyQuotes.feedDataPopularQuotes);
            }

            // Quotes Topic

            if (DataModel.QuotesTopic.SearchInQuotesTopic(txtSearch.ToString()).Count == 0)
            { }
            else
            {
                Statique._HubPageViewModel.InsertTopicsList(DataModel.QuotesTopic.SearchInQuotesTopic(txtSearch.ToString()));
            }

            if (txtboxSearch.Text.Equals(""))
            {
                Statique._HubPageViewModel.InsertTopicsList(DataModel.QuotesTopic.feedDataQuotesTopic);
            }
        }

        private void gridViewQuotesTopic_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!ConnectedToInternet())
            {
                MessageDialog msgDialog = new MessageDialog("Unable to load data. Please check your connection and try again.", "No Internet connection!");
                UICommand CloseBtn = new UICommand("OK");
                msgDialog.Commands.Add(CloseBtn);
                msgDialog.ShowAsync();
            }
            else
            {
                selectedTopic = e.ClickedItem as DataModel.QuotesTopic;
                this.Frame.Navigate(typeof(Views.QuotesTopicView), selectedTopic);
            }
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

        private async void QuoteOfDayListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedQuotes = QuoteOfDayListView.SelectedItem as DataModel.DailyQuotes;

            try
            {
                Quotes quo = new Quotes();
                quo.QuoteAuthor = selectedQuotes.QuoteAuthor;
                quo.QuoteText = selectedQuotes.QuoteText;

                MessageDialog msgDialog = new MessageDialog(selectedQuotes.QuoteText, selectedQuotes.QuoteAuthor);
                msgDialog.Commands.Add(new UICommand("Ok"));

                UICommand ShareBtn = new UICommand("Share");
                ShareBtn.Invoked = ShareBtnClick;
                msgDialog.Commands.Add(ShareBtn);


                if (!Statique._FavoriteQuotesViewModel.ExistQuote(quo))
                {
                    UICommand FavoriteBtn = new UICommand("Favorite");
                    FavoriteBtn.Invoked = FavoriteBtnBtnClick;
                    msgDialog.Commands.Add(FavoriteBtn);
                }


                await msgDialog.ShowAsync();
            }
            catch { }
        }

        private async void PopularQuotesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedQuotes = PopularQuotesListView.SelectedItem as DataModel.DailyQuotes;

            try
            {
                Quotes quo = new Quotes();
                quo.QuoteAuthor = selectedQuotes.QuoteAuthor;
                quo.QuoteText = selectedQuotes.QuoteText;

                MessageDialog msgDialog = new MessageDialog(selectedQuotes.QuoteText, selectedQuotes.QuoteAuthor);
                msgDialog.Commands.Add(new UICommand("Ok"));

                UICommand ShareBtn = new UICommand("Share");
                ShareBtn.Invoked = ShareBtnClick;
                msgDialog.Commands.Add(ShareBtn);


                if (!Statique._FavoriteQuotesViewModel.ExistQuote(quo))
                {
                    UICommand FavoriteBtn = new UICommand("Favorite");
                    FavoriteBtn.Invoked = FavoriteBtnBtnClick;
                    msgDialog.Commands.Add(FavoriteBtn);
                }


                await msgDialog.ShowAsync();
            }
            catch { }
        }
    }
}


