using NotesApp.Common;
using NotesApp.DataModel;
using NotesApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.UserProfile;
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


namespace NotesApp.SettingsFlyouts
{

    public sealed partial class WriteQuotes : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public WriteQuotes()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            
        }


        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

        }

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


        private void CloseFlyout(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Popup)
                (this.Parent as Popup).IsOpen = false;

            SettingsPane.Show();
        }

        private async void btnAddQuotes_Click(object sender, RoutedEventArgs e)
        {
            if (!txbWriteQuote.Text.Equals(""))
            {
                Quotes quote = new Quotes();
                quote.QuotesTags = "";
                quote.QuoteAuthor = await UserInformation.GetDisplayNameAsync();
                quote.QuoteText = txbWriteQuote.Text;

                Statique._MyQuotesViewModel.addQuotes(quote);
            }

            if(this.Parent is Popup)
                (this.Parent as Popup).IsOpen = false;
        }

        
    }
}
