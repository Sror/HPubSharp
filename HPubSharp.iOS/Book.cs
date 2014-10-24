using System;
using System.Collections.Generic;
using System.IO;

//using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using MiniZip.ZipArchive;
using Newtonsoft.Json.Linq;

namespace HPubSharp.iOS
{
	/// <summary>
	/// Object refrencing the content of a hpub document.
	/// </summary>
	public class Book : IBook
	{
		#region Private Properties

		private string __BasePath;
		//TODO Add to device specific configurations.
		private JObject __BookJson;

		private IList<string> __Author;
		private bool __AvailableLocally = false;
		private IList<string> __Contents;
		private DateTime __Date;
		private string __Icon;
		private string __Id;
		private string __Title;
		private string __Url;

		#endregion

		#region Getters/Setters

		/// <summary>
		/// Gets or sets the base path of the hpub document.
		/// </summary>
		/// <value>The base path.</value>
		public string BasePath {
			get {
				return this.__BasePath;
			}
			set {
				this.__BasePath = value;
			}
		}

		/// <summary>
		/// Gets the author list of the hpub document.
		/// </summary>
		/// <value>The author's name.</value>
		public IList<string> Author {
			get {
				return this.__Author;
			}
		}

		/// <summary>
		/// Gets or sets the local availability.
		/// </summary>
		/// <value>Is the hpub downloaded locally. </value>
		public bool AvailableLocally {
			get {
				return this.__AvailableLocally;
			}
			set {
				this.__AvailableLocally = value;
			}
		}

		/// <summary>
		/// Gets html content pages list of the hpub document.
		/// </summary>
		/// <value>The contents.</value>
		public IList<string> Contents {
			get {
				return this.__Contents;
			}
		}

		/// <summary>
		/// Gets the date the hpub was published.
		/// </summary>
		/// <value>The publish date.</value>
		public DateTime Date {
			get {
				return this.__Date;
			}
		}

		/// <summary>
		/// Gets the icon path.
		/// </summary>
		/// <value>The icon path.</value>
		public string Icon {
			get {
				return this.__Icon;
			}
		}

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id {
			get {
				return this.__Id;
			}
		}

		/// <summary>
		/// Gets the title of the hpub document.
		/// </summary>
		/// <value>The title.</value>
		public string Title {
			get {
				return this.__Title;
			}
		}

