using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.Foundation;
using HPubSharp;
using HPubSharp.iOS;

[assembly: ExportRenderer (typeof(BaseUrlWebView), typeof(BaseUrlWebViewRenderer))]

// Analysis disable once InconsistentNaming
namespace HPubSharp.iOS
{
	public class BaseUrlWebViewRenderer : WebViewRenderer
	{
		public override void LoadHtmlString (string s, NSUrl baseUrl)
		{
		
			if (Device.OS == TargetPlatform.iOS) {
				//TODO Un Hardcode the bookshelf Location
				baseUrl = new NSUrl (NSBundle.MainBundle.BundlePath + baseUrl.AbsoluteString.TrimStart ('.'), true);
			}
			base.LoadHtmlString (s, baseUrl);
		}
	}
}

