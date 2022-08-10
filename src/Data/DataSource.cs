using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace BlazorTableTest.Data
{
    public class DataSource : IEnumerable<DataRow>
    {
        private List<DataRow> _rows;

        public int Count { get => _rows.Count; }

        public string[] FieldNames { get; private set; }

        public List<ExpandoObject> ObjectRepresentations => _rows.Select(r => (ExpandoObject)r.ObjectRepresentation).ToList();

        public DataSource()
        {
            _rows = new List<DataRow>();
        }

        public IEnumerator<DataRow> GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        public static DataSource LoadFromCSV(MemoryStream data)
        {
            DataSource dataSource = new DataSource();

            using (var parser = new TextFieldParser(data))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Get the names of our fields (first row)
                string[] fieldNames = parser.ReadFields();
                dataSource.FieldNames = fieldNames;

                while (!parser.EndOfData)
                {
                    DataRow dataRow = new DataRow(fieldNames);

                    string[] row = parser.ReadFields();

                    for (int i = 0; i < row.Length; i++)
                    {
                        string fieldName = fieldNames[i];

                        dataRow[fieldName].SetValue(row[i]);

                        if (fieldName.ToLower() == "id" && dataRow.PrimaryField == null)
                        {
                            dataRow.PrimaryField = dataRow[fieldName];
                        }

                        if (fieldName.ToLower() == "lon" && dataRow.LonField == null)
                        {
                            dataRow[fieldName].Type = DataType.Longitude;
                            dataRow.LonField = dataRow[fieldName];
                        }
                        else if (fieldName.ToLower() == "lat" && dataRow.LatField == null)
                        {
                            dataRow[fieldName].Type = DataType.Latitude;
                            dataRow.LatField = dataRow[fieldName];
                        }
                    }

                    dataSource._rows.Add(dataRow);
                }
            }

            return dataSource;
        }
    }

    public class Sutun
    {
        public int id { get; set; }
        public string name { get; set; }
        public string whatever_field { get; set; }
    }
}
