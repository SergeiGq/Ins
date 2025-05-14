using DbIns;
using DbIns.Models;
using Ins.ReqModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ins.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly DbInsContext _insContext;

    public AdminController(DbInsContext insContext)
    {
        _insContext = insContext;
    }
    
    [HttpGet("getAllArgForClient")]
    public List<ModelForReqAllAgr> GetAllArgForCurrentClient()
    {
        var resListForAgr = new List<ModelForReqAllAgr>();
        foreach (var item in _insContext.Agreements.Include(agreement => agreement.InsType)
                     .Include(agreement => agreement.InsPerson))
        {
            var modelReq = new ModelForReqAllAgr()
            {
                Price = item.Price,
                FinishDate = item.FinishDate,
                StartDate = item.StartDate,
                TypeIns = item.InsType.Name,
                FIOClient = item.InsPerson.FIO
            };
            resListForAgr.Add(modelReq);
        }
        return resListForAgr;
    }
    
    
}