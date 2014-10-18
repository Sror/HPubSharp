using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HPubSharp
{

	public interface IBaseUrl
	{
		string Get ();

	}


	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}

		public MainPage (IBook book)
		{
			InitializeComponent ();

			var MainWebView = new BaseUrlWebView (); // temporarily use this so we can custom-render in iOS
			var htmlSource = new HtmlWebViewSource ();

			htmlSource.Html = book.Contents [0];
			if (Device.OS != TargetPlatform.iOS) {
				// the BaseUrlWebViewRenderer does this for iOS, until bug is fixed
				htmlSource.BaseUrl = DependencyService.Get<IBaseUrl> ().Get ();
			}

			//this.MainWebView.Title = book.Title;
			MainWebView.Source = htmlSource;

			this.Content = MainWebView;
		}
	}
}

