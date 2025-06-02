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
public class InsController : ControllerBase
{
    private readonly DbInsContext _insContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public InsController(DbInsContext insContext, IHttpContextAccessor httpContextAccessor)
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

        return File(pdfBytes, "application/pdf", "Agreement.pdf");
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
            .Replace("{{AnimalType}}", model.Animal?.TypeAnimal ?? "Не указано")// Проверка на null
            .Replace("{{StartDate}}", model.StartDate.ToString("dd/MM/yyyy"))
            .Replace("{{FinishDate}}", model.FinishDate.ToString("dd/MM/yyyy"))
            .Replace("{{Price}}", model.Price.ToString("F2", CultureInfo.InvariantCulture))
            .Replace("{{InsCompanyName}}", model.InsCompanyName)
            .Replace("{{TypeInsName}}", model.TypeInsName);

        return htmlTemplate; 
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



    [Authorize]
    [HttpPost("addNewAgreements")]
    public async Task<IActionResult> AddNewAgreements([FromBody] ModelAgreements agr)
    {
        if (agr == null)
        {
            return BadRequest("Данные соглашения пусты.");
        }

        // Используйте асинхронные методы для работы с БД
        var typeIns = await _insContext.InsTypes.FirstOrDefaultAsync(n => n.Name == agr.TypeInsName);
        var company = await _insContext.InsCompanies.FirstOrDefaultAsync(n => n.Name == agr.InsCompanyName);

        if (typeIns == null)
        {
            return NotFound("Услуги: " + agr.TypeInsName + " нет"); // Изменено на NotFound
        }

        if (company == null)
        {
            return NotFound("Такой компании нет."); // Изменено на NotFound
        }

        var clientReturned = GetClientByToken();
        var client = await _insContext.Clients.FirstOrDefaultAsync(n => n.Id == clientReturned.Id);

        // Создаем новое соглашение
        var newAgreement = new Agreement
        {
            Id = Guid.NewGuid(),
            InsType = typeIns,
            IdInsType = typeIns.Id,
            InsCompany = company,
            IdInsCompany = company.Id,
            Price = agr.Price,
            StartDate = agr.StartDate,
            FinishDate = agr.FinishDate,
            Client = client,
            IdClient = client.Id
        };

        newAgreement.InsPerson = await AddNewPerson(agr.Person);
        newAgreement.IdInsPerson = newAgreement.InsPerson.Id; // Устанавливаем IdInsPerson

        switch (typeIns.Name)
        {
            case "Страхование квартиры":
                var addressAp = await AddNewAddress(agr.Apartment.Address);
                var apartment = new Apartment
                {
                    Id = Guid.NewGuid(),
                    IdAddress = addressAp.Id,
                    Address = addressAp
                };
                newAgreement.Apartment = apartment;
                newAgreement.IdApartment = apartment.Id;
                await _insContext.Apartments.AddAsync(apartment);
                break;

            case "Защита питомцев":
                var animal = new Animal
                {
                    Id = Guid.NewGuid(),
                    Passport = agr.Animal.Passport,
                    Name = agr.Animal.Name,
                    TypeAnimal = agr.Animal.TypeAnimal
                };
                newAgreement.Animal = animal;
                newAgreement.IdAnimal = animal.Id;
                await _insContext.Animals.AddAsync(animal);
                break;
        }

        await _insContext.Agreements.AddAsync(newAgreement);
        await _insContext.SaveChangesAsync();

        return CreatedAtAction(nameof(AddNewAgreements), new { id = newAgreement.Id },
            newAgreement); // Возвращаем созданное соглашение
    }

    [Authorize]
    [HttpPost("addNewAgreementsWithoutDateFinish")]
    public async Task<IActionResult> AddNewAgreementsWithoutDateFinish([FromBody] ModelAgreementsWithoutDateFinish agr)
    {
        if (agr == null)
        {
            return BadRequest("Данные соглашения пусты.");
        }

        // Используйте асинхронные методы для работы с БД
        var typeIns = await _insContext.InsTypes.FirstOrDefaultAsync(n => n.Name == agr.TypeInsName);
        var company = await _insContext.InsCompanies.FirstOrDefaultAsync(n => n.Name == agr.InsCompanyName);

        if (typeIns == null)
        {
            return NotFound("Такой услуги нет.");
        }

        if (company == null)
        {
            return NotFound("Такой компании нет.");
        }

        var clientReturned = GetClientByToken();
        var client = await _insContext.Clients.FirstOrDefaultAsync(n => n.Id == clientReturned.Id);

        var newAgreement = new Agreement
        {
            Id = Guid.NewGuid(),
            InsType = typeIns,
            IdInsType = typeIns.Id,
            InsCompany = company,
            IdInsCompany = company.Id,
            Price = agr.Price,
            StartDate = agr.StartDate,
            FinishDate = agr.StartDate.AddYears(1),
            Client = client,
            IdClient = client.Id
        };

        newAgreement.InsPerson = await AddNewPerson(agr.Person);
        newAgreement.IdInsPerson = newAgreement.InsPerson.Id;

        switch (typeIns.Name)
        {
            case "Страхование квартиры":
                var addressAp = await AddNewAddress(agr.Apartment.Address);
                var apartment = new Apartment
                {
                    Id = Guid.NewGuid(),
                    IdAddress = addressAp.Id,
                    Address = addressAp
                };
                newAgreement.Apartment = apartment;
                newAgreement.IdApartment = apartment.Id;
                await _insContext.Apartments.AddAsync(apartment);
                break;

            case "Защита питомцев":
                var animal = new Animal
                {
                    Id = Guid.NewGuid(),
                    Passport = agr.Animal.Passport,
                    Name = agr.Animal.Name,
                    TypeAnimal = agr.Animal.TypeAnimal
                };
                newAgreement.Animal = animal;
                newAgreement.IdAnimal = animal.Id;
                await _insContext.Animals.AddAsync(animal);
                break;
        }

        await _insContext.Agreements.AddAsync(newAgreement);
        await _insContext.SaveChangesAsync();

        return CreatedAtAction(nameof(AddNewAgreements), new { id = newAgreement.Id },
            newAgreement); 
    }

