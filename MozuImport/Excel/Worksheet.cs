using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace MozuImport.Excel
{
    public class Worksheet
    {
        private readonly IDictionary<string, IList<RowData>> _cellData;
        private IDictionary<string, int> _columnIndexes;

        // Access to data objects
        private ExcelWorksheet _workSheet;
        private ExcelRange _usedRange;

        // members to track the key and row cursors
        private int _keyCursor;
        private int _rowCursor;
        private int _maxColumn = 0;
        private int _maxRow = 0;

        public Worksheet()
        {
            _cellData = new Dictionary<string, IList<RowData>>();
            _columnIndexes = new Dictionary<string, int>();

            _keyCursor = 0;
            _rowCursor = -1;
        }

        public void Load(string filePath, string workSheetName, string keyColumn, bool keyToUpper=false)
        {
            // Load the excel workbook and select the worksheet
            var fileInfo = new FileInfo(filePath);
            var package = new ExcelPackage(fileInfo);
            _workSheet = package.Workbook.Worksheets[workSheetName];

            // Get the used range
            _usedRange = GetUsedRange();

            // Create column index
            _columnIndexes = CreateColumnIndex();

            // access the cells from row two to the end
            for (int rowNumber = 2; rowNumber <= _maxRow; ++rowNumber)
            {
                // Create the rowData from the cellValues
                var rowData = new RowData(GetRowCellValues(rowNumber), _columnIndexes);

                // Get the data value of the keyColumn for this row
                var keyColumnValue = rowData[keyColumn];
                if (keyToUpper)
                    keyColumnValue = keyColumnValue.ToUpper();

                if (!_cellData.ContainsKey(keyColumnValue))
                {
                    _cellData[keyColumnValue] = new List<RowData>() { rowData };
                }
                else
                {
                    // Add rowData to current 
                    _cellData[keyColumnValue].Add(rowData);
                }
            }
        }

        private IDictionary<string, int> CreateColumnIndex()
        {
            var index = new Dictionary<string, int>();
            for (int column = 1; column <= _maxColumn; column++)
            {
                var keyName = _workSheet.Cells[1, column].GetValue<string>();
                if (!string.IsNullOrEmpty(keyName))
                {
                    index.Add(keyName, column);
                }
            }
            return index;
        }

        private ExcelRange GetUsedRange()
        {
            // find the _maxColumn - ignoring random empty columns
            for (_maxColumn = 1; _maxColumn <= 999; _maxColumn++)
            {
                // here is where we get the next two columns to 
                // decide if the current column is the last one.
                var nextTwo = _workSheet.Cells[1, _maxColumn + 1, 1, _maxColumn + 2];

                // if this next two columns contain all empty values then we are done
                if (nextTwo.All(v => string.IsNullOrEmpty(v.ToString())))
                {
                    break; //out
                }
            }

            // find the _maxRow
            for (_maxRow = 1; _maxRow <= 999999; _maxRow++)
            {
                var nextTwo = _workSheet.Cells[_maxRow + 1, 1, _maxRow + 2, _maxColumn];

                // if next two rows contain all empty values then we are done
                if (nextTwo.All(v => string.IsNullOrEmpty(v.ToString())))
                {
                    break; //out
                }
            }

            return _workSheet.Cells[1, 1, _maxRow, _maxColumn];
        }

        /// <summary>
        /// Returns true of the worksheet has a column with the columnName
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool HasColumn(string columnName)
        {
            return _columnIndexes.ContainsKey(columnName);
        }

        /// <summary>
        /// Get a list of strings for the header row
        /// </summary>
        public IList<string> HeaderData
        {
            get
            {
                return GetDataForRow(1);
            }
        }

        public bool HasRows(string keyValue)
        {
            return _cellData.ContainsKey(keyValue);
        }

        public IList<RowData> GetRows(string keyValue)
        {
            if (HasRows(keyValue))
                return _cellData[keyValue];
            else
                return new List<RowData>();
        }

        public RowData NextRow()
        {
            if (_keyCursor >= _cellData.Keys.Count)
                return null;
            else
            {
                if (_rowCursor < _cellData.Values.ElementAt(_keyCursor).Count - 1)
                {
                    _rowCursor++;
                }
                else
                {
                    // advance the keycursor and reset the rowcursor
                    _keyCursor++;
                    _rowCursor = 0;
                }

                if (_keyCursor < _cellData.Keys.Count &&
                    _rowCursor < _cellData.Values.ElementAt(_keyCursor).Count)
                {
                    return new RowData(_cellData.Values.ElementAt(_keyCursor).ElementAt(_rowCursor),
                        _columnIndexes);
                }
                else
                {
                    return null;
                }
            }
        }

        private IList<string> GetRowCellValues(int rowNumber)
        {
            var range = _workSheet.Cells[rowNumber, 1, rowNumber, _maxColumn];
            List<string> values = new List<string>();
            for (int columnNumber = 1; columnNumber <= _maxColumn; columnNumber++)
            {
                values.Add(Convert.ToString(_workSheet.GetValue(rowNumber, columnNumber)));
            }
            return values;
        }

        /// <summary>
        /// Get a list of strings contained in data for rowNumber
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
        public RowData GetDataForRow(int rowNumber)
        {
            var range = _workSheet.Cells[rowNumber, 1, rowNumber, _maxColumn];
            var rowData = range.Where(v => !string.IsNullOrEmpty(v.ToString())).Select(v => v.Value.ToString()).ToList();
            return new RowData(rowData, _columnIndexes);
        }

        public string Cell(string columnName, int rowNumber)
        {
            int columnNumber = _columnIndexes[columnName];
            return _workSheet.Cells[rowNumber, columnNumber].GetValue<string>();
        }
    }

    public class RowData : List<string>
    {
        private readonly IDictionary<string, int> _columnIndexes;

        public RowData(IEnumerable<string> values, IDictionary<string, int> columnIndexes)
            : base(values)
        {
            this._columnIndexes = columnIndexes;
        }

        public string this[string keyColumn]
        {
            get { return Cell(keyColumn); }
        }

        public string Cell(string columnName)
        {
            if (this._columnIndexes.ContainsKey(columnName))
            {
                // excel column indexes start at 1, 
                // but the base collection starts at 0
                return base[_columnIndexes[columnName] - 1];
            }
            else
            {
                return null;
            }
        }
    }
}
