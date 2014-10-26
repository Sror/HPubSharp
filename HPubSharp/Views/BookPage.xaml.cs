using Xamarin.Forms;

namespace HPubSharp
{
	/// <summary>
	/// Creates a book's ContentPage. 
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

			var PageWebView = new BaseUrlWebView (); // temporarily use this so we can custom-render in iOS
			var HtmlSource = new HtmlWebViewSource ();

			HtmlSource.Html = content;
			if (Device.OS != TargetPlatform.iOS) {
				// the BaseUrlWebViewRenderer does this for iOS, until bug is fixed
				HtmlSource.BaseUrl = DependencyService.Get<IBaseUrl> ().Get ();
			} 
				
			//Add HTML Source to WebView
			PageWebView.Source = HtmlSource;

			//Set Content to WebView
			Content = PageWebView;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HPubSharp.BookPage"/> class.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="baseUrl">Base URL.</param>
		public BookPage (string content, string baseUrl)
		{
			InitializeComponent ();

			var PageWebView = new BaseUrlWebView (); // temporarily use this so we can custom-render in iOS
			var HtmlSource = new HtmlWebViewSource ();

			HtmlSource.Html = content;
			HtmlSource.BaseUrl = Device.OS != TargetPlatform.iOS ? DependencyService.Get<IBaseUrl> ().Get (baseUrl) : baseUrl;

			//Add HTML Source to WebView
			PageWebView.Source = HtmlSource;

			//Set Content to WebView
			Content = PageWebView;
		}
	}
}

