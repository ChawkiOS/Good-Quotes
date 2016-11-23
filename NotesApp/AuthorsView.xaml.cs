using NotesApp.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
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

    public sealed partial class AuthorsView : Page
    {
        List<Authors> selectedAuthor = new List<Authors>();

        string authorName ="";

        public AuthorsView()
        {
            this.InitializeComponent();

            Window.Current.SizeChanged += Window_SizeChanged;

            DataContext = ViewModel.Statique._AuthorsViewModel;
        }


        private void authorsGridView_ItemClick(object sender, ItemClickEventArgs e)
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
                authorName = e.ClickedItem as string;
                selectedAuthor = getAuthor(authorName);
                this.Frame.Navigate(typeof(Views.AuthorQuotesView), selectedAuthor);
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

        public static List<Authors> getAuthor(string queryText)
        {
            return Authors.feedDataAuthors.Where(p => p.AuthorName.ToLower().Equals(queryText.ToLower())).ToList();
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
    }
}
