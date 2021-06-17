using ClosedXML.Excel;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using WebExtraction.Model;
using System.Linq;

namespace WebExtraction
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Task1: Json extraction");
            using (StreamReader r = new StreamReader("Content/task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html"))
            {
                string htmlString = r.ReadToEnd();
                Console.WriteLine("Json extraction started:(extracting...)");
                var jsonString = await WebExtractorUtility.ExtractJson(htmlString);
                string filename = $@"C:\Users\noomi\source\repos\WebExtraction\WebExtraction\Exported\{Guid.NewGuid()}.json";
                File.WriteAllText(filename, jsonString);
                Console.WriteLine($"File exported successfully to: {filename}");
            }
            Console.WriteLine("------------------------------");
            Console.WriteLine("Task2: Export Excel from json");
            using (StreamReader r = new StreamReader("Content/hotelRates.json"))
            {
                string json = r.ReadToEnd();
                var response = JsonSerializer.Deserialize<JsonHotelObject>(json);

                using var workbook = new XLWorkbook();
                IXLWorksheet worksheet = workbook.Worksheets.Add($"HotelRates");
                worksheet.Cell(1, 1).Value = "ARRIVAL_DATE";
                worksheet.Cell(1, 2).Value = "DEPARTURE_DATE";
                worksheet.Cell(1, 3).Value = "PRICE";
                worksheet.Cell(1, 4).Value = "CURRENCY";
                worksheet.Cell(1, 5).Value = "RATENAME";
                worksheet.Cell(1, 6).Value = "ADULTS";
                worksheet.Cell(1, 7).Value = "BREAKFAST_INCLUDED";
                int rowNumber = 2;
                foreach (var rate in response.HotelRates)
                {
                    worksheet.Cell(rowNumber, 1).Value = rate.TargetDay;
                    worksheet.Cell(rowNumber, 2).Value = rate.TargetDay.AddDays(rate.Los);
                    worksheet.Cell(rowNumber, 3).Value = rate.Price.NumericFloat;
                    worksheet.Cell(rowNumber, 4).Value = rate.Price.Currency;
                    worksheet.Cell(rowNumber, 5).Value = rate.RateName;
                    worksheet.Cell(rowNumber, 6).Value = rate.Adults;
                    // check breakfast only
                    int? isBreakFastIncluded = null;
                    var breakfastCheck = rate.RateTags.FirstOrDefault(t => t.Name == "breakfast");
                    if (breakfastCheck != null)
                    {
                        if (breakfastCheck.Shape) isBreakFastIncluded = 1;
                        else isBreakFastIncluded = 0;
                    }
                    worksheet.Cell(rowNumber, 7).Value = isBreakFastIncluded;
                    rowNumber++;
                }

                worksheet.Columns().AdjustToContents();
                await using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                string filename = $@"C:\Users\noomi\source\repos\WebExtraction\WebExtraction\Exported\{Guid.NewGuid()}.xlsx";
                if (ExcelExportUtility.ByteArrayToFile(filename, stream.ToArray()))
                {
                    Console.WriteLine($"File exported successfully to: {filename}");
                }
            }
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Press any key to exit");
            Console.ReadKey();
        }
    }
}
