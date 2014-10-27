using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Reflection;

namespace HPubSharp
{
	public partial class BookIconView : StackLayout
	{
		private IBook Book;

		public BookIconView (IBook book)
		{
			Book = book;

			InitializeComponent ();

			//Set Book Title binding
			//TODO: Get this to work in zaml.
			TitleLable.BindingContext = Book;
			TitleLable.SetBinding (Label.TextProperty, "Title");

//			//Calculate luminance 
			double luminance = 0.30 * ReadOrDownloadButton.BackgroundColor.R +
			                   0.59 * ReadOrDownloadButton.BackgroundColor.G +
			                   0.11 * ReadOrDownloadButton.BackgroundColor.B;


			//Set button color baised on luminance.
			ReadOrDownloadButton.TextColor = luminance > 0.5 ? Color.Blue : Color.White;

			//Is book local on downloadable.
			ReadOrDownloadButton.Text = Book.AvailableLocally ? "Read" : "Download";

			//Button Click handler
			ReadOrDownloadButton.Clicked += async (senderObj, eventArg) => {  
				var thisButton = (Button)senderObj;
				if (thisButton.Text == "Read") {
					await Navigation.PushAsync (new BookView (book));
				} else {
					//Do we have a netowrk connection
					await Book.Download ();
					thisButton.Text = "Read";
//					if (!Book.Reachable ()) {
//						MessagingCenter.Send (this, "Connection Error");
//					} else {
//						await Book.Download ();
//						thisButton.Text = "Read";
//					}
				}
			};


			//Set Icon Image if exists.
			Icon.HeightRequest = 100;
			if (Book.Icon != null) {
				Icon.Source = Book.Icon.StartsWith ("http", StringComparison.Ordinal) ? ImageSource.FromUri (new Uri (Book.Icon)) : ImageSource.FromFile (Book.Icon);
			} 
		}
	}
}

