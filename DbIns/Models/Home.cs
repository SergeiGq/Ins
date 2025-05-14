using System.ComponentModel.DataAnnotations.Schema;

namespace DbIns.Models;

public class Home
{
    public Guid Id { get; set; }
    public Guid IdAddress { get; set; }
    
    [ForeignKey(nameof(IdAddress))]
    public Address Address { get; set; }
}