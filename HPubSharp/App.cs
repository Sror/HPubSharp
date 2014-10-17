using System;
using Xamarin.Forms;

namespace HPubSharp
{
	/// <summary>
	/// The main class in the PCL.
	/// </summary>
	public class App
	{
		/// <summary>
		/// Returns the main view controller generated from the xaml.
		/// </summary>
		/// <returns>The main page view.</returns>
		public static Page GetMainPage ()
		{
			return new MainPage ();
		}
	}
}

