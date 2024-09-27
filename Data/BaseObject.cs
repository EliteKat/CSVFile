using CsvHelper.Configuration.Attributes;

namespace CSVFile.Data
{
	public abstract class BaseObject
	{
		public Guid Id { get; set; }
        
		[Name("Name")]
        public string? Name { get; set; }
	}
}