		/// <summary>
		/// Gets the URL to the hbub document.
		/// </summary>
		/// <value>The URL.</value>
		public string Url {
			get {
				return this.__Url;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="HPubSharp.iOS.Book"/> class by reading the book.json file in
		/// the hpub document package.
		/// </summary>
		public Book (string basePath)
		{
			this.__Init (basePath);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HPubSharp.iOS.Book"/> class.
		/// </summary>
		/// <param name="jsonBook">Json book.</param>
		public Book (JObject jsonBook)
		{
			this.__Init (jsonBook);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Download the hpub file and loads the content.
		/// </summary>
		public async Task Download ()
		{
			try {
				var webClient = new WebClient ();
				var urlString = (this.Url.StartsWith ("book:")) ? this.Url.Replace ("book:", "http:") : this.Url;
				if (!urlString.EndsWith (".hpub")) {
					urlString += ".hpub";
				}
				var Uri = new Uri (urlString);

				string localDir = Path.Combine (Configs.BookshelfPath, this.Id);
				string localFilename = (urlString.Split ('/').Last ()).Replace (".hpub", ".zip");
				string localPath = Path.Combine (localDir, localFilename);

				Directory.CreateDirectory (localDir);
				File.WriteAllBytes (localPath, await webClient.DownloadDataTaskAsync (Uri)); // writes to local storage  

				Console.WriteLine (localDir);
				//ZipFile.ExtractToDirectory (localPath, localDir);

				//Unzip hpub package
				var zip = new ZipArchive ();
				zip.UnzipOpenFile (localPath);
				zip.UnzipFileTo (localDir, true);

				zip.OnError += (sender, args) => {
					Console.WriteLine ("Error while unzipping: {0}", args);
				};

				zip.UnzipCloseFile ();

				//for debugging
//				foreach (var files in Directory.GetFiles(localDir)) {
//					Console.WriteLine ("Processed file '{0}'.", files);
//				}

				//this.__Init (localDir);  //TODO: Figure out why this doesn't work.

				this.__BasePath = (localDir.EndsWith (Path.DirectorySeparatorChar.ToString ())) ? localDir : localDir + Path.DirectorySeparatorChar;
				//Read book.json from hpub
				this.__BookJson = JObject.Parse (File.ReadAllText (System.IO.Path.Combine (this.BasePath + "book.json")));
				this.__Contents = new List<string> ();

				//Get the contents of each content page
				foreach (string value in this.__BookJson ["contents"].ToObject<IList<string>> ()) {
					var temp = File.ReadAllText (System.IO.Path.Combine (this.__BasePath + value));
					this.__Contents.Add (temp);
				}
				this.__AvailableLocally = true;

			} catch (Exception e) {
				Console.WriteLine ("An error occurred: '{0}'", e);
				//TODO Add App Specfix Error and graceful exit. 
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Initilizes the the class usinge the basePath.
		/// </summary>
		/// <param name="basePath">Base path.</param>
		private void __Init (string basePath)
		{
			if (this.__IsJson (basePath)) { 
				this.AvailableLocally = false;
				//Read book.json from hpub
				this.__BookJson = JObject.Parse (basePath);

				this.__Author = this.__BookJson ["author"].ToObject<IList<string>> ();
				this.__Title = (string)this.__BookJson ["title"];
				this.__Url = (string)this.__BookJson ["url"];
				this.__Date = DateTime.Parse ((string)this.__BookJson ["date"]);
			} else if (File.Exists (basePath + @"book.json")) {

				this.BasePath = (basePath.EndsWith (Path.DirectorySeparatorChar.ToString ())) ? basePath : basePath + Path.DirectorySeparatorChar;
				//Read book.json from hpub
				this.__BookJson = JObject.Parse (File.ReadAllText (System.IO.Path.Combine (this.BasePath + "book.json")));

				this.__Author = this.__BookJson ["author"].ToObject<IList<string>> ();
				this.__Title = (string)this.__BookJson ["title"];
				this.__Contents = new List<string> ();
				this.__Url = (string)this.__BookJson ["url"];
				this.__Date = DateTime.Parse ((string)this.__BookJson ["date"]);

				//Get the contents of each content page
				foreach (string value in this.__BookJson ["contents"].ToObject<IList<string>> ()) {
					var temp = File.ReadAllText (System.IO.Path.Combine (this.__BasePath + value));
					this.__Contents.Add (temp);
				}
				this.__AvailableLocally = true;
			}

			this.__Id = this.Url.GetHashCode ().ToString ();
		}

		/// <summary>
		/// Initilizes the the class usinge the book.json string.
		/// </summary>
		/// <param name="jsonBook">Json book.</param>
		private void __Init (JObject jsonBook)
		{
			this.AvailableLocally = false;
			//Read book.json from hpub
			this.__BookJson = JObject.FromObject (jsonBook);

			this.__Author = jsonBook ["author"].ToObject<IList<string>> ();
			this.__Title = (string)jsonBook ["title"];
			this.__Url = (string)jsonBook ["url"];
			this.__Date = DateTime.Parse ((string)jsonBook ["date"]);
			this.__Id = this.Url.GetHashCode ().ToString ();
		}

		/// <summary>
		/// Checks if a string is json.
		/// </summary>
		/// <returns><c>true</c>, if the Input is json, <c>false</c> otherwise.</returns>
		/// <param name="input">Input.</param>
		private bool __IsJson (string input)
		{
			input = input.Trim ();
			return input.StartsWith ("{") && input.EndsWith ("}")
			|| input.StartsWith ("[") && input.EndsWith ("]");
		}

		#endregion
	}
}

