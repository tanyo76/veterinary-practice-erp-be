using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersRestApi.Entities;
using UsersRestApi.Repositories;

public class DeleteEmployeesDto
{
    public int[] userIds { get; set; }
    public int clinicId { get; set; }
}

namespace UsersRestApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeToClinicController : ControllerBase
    {

        [HttpGet("{clinicId}")]
        public IActionResult Get(int clinicId)
        {

            EmployeeToClinicRepository empRepo = new EmployeeToClinicRepository();

            List<EmployeeToClinic> empsToClinic = empRepo.GetAll().FindAll(emp => emp.ClinicId == clinicId);

            return Ok(new { employees = empsToClinic });
        }

        [HttpPost]

        public IActionResult Delete([FromBody] DeleteEmployeesDto dto)
        {
            EmployeeToClinicRepository empRepo = new EmployeeToClinicRepository();


            EmployeeToClinic[] empToClinicRecords = empRepo.GetAll()
            .FindAll(emp => (emp.ClinicId == dto.clinicId) && dto.userIds.Contains(emp.Id))
            .ToArray();

            empRepo.DeleteMany(empToClinicRecords);

            return Ok();
        }
    }
}

