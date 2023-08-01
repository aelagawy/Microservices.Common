﻿using Dapper;
using System.Reflection;

namespace Microservices.Common.Mappings
{
    /// <summary>
    /// Uses the Name value of the ColumnAttribute specified, otherwise maps as usual.
    /// https://gist.github.com/senjacob/8539127
    /// </summary>
    /// <typeparam name="T">The type of the object that this mapper applies to.</typeparam>
    public class ColumnAttributeTypeMapper<T> : FallbackTypeMapper
	{
		public static readonly string ColumnAttributeName = "ColumnAttribute";

		public ColumnAttributeTypeMapper()
			: base(new SqlMapper.ITypeMap[]
			{
				new CustomPropertyTypeMap(typeof (T), SelectProperty),
				new DefaultTypeMap(typeof (T))
			})
		{
		}

		private static PropertyInfo? SelectProperty(Type type, string columnName)
		{
			return
				type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).
					FirstOrDefault(
						prop =>
						prop.GetCustomAttributes(false)
							// Search properties to find the one ColumnAttribute applied with Name property set as columnName to be Mapped 
							.Any(attr => attr.GetType().Name == ColumnAttributeName
										 &&
										 attr.GetType().GetProperties(BindingFlags.Public |
																	  BindingFlags.NonPublic |
																	  BindingFlags.Instance)
											 .Any(
												 f =>
												 f.Name == "Name" &&
												 f.GetValue(attr).ToString().ToLower() == columnName.ToLower()))
						&& // Also ensure the property is not read-only
						(prop.DeclaringType == type
							 ? prop.GetSetMethod(true)
							 : prop.DeclaringType.GetProperty(prop.Name,
															  BindingFlags.Public | BindingFlags.NonPublic |
															  BindingFlags.Instance).GetSetMethod(true)) != null
					);
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class ColumnAttribute : Attribute
	{
		public string Name { get; set; }
		public ColumnAttribute() { }
		public ColumnAttribute(string Name) { this.Name = Name; }
	}

	public class FallbackTypeMapper : SqlMapper.ITypeMap
	{
		private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;

		public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
		{
			_mappers = mappers;
		}

		public ConstructorInfo? FindConstructor(string[] names, Type[] types)
		{
			foreach (var mapper in _mappers)
			{
				try
				{
					ConstructorInfo result = mapper.FindConstructor(names, types);
					if (result != null)
					{
						return result;
					}
				}
				catch (NotImplementedException)
				{
				}
			}
			return null;
		}

		public ConstructorInfo? FindExplicitConstructor()
		{
			foreach (var mapper in _mappers)
			{
				try
				{
					ConstructorInfo result = mapper.FindExplicitConstructor();
					if (result != null)
					{
						return result;
					}
				}
				catch (NotImplementedException)
				{
				}
			}
			return null;
		}

		public SqlMapper.IMemberMap? GetConstructorParameter(ConstructorInfo constructor, string columnName)
		{
			foreach (var mapper in _mappers)
			{
				try
				{
					var result = mapper.GetConstructorParameter(constructor, columnName);
					if (result != null)
					{
						return result;
					}
				}
				catch (NotImplementedException)
				{
				}
			}
			return null;
		}

		public SqlMapper.IMemberMap? GetMember(string columnName)
		{
			foreach (var mapper in _mappers)
			{
				try
				{
					var result = mapper.GetMember(columnName);
					if (result != null)
					{
						return result;
					}
				}
				catch (NotImplementedException)
				{
				}
			}
			return null;
		}
	}
}
