using System;

namespace HPubSharp.iOS
{
	/// <summary>
	/// System specific configurations.
	/// </summary>
	public static class Configs
	{
		/// <summary>
		/// Gets the path to the bookshelf directory.
		/// </summary>
		/// <value>The bookshelf path.</value>
		public static string BookshelfPath { 
			get{ return @"./Bookshelf/"; } 
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="HPubSharp.iOS.Configs"/> use bookshelf.
		/// </summary>
		/// <value><c>true</c> if use bookshelf; otherwise, <c>false</c>.</value>
		public static bool UseBookshelf { 
			get{ return true; } 
		}
	}
}

