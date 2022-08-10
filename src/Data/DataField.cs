using System;

namespace BlazorTableTest.Data
{
    public class DataField
    {
        private DataType _dataType;

        public DataType Type
        {
            get => _dataType;
            set 
            {
                _dataType = value;
            }
        }
        
        public string Name { get; }

        private object _value;

        public event EventHandler ValueChanged;

        public DataField(string name)
        {
            this.Name = name;
            this.Type = DataType.String;
            _value = "";
        }


        public T GetValue<T>()
        {
            return (T)Convert.ChangeType(_value, typeof(T));
        }

        public object GetValue()
        {
            return _value;
        }

        public void SetValue(object value)
        {
            _value = value;

            this.ValueChanged?.Invoke(this, new EventArgs());
        }
    }

    public enum DataType
    {
        String,
        Numeric,
        Longitude,
        Latitude
    }
}
