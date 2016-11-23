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
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace GoodQuotes.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class myQuotes : Page
    {
        Quotes selectedQuotes = new Quotes();
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public myQuotes()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Window_SizeChanged;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            DataContext = Statique._MyQuotesViewModel;

            _window = Window.Current.Bounds;
            Window.Current.SizeChanged += OnWindowSizeChanged;

            SettingsPane.GetForCurrentView().CommandsRequested += CommandsRequested;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

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

                UICommand MakePucBtn = new UICommand("Make Picture");
                MakePucBtn.Invoked = MakePucBtnClick;
                msgDialog.Commands.Add(MakePucBtn);

                await msgDialog.ShowAsync();
            }
            catch { }
        }

        private void MakePucBtnClick(IUICommand command)
        {
            throw new NotImplementedException();
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








        private Rect _window;
        private Popup _popUp;
        private const double WIDTH = 646;

        private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            _window = Window.Current.Bounds;
        }


        private void CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand("Write Quotes", "Write Quotes", Handler));
        }


        private void Handler(IUICommand command)
        {
            _popUp = new Popup
            {
                Width = WIDTH,
                Height = _window.Height,
                IsLightDismissEnabled = true,
                IsOpen = true
            };
            _popUp.Closed += OnPopupClosed;
            Window.Current.Activated += OnWindowActivated;
            _popUp.Child = new SettingsFlyouts.WriteQuotes { Width = WIDTH, Height = _window.Height };
            _popUp.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (_window.Width - WIDTH) : 0);
            _popUp.SetValue(Canvas.TopProperty, 0);

        }


        private void OnWindowActivated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                try
                {
                    _popUp.IsOpen = false;
                }
                catch { }
            }
        }

        private void OnPopupClosed(object sender, object e)
        {
            Window.Current.Activated -= OnWindowActivated;
        }

        private void btnAddQuotes_Click(object sender, RoutedEventArgs e)
        {
            this.Handler(new UICommand());
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
                //if (size.Width == 320)
                //    state = "Narrow";
                //else if (size.Width <= 600)
                //    state = "Narrow";
                //else
                state = "Narrow";
            }

            System.Diagnostics.Debug.WriteLine("Width: {0}, New VisulState: {1}",
                size.Width, state);

            VisualStateManager.GoToState(this, state, true);
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HubPage), "0");
        }
    }
}
