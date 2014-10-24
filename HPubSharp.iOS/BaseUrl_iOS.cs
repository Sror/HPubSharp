using Xamarin.Forms;
using MonoTouch.Foundation;
using HPubSharp.iOS;

[assembly: Dependency (typeof(BaseUrl_iOS))]

namespace HPubSharp.iOS
{
	public class BaseUrl_iOS : IBaseUrl
	{
		public string Get ()
		{
			return NSBundle.MainBundle.BundlePath;
		}

		public string Get (string baseUrl)
		{
			return (NSBundle.MainBundle.BundlePath.EndsWith ("/")) ? NSBundle.MainBundle.BundlePath + baseUrl : NSBundle.MainBundle.BundlePath + "/" + baseUrl;
		}
	}
}

