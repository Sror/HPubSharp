﻿using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HPubSharp
{
	public interface IBook : INotifyPropertyChanged
	{
	
		IList<string> Author { get; }

		bool AvailableLocally { get; set; }

		string BasePath { get; set; }

		IList<string> Contents { get; }

		DateTime Date { get; }

		string Icon { get; }

		string Id { get; }

		string Title { get; }

		string Url { get; }

		Task Download ();

		bool Reachable ();
	}
}

