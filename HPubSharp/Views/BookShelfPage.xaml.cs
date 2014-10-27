using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HPubSharp
{
	/// <summary>
	/// Bookshelf page.
	/// </summary>
	public partial class BookShelfPage : ContentPage
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="HPubSharp.BookShelfPage"/> class.
		/// </summary>
		/// <param name="bookshelf">Bookshelf.</param>
		public BookShelfPage (IBookshelf bookshelf)
		{
			InitializeComponent ();

			//Remove nav bar on bookshelf view.
			NavigationPage.SetHasNavigationBar (this, false);

			MessagingCenter.Subscribe <BookIconView> (this, "Connection Error", async (sender) => 
				await this.DisplayAlert ("Offline Notice", "Network connection unavailable. Please connnect and try again.", "OK"));

			int column = 0;
			bool initilized = false;
			//Load bookshelf content asyncronously.
			Appearing += async (sender, e) => {
				if (!initilized) {
				
					var availableBooks = await bookshelf.DownloadableBooks ();

					foreach (IBook AvaiBook in availableBooks) {
						bookshelf.AddBook (AvaiBook);
					}
						
					if (bookshelf.Books.Count != 0) {
						foreach (var book in bookshelf.Books) {
							if ((column % 4) == 0) {
								BookShelfContentGrid.RowDefinitions.Add (new RowDefinition { Height = 220 });
								column = 0;
							}
								
							var BookIcons = new BookIconView (book);

							//Add BookIcons to gridview
							BookShelfContentGrid.Children.Add (BookIcons, column, BookShelfContentGrid.RowDefinitions.Count - 1);

							//Incriment column counter
							column++;
						}
					}
				}
				initilized = true;
			};

			//Load MainView
			Content = MainView;
		}

		#endregion
	}
}

