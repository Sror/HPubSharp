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
		/// <summary>
		/// Initializes a new instance of the <see cref="HPubSharp.BookShelfPage"/> class.
		/// </summary>
		/// <param name="bookshelf">Bookshelf.</param>
		public BookShelfPage (IBookshelf bookshelf)
		{
			InitializeComponent ();

			//Remove nav bar on bookshelf view.
			NavigationPage.SetHasNavigationBar (this, false);

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
								BookShelfContentGrid.RowDefinitions.Add (new RowDefinition { Height = 200 });
								column = 0;
							}

							//Make boohshelf button
							var tempButton = new Button ();
							tempButton.BackgroundColor = Color.Accent;

							//Calculate luminance 
							double luminance = 0.30 * tempButton.BackgroundColor.R +
							                   0.59 * tempButton.BackgroundColor.G +
							                   0.11 * tempButton.BackgroundColor.B;

							//Set button color baised on luminance.
							tempButton.TextColor = luminance > 0.5 ? Color.Blue : Color.White;
							tempButton.Font = Font.SystemFontOfSize (NamedSize.Medium, FontAttributes.Bold);


							//Is book local on downloadable.
							tempButton.Text = book.AvailableLocally ? "Read" : "Download";

							//Button Click handler
							tempButton.Clicked += async (senderObj, eventArg) => {  
								var thisButton = (Button)senderObj;
								if (thisButton.Text == "Read") {
									await Navigation.PushAsync (new BookView (book));
								} else {
									//Do we have a netowrk connection
									if (!bookshelf.Online) {
										await DisplayAlert ("Offline Notice", "Network connection unavailable. Please connnect and try again.", "OK");
									} else {
										await book.Download ();
										thisButton.Text = "Read";
									}
								}
							};

							//Create Book Icon
							var tempIcon = new Image ();
							tempIcon.HeightRequest = 100;
							if (book.Icon != null) {
								tempIcon.Source = book.Icon.StartsWith ("http", StringComparison.Ordinal) ? ImageSource.FromUri (new Uri (book.Icon)) : ImageSource.FromFile (book.Icon);
							} else {
								tempIcon.Source = ImageSource.FromFile ("Book-Icon.png");
							}

							//Create a stacked layout for book.
							var tempStack = new StackLayout {
								Padding = 10,
								Children = {
									tempIcon,
									new Label { Text = book.Title },
									tempButton
								}
							};

							//Add stack to gridview
							BookShelfContentGrid.Children.Add (tempStack, column, BookShelfContentGrid.RowDefinitions.Count - 1);

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
	}
}

