using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace DbIns.Models;

public class Address
{
    public Guid Id { get; set; }
    public string NameStreet { get; set; }
    public int NumberHome { get; set; }
    public int Entrance { get; set; }
    public int Apartment { get; set; }
    public Guid IdCity { get; set; }
    
    [ForeignKey(nameof(IdCity))]
    public City? City { get; set; }
}