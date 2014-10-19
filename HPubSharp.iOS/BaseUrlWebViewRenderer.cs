﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using HPubSharp;
using HPubSharp.iOS;

[assembly: ExportRenderer (typeof(BaseUrlWebView), typeof(BaseUrlWebViewRenderer))]

namespace HPubSharp.iOS
{
	public class BaseUrlWebViewRenderer : WebViewRenderer
	{
		public override void LoadHtmlString (string s, NSUrl baseUrl)
		{
		
			if (baseUrl == null) {
				//TODO Un Hardcode the bookshelf Location
				baseUrl = new NSUrl (NSBundle.MainBundle.BundlePath + @"/Bookshelf/book/", true);
			}
			base.LoadHtmlString (s, baseUrl);
		}
	}
}
