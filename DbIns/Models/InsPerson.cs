using System.ComponentModel.DataAnnotations.Schema;

namespace DbIns.Models;

public class InsPerson
{
    public Guid Id { get; set; }
    public string FIO { get; set; }
    public Guid IdAddress { get; set; }
    public string Passport { get; set; }
    public Guid? IdTransport { get; set; }
    public Guid? IdLicenseAuto { get; set; }
    public DateTime Birthday { get; set; }
    
    [ForeignKey(nameof(IdTransport))]
    public Transport Transport { get; set; }
    [ForeignKey(nameof(IdLicenseAuto))]
    public LicenseAuto LicenseAuto { get; set; }
    [ForeignKey(nameof(IdAddress))]
    public Address Address { get; set; }
}