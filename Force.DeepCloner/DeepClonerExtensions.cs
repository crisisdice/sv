using System;
using System.Security;
using Force.DeepCloner.Helpers;

namespace Force.DeepCloner
{
	/// <summary>
	/// Extensions for object cloning
	/// </summary>
	public static class DeepClonerExtensions
	{
		/// <summary>
		/// Performs deep (full) copy of object and related graph
		/// </summary>
		public static T DeepClone<T>(this T obj)
		{
			return DeepClonerGenerator.CloneObject(obj);
		}

		/// <summary>
		/// Performs deep (full) copy of object and related graph to existing object
		/// </summary>
		/// <returns>existing filled object</returns>
		/// <remarks>Method is valid only for classes, classes should be descendants in reality, not in declaration</remarks>
		public static TTo DeepCloneTo<TFrom, TTo>(this TFrom objFrom, TTo objTo) where TTo : class, TFrom
		{
			return (TTo)DeepClonerGenerator.CloneObjectTo(objFrom, objTo, isDeep: true);
		}

		/// <summary>
		/// Performs shallow copy of object to existing object
		/// </summary>
		/// <returns>existing filled object</returns>
		/// <remarks>Method is valid only for classes, classes should be descendants in reality, not in declaration</remarks>
		public static TTo ShallowCloneTo<TFrom, TTo>(this TFrom objFrom, TTo objTo) where TTo : class, TFrom
		{
			return (TTo)DeepClonerGenerator.CloneObjectTo(objFrom, objTo, isDeep: false);
		}

		/// <summary>
		/// Performs shallow (only new object returned, without cloning of dependencies) copy of object
		/// </summary>
		public static T ShallowClone<T>(this T obj)
		{
			return ShallowClonerGenerator.CloneObject(obj);
		}

		static DeepClonerExtensions()
		{
			if (!PermissionCheck())
			{
				throw new SecurityException("DeepCloner should have enough permissions to run. Grant FullTrust or Reflection permission.");
			}
		}

		private static bool PermissionCheck()
		{
			try
			{
				new object().ShallowClone();
			}
			catch (VerificationException)
			{
				return false;
			}
			catch (MemberAccessException)
			{
				return false;
			}
			return true;
		}
	}
}