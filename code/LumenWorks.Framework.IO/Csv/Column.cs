namespace LumenWorks.Framework.IO.Csv
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Metadata about a CSV column.
    /// </summary>
    public class Column
    {
        private Type _type;
        private string _typeName;

        /// <summary>
        /// Creates a new instance of the <see cref="Column" /> class.
        /// </summary>
        public Column() => 
            Type = typeof(string);

        /// <summary>
        /// Get or set the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the type.
        /// </summary>
        public Type Type
        {
            get => _type;
            set
            {
                _type = value;
                _typeName = value.Name;
            }
        }

        /// <summary>
        /// Converts the value into the column type.
        /// </summary>
        /// <param name="value">Value to use</param>
        /// <param name="provider">FormatProvider to use</param>
        /// <returns>Converted value.</returns>
        public object Convert(string value, IFormatProvider provider = null) =>
            Convert(value, NumberStyles.Number, provider);

        /// <summary>
        /// Converts the value into the column type.
        /// </summary>
        /// <param name="value">Value to use</param>
        /// <param name="styles">NumberStyles to use.</param>
        /// <param name="provider">FormatProvider to use.</param>
        /// <returns>Converted value.</returns>
        public object Convert(string value, NumberStyles styles, IFormatProvider provider)
        {
            TryConvert(value, styles, provider, out var x);

            return x;
        }

        /// <summary>
        /// Converts the value into the column type.
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <param name="styles">NumberStyles to use.</param>
        /// <param name="provider">FormatProvider to use.</param>
        /// <param name="result">Object to hold the converted value.</param>
        /// <returns>true if the conversion was successful, otherwise false.</returns>
        public bool TryConvert(string value, NumberStyles styles, IFormatProvider provider, out object result)
        {
            bool converted;

            switch (_typeName)
            {
                case "Guid":
                    try
                    {
                        result = new Guid(value);
                        converted = true;
                    }
                    catch
                    {
                        result = Guid.Empty;
                        converted = false;
                    }
                    break;

                case "Byte[]":
                    {
                        try
                        {
                            result = System.Convert.FromBase64String(value);
                            converted = true;
                        }
                        catch
                        {
                            result = new byte[0];
                            converted = false;
                        }
                    }
                    break;

                case "Int32":
                    {
                        converted = int.TryParse(value, styles, provider, out var x);
                        result = x;
                    }
                    break;

                case "Int64":
                    {
                        converted = long.TryParse(value, styles, provider, out var x);
                        result = x;
                    }
                    break;

                case "Single":
                    {
                        converted = float.TryParse(value, styles, provider, out var x);
                        result = x;
                    }
                    break;

                case "Double":
                    {
                        converted = double.TryParse(value, styles, provider, out var x);
                        result = x;
                    }
                    break;

                case "Decimal":
                    {
                        converted = decimal.TryParse(value, styles, provider, out var x);
                        result = x;
                    }
                    break;

                case "DateTime":
                    {
                        converted = DateTime.TryParse(value, provider, DateTimeStyles.None, out var x);
                        result = x;
                    }
                    break;

                default:
                    converted = false;
                    result = value;
                    break;
            }

            return converted;
        }
    }
}
