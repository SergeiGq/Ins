using System.ComponentModel.DataAnnotations.Schema;

namespace DbIns.Models;

public class Agreement
{
    public Guid Id { get; set; }
    public Guid IdClient { get; set; }
    public Guid? IdInsPerson { get; set; }
    public Guid? IdAnimal { get; set; }
    public Guid? IdApartment { get; set; }
    public Guid IdInsCompany { get; set; }
    public Guid IdInsType { get; set; }
    public double Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    
    [ForeignKey(nameof(IdApartment))]
    public Apartment Apartment { get; set; }
    [ForeignKey(nameof(IdClient))]
    public Client? Client { get; set; }
    [ForeignKey(nameof(IdInsPerson))]
    public InsPerson InsPerson { get; set; }
    [ForeignKey(nameof(IdAnimal))]
    public Animal Animal { get; set; }
    [ForeignKey(nameof(IdInsCompany))]
    public InsCompany InsCompany { get; set; }
    [ForeignKey(nameof(IdInsType))]
    public InsType InsType { get; set; }


}