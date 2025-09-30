using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CsvReader.Services
{
    /// <summary>
    /// Service for loading and parsing CSV files
    /// </summary>
    public class CsvReaderService
    {
        /// <summary>
        /// Load CSV file and return data as List of Lists
        /// </summary>
        /// <param name="filePath">Path to CSV file</param>
        /// <returns>CSV data as List&lt;List&lt;string&gt;&gt;</returns>
        public List<List<string>> LoadCsv(string filePath)
        {
            var records = new List<List<string>>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false, // We'll treat all rows as data
                MissingFieldFound = null, // Don't throw on missing fields
                BadDataFound = null // Don't throw on bad data
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, config))
            {
                while (csv.Read())
                {
                    var row = new List<string>();
                    var recordLength = csv.Parser.Record?.Length ?? 0;

                    for (int i = 0; i < recordLength; i++)
                    {
                        row.Add(csv.GetField(i) ?? string.Empty);
                    }

                    records.Add(row);
                }
            }

            return records;
        }
    }
}
