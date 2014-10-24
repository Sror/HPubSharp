using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using System;

namespace HPubSharp.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{


			Forms.Init ();

			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			if (Configs.UseBookshelf) {
				var BookShelf = new Bookshelf (@"http://media.todd-henderson.me/bookshelf.json");

				// defined a root view controller 
				window.RootViewController = App.GetMainPage (BookShelf).CreateViewController ();
			} else {
				var Book = new Book (Configs.BookshelfPath + "book/");

				// defined a root view controller
				window.RootViewController = App.GetMainPage (Book).CreateViewController ();
			}

			// make the window visible
			try {
				window.MakeKeyAndVisible ();
			} catch (Exception e) {
				Console.WriteLine ("An error occurred: '{0}'", e);
				//TODO Add App Specfix Error and graceful exit. 
			}
			return true;
		}
	}
}

