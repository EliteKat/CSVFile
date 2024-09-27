using CSVFile.Data;
using CsvHelper.Configuration.Attributes;

namespace CSVFile.Models
{
	public class CSVData : BaseObject
	{
		public DateTime DateOfBirth { get; set; }
		public bool Married { get; set; }
		public string Phone {  get; set; }
		public decimal Salary { get; set; }
	}
}
