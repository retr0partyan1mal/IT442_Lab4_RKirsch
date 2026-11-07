using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace MMABooksEFClasses.MarisModels
{
    public partial class Product
    {
        public Product()
        {
            InvoiceLineItem = new HashSet<InvoiceLineItem>();
        }

        public string ProductCode { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int OnHandQuantity { get; set; }

        public override string ToString()
        {
            return ProductCode + ", " + Description + ", " + UnitPrice + ", " + OnHandQuantity;
        }

        public virtual ICollection<InvoiceLineItem> InvoiceLineItem { get; set; }

        [Serializable()]
        public class DecimalFormatConverter : JsonConverter<decimal>
        {
            public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.GetDecimal();
            }

            public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                writer.WriteRawValue(value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
            }
        }
    }
}
