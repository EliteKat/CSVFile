using CSVFile.Models;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace CSVFile.ClassMaps
{
    public class CSVDataMap : ClassMap<CSVData>
    {
        public CSVDataMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.DateOfBirth)
                .Name("Date of birth")
                .TypeConverter<DateTimeConverter>()
                .TypeConverterOption.Format("yyyy-MM-dd");
            // using BooleanConverter from CsvHelper library in order to convert "yes" and "no" to bool
            Map(m => m.Married)
                .Name("Married")
                .TypeConverterOption.BooleanValues(true, true, "Yes", "yes")
                .TypeConverterOption.BooleanValues(false, false, "No", "no");
            Map(m => m.Phone).Name("Phone");
            Map(m => m.Salary).Name("Salary");
        }
    }
}
