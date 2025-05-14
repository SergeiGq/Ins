using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DbIns.Models;

public class ReqCity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Region { get; set; }
    
}