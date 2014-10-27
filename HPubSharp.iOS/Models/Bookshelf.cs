using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

// Analysis disable once InconsistentNaming
namespace HPubSharp.iOS
{
	/// <summary>
	/// Bookshelf for storing and using Books.
	/// </summary>
	public class Bookshelf : IBookshelf
	{
		#region Private Properties

		IList<IBook> __Books = new List<IBook> ();
		string __BookshelfFeedUrl;

		#endregion

		#region Getters/Setters

		/// <summary>
		/// Gets the books.
		/// </summary>
		/// <value>The books.</value>
		public IList<IBook> Books { 
			get { 
				if (__Books.Count == 0) {
					string[] Folders = Directory.GetDirectories (Path.Combine (Configs.BookshelfPath));

					foreach (var Folder in Folders) {
						if (File.Exists (Folder + @"/book.json")) {
							AddBook (new Book (Folder + @"/"));
						}
					}
				}

				return __Books; 
			} 
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="HPubSharp.iOS.Bookshelf"/> is online.
		/// </summary>
		/// <value><c>true</c> if online; otherwise, <c>false</c>.</value>
		public bool Online { 
			get { 
				return (Reachability.InternetConnectionStatus () != NetworkStatus.NotReachable); 
			} 
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="HPubSharp.iOS.Bookshelf"/> class.
		/// </summary>
		public Bookshelf ()
		{
			//Default Constructor
			// Future feature added here.
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HPubSharp.iOS.Bookshelf"/> class.
		/// </summary>
		/// <param name="url">URL to json feed containing downloadable books.</param>
		public Bookshelf (string url) : this ()
		{
			__BookshelfFeedUrl = url;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds the book to the bookshelf.
		/// </summary>
		/// <param name="book">Book.</param>
		public void AddBook (IBook book)
		{
			__Books.Add (book);
		}

		/// <summary>
		/// Gets the books available for download.
		/// </summary>
		/// <returns>A list of Books.</returns>
		public async Task<IList<IBook>> DownloadableBooks ()
		{ 
			var BookList = new List<IBook> ();
			var BookFeed = await __getBookshelfJSON (__BookshelfFeedUrl);

			foreach (JObject book in BookFeed) {
				foreach (IBook ibook in Books) {
					if (!ibook.Url.Equals ((string)book ["url"])) {
						BookList.Add (new Book (book));
					}
				}
			}

			return BookList;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the bookshelf JSON.
		/// </summary>
		/// <returns>The bookshelf JSON.</returns>
		/// <param name="url">URL.</param>
		async Task<JArray> __getBookshelfJSON (string url)
		{
			using (var httpClient = new HttpClient ()) {
				var json = await httpClient.GetStringAsync (url);
				return  JArray.Parse (json);
			}
		}

		#endregion
	}
}


