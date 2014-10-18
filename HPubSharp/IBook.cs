using System.Collections.Generic;

namespace HPubSharp
{
	public interface IBook
	{
		string BasePath { get; set; }

		IList<string> Author { get; }

		IList<string> Contents { get; }

		string Title { get; }
	}
}

