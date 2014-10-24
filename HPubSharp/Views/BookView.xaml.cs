using System;

using Xamarin.Forms;

namespace HPubSharp
{
	/// <summary>
	/// Creates a CarouselPage View of pages.
	/// </summary>
	public partial class BookView : CarouselPage
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="HPubSharp.BookView"/> class by adding a BookPages for each content page
		/// in the book.content array.
		/// </summary>
		/// <param name="book">Book.</param>
		public BookView (IBook book)
		{
			InitializeComponent ();

			//TODO Figure out how to add a gesture to entire page. 
//			Device.StartTimer (TimeSpan.FromSeconds (2), this.__HideNavigation);
//
//			var tapGestureRecognizer = new TapGestureRecognizer ();
//			tapGestureRecognizer.NumberOfTapsRequired = 2; // double-tap
//			tapGestureRecognizer.Tapped += this.__ShowNavigation;

			this.Title = book.Title;

			foreach (string Content in book.Contents) {
				this.Children.Add (new BookPage (Content, book.BasePath));
			}


		}

		private bool __HideNavigation ()
		{
			NavigationPage.SetHasNavigationBar (this, false);
			return false;
		}

		private void __ShowNavigation (object sender, EventArgs eventArgs)
		{
			NavigationPage.SetHasNavigationBar (this, true);
			Device.StartTimer (TimeSpan.FromSeconds (2), this.__HideNavigation);
		}
	}
}

