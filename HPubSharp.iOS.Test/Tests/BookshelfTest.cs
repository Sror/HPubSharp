using System;
using System.IO;
using System.Collections.Generic;

using Nito.AsyncEx;

using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HPubSharp.iOS.Test
{
	[TestFixture]
	public class BookshelfTest
	{
		private Bookshelf Bookshelf = new Bookshelf (@"http://media.todd-henderson.me/bookshelf.json");

		[Test]
		public void Online ()
		{
			Assert.True (Bookshelf.Online);
		}

		[Test]
		public void DownloadableBooks ()
		{
			AsyncContext.Run (async () => {
				var Books = await Bookshelf.DownloadableBooks ();

				var Authors = new List<string> ();
				Authors.Add (@"Arthur Conan Doyle");

				Assert.AreEqual (Books [0].Title, "The Study in Scarlet");
				Assert.AreEqual (Books [0].Author, Authors);
				Assert.AreEqual (Books [0].Date, DateTime.Parse ("2011-06-15"));
				Assert.AreEqual (Books [0].Url, "book://media.todd-henderson.me/arthurconandoyle-thestudyinscarlet");
			});
		}

	}
}
