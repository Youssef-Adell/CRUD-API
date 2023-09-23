using System;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces.IServices;
using Core.DTOs;

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

    [HttpGet("{id:Guid}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _service.CompanyService.GetCompany(id, trackChanges: false);
        return Ok(company);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
    {
        if (company == null)
            return BadRequest("CompanyForCreationDto object is null");

        CompanyDto companyToReturn = _service.CompanyService.CreateCompany(company);

        return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);
    }


    [HttpGet("Collection/{ids}", Name = "GetCompanyCollectionByIds")]
    public IActionResult GetCompanyCollection(IEnumerable<Guid> ids)
    {
        IEnumerable<CompanyDto> companyCollectionToReturn = _service.CompanyService.GetCompanyCollection(ids, trackChanges: false);

        return Ok(companyCollectionToReturn);
    }


    [HttpPost("Collection")]
    public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
    {
        //check if null
        if (companies is null)
            return BadRequest("IEnumerable<CompanyForCreationDto> object is null");

        //call service
        var createdCollection = _service.CompanyService.CreateCompanyCollection(companies);

        //return created Collection
        return CreatedAtRoute("GetCompanyCollectionByIds", new { ids = createdCollection.ids }, createdCollection.companies);
    }
}
