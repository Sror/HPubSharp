using System.Collections.Generic;
using System;

namespace HPubSharp
{
	public interface IBook
	{

		IList<string> Author { get; }

		bool AvailableLocally { get; set; }

		string BasePath { get; set; }

		IList<string> Contents { get; }

		DateTime Date { get; }

		string Icon { get; }

		string Title { get; }

		string Url { get; }
	}
}

