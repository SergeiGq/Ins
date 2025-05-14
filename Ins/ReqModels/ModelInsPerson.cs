using DbIns.Models;

namespace Ins.ReqModels;

public class ModelInsPerson
{
    public string FIO { get; set; }
    public string Passport { get; set; }
    public DateTime Birthday { get; set; }
    public ReqAddress Address { get; set; }
    public ModelTransport Transport { get; set; }
    public ModelLicense License { get; set; }
}