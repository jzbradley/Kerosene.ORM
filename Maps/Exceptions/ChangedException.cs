﻿using System;

namespace Kerosene.ORM.Maps
{
	// ====================================================
	/// <summary>
	/// Represents an attempt to update or delete a record that has been changed in the database
	/// while its state was kept in the cache.
	/// </summary>
	[Serializable]
	public class ChangedException : Exception
	{
		public ChangedException() { }
		public ChangedException(string message) : base(message) { }
		public ChangedException(string message, Exception inner) : base(message, inner) { }
	}
}
