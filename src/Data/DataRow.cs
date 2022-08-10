using System;
using System.Collections.Generic;
using System.Dynamic;

namespace BlazorTableTest.Data
{
    public class DataRow
    {
        private Dictionary<string, DataField> _fields;

        public DataField LatField { get; set; }

        public DataField LonField { get; set; }

        public DataField PrimaryField { get; set; }

        /// <summary>
        /// Provides an object representation containing the fields in this DataRow.
        /// </summary>
        public dynamic ObjectRepresentation;

        /// <summary>
        /// Number of columns in this row
        /// </summary>
        public int Count { get => _fields.Count; }

        public DataField this[string name]
        {
            get
            {
                return _fields[name];
            }
        }

        public DataRow(string[] fieldNames)
        {
            _fields = new Dictionary<string, DataField>();

            foreach (var fieldName in fieldNames)
            {
                var dataField = new DataField(fieldName);
                dataField.ValueChanged += DataField_ValueChanged;

                _fields.Add(fieldName, dataField);
            }

            object person = new { Lol = "" };
            
           
            this.UpdateObjectRepresentation();
        }

        private void UpdateObjectRepresentation()
        {
            this.ObjectRepresentation = new ExpandoObject();

            foreach (var fieldName in _fields.Keys)
            {
                ((IDictionary<String, object>)this.ObjectRepresentation)[fieldName] = _fields[fieldName].GetValue();
            }
        }

        private void DataField_ValueChanged(object sender, System.EventArgs e)
        {
            this.UpdateObjectRepresentation();
        }
    }
}
