using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace DbIns.Models;

public class ReqAddress
{
    public string NameStreet { get; set; }
    public int NumberHome { get; set; }
    public int Entrance { get; set; }
    public int Apartment { get; set; }
    public string NameCity { get; set; }
    public string Region { get; set; }
}