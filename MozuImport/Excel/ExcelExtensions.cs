using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;

namespace MozuImport.Excel
{
    public static class ExcelExtensions
    {
        public static string AsString(this object[,] values, int row, int col, string defaultValue)
        {
            if (values[row, col] == null)
                return defaultValue;
            else
            {
                return values[row, col].ToString();
            }
        }

        public static bool AsBoolean(this object[,] values, int row, int col, bool defaultValue)
        {
            string text = values.AsString(row, col, defaultValue.ToString());
            if ((String.Compare(text, "yes", true) == 0) ||
                (String.Compare(text, "y", true) == 0) ||
                (String.Compare(text, "t", true) == 0) ||
                (String.Compare(text, "true", true) == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Int32 AsInt(this object[,] values, int row, int col, int defaultValue)
        {
            string text = values.AsString(row, col, defaultValue.ToString());
            return Convert.ToInt32(text);
        }

        public static Int32 AsDecimal(this object[,] values, int row, int col, decimal defaultValue)
        {
            string text = values.AsString(row, col, defaultValue.ToString());
            return Convert.ToInt32(text);
        }

        public static Int32 ToInt32(this string text, Int32 defaultValue)
        {
            Int32 valueInt = 0;
            if (Int32.TryParse(text, out valueInt))
            {
                return valueInt;
            }
            else
            {
                return defaultValue;
            }
        }

        public static Decimal ToDecimal(this string text, Decimal defaultValue)
        {
            Decimal valueInt = 0;
            if (Decimal.TryParse(text, out valueInt))
            {
                return valueInt;
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime ToDateTime(this string text, DateTime defaultValue)
        {
            DateTime value;
            if (DateTime.TryParse(text, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public static Boolean ToBoolean(this string text, bool defaultValue)
        {
            if ((System.String.Compare(text, "y", System.StringComparison.OrdinalIgnoreCase) == 0) ||
                (System.String.Compare(text, "yes", System.StringComparison.OrdinalIgnoreCase) == 0) ||
                (System.String.Compare(text, "t", System.StringComparison.OrdinalIgnoreCase) == 0) ||
                (System.String.Compare(text, "true", System.StringComparison.OrdinalIgnoreCase) == 0)||
                (System.String.Compare(text, "1", System.StringComparison.OrdinalIgnoreCase) == 0))
            {
                return true;
            }
            else if ((System.String.Compare(text, "n", System.StringComparison.OrdinalIgnoreCase) == 0) ||
                (System.String.Compare(text, "no", System.StringComparison.OrdinalIgnoreCase) == 0) ||
                (System.String.Compare(text, "f", System.StringComparison.OrdinalIgnoreCase) == 0) ||
                (System.String.Compare(text, "false", System.StringComparison.OrdinalIgnoreCase) == 0) ||
                (System.String.Compare(text, "0", System.StringComparison.OrdinalIgnoreCase) == 0))
            {
                return false;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int MaxRowNumber(this OfficeOpenXml.ExcelWorksheet worksheet)
        {
            // find the maxRow
            int maxRow = 0;
            int maxColumn = worksheet.MaxColumnNumber();

            for (maxRow = 1; maxRow <= 999999; maxRow++)
            {
                var nextTwo = worksheet.Cells[maxRow + 1, 1, maxRow + 2, maxColumn];

                // if next two rows contain all empty values then we are done
                if (nextTwo.All(v => string.IsNullOrEmpty(v.ToString())))
                {
                    break; //out
                }
            }
            return maxRow;
        }

        public static int MaxColumnNumber(this OfficeOpenXml.ExcelWorksheet worksheet)
        {
            // find the _maxColumn - ignoring random empty columns
            int maxColumn = 0;
            for (maxColumn = 1; maxColumn <= 999; maxColumn++)
            {
                // here is where we get the next two columns to 
                // decide if the current column is the last one.
                var nextTwo = worksheet.Cells[1, maxColumn + 1, 1, maxColumn + 2];

                // if this next two columns contain all empty values then we are done
                if (nextTwo.All(v => string.IsNullOrEmpty(v.ToString())))
                {
                    break; //out
                }
            }
            return maxColumn;
        }

        public static int NextRowNumber(this OfficeOpenXml.ExcelWorksheet worksheet)
        {
            return worksheet.MaxRowNumber() + 1;
        }

        public static IEnumerable<string> ColumnHeaders(this OfficeOpenXml.ExcelWorksheet worksheet)
        {
            List<string> headers = new List<string>();
            int maxColumn = worksheet.MaxColumnNumber();
            for (int columnNumber = 1; columnNumber < maxColumn; columnNumber++)
            {
                headers.Add(worksheet.GetValue(1, columnNumber).ToString());
            }
            return headers;
        }

        public static ExcelRange Cell(this OfficeOpenXml.ExcelWorksheet worksheet, int rowNumber, String headerName)
        {
            int columnNumber = worksheet.ColumnHeaderNumber(headerName, true);
            return worksheet.Cells[rowNumber, columnNumber];
        }

        public static int ColumnHeaderNumber(this OfficeOpenXml.ExcelWorksheet worksheet, String headerName, bool ignoreCase)
        {
            const int maxColumn = 9999;
            for (int columnNumber = 1; columnNumber < maxColumn; columnNumber++)
            {
                if (ignoreCase)
                {
                    if (string.Compare(headerName, worksheet.GetValue(1, columnNumber).ToString().Trim(),
                        StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return columnNumber;
                    }
                }
                else
                {
                    if (headerName == worksheet.GetValue(1, columnNumber).ToString().Trim())
                    {
                        return columnNumber;
                    }
                }
            }
            // There is no match
            throw new ApplicationException("Column header not found \'" + headerName + "\'");
        }


    }
}
