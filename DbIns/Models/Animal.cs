using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DbIns.Models;

public class Animal
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string TypeAnimal { get; set; }
    public string Passport { get; set; }
    
}