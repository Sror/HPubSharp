using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using MiniZip.ZipArchive;
using Newtonsoft.Json.Linq;

// Analysis disable once InconsistentNaming
namespace HPubSharp.iOS
{
	/// <summary>
	/// Object refrencing the content of a hpub document.
	/// </summary>
	public class Book : IBook
	{
		#region Private Properties

		JObject __BookJson;

		IList<string> __Author;
		IList<string> __Contents;
		DateTime __Date;
		#pragma warning disable 169
		string __Icon;
		//TODO: Impliment later;
		#pragma warning restore 169
		string __Id;
		string __Title;
		string __Url;

		#endregion

		#region Getters/Setters

		/// <summary>
		/// Gets or sets the base path of the hpub document.
		/// </summary>
		/// <value>The base path.</value>
		public string BasePath {  //TODO Add to device specific configurations.
			get;
			set;
		}

		/// <summary>
		/// Gets the author list of the hpub document.
		/// </summary>
		/// <value>The author's name.</value>
		public IList<string> Author {
			get {
				return __Author;
			}
		}

		/// <summary>
		/// Gets or sets the local availability.
		/// </summary>
		/// <value>Is the hpub downloaded locally. </value>
		public bool AvailableLocally {
			get;
			set;
		}

		/// <summary>
		/// Gets html content pages list of the hpub document.
		/// </summary>
		/// <value>The contents.</value>
		public IList<string> Contents {
			get {
				return __Contents;
			}
		}

		/// <summary>
		/// Gets the date the hpub was published.
		/// </summary>
		/// <value>The publish date.</value>
		public DateTime Date {
			get {
				return __Date;
			}
		}

		/// <summary>
		/// Gets the icon path.
		/// </summary>
		/// <value>The icon path.</value>
		public string Icon {
			get {
				return __Icon;
			}
		}

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id {
			get {
				return __Id;
			}
		}

		/// <summary>
		/// Gets the title of the hpub document.
		/// </summary>
		/// <value>The title.</value>
		public string Title {
			get {
				return __Title;
			}
		}

		/// <summary>
		/// Gets the URL to the hbub document.
		/// </summary>
		/// <value>The URL.</value>
		public string Url {
			get {
				return __Url;
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
			__Init (basePath);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HPubSharp.iOS.Book"/> class.
		/// </summary>
		/// <param name="jsonBook">Json book.</param>
		public Book (JObject jsonBook)
		{
			__Init (jsonBook);
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
				var urlString = (Url.StartsWith ("book:", StringComparison.Ordinal)) ? Url.Replace ("book:", "http:") : Url;
				if (!urlString.EndsWith (".hpub", StringComparison.Ordinal)) {
					urlString += ".hpub";
				}
				var Uri = new Uri (urlString);

				string localDir = Path.Combine (Configs.BookshelfPath, Id);
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

				zip.OnError += (sender, args) => Console.WriteLine ("Error while unzipping: {0}", args);

				zip.UnzipCloseFile ();

				//for debugging
//				foreach (var files in Directory.GetFiles(localDir)) {
//					Console.WriteLine ("Processed file '{0}'.", files);
//				}

				//this.__Init (localDir);  //TODO: Figure out why this doesn't work.

				BasePath = (localDir.EndsWith (Path.DirectorySeparatorChar.ToString (), StringComparison.Ordinal)) ? localDir : localDir + Path.DirectorySeparatorChar;
				//Read book.json from hpub
				__BookJson = JObject.Parse (File.ReadAllText (Path.Combine (BasePath + "book.json")));
				__Contents = new List<string> ();

				//Get the contents of each content page
				foreach (string value in __BookJson ["contents"].ToObject<IList<string>> ()) {
					var temp = File.ReadAllText (Path.Combine (BasePath + value));
					__Contents.Add (temp);
				}
				AvailableLocally = true;

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
		void __Init (string basePath)
		{
			if (__IsJson (basePath)) { 
				AvailableLocally = false;
				//Read book.json from hpub
				__BookJson = JObject.Parse (basePath);

				__Author = __BookJson ["author"].ToObject<IList<string>> ();
				__Title = (string)__BookJson ["title"];
				__Url = (string)__BookJson ["url"];
				__Date = DateTime.Parse ((string)__BookJson ["date"]);
			} else if (File.Exists (basePath + @"book.json")) {

				BasePath = (basePath.EndsWith (Path.DirectorySeparatorChar.ToString (), StringComparison.Ordinal)) ? basePath : basePath + Path.DirectorySeparatorChar;
				//Read book.json from hpub
				__BookJson = JObject.Parse (File.ReadAllText (Path.Combine (BasePath + "book.json")));

				__Author = __BookJson ["author"].ToObject<IList<string>> ();
				__Title = (string)__BookJson ["title"];
				__Contents = new List<string> ();
				__Url = (string)__BookJson ["url"];
				__Date = DateTime.Parse ((string)__BookJson ["date"]);

				//Get the contents of each content page
				foreach (string value in __BookJson ["contents"].ToObject<IList<string>> ()) {
					var temp = File.ReadAllText (Path.Combine (BasePath + value));
					__Contents.Add (temp);
				}
				AvailableLocally = true;
			}

			__Id = Url.GetHashCode ().ToString ();
		}

		/// <summary>
		/// Initilizes the the class usinge the book.json string.
		/// </summary>
		/// <param name="jsonBook">Json book.</param>
		void __Init (JObject jsonBook)
		{
			AvailableLocally = false;
			//Read book.json from hpub
			__BookJson = JObject.FromObject (jsonBook);

			__Author = jsonBook ["author"].ToObject<IList<string>> ();
			__Title = (string)jsonBook ["title"];
			__Url = (string)jsonBook ["url"];
			__Date = DateTime.Parse ((string)jsonBook ["date"]);
			__Id = Url.GetHashCode ().ToString ();
		}

		/// <summary>
		/// Checks if a string is json.
		/// </summary>
		/// <returns><c>true</c>, if the Input is json, <c>false</c> otherwise.</returns>
		/// <param name="input">Input.</param>
		static bool __IsJson (string input)
		{
			input = input.Trim ();
			return input.StartsWith ("{", StringComparison.Ordinal) && input.EndsWith ("}", StringComparison.Ordinal)
			|| input.StartsWith ("[", StringComparison.Ordinal) && input.EndsWith ("]", StringComparison.Ordinal);
		}

		#endregion
	}
}

