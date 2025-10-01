// Excel support temporarily disabled due to EPPlus package download issues
// Will be re-enabled once network/NuGet issues are resolved

/*
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace CsvReader.Services
{
    /// <summary>
    /// Service for loading and parsing Excel (.xlsx) files
    /// </summary>
    public class ExcelReaderService
    {
        public ExcelReaderService()
        {
            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        /// <summary>
        /// Load Excel file and return data as List of Lists
        /// </summary>
        /// <param name="filePath">Path to Excel file</param>
        /// <param name="worksheetIndex">Index of worksheet to read (default: 0)</param>
        /// <returns>Excel data as List&lt;List&lt;string&gt;&gt;</returns>
        public List<List<string>> LoadExcel(string filePath, int worksheetIndex = 0)
        {
            var records = new List<List<string>>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                if (package.Workbook.Worksheets.Count == 0)
                {
                    throw new InvalidDataException("Excel file contains no worksheets");
                }

                var worksheet = package.Workbook.Worksheets[worksheetIndex];
                
                if (worksheet == null)
                {
                    throw new InvalidDataException($"Worksheet at index {worksheetIndex} not found");
                }

                // Read all rows and columns
                int rowCount = worksheet.Dimension?.Rows ?? 0;
                int colCount = worksheet.Dimension?.Columns ?? 0;

                for (int row = 1; row <= rowCount; row++)
                {
                    var rowData = new List<string>();
                    
                    for (int col = 1; col <= colCount; col++)
                    {
                        var cellValue = worksheet.Cells[row, col].Value?.ToString() ?? string.Empty;
                        rowData.Add(cellValue);
                    }
                    
                    records.Add(rowData);
                }
            }

            return records;
        }
    }
}
*/
