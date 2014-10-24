using System;
using Xamarin.Forms;

namespace HPubSharp
{
	// required temporarily for iOS, due to BaseUrl bug
	public interface IBaseUrl
	{
		string Get ();

		string Get (string baseUrl);

	}

	// required temporarily for iOS, due to BaseUrl bug
	public class BaseUrlWebView : WebView
	{

	}
}

