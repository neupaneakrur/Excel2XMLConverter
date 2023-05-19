using Converter.Models;
using Converter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace Converter.Controllers;

public class HomeController : Controller
{
    private readonly ExcelToXmlConverter _excelToXmlConverter;

    public HomeController(ExcelToXmlConverter excelToXmlConverter)
    {
        _excelToXmlConverter = excelToXmlConverter;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Upload(IFormFile inputFile)
    {
        if (inputFile == null || inputFile.Length == 0)
        {
            ModelState.AddModelError("inputFile", "Please select an Excel file.");
            return View("Index");
        }

        var informations = new List<InformationModel>();

        using (var stream = inputFile.OpenReadStream())
        {
            var workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);

            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                var currencyCell = sheet.GetRow(row).GetCell(3);
                string currencyValue = currencyCell?.ToString() ?? string.Empty;
                Console.WriteLine($"Currency value: {currencyValue}");
                var information = new InformationModel
                {
                    Date = DateTime.Parse(sheet.GetRow(row).GetCell(0)?.ToString() ?? string.Empty),
                    Type = sheet.GetRow(row).GetCell(1)?.ToString(),
                    Amount = decimal.Parse(sheet.GetRow(row).GetCell(2)?.ToString() ?? "0"),
                    Currency = sheet.GetRow(row).GetCell(3)?.ToString() ?? string.Empty,
                    EventType = sheet.GetRow(row).GetCell(4)?.ToString() ?? string.Empty,
                    CreatedAt = DateTime.Parse(sheet.GetRow(row).GetCell(5)?.ToString() ?? string.Empty)
                };

                informations.Add(information);
            }
            workbook.Close();
        }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var xmlFilePath = Path.Combine(desktopPath, $"{Path.GetFileNameWithoutExtension(inputFile.FileName)}.xml");
        _excelToXmlConverter.ConvertToXml(informations, xmlFilePath);
        var fileBytes = System.IO.File.ReadAllBytes(xmlFilePath);
        return File(fileBytes, "text/xml", "output.xml");
        //return File(System.IO.File.ReadAllBytes(xmlFilePath), "text/xml", "output.xml");
    }
}
