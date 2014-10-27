using System;

using Xamarin.Forms;

namespace HPubSharp
{
	/// <summary>
	/// Creates a CarouselPage View of pages.
	/// </summary>
	public partial class BookView : CarouselPage
	{
		#region Constructors

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

			Title = book.Title;

			foreach (string Content in book.Contents) {
				Children.Add (new BookPage (Content, book.BasePath));
			}


		}

		#endregion

		#region Private Methods

		bool __HideNavigation ()
		{
			NavigationPage.SetHasNavigationBar (this, false);
			return false;
		}

		void __ShowNavigation (object sender, EventArgs eventArgs)
		{
			NavigationPage.SetHasNavigationBar (this, true);
			Device.StartTimer (TimeSpan.FromSeconds (2), __HideNavigation);
		}

		#endregion
	}
}