    [HttpGet("getClientByToken")]
    public Client GetClientByToken()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
        var client = _insContext.Clients
            .Include(c => c.Address) 
            .FirstOrDefault(c => c.UserId == userId);

        return client;
    }

    [HttpGet("getagreements")]
    public async Task<List<ModelReqAgr>> GetAllAgreementsForCurrentClient()
    {
        var idClient = GetClientByToken().Id;

      
        List<Agreement> listAgr = await _insContext.Agreements
            .Where(n => n.IdClient == idClient)
            .Include(a => a.InsPerson)
            .Include(a => a.InsType)
            .Include(a => a.InsCompany)
            .ToListAsync(); 

        var listModelReqAgr = new List<ModelReqAgr>();

        foreach (var item in listAgr)
        {
            var modelReqAgr = new ModelReqAgr
            {
                FinishDate = item.FinishDate,
                StartDate = item.StartDate,
                Price = item.Price,
                FIO = item.InsPerson?.FIO, 
                Company = item.InsCompany?.Name, 
                TypeIns = item.InsType?.Name 
            };

            listModelReqAgr.Add(modelReqAgr); 
        }

        return listModelReqAgr; 
    }
    
    [HttpPost("addNewAddress")]
    private async Task<Address> AddNewAddress(ReqAddress address)
    {
        var findCity = await _insContext.Cities
            .FirstOrDefaultAsync(n => n.Name == address.NameCity && n.Region == address.Region);

        if (findCity == null)
        {
            var newCity = new City
            {
                Id = Guid.NewGuid(),
                Name = address.NameCity,
                Region = address.Region
            };
            await _insContext.Cities.AddAsync(newCity);
            await _insContext.SaveChangesAsync();

            findCity = newCity; 
        }

        var newAddress = new Address
        {
            Id = Guid.NewGuid(),
            NameStreet = address.NameStreet,
            NumberHome = address.NumberHome,
            Apartment = address.Apartment,
            Entrance = address.Entrance,
            IdCity = findCity.Id,
            City = findCity
        };

        await _insContext.Addresses.AddAsync(newAddress);
        await _insContext.SaveChangesAsync();

        return newAddress; 
    }

    [HttpPost("AddNewPerson")]
    public async Task<InsPerson> AddNewPerson(ModelInsPerson person)
    {
        if (person == null)
        {
            throw new ArgumentException("Данные человека пусты.");
        }

        var newPersonIns = new InsPerson
        {
            Id = Guid.NewGuid(),
            Passport = person.Passport,
            FIO = person.FIO,
            Birthday = person.Birthday
        };

        var address = await AddNewAddress(person.Address);
        newPersonIns.Address = address;
        newPersonIns.IdAddress = address.Id;

        if (!string.IsNullOrEmpty(person.Transport?.CityNumber)) // Проверка на null
        {
            var transport = new Transport
            {
                Id = Guid.NewGuid(),
                CityNumber = person.Transport.CityNumber,
                Mark = person.Transport.Mark,
                VIN = person.Transport.VIN
            };

            newPersonIns.Transport = transport;
            newPersonIns.IdTransport = transport.Id;

            var licenseAuto = new LicenseAuto
            {
                Id = Guid.NewGuid(),
                Number = person.License?.Number // Проверка на null
            };

            newPersonIns.IdLicenseAuto = licenseAuto.Id;
            newPersonIns.LicenseAuto = licenseAuto;

            await _insContext.Transports.AddAsync(transport);
            await _insContext.SaveChangesAsync();

            await _insContext.LicenseAutos.AddAsync(licenseAuto);
            await _insContext.SaveChangesAsync();
        }

        await _insContext.InsPersons.AddAsync(newPersonIns);
        await _insContext.SaveChangesAsync();

        return newPersonIns; // Вернуть созданного человека
    }
}