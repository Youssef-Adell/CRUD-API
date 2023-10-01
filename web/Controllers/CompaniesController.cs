using System;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces.IServices;
using Core.DTOs;
using Web.ModelBinders;
using System.Collections.Generic;
using Web.ActionFilters;

namespace Web.Controllers;

[ApiController]
[Route("api/Companies")]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;

    public CompaniesController(IServiceManager service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllCompanies()
    {
        IEnumerable<CompanyDto> companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet("{id:Guid}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        CompanyDto company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);
        return Ok(company);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
    {
        CompanyDto companyToReturn = await _service.CompanyService.CreateCompanyAsync(company);

        return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);
    }


    [HttpGet("Collection/{ids}", Name = "GetCompanyCollectionByIds")]
    public async Task<IActionResult> GetCompanyCollection([ModelBinder(typeof(IEnumerableOfTModelBinder))] IEnumerable<Guid> ids)
    {
        IEnumerable<CompanyDto> companyCollectionToReturn = await _service.CompanyService.GetCompanyCollectionAsync(ids, trackChanges: false);

        return Ok(companyCollectionToReturn);
    }


    [HttpPost("Collection")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
    {
        //call service
        (IEnumerable<CompanyDto> companies, string ids) createdCollection = await _service.CompanyService.CreateCompanyCollectionAsync(companies);

        //return created Collection
        return CreatedAtRoute("GetCompanyCollectionByIds", new { ids = createdCollection.ids }, createdCollection.companies);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _service.CompanyService.DeleteCompanyAsync(id);

        return NoContent();
    }

    [HttpPut("{id:Guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, CompanyForUpdatenDto companyForUpdate)
    {
        await _service.CompanyService.UpdateCompanyAsync(id, companyForUpdate);

        return NoContent();
    }
}
