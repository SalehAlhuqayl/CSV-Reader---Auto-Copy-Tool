using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using System.Linq;

namespace CsvReader.Services
{
    /// <summary>
    /// Service for loading and parsing Excel (.xlsx) files using built-in .NET libraries
    /// No external packages required!
    /// </summary>
    public class ExcelReaderService
    {
        /// <summary>
        /// Load Excel file and return data as List of Lists
        /// XLSX files are ZIP archives containing XML - we can read them natively!
        /// </summary>
        /// <param name="filePath">Path to Excel file</param>
        /// <returns>Excel data as List&lt;List&lt;string&gt;&gt;</returns>
        public List<List<string>> LoadExcel(string filePath)
        {
            var records = new List<List<string>>();

            try
            {
                using (var archive = ZipFile.OpenRead(filePath))
                {
                    // Find the worksheet XML file (first sheet)
                    var sheetEntry = archive.GetEntry("xl/worksheets/sheet1.xml");
                    if (sheetEntry == null)
                    {
                        throw new InvalidDataException("Invalid Excel file format");
                    }

                    // Load shared strings (Excel stores text in a separate file)
                    var sharedStrings = LoadSharedStrings(archive);

                    // Read the worksheet
                    using (var stream = sheetEntry.Open())
                    {
                        var doc = XDocument.Load(stream);
                        XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

                        var rows = doc.Descendants(ns + "row");
                        
                        foreach (var row in rows)
                        {
                            var rowData = new List<string>();
                            var cells = row.Elements(ns + "c");
                            
                            int currentCol = 0;
                            foreach (var cell in cells)
                            {
                                // Get cell reference (e.g., "A1", "B2")
                                var cellRef = cell.Attribute("r")?.Value;
                                var cellType = cell.Attribute("t")?.Value;
                                var valueElement = cell.Element(ns + "v");
                                
                                if (valueElement != null)
                                {
                                    var value = valueElement.Value;
                                    
                                    // If type is 's', it's a shared string
                                    if (cellType == "s" && int.TryParse(value, out int stringIndex))
                                    {
                                        if (stringIndex < sharedStrings.Count)
                                        {
                                            rowData.Add(sharedStrings[stringIndex]);
                                        }
                                        else
                                        {
                                            rowData.Add(value);
                                        }
                                    }
                                    else
                                    {
                                        rowData.Add(value);
                                    }
                                }
                                else
                                {
                                    rowData.Add(string.Empty);
                                }
                                
                                currentCol++;
                            }
                            
                            records.Add(rowData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"Error reading Excel file: {ex.Message}", ex);
            }

            return records;
        }

        /// <summary>
        /// Load shared strings from Excel file
        /// </summary>
        private List<string> LoadSharedStrings(ZipArchive archive)
        {
            var strings = new List<string>();
            var sharedStringsEntry = archive.GetEntry("xl/sharedStrings.xml");
            
            if (sharedStringsEntry != null)
            {
                using (var stream = sharedStringsEntry.Open())
                {
                    var doc = XDocument.Load(stream);
                    XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
                    
                    var stringItems = doc.Descendants(ns + "si");
                    foreach (var item in stringItems)
                    {
                        var textElement = item.Descendants(ns + "t").FirstOrDefault();
                        strings.Add(textElement?.Value ?? string.Empty);
                    }
                }
            }
            
            return strings;
        }
    }
}
