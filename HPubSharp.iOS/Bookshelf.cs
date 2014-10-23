using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace HPubSharp.iOS
{
	public class Bookshelf : IBookshelf
	{
		#region Private Properties

		private IList<IBook> __Books = new List<IBook> ();
		private bool __Online = false;

		#endregion

		#region Getters/Setters

		public IList<IBook> Books { 
			get { 
				return this.__Books; 
			} 
		}

		public bool Online { 
			get { 
				return this.__Online; 
			} 
		}

		#endregion

		#region Constructors

		public Bookshelf ()
		{
			string[] Folders = Directory.GetDirectories (Path.Combine (Configs.BookshelfPath));

			foreach (var Folder in Folders) {
				if (File.Exists (Folder + @"/book.json")) {
					this.AddBook (new Book (Folder + @"/"));
				}
			}
		}

		public Bookshelf (string url) : this ()
		{
			var internetStatus = Reachability.InternetConnectionStatus ();

			if (internetStatus != NetworkStatus.NotReachable) {
				var BookshelfJson = this.__getBookshelfJSON (url);

				this.__Online = true;
			}
		}

		#endregion

		#region Public Methods

		public void AddBook (IBook book)
		{
			this.__Books.Add (book);
		}

		#endregion

		#region Private Methods

		private async Task<JObject> __getBookshelfJSON (string url)
		{
			HttpClient client = new HttpClient ();
			Task<string> getResponseTask = client.GetStringAsync (url);

			return JObject.Parse (await getResponseTask);
		}

		#endregion
	}
}

