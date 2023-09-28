using Core.DTOs;
using Core.Interfaces.IServices;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/Companies/{companyId}/Employees")]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;

    public EmployeesController(IServiceManager service) =>
        _service = service;


    [HttpGet]
    public IActionResult GetEmployeesForCompany(Guid companyId)
    {
        var employees = _service.EmployeeService.GetEmployees(companyId, trackChanges: false);

        return Ok(employees);
    }

    [HttpGet("{id:Guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, id, trackChanges: false);

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
        //check if the data sent by client is valid or not
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        //call service 
        EmployeeDto createdEmployee = _service.EmployeeService.CreateEmployeeForCompany(companyId, employee);

        //return created resource and its location
        return CreatedAtRoute("GetEmployeeForCompany", new { companyId = companyId, id = createdEmployee.Id }, createdEmployee);
    }

    [HttpDelete("{id:Guid}")]
    public IActionResult DeleteEmployeeFromCompany(Guid companyId, Guid id)
    {
        _service.EmployeeService.DeleteEmployeeFromCompany(companyId, id);

        return NoContent();
    }

    [HttpPut("{id:Guid}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        _service.EmployeeService.UpdateEmployeeForCompany(companyId, id, employeeForUpdate);

        return NoContent();
    }


    // content-type of request should be: application/json-patch+json
    // request body should contain array of operations [{"op":,"path":,"valuw":},]
    [HttpPatch("{id:Guid}")]
    public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        var result = _service.EmployeeService.GetEmployeeForPatch(companyId, id);

        patchDoc.ApplyTo(result.employeeToPatch, ModelState);

        TryValidateModel(result.employeeToPatch);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        _service.EmployeeService.SaveEmployeeForPatch(result.employeeToPatch, result.employeeEntity);

        return NoContent();
    }
}
