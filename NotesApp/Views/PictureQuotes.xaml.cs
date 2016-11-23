using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Graphics;
using Windows.UI;
using Windows.UI.Xaml.Shapes;


namespace NotesApp.Views
{

    public sealed partial class PictureQuotes : Page
    {
        public PictureQuotes()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Window_SizeChanged;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Canvas canv = new Canvas();
            
            ////adapt convas to image
            //canv.Width = 800;
            //canv.Height = 550;

            //StackPanel stk = new StackPanel();
            //stk.Orientation = Orientation.Vertical;

            //stk.Width = 800;
            //stk.Height = 550;

            ////background image
            //Image img = new Image();
            //img.Source = new BitmapImage(new Uri("ms-appx:///Assets/PictureQuotes/22.jpeg"));
            //img.Margin = new Thickness(0);
            //img.Width = 800;
            //img.Stretch = Stretch.Fill;
            //img.Height = 550;
            //img.Opacity = 0.6;
            
            ////black bg
            //Rectangle rect = new Rectangle();
            //rect.Height = 550;
            //rect.Width = 800;
            //rect.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

            //canv.Children.Add(rect);
            //canv.Children.Add(img);

            //// Image Sigle
            //Image imgSigle = new Image();
            //imgSigle.Source = new BitmapImage(new Uri("ms-appx:///Assets/Sigle.png"));
            //imgSigle.Margin = new Thickness(490, 495, 0, 0);
            //imgSigle.Width = 300;
            //imgSigle.Height = 45;
            //imgSigle.Opacity = 0.5;
            

            ////quote text
            //TextBlock qtxt = new TextBlock();
            //qtxt.Width = 740;
            //qtxt.Text = "Le Lorem Ipsum est simplement du faux texte employé dans la composition et la mise en page avant impression.";
            //qtxt.TextWrapping = TextWrapping.WrapWholeWords;
            //qtxt.FontSize = 28;
            //qtxt.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe UI");
            //qtxt.Margin = new Thickness(30, 30, 30, 30);
            //qtxt.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            //qtxt.FontWeight = Windows.UI.Text.FontWeights.Normal;
            //stk.Children.Add(qtxt);

            ////quote author text
            //TextBlock qatxt = new TextBlock();
            //qatxt.Width = 740;
            //qatxt.FontStyle = Windows.UI.Text.FontStyle.Italic;
            //qatxt.Text = "Chawki Messaoudi";
            //qatxt.TextWrapping = TextWrapping.WrapWholeWords;
            //qatxt.FontSize = 25;
            //qatxt.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe UI");
            //qatxt.Margin = new Thickness(30, 30, 30, 30);
            //qatxt.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            //qatxt.FontWeight = Windows.UI.Text.FontWeights.SemiLight;
            //stk.Children.Add(qatxt);

            //canv.Children.Add(imgSigle);

            //canv.Children.Add(stk);

            //stkImg.Children.Add(canv);
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
