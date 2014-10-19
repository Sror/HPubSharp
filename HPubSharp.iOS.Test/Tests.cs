using System;
using System.IO;
using System.Collections.Generic;

using NUnit.Framework;

namespace HPubSharp.iOS.Test
{
	[TestFixture]
	public class BookTest
	{
		private Book Book = new Book ();

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
	}
}
