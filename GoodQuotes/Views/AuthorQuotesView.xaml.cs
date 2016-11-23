using GoodQuotes.Common;
using GoodQuotes.DataModel;
using GoodQuotes.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

    public sealed partial class AuthorQuotesView : Page
    {
        Quotes selectedQuotes = new Quotes();
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        List<Authors> selectedAuthor = new List<Authors>();

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public AuthorQuotesView()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Window_SizeChanged;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            DataContext = ViewModel.Statique._AuthorQuotesViewModel;
        }


        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            selectedAuthor = e.NavigationParameter as List<Authors>;
            pageTitle.Text = selectedAuthor[0].AuthorName;
            await Statique._AuthorQuotesViewModel.getList(selectedAuthor[0]);
            progress.Visibility = Visibility.Collapsed;
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


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
            catch { }
        }

        private void MakePucBtnClick(IUICommand command)
        {
            //throw new NotImplementedException();
        }

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
    }
}
