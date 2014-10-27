using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HPubSharp
{
	public interface IBookshelf
	{
	
		IList<IBook> Books { get; }

		bool Online { get; }

		void AddBook (IBook book);

		Task<IList<IBook>> DownloadableBooks ();
	}
}

