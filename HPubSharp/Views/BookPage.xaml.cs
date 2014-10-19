using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HPubSharp
{
	/// <summary>
	/// Creates a book ContentPage. 
	/// </summary>
	public partial class BookPage: ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HPubSharp.BookPage"/> class from the HTML content string.
		/// </summary>
		/// <param name="content">Content.</param>
		public BookPage (string content)
		{
			InitializeComponent ();

			var MainWebView = new BaseUrlWebView (); // temporarily use this so we can custom-render in iOS
			var HtmlSource = new HtmlWebViewSource ();

			HtmlSource.Html = content;
			if (Device.OS != TargetPlatform.iOS) {
				// the BaseUrlWebViewRenderer does this for iOS, until bug is fixed
				HtmlSource.BaseUrl = DependencyService.Get<IBaseUrl> ().Get ();
			}
				
			//Add HTML Source to view
			MainWebView.Source = HtmlSource;

			//Set Content to MainWebView
			this.Content = MainWebView;
		}
	}
}

