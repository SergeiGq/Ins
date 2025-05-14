using System.ComponentModel.DataAnnotations.Schema;
using DbIns.Models;

namespace Ins.ReqModels;

public class ModelClient
{
    public string FIO { get; set; }

    public string Passport { get; set; }
    public string Phone { get; set; }
    public DateTime Birthday { get; set; }

    public ModelTransport Transport { get; set; }
    public ModelLicense License { get; set; }

}