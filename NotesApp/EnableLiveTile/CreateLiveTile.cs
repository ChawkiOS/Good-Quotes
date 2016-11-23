using NotesApp.EnableLiveTile.NotificationsExtensions.TileContent;
using NotesApp.EnableLiveTile.TileContent;
using Windows.UI.Notifications;

namespace NotesApp.EnableLiveTile
{
    public class CreateLiveTile
    {
        public static void ShowliveTile(bool IsLiveTile1, string name)
        {
            string wideTilesrc = "ms-appx:///Assets/BigLiveTile1.jpg";
            string smallTilesrc = "ms-appx:///Assets/SmallLiveTile1.jpg";

            if (!IsLiveTile1)
            {
                wideTilesrc = "ms-appx:///Assets/BigLiveTile2.jpg";
                smallTilesrc = "ms-appx:///Assets/SmallLiveTile2.jpg";
            }
            // Note: This sample contains an additional project, NotificationsExtensions.
            // NotificationsExtensions exposes an object model for creating notifications, but you can also 
            // modify the strings directly. See UpdateTileWithImageWithStringManipulation_Click for an example

            // Create notification content based on a visual template.
            ITileWideImageAndText01 tileContent = TileContentFactory.CreateTileWideImageAndText01();
            tileContent.TextCaptionWrap.Text = name;
            tileContent.Image.Src = wideTilesrc;
            tileContent.Image.Alt = "Live tile";

            // Users can resize tiles to square or wide.
            // Apps can choose to include only square assets (meaning the app's tile can never be wide), or
            // include both wide and square assets (the user can resize the tile to square or wide).
            // Apps should not include only wide assets.

            // Apps that support being wide should include square tile notifications since users
            // determine the size of the tile.

            // create the square template and attach it to the wide template
            ITileSquareImage squareContent = TileContentFactory.CreateTileSquareImage();
            squareContent.Image.Src = smallTilesrc;
            squareContent.Image.Alt = "Live tile";
            tileContent.SquareContent = squareContent;

            // Send the notification to the app's application tile.
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileContent.CreateNotification());
        }

    }
}
