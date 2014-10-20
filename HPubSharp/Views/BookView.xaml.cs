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
	
			foreach (string Content in book.Contents) {
				this.Children.Add (new BookPage (Content));
			}
		}
	}
}

