﻿using System.IO;
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
		//TODO Add to device specific configurations.
		private string __BasePath = @"./Bookshelf/book/";
		private JObject __BookJson;

		private IList<string> __Author;
		private IList<string> __Contents;
		private string __Title;

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
		/// <value>The author.</value>
		public IList<string> Author {
			get {
				return this.__Author;
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
		/// Gets the title of the hpub document.
		/// </summary>
		/// <value>The title.</value>
		public string Title {
			get {
				return this.__Title;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HPubSharp.iOS.Book"/> class by reading the book.json file in
		/// the hpub document package.
		/// </summary>
		public Book ()
		{
			//Read book.json from hpub
			this.__BookJson = JObject.Parse (File.ReadAllText (System.IO.Path.Combine (this.__BasePath + "book.json")));

			this.__Author = this.__BookJson ["author"].ToObject<IList<string>> ();
			this.__Title = (string)this.__BookJson ["title"];
			this.__Contents = new List<string> ();

			//Get the contents of each content page
			foreach (string value in this.__BookJson ["contents"].ToObject<IList<string>> ()) {
				var temp = File.ReadAllText (System.IO.Path.Combine (this.__BasePath + value));
				this.__Contents.Add (temp);
			}
		}
	}
}
