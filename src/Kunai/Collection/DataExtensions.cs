using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kunai.CollectionExt
{
	public static class DataExtensions
	{
		/// Converts a DataRow object into a Hashtable object, 
		/// where the key is the ColumnName and the value is the row value.
		/// </summary>
		/// <param name="dr"></param>
		/// <returns></returns>
		public static Hashtable ToHashTable(this DataRow dr)
		{
			Hashtable htReturn = new Hashtable(dr.ItemArray.Length);
			foreach (DataColumn dc in dr.Table.Columns)
				htReturn.Add(dc.ColumnName, dr[dc.ColumnName]);

			return htReturn;
		}

		public static DataTable Dedup(this DataTable tblIn, string KeyColName)
		{
			DataTable tblOut = tblIn.Clone();
			foreach (DataRow row in tblIn.Rows)
			{
				bool found = false;
				string caseIDToTest = row[KeyColName].ToString();
				foreach (DataRow row2 in tblOut.Rows)
				{
					if (row2[KeyColName].ToString() == caseIDToTest)
					{
						found = true;
						break;
					}
				}
				if (!found)
					tblOut.ImportRow(row);
			}
			return tblOut;
		}

		public static DataTable SelectRows(this DataTable dt, string whereExpression, string orderByExpression)
		{
			dt.DefaultView.RowFilter = whereExpression;
			dt.DefaultView.Sort = orderByExpression;
			return dt.DefaultView.ToTable();
		}

		/// <summary>
		/// This method extends the GetBoolean method of the data reader to allow calling by the field name
		/// </summary>
		/// <param name="dataReader">The datareader object we are extending</param>
		/// <param name="fieldName">The field name that we are getting the Boolean value for</param>
		/// <returns></returns>
		public static bool GetBoolean(this IDataReader dataReader, string fieldName)
		{
			var fieldOrdinal = dataReader.GetOrdinal(fieldName);
			var retVal = false;

			if (!dataReader.IsDBNull(fieldOrdinal))
			{
				try
				{
					retVal = dataReader.GetBoolean(fieldOrdinal);
				}
				catch (InvalidCastException)
				{
					//We will swallow this exception as it's expected if our value has a dataType of bit. 
					//We will try and handle that by casting to an Int16.
					//If it fails here, we will allow the exception to get thrown
					return (dataReader.GetInt16(fieldOrdinal) == 1);
				}

			}

			return retVal;
		}

		/// <summary>
		/// This method extends the GetDateTime method of the data reader to allow calling by the field name
		/// </summary>
		/// <param name="dataReader">The datareader object we are extending</param>
		/// <param name="fieldName">The field name that we are getting the DateTime value for</param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime GetDateTime(this IDataReader dataReader, string fieldName, DateTime defaultValue = default(DateTime))
		{
			var fieldOrdinal = dataReader.GetOrdinal(fieldName);
			return dataReader.IsDBNull(fieldOrdinal) ? defaultValue : dataReader.GetDateTime(fieldOrdinal);
		}

		/// <summary>
		/// This method extends the GetDecimal method of the data reader to allow calling by the field name
		/// </summary>
		/// <param name="dataReader">The datareader object we are extending</param>
		/// <param name="fieldName">The field name that we are getting the Decimal value for</param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Decimal GetDecimal(this IDataReader dataReader, string fieldName, Decimal defaultValue = 0m)
		{
			var fieldOrdinal = dataReader.GetOrdinal(fieldName);
			return dataReader.IsDBNull(fieldOrdinal) ? defaultValue : dataReader.GetDecimal(fieldOrdinal);
		}

		/// <summary>
		/// This method extends the GetDouble method of the data reader to allow calling by the field name
		/// </summary>
		/// <param name="dataReader">The datareader object we are extending</param>
		/// <param name="fieldName">The field name that we are getting the Double value for</param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double GetDouble(this IDataReader dataReader, string fieldName, double defaultValue = 0d)
		{
			var fieldOrdinal = dataReader.GetOrdinal(fieldName);
			return dataReader.IsDBNull(fieldOrdinal) ? defaultValue : dataReader.GetDouble(fieldOrdinal);
		}

		/// <summary>
		/// This method extends the GetFloat method of the data reader to allow calling by the field name
		/// </summary>
		/// <param name="dataReader">The datareader object we are extending</param>
		/// <param name="fieldName">The field name that we are getting the Float value for</param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float GetFloat(this IDataReader dataReader, string fieldName, float defaultValue = 0f)
		{
			var fieldOrdinal = dataReader.GetOrdinal(fieldName);
			return dataReader.IsDBNull(fieldOrdinal) ? defaultValue : dataReader.GetFloat(fieldOrdinal);
		}

		/// <summary>
		/// This method extends the GetGuid method of the data reader to allow calling by the field name
		/// </summary>
		/// <param name="dataReader">The datareader object we are extending</param>
		/// <param name="fieldName">The field name that we are getting the Guid value for</param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Guid GetGuid(this IDataReader dataReader, string fieldName, Guid defaultValue = default(Guid))
		{
			var fieldOrdinal = dataReader.GetOrdinal(fieldName);
			return dataReader.IsDBNull(fieldOrdinal) ? defaultValue : dataReader.GetGuid(fieldOrdinal);
		}

		/// <summary>
		/// This method extends the GetInt16 method of the data reader to allow calling by the field name
		/// </summary>
		/// <param name="dataReader">The datareader object we are extending</param>
		/// <param name="fieldName">The field name that we are getting the Int16 value for</param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Int16 GetInt16(this IDataReader dataReader, string fieldName, Int16 defaultValue)
		{
			var fieldOrdinal = dataReader.GetOrdinal(fieldName);
			return dataReader.IsDBNull(fieldOrdinal) ? defaultValue : dataReader.GetInt16(fieldOrdinal);
		}

		/// <summary>
		/// This method extends the GetInt32 method of the data reader to allow calling by the field name
		/// </summary>
		/// <param name="dataReader">The datareader object we are extending</param>
		/// <param name="fieldName">The field name that we are getting the Int32 value for</param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Int32 GetInt32(this IDataReader dataReader, string fieldName, Int32 defaultValue = 0)
		{
			var fieldOrdinal = dataReader.GetOrdinal(fieldName);
			return dataReader.IsDBNull(fieldOrdinal) ? defaultValue : dataReader.GetInt32(fieldOrdinal);
		}

		/// <summary>
		/// This method extends the GetInt64 method of the data reader to allow calling by the field name
		/// </summary>
		/// <param name="dataReader">The datareader object we are extending</param>
		/// <param name="fieldName">The field name that we are getting the Int64 value for</param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Int64 GetInt64(this IDataReader dataReader, string fieldName, Int64 defaultValue = 0)
		{
			var fieldOrdinal = dataReader.GetOrdinal(fieldName);
			return dataReader.IsDBNull(fieldOrdinal) ? defaultValue : dataReader.GetInt64(fieldOrdinal);
		}

		/// <summary>
		/// This method extends the GetString method of the data reader to allow calling by the field name
		/// </summary>
		/// <param name="dataReader">The datareader object we are extending</param>
		/// <param name="fieldName">The field name that we are getting the string value for</param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string GetString(this IDataReader dataReader, string fieldName, string defaultValue = "")
		{
			var fieldOrdinal = dataReader.GetOrdinal(fieldName);
			return dataReader.IsDBNull(fieldOrdinal) ? defaultValue : dataReader.GetString(fieldOrdinal);
		}

		/// <summary>
		/// Creates a cloned and detached copy of a DataRow instance
		/// </summary>
		/// <typeparam name="T">The type of the DataRow if strongly typed</typeparam>
		/// <returns>
		/// An instance of the new DataRow
		/// </returns>
		public static T Clone<T>(this DataRow dataRow, DataTable parentTable)
			where T : DataRow
		{
			T clonedRow = (T)parentTable.NewRow();
			clonedRow.ItemArray = dataRow.ItemArray;
			return clonedRow;
		}

		public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
		{
			DataTable dtReturn = new DataTable();

			// column names 
			PropertyInfo[] oProps = null;

			if (varlist == null) return dtReturn;

			foreach (T rec in varlist)
			{
				// Use reflection to get property names, to create table, Only first time, others will follow 
				if (oProps == null)
				{
					oProps = ((Type)rec.GetType()).GetProperties();
					foreach (PropertyInfo pi in oProps)
					{
						Type colType = pi.PropertyType;

						if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
						{
							colType = colType.GetGenericArguments()[0];
						}

						dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
					}
				}

				DataRow dr = dtReturn.NewRow();

				foreach (PropertyInfo pi in oProps)
				{
					dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
					(rec, null);
				}

				dtReturn.Rows.Add(dr);
			}
			return dtReturn;
		}


		public static void ToCSV(this DataTable table, string delimiter, bool includeHeader)
		{

			StringBuilder result = new StringBuilder();



			if (includeHeader)
			{

				foreach (DataColumn column in table.Columns)
				{

					result.Append(column.ColumnName);

					result.Append(delimiter);

				}



				result.Remove(--result.Length, 0);

				result.Append(Environment.NewLine);

			}



			foreach (DataRow row in table.Rows)
			{

				foreach (object item in row.ItemArray)
				{

					if (item is System.DBNull)

						result.Append(delimiter);

					else
					{

						string itemAsString = item.ToString();

						// Double up all embedded double quotes

						itemAsString = itemAsString.Replace("\"", "\"\"");



						// To keep things simple, always delimit with double-quotes

						// so we don't have to determine in which cases they're necessary

						// and which cases they're not.

						itemAsString = "\"" + itemAsString + "\"";



						result.Append(itemAsString + delimiter);

					}

				}



				result.Remove(--result.Length, 0);

				result.Append(Environment.NewLine);

			}


			using (StreamWriter writer = new StreamWriter(@"C:\log.csv", true))
			{
				writer.Write(result.ToString());

			}




		}


		public static List<string> ToCSV(this IDataReader dataReader, bool includeHeaderAsFirstRow, string separator)
		{
			List<string> csvRows = new List<string>();
			StringBuilder sb = null;

			if (includeHeaderAsFirstRow)
			{
				sb = new StringBuilder();
				for (int index = 0; index < dataReader.FieldCount; index++)
				{
					if (dataReader.GetName(index) != null)
						sb.Append(dataReader.GetName(index));

					if (index < dataReader.FieldCount - 1)
						sb.Append(separator);
				}
				csvRows.Add(sb.ToString());
			}

			while (dataReader.Read())
			{
				sb = new StringBuilder();
				for (int index = 0; index < dataReader.FieldCount - 1; index++)
				{
					if (!dataReader.IsDBNull(index))
					{
						string value = dataReader.GetValue(index).ToString();
						if (dataReader.GetFieldType(index) == typeof(String))
						{
							//If double quotes are used in value, ensure each are replaced but 2.
							if (value.IndexOf("\"") >= 0)
								value = value.Replace("\"", "\"\"");

							//If separtor are is in value, ensure it is put in double quotes.
							if (value.IndexOf(separator) >= 0)
								value = "\"" + value + "\"";
						}
						sb.Append(value);
					}

					if (index < dataReader.FieldCount - 1)
						sb.Append(separator);
				}

				if (!dataReader.IsDBNull(dataReader.FieldCount - 1))
					sb.Append(dataReader.GetValue(dataReader.FieldCount - 1).ToString().Replace(separator, " "));

				csvRows.Add(sb.ToString());
			}
			dataReader.Close();
			sb = null;
			return csvRows;
		}
	}
}
