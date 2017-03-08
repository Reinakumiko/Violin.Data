using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Violin.Data.Database.Attributes
{
	/// <summary>
	/// 用于标记类或成员在数据库中的名称
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct|AttributeTargets.Method|AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
	public sealed class NameAttribute : Attribute
	{
		/// <summary>
		/// 成员名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 实例化一个新的特性以标记该成员的名称
		/// </summary>
		/// <param name="name">成员名</param>
		public NameAttribute(string name)
		{
			Name = name;
		}
	}
}
