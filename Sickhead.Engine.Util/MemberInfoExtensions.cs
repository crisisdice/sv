using System;
using System.Reflection;

namespace Sickhead.Engine.Util
{
	/// <summary>
	/// Allows Set/GetValue of MemberInfo(s) so that code does not need to
	/// be written to work specifically on PropertyInfo or FieldInfo.
	/// </summary>
	public static class MemberInfoExtensions
	{
		public static Type GetDataType(this MemberInfo info)
		{
			if (info is PropertyInfo)
			{
				return (info as PropertyInfo).PropertyType;
			}
			if (info is FieldInfo)
			{
				return (info as FieldInfo).FieldType;
			}
			throw new InvalidOperationException($"MemberInfo.GetDataType is not possible for type={info.GetType()}");
		}

		public static object GetValue(this MemberInfo info, object obj)
		{
			return info.GetValue(obj, null);
		}

		public static void SetValue(this MemberInfo info, object obj, object value)
		{
			info.SetValue(obj, value, null);
		}

		public static object GetValue(this MemberInfo info, object obj, object[] index)
		{
			if (info is PropertyInfo)
			{
				return (info as PropertyInfo).GetValue(obj, index);
			}
			if (info is FieldInfo)
			{
				return (info as FieldInfo).GetValue(obj);
			}
			throw new InvalidOperationException($"MemberInfo.GetValue is not possible for type={info.GetType()}");
		}

		public static void SetValue(this MemberInfo info, object obj, object value, object[] index)
		{
			if (info is PropertyInfo)
			{
				(info as PropertyInfo).SetValue(obj, value, index);
				return;
			}
			if (info is FieldInfo)
			{
				(info as FieldInfo).SetValue(obj, value);
				return;
			}
			_ = info is MethodInfo;
			throw new InvalidOperationException($"MemberInfo.SetValue is not possible for type={info.GetType()}");
		}

		public static bool IsStatic(this MemberInfo info)
		{
			if (info is PropertyInfo)
			{
				return (info as PropertyInfo).GetGetMethod(nonPublic: true).IsStatic;
			}
			if (info is FieldInfo)
			{
				return (info as FieldInfo).IsStatic;
			}
			if (info is MethodInfo)
			{
				return (info as MethodInfo).IsStatic;
			}
			throw new InvalidOperationException($"MemberInfo.IsStatic is not possible for type={info.GetType()}");
		}

		/// <summary>
		/// Returns true if this is a property or field that is accessible to be set via reflection
		/// on all platforms. Note: windows phone can only set public or internal scope members.
		/// </summary>        
		public static bool CanBeSet(this MemberInfo info)
		{
			if (info is PropertyInfo)
			{
				PropertyInfo obj = info as PropertyInfo;
				MethodAttributes methodAtt = obj.GetSetMethod().Attributes;
				if (obj.CanWrite)
				{
					if ((methodAtt & MethodAttributes.Public) != MethodAttributes.Public)
					{
						return (methodAtt & MethodAttributes.Assembly) != MethodAttributes.Assembly;
					}
					return false;
				}
				return true;
			}
			if (info is FieldInfo)
			{
				FieldInfo fi = info as FieldInfo;
				if (!fi.IsPrivate)
				{
					return !fi.IsFamily;
				}
				return false;
			}
			throw new InvalidOperationException($"MemberInfo.CanSet is not possible for type={info.GetType()}");
		}

		/// <summary>
		/// In Win8 the static Delegate.Create was removed and added
		/// instead as an instance method on MethodInfo. Therefore it 
		/// is most portable if the new api is used and this extension
		/// translates it to the older API on those platforms.
		/// </summary>        
		public static Delegate CreateDelegate(this MethodInfo method, Type type, object target)
		{
			return Delegate.CreateDelegate(type, target, method);
		}

		public static Delegate CreateDelegate(this MethodInfo method, Type type)
		{
			return Delegate.CreateDelegate(type, method);
		}
	}
}
