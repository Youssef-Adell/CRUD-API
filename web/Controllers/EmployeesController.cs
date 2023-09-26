using Core.DTOs;
using Core.Interfaces.IServices;
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
}
