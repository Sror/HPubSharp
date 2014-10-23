using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HPubSharp
{
	public partial class BookShelfPage : ContentPage
	{
		public BookShelfPage (IBookshelf bookshelf)
		{
			InitializeComponent ();

			NavigationPage.SetHasNavigationBar (this, false);

			int column = 0;
			if (bookshelf.Books.Count != 0) {
				foreach (var book in bookshelf.Books) {
					if ((this.MainGrid.ColumnDefinitions.Count % 4) == 0) {
						this.MainGrid.RowDefinitions.Add (new RowDefinition { Height = 200 });
						column = 0;
					}

					var tempButton = new Button ();
					tempButton.BackgroundColor = Color.Accent;

					//Calculate luminance 
					double luminance = 0.30 * tempButton.BackgroundColor.R +
					                   0.59 * tempButton.BackgroundColor.G +
					                   0.11 * tempButton.BackgroundColor.B;

					tempButton.TextColor = luminance > 0.5 ? Color.Blue : Color.White;
					tempButton.Font = Font.SystemFontOfSize (NamedSize.Medium, FontAttributes.Bold);

					if (book.AvailableLocally) {
						tempButton.Text = "Read";
						tempButton.Clicked += (object sender, EventArgs e) => {
							Navigation.PushAsync (new BookView (book));
						};
					} else {
						tempButton.Text = "Download";
						tempButton.Clicked += (object sender, EventArgs e) => {
							if (!bookshelf.Online) {
								DisplayAlert ("Offline Notice", "Network connection unavailable. Please connnect and try again.", "OK");
							}
						};
					}
					var tempIcon = new Image ();

					if (book.Icon != null) {
						if (book.Icon.StartsWith ("http")) {
							tempIcon.Source = ImageSource.FromUri (new Uri (book.Icon));
						} else {
							tempIcon.Source = ImageSource.FromFile (book.Icon);
						}
					} else {
						tempIcon.Source = ImageSource.FromFile ("Book-Icon.png");
					}

					var tempStack = new StackLayout {
						Padding = 10,
						Children = {
							tempIcon,
							new Label { Text = book.Title },
							tempButton
						}
					};
					this.MainGrid.Children.Add (tempStack, column, this.MainGrid.RowDefinitions.Count);

					column++;
				}
			}

			this.Content = this.MainGrid;
		}
	}
}

