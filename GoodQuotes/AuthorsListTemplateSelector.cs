using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GoodQuotes
{
    public class AuthorsListTemplateSelector : DataTemplateSelector
    {
        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item, Windows.UI.Xaml.DependencyObject container)
        {
            var currentFrame = Window.Current.Content as Frame;
            var currentPage = currentFrame.Content as Page;

            if (item != null && currentPage != null)
            {
                var data = item as string;
                if (!string.IsNullOrEmpty(data))
                {
                    if (data.Length > 1)
                    {
                        var gridItem = container as GridViewItem;
                        if (gridItem != null)
                            gridItem.IsHitTestVisible = true;
                        return currentPage.Resources["NameTemplate"] as DataTemplate;
                    }
                       
                    else if (data.Length == 1)
                    {
                        var gridItem = container as GridViewItem;
                        if (gridItem != null)
                            gridItem.IsHitTestVisible = false;

                        return currentPage.Resources["FirstCharacterTemplate"] as DataTemplate;
                    }
                }
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
