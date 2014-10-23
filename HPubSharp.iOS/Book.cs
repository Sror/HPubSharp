using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;

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
			if (this.__IsJson (basePath)) { 
				this.AvailableLocally = false;
				//Read book.json from hpub
				this.__BookJson = JObject.Parse (basePath);

				this.__Author = this.__BookJson ["author"].ToObject<IList<string>> ();
				this.__Title = (string)this.__BookJson ["title"];
				this.__Url = (string)this.__BookJson ["url"];
				this.__Date = DateTime.Parse ((string)this.__BookJson ["date"]);

			} else if (File.Exists (basePath + @"book.json")) {

				this.BasePath = basePath;
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
		}

		#endregion

		#region Private Methods

		private bool __IsJson (string input)
		{
			input = input.Trim ();
			return input.StartsWith ("{") && input.EndsWith ("}")
			|| input.StartsWith ("[") && input.EndsWith ("]");
		}

		#endregion
	}
}

