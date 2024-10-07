using CSVFile.ClassMaps;
using CSVFile.Data;
using CSVFile.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using System.Text;

namespace CSVFile.Services
{
    public interface ICSVDataService
    {
        public Task<bool> UploadAsync(IFormFile file);
        public Task<byte[]> DownloadAsync();
        public Task<bool> EditAsync(Guid id, CSVData updatedData);
        public Task<bool> DeleteAsync(Guid id);
    }

    public class CSVDataService : ICSVDataService
    {
        private readonly AppDbContext _context;

        public CSVDataService(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<bool> UploadAsync(IFormFile file)
        {
            try
            {
                using (StreamReader stream = new StreamReader(file.OpenReadStream()))
                using (CsvReader csv = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";", Encoding = Encoding.UTF8 }))
                {
                    List<CSVData> records = new List<CSVData>();
                    csv.Context.RegisterClassMap<CSVDataMap>();
                    records = csv.GetRecords<CSVData>().ToList();

                    _context.CSVDatas.AddRange(records);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (CsvHelperException ex)
            {
                Console.WriteLine($"Error reading CSV: {ex.Message}");
                Console.WriteLine($"InnerException: {ex.InnerException}");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // return false if there is an error somewhere
            return false;
        }

        public async Task<byte[]> DownloadAsync()
        {
            try
            {
                List<CSVData> records = await _context.CSVDatas.ToListAsync();
                if (records == null || !records.Any())
                    throw new InvalidOperationException("No data available to download.");

                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(records);
                    streamWriter.Flush();
                    return memoryStream.ToArray();
                }
            }
            catch (CsvHelperException ex)
            {
                throw new ApplicationException("An error occurred while processing the CSV file.", ex);
            }
            catch (IOException ex)
            {
                throw new ApplicationException("An error occurred while writing the CSV file.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
        }

        public async Task<bool> EditAsync(Guid id, CSVData updatedData)
        {
            var csvData = await _context.CSVDatas.FindAsync(id);
            if (csvData == null)
                return false;

            csvData.Name = updatedData.Name;
            csvData.DateOfBirth = updatedData.DateOfBirth;
            csvData.Married = updatedData.Married;
            csvData.Phone = updatedData.Phone;
            csvData.Salary = updatedData.Salary;

            try
            {
                _context.CSVDatas.Update(csvData);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Unable to save changes: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var csvData = await _context.CSVDatas.FindAsync(id);
            if (csvData == null)
                return false;

            _context.CSVDatas.Remove(csvData);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Unable to delete: {ex.Message}");
            }

            return false;
        }
    }
}
