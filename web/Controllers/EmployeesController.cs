using System.Text.Json;
using Core.DTOs;
using Core.Interfaces.IServices;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Web.ActionFilters;

namespace Web.Controllers;

[ApiController]
[Route("api/Companies/{companyId}/Employees")]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;

    public EmployeesController(IServiceManager service) =>
        _service = service;


    [HttpGet]
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
    {
        PagedList<EmployeeDto> pagedResult = await _service.EmployeeService.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);

        Response.Headers.Add("Pagination-Metadata", JsonSerializer.Serialize(pagedResult.Metadata));

        return Ok(pagedResult);
    }

    [HttpGet("{id:Guid}", Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
        EmployeeDto employee = await _service.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false);

        return Ok(employee);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
        //call service 
        EmployeeDto createdEmployee = await _service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee);

        //return created resource and its location
        return CreatedAtRoute("GetEmployeeForCompany", new { companyId = companyId, id = createdEmployee.Id }, createdEmployee);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteEmployeeFromCompany(Guid companyId, Guid id)
    {
        await _service.EmployeeService.DeleteEmployeeFromCompanyAsync(companyId, id);

        return NoContent();
    }

    [HttpPut("{id:Guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate)
    {
        await _service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employeeForUpdate);

        return NoContent();
    }

    // in patch request
    // content-type of request should be: application/json-patch+json
    // request body should contain array of operations [{"op":,"path":,"valuw":},]
    [HttpPatch("{id:Guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        var result = await _service.EmployeeService.GetEmployeeForPatchAsync(companyId, id);

        patchDoc.ApplyTo(result.employeeToPatch, ModelState);

        TryValidateModel(result.employeeToPatch);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _service.EmployeeService.SaveEmployeeForPatchAsync(result.employeeToPatch, result.employeeEntity);

        return NoContent();
    }
}
