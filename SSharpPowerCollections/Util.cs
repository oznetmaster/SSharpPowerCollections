//******************************
// Written by Peter Golde
// Copyright (c) 2004-2005, Wintellect
//
// Use and restribution of this code is subject to the license agreement 
// contained in the file "License.txt" accompanying this file.
//
// Modified November 2013 for Crestron S#
//
//******************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace Wintellect.PowerCollections
	{
	/// <summary>
	/// A holder class for various internal utility functions that need to be shared.
	/// </summary>
	internal static class Util
		{
		/// <summary>
		/// Determine if a type is cloneable: either a value type or implementing
		/// ICloneable.
		/// </summary>
		/// <param name="type">Type to check.</param>
		/// <param name="isValue">Returns if the type is a value type, and does not implement ICloneable.</param>
		/// <returns>True if the type is cloneable.</returns>
		public static bool IsCloneableType (Type type, out bool isValue)
			{
			isValue = false;

			if (typeof (ICloneable).IsAssignableFrom (type))
				{
				return true;
				}
			else if (type.IsValueType)
				{
				isValue = true;
				return true;
				}
			else
				return false;
			}

		/// <summary>
		/// Returns the simple name of the class, for use in exception messages. 
		/// </summary>
		/// <returns>The simple name of this class.</returns>
		public static string SimpleClassName (Type type)
			{
			string name = type.Name;

			// Just use the simple name.
			int index = name.IndexOfAny (new char[] { '<', '{', '`' });
			if (index >= 0)
				name = name.Substring (0, index);

			return name;
			}

		/// <summary>
		/// Wrap an enumerable so that clients can't get to the underlying 
		/// implementation via a down-cast.
		/// </summary>
		[Serializable]
		class WrapEnumerable<T> : IEnumerable<T>
			{
			IEnumerable<T> wrapped;

			/// <summary>
			/// Create the wrapper around an enumerable.
			/// </summary>
			/// <param name="wrapped">IEnumerable to wrap.</param>
			public WrapEnumerable (IEnumerable<T> wrapped)
				{
				this.wrapped = wrapped;
				}

			public IEnumerator<T> GetEnumerator ()
				{
				return wrapped.GetEnumerator ();
				}

			IEnumerator IEnumerable.GetEnumerator ()
				{
				return ((IEnumerable)wrapped).GetEnumerator ();
				}
			}

		/// <summary>
		/// Wrap an enumerable so that clients can't get to the underlying
		/// implementation via a down-case
		/// </summary>
		/// <param name="wrapped">Enumerable to wrap.</param>
		/// <returns>A wrapper around the enumerable.</returns>
		public static IEnumerable<T> CreateEnumerableWrapper<T> (IEnumerable<T> wrapped)
			{
			return new WrapEnumerable<T> (wrapped);
			}

		/// <summary>
		/// Gets the hash code for an object using a comparer. Correctly handles
		/// null.
		/// </summary>
		/// <param name="item">Item to get hash code for. Can be null.</param>
		/// <param name="equalityComparer">The comparer to use.</param>
		/// <returns>The hash code for the item.</returns>
		public static int GetHashCode<T> (T item, IEqualityComparer<T> equalityComparer)
			{
			if (item == null)
				return 0x1786E23C;
			else
				return equalityComparer.GetHashCode (item);
			}
		}

	internal class ArgumentOutOfRangeExceptionEx : ArgumentOutOfRangeException
		{
#if NETCF_3_5
		public ArgumentOutOfRangeExceptionEx (string paramName, object actualValue, string message)
			: base (paramName, message + " : " + actualValue)
			{

			}
#endif
		}

#if TARGET_SSHARP
	internal static class Debug
		{
		public static void Assert (bool condition)
			{
#if DEBUG
			if (!condition)
				return;

			try
				{
				throw new AssertException ();
				}
			catch (AssertException ex)
				{
				Crestron.SimplSharp.ErrorLog.Exception ("Assert failed", ex);
				}
#endif
			}

		public static void Assert (bool condition, string message)
			{
#if DEBUG
			if (!condition)
				return;

			Crestron.SimplSharp.ErrorLog.Error (message);
#endif
			}

		public static void Assert (bool condition, string message, string detailMessage)
			{
#if DEBUG
			if (!condition)
				return;

			Crestron.SimplSharp.ErrorLog.Error (message, detailMessage);
#endif
			}
		}

	internal class AssertException : Exception
		{

		}

	internal static class Console
		{
		public static void WriteLine (string format, params object[] arg)
			{
			Crestron.SimplSharp.CrestronConsole.PrintLine (format, arg);
			}

		public static void Write (string format, params object[] arg)
			{
			Crestron.SimplSharp.CrestronConsole.Print (format, arg);
			}

		public static void WriteLine ()
			{
			Crestron.SimplSharp.CrestronConsole.PrintLine (String.Empty);
			}
		}
#endif
	}
