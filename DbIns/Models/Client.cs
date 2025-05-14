using System.ComponentModel.DataAnnotations.Schema;

namespace DbIns.Models;

public class Client
{
    public Guid Id { get; set; }
    public string FIO { get; set; }
    public Guid IdAddress { get; set; }
    public string Passport { get; set; }
    public string Phone { get; set; }
    [ForeignKey(nameof(IdAddress))]
    public Address Address { get; set; }
    
    public DateTime Birthday { get; set; }
    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}