using System.Globalization;
using System.Security.Claims;
using DbIns;
using DbIns.Models;
using Ins.ReqModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using PuppeteerSharp.Media;

[ApiController]
[Route("[controller]")]
public class PdfController : ControllerBase
{
    private readonly DbInsContext _insContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PdfController(DbInsContext insContext, IHttpContextAccessor httpContextAccessor)
    {
        _insContext = insContext;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("generatePdfForAnimal")]
    public async Task<IActionResult> GeneratePdfForAnimal([FromBody] ModelAgreements agr)
    {
        if (agr == null)
        {
            return BadRequest("Данные пусты.");
        }

        // Рендерим HTML для PDF с помощью шаблона
        var htmlContent = await RenderViewToStringAsyncForAnimal("FormPdfAnimal", agr);
        var pdfBytes = await GeneratePdfFromHtml(htmlContent);

        return File(pdfBytes, "application/pdf", "AgrAnimal.pdf");
    }
    [HttpPost("generatePdfForPerson")]
    public async Task<IActionResult> GeneratePdfForPerson([FromBody] ModelAgreements agr)
    {
        if (agr == null)
        {
            return BadRequest("Данные пусты.");
        }

        // Рендерим HTML для PDF с помощью шаблона
        var htmlContent = await RenderViewToStringAsyncForPerson("FormPdfPerson", agr);
        var pdfBytes = await GeneratePdfFromHtml(htmlContent);

        return File(pdfBytes, "application/pdf", "AgrPerson.pdf");
    }
    [HttpPost("generatePdfForApartment")]
    public async Task<IActionResult> GeneratePdfForApartment([FromBody] ModelAgreements agr)
    {
        if (agr == null)
        {
            return BadRequest("Данные пусты.");
        }

        // Рендерим HTML для PDF с помощью шаблона
        var htmlContent = await RenderViewToStringAsyncForApartment("FormPdfApartment", agr);
        var pdfBytes = await GeneratePdfFromHtml(htmlContent);

        return File(pdfBytes, "application/pdf", "AgrApartment.pdf");
    }
    
