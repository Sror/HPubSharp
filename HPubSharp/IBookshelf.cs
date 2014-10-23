using System;
using System.Collections.Generic;

namespace HPubSharp
{
	public interface IBookshelf
	{
	
		IList<IBook> Books { get; }

		bool Online { get; }

		void AddBook (IBook book);
	}
}

