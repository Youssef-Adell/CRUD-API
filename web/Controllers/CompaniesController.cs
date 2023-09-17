using Microsoft.AspNetCore.Mvc;
using Core.Interfaces.IServices;

namespace Web.Controllers;

[ApiController]
[Route("api/Companies")]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;

    public CompaniesController(IServiceManager service) => _service = service;

    [HttpGet]
    public IActionResult GetAllCompanies()
    {
        var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);
        return Ok(companies);
    }
}
