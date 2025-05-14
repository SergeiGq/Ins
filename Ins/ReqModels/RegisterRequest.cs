using DbIns.Models;

namespace Ins.ReqModels;

public class RegisterRequest
{
    public RegisterModel Register { get; set; }
    public ModelForRegister Client { get; set; }
    public ReqAddress Address { get; set; }
}