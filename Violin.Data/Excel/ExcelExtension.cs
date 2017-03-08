namespace Violin.Data.Excel
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.IO;
	using System.Linq;
	using ClosedXML.Excel;

	public static class ExcelExtension
	{
		/// <summary>
		/// 将一个 <see cref="XLWorkbook"/> 实例的 Excel 表格转换为等效的 <see cref="DataTable"/>
		/// </summary>
		/// <param name="workbook">可供读取的 <see cref="XLWorkbook"/> 实例</param>
		/// <param name="workbookIndex">需要读取的工作簿名称</param>
		/// <param name="hasHeader">该 Excel 是否包含表头</param>
		/// <returns>转换后的 <see cref="DataTable"/> 实例</returns>
		static public DataTable ToDataTable(this XLWorkbook workbook, int workbookIndex = 1, int sheetIndex = 1, bool hasHeader = true)
		{
			return workbook.Worksheet(workbookIndex).GetTable(hasHeader);
		}

		/// <summary>
		/// 将一个 <see cref="XLWorkbook"/> 实例的 Excel 表格转换为等效的 <see cref="DataTable"/>
		/// </summary>
		/// <param name="workbook">可供读取的 <see cref="XLWorkbook"/> 实例</param>
		/// <param name="workbookName">需要读取的工作簿名称</param>
		/// <param name="hasHeader">该 Excel 是否包含表头</param>
		/// <returns>转换后的 <see cref="DataTable"/> 实例</returns>
		static public DataTable ToDataTable(this XLWorkbook workbook, string workbookName, string sheetName, bool hasHeader = true)
		{
			return workbook.Worksheet(workbookName).GetTable(hasHeader);
		}

		/// <summary>
		/// 获取 <see cref="IXLWorksheet"/> 中的表格并将其转换为 <see cref="DataTable"/>
		/// </summary>
		/// <param name="sheet">需要查询的 <see cref="IXLWorksheet"/> 实例</param>
		/// <param name="hasHeader">该 <see cref="IXLWorksheet"/> 中是否包含表头</param>
		/// <returns>该 <see cref="IXLWorksheet"/> 实例等效的 <see cref="DataTable"/></returns>
		static private DataTable GetTable(this IXLWorksheet sheet, bool hasHeader)
		{
			var table = new DataTable();
			var header = hasHeader;

			foreach (var row in sheet.Rows())
			{
				if(header)
				{
					table.Columns.AddRange(row.ToDataRow(r => new DataColumn(r.GetString())).ToArray());

					header = false;
					continue;
				}


				table.Rows.Add(row.ToDataRow(r => r.GetValue<dynamic>()).ToArray());
			}

			return table;
		}

		/// <summary>
		/// 迭代 <see cref="IXLRow"/> 实例并将返回自定内容的 <seealso cref="IEnumerable{T}"/> 集合
		/// </summary>
		/// <typeparam name="T">返回集合中的内容</typeparam>
		/// <param name="row">需要迭代的 <see cref="IXLRow"/> 实例</param>
		/// <param name="func">要对 <see cref="IXLRow"/> 中每个 <see cref="IXLCell"/> 操作的 <see cref="Func{T1, T2}"/> 委托</param>
		/// <returns>返回通过 <see cref="Func{T, TResult}"/> 中得到的值的 <see cref="IEnumerable{T}"/> 集合 </returns>
		static private IEnumerable<T> ToDataRow<T>(this IXLRow row, Func<IXLCell, T> func)
		{
			var list = new List<T>();

			foreach (var cell in row.Cells())
			{
				list.Add(func(cell));
			}

			return list;
		}

		/// <summary>
		/// 将 <see cref="DataTable"/> 中的内容以 Excel2007 的格式写入 <see cref="MemoryStream"/>
		/// </summary>
		/// <param name="table">要写入 <see cref="MemoryStream"/> 的 <see cref="DataTable"/> 数据表实例</param>
		/// <returns>一个可读取的 <see cref="MemoryStream"/> 实例</returns>
		static public MemoryStream ToExcel(this DataTable table)
		{
			var workbook = new XLWorkbook();
			workbook.AddWorksheet(table);

			MemoryStream excelWrite = new MemoryStream();
			workbook.SaveAs(excelWrite, true);

			return new MemoryStream(excelWrite.ToArray());
		}

		/// <summary>
		/// 将 <see cref="DataTable"/> 中的内容以 Excel2007 的格式写入磁盘
		/// </summary>
		/// <param name="table">要写入 <see cref="MemoryStream"/> 的 <see cref="DataTable"/> 数据表实例</param>
		/// <param name="filePath">保存文件的存储路径</param>
		static public void ToExcel(this DataTable table, string filePath)
		{
			using (var file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
			{
				var workbook = new XLWorkbook();
				workbook.AddWorksheet(table, "Sheet1");
				workbook.SaveAs(file, false);
			}
		}
	}
}