    [HttpPost("generatePdfForTravelling")]
    public async Task<IActionResult> GeneratePdfForTravelling([FromBody] ModelAgreements agr)
    {
        if (agr == null)
        {
            return BadRequest("Данные пусты.");
        }

        // Рендерим HTML для PDF с помощью шаблона
        var htmlContent = await RenderViewToStringAsyncForTravelling("FormPdfTravelling", agr);
        var pdfBytes = await GeneratePdfFromHtml(htmlContent);

        return File(pdfBytes, "application/pdf", "AgrApartment.pdf");
    }
    [HttpPost("generatePdfForOSAGO")]
    public async Task<IActionResult> GeneratePdfForOSAGO([FromBody] ModelAgreements agr)
    {
        if (agr == null)
        {
            return BadRequest("Данные пусты.");
        }

        // Рендерим HTML для PDF с помощью шаблона
        var htmlContent = await RenderViewToStringAsyncForOSAGO("FormPdfOSAGO", agr);
        var pdfBytes = await GeneratePdfFromHtml(htmlContent);

        return File(pdfBytes, "application/pdf", "AgrApartment.pdf");
    }
    private async Task<string> RenderViewToStringAsyncForOSAGO(string viewName, ModelAgreements model)
    {
        // Путь к файлу шаблона
        string templatePath = Path.Combine("Templates", $"{viewName}.html");

        // Загрузка содержимого шаблона
        string htmlTemplate = await System.IO.File.ReadAllTextAsync(templatePath);

        // Замена переменных в шаблоне
        htmlTemplate = htmlTemplate
            .Replace("{{PersonFIO}}", model.Person.FIO)
            .Replace("{{PersonPassport}}", model.Person.Passport ?? "Не указано") // Проверка на null
            .Replace("{{Birthday}}", model.Person.Birthday.ToString("dd/MM/yyyy"))
            .Replace("{{License}}", model.Person.License.Number)
            .Replace("{{MarkAuto}}", model.Person.Transport.Mark)
            .Replace("{{NumberAuto}}", model.Person.Transport.CityNumber)
            .Replace("{{VIN}}", model.Person.Transport.VIN)
            .Replace("{{AddressStreetName}}", model.Apartment.Address.NameStreet?? "Не указано")
            .Replace("{{NumberHome}}", model.Apartment.Address.NumberHome.ToString() ?? "Не указано") // Проверка на null
            .Replace("{{Entrance}}", model.Apartment.Address.Entrance.ToString() ?? "Не указано") // Проверка на null
            .Replace("{{Apartment}}", model.Apartment.Address.Apartment.ToString() ?? "Не указано") // Проверка на null
            .Replace("{{NameCity}}", model.Apartment.Address.NameCity)
            .Replace("{{Region}}", model.Apartment.Address.Region)
            .Replace("{{StartDate}}", model.StartDate.ToString("dd/MM/yyyy"))
            .Replace("{{FinishDate}}", model.FinishDate.ToString("dd/MM/yyyy"))
            .Replace("{{Price}}", model.Price.ToString("F2", CultureInfo.InvariantCulture))
            .Replace("{{InsCompanyName}}", model.InsCompanyName)
            .Replace("{{TypeInsName}}", model.TypeInsName);

        return htmlTemplate; // Возвращаем готовый HTML контент
    }
    private async Task<string> RenderViewToStringAsyncForTravelling(string viewName, ModelAgreements model)
    {
        // Путь к файлу шаблона
        string templatePath = Path.Combine("Templates", $"{viewName}.html");

        // Загрузка содержимого шаблона
        string htmlTemplate = await System.IO.File.ReadAllTextAsync(templatePath);

        // Замена переменных в шаблоне
        htmlTemplate = htmlTemplate
            .Replace("{{PersonFIO}}", model.Person.FIO)
            .Replace("{{PersonPassport}}", model.Person.Passport ?? "Не указано") // Проверка на null
            .Replace("{{Birthday}}", model.Person.Birthday.ToString("dd/MM/yyyy"))
            .Replace("{{AddressStreetName}}", model.Apartment.Address.NameStreet?? "Не указано")
            .Replace("{{NumberHome}}", model.Apartment.Address.NumberHome.ToString() ?? "Не указано") // Проверка на null
            .Replace("{{Entrance}}", model.Apartment.Address.Entrance.ToString() ?? "Не указано") // Проверка на null
            .Replace("{{Apartment}}", model.Apartment.Address.Apartment.ToString() ?? "Не указано") // Проверка на null
            .Replace("{{NameCity}}", model.Apartment.Address.NameCity)
            .Replace("{{Region}}", model.Apartment.Address.Region)
            .Replace("{{StartDate}}", model.StartDate.ToString("dd/MM/yyyy"))
            .Replace("{{FinishDate}}", model.FinishDate.ToString("dd/MM/yyyy"))
            .Replace("{{Price}}", model.Price.ToString("F2", CultureInfo.InvariantCulture))
            .Replace("{{InsCompanyName}}", model.InsCompanyName)
            .Replace("{{TypeInsName}}", model.TypeInsName);

        return htmlTemplate; // Возвращаем готовый HTML контент
    }
    private async Task<string> RenderViewToStringAsyncForApartment(string viewName, ModelAgreements model)
    {
        // Путь к файлу шаблона
        string templatePath = Path.Combine("Templates", $"{viewName}.html");

        // Загрузка содержимого шаблона
        string htmlTemplate = await System.IO.File.ReadAllTextAsync(templatePath);

        // Замена переменных в шаблоне
        htmlTemplate = htmlTemplate
            .Replace("{{PersonFIO}}", model.Person.FIO)
            .Replace("{{PersonPassport}}", model.Person.Passport ?? "Не указано") // Проверка на null
            .Replace("{{Birthday}}", model.Person.Birthday.ToString("dd/MM/yyyy"))
            .Replace("{{AddressStreetName}}", model.Apartment.Address.NameStreet?? "Не указано")
            .Replace("{{NumberHome}}", model.Apartment.Address.NumberHome.ToString() ?? "Не указано") // Проверка на null
            .Replace("{{Entrance}}", model.Apartment.Address.Entrance.ToString() ?? "Не указано") // Проверка на null
            .Replace("{{Apartment}}", model.Apartment.Address.Apartment.ToString() ?? "Не указано") // Проверка на null
            .Replace("{{NameCity}}", model.Apartment.Address.NameCity)
            .Replace("{{Region}}", model.Apartment.Address.Region)
            .Replace("{{StartDate}}", model.StartDate.ToString("dd/MM/yyyy"))
            .Replace("{{FinishDate}}", model.FinishDate.ToString("dd/MM/yyyy"))
            .Replace("{{Price}}", model.Price.ToString("F2", CultureInfo.InvariantCulture))
            .Replace("{{InsCompanyName}}", model.InsCompanyName)
            .Replace("{{TypeInsName}}", model.TypeInsName);

        return htmlTemplate; // Возвращаем готовый HTML контент
    }
    private async Task<string> RenderViewToStringAsyncForPerson(string viewName, ModelAgreements model)
    {
        // Путь к файлу шаблона
        string templatePath = Path.Combine("Templates", $"{viewName}.html");

        // Загрузка содержимого шаблона
        string htmlTemplate = await System.IO.File.ReadAllTextAsync(templatePath);

        // Замена переменных в шаблоне
        htmlTemplate = htmlTemplate
            .Replace("{{PersonFIO}}", model.Person.FIO)
            .Replace("{{PersonPassport}}", model.Person.Passport ?? "Не указано") // Проверка на null
            .Replace("{{Birthday}}", model.Person.Birthday.ToString("dd/MM/yyyy"))
            .Replace("{{AnimalPassword}}", model.Animal?.Passport ?? "Не указано")
            .Replace("{{AnimalType}}", model.Animal?.TypeAnimal ?? "Не указано") // Проверка на null
            .Replace("{{StartDate}}", model.StartDate.ToString("dd/MM/yyyy"))
            .Replace("{{FinishDate}}", model.FinishDate.ToString("dd/MM/yyyy"))
            .Replace("{{Price}}", model.Price.ToString("F2", CultureInfo.InvariantCulture))
            .Replace("{{InsCompanyName}}", model.InsCompanyName)
            .Replace("{{TypeInsName}}", model.TypeInsName);

        return htmlTemplate; // Возвращаем готовый HTML контент
    }
    private async Task<string> RenderViewToStringAsyncForAnimal(string viewName, ModelAgreements model)
    {
        // Путь к файлу шаблона
        string templatePath = Path.Combine("Templates", $"{viewName}.html");

        // Загрузка содержимого шаблона
        string htmlTemplate = await System.IO.File.ReadAllTextAsync(templatePath);

        // Замена переменных в шаблоне
        htmlTemplate = htmlTemplate
            .Replace("{{PersonFIO}}", model.Person.FIO)
            .Replace("{{PersonPassport}}", model.Person.Passport ?? "Не указано") // Проверка на null
            .Replace("{{AnimalName}}", model.Animal?.Name ?? "Не указано") // Проверка на null
            .Replace("{{AnimalPassword}}", model.Animal?.Passport ?? "Не указано")
            .Replace("{{AnimalType}}", model.Animal?.TypeAnimal ?? "Не указано") // Проверка на null
            .Replace("{{StartDate}}", model.StartDate.ToString("dd/MM/yyyy"))
            .Replace("{{FinishDate}}", model.FinishDate.ToString("dd/MM/yyyy"))
            .Replace("{{Price}}", model.Price.ToString("F2", CultureInfo.InvariantCulture))
            .Replace("{{InsCompanyName}}", model.InsCompanyName)
            .Replace("{{TypeInsName}}", model.TypeInsName);

        return htmlTemplate; // Возвращаем готовый HTML контент
    }

    private async Task<byte[]> GeneratePdfFromHtml(string htmlContent)
    {
        // Ensure PuppeteerSharp downloads its own Chromium browser
        await new BrowserFetcher().DownloadAsync();

        // Initialize Puppeteer
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true, // Run in headless mode
            // No need to set ExecutablePath, Puppeteer will use the downloaded Chromium
        });

        using (var page = await browser.NewPageAsync())
        {
            // Load the HTML content
            await page.SetContentAsync(htmlContent);

            // Generate PDF
            var pdfBytes = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
            });

            // Close the browser
            await browser.CloseAsync();

            return pdfBytes; // Return PDF bytes
        }
    }
}
