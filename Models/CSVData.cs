using CSVFile.Data;

namespace CSVFile.Models
{
	public class CSVData : BaseObject
	{
		public DateOnly DateOfBirth { get; set; }
		public bool Married { get; set; }
		public string Phone {  get; set; }
		public decimal Salary { get; set; }
	}
}
