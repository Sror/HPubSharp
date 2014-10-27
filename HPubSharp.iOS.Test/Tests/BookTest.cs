using System;
using System.IO;
using System.Collections.Generic;

using NUnit.Framework;

namespace HPubSharp.iOS.Test
{
	[TestFixture]
	public class BookTest
	{
		private Book Book = new Book (Configs.BookshelfPath + "book/");

		[Test]
		public void BookAuthor ()
		{
		
			var Authors = new List<string> ();

			Authors.Add (@"Arthur Conan Doyle");
			Assert.AreEqual (Authors, Book.Author);
		}

		[Test]
		public void BookTitle ()
		{
			//Check the title.
			Assert.AreEqual (@"The Study in Scarlet", Book.Title);
		}

		[Test]
		public void BookContent ()
		{
			//Sanity check. Check for the correct number of content pages in fixture hPub
			Assert.AreEqual (17, Book.Contents.Count);

			//Check the first content page as a sample
			var FirstContent = File.ReadAllText (System.IO.Path.Combine (Book.BasePath + @"Book Cover.html"));
			Assert.AreEqual (FirstContent, Book.Contents [0]);
		}

		[Test]
		public void AvailableLocally ()
		{
			//Should be available Locally
			Assert.IsTrue (Book.AvailableLocally);
		}

		[Test]
		public void Date ()
		{
			//Is date parssed correctly
			Assert.AreEqual (Book.Date, DateTime.Parse ("2011-06-15"));
		}

		[Test]
		public void Icon ()
		{
			//Not set in json
			Assert.IsNull (Book.Icon);
		}

		[Test]
		public void Id ()
		{
			//Should not be null
			Assert.IsNotNull (Book.Id);
		}

		[Test]
		public void Url ()
		{
			//Should Be available Locally
			Assert.AreEqual (Book.Url, "book://bakerframework.com/books/arthurconandoyle-thestudyinscarlet");
		}

	}
}
