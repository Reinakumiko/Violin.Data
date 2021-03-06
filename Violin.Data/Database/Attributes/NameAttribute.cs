﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Violin.Data.Database.Attributes
{
	/// <summary>
	/// 用于标记类或成员在数据库中的名称
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct|AttributeTargets.Method|AttributeTargets.Property|AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	public sealed class NameAttribute : Attribute
	{
		/// <summary>
		/// 成员名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 为字段指定数据类型
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		/// 为字段设置数据格式
		/// </summary>
		public string Format { get; set; }

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
