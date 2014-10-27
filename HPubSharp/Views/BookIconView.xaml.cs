using System;
using Xamarin.Forms;

namespace HPubSharp
{
	public partial class BookIconView : StackLayout
	{

		public BookIconView (IBook book)
		{

			InitializeComponent ();

			//Set Book Title binding
			//TODO: Get this to work in zaml.
			TitleLable.BindingContext = book;
			TitleLable.SetBinding (Label.TextProperty, "Title");

//			//Calculate luminance 
			double luminance = 0.30 * ReadOrDownloadButton.BackgroundColor.R +
			                   0.59 * ReadOrDownloadButton.BackgroundColor.G +
			                   0.11 * ReadOrDownloadButton.BackgroundColor.B;


			//Set button color baised on luminance.
			ReadOrDownloadButton.TextColor = luminance > 0.5 ? Color.Blue : Color.White;

			//Is book local on downloadable.
			ReadOrDownloadButton.Text = book.AvailableLocally ? "Read" : "Download";

			//Button Click handler
			ReadOrDownloadButton.Clicked += async (senderObj, eventArg) => {  
				var thisButton = (Button)senderObj;
				if (thisButton.Text == "Read") {
					await Navigation.PushAsync (new BookView (book));
				} else {
					//Do we have a netowrk connection
					await book.Download ();
					thisButton.Text = "Read";
//					if (!book.Reachable ()) {
//						MessagingCenter.Send (this, "Connection Error");
//					} else {
//						await book.Download ();
//						thisButton.Text = "Read";
//					}
				}
			};


			//Set Icon Image if exists.
			Icon.HeightRequest = 100;
			if (book.Icon != null) {
				Icon.Source = book.Icon.StartsWith ("http", StringComparison.Ordinal) ? ImageSource.FromUri (new Uri (book.Icon)) : ImageSource.FromFile (book.Icon);
			} 
		}
	}
}

