using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersRestApi.Repositories;
using UsersRestApi.Entities;
using System.Collections.Generic;


namespace UsersRestApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {

        [HttpPost]
        public IActionResult Post([FromBody] Clinic clinicDto)
        {

            ClinicsRepository repo = new ClinicsRepository();
            repo.Add(clinicDto);

            return Ok(new { clinicId = clinicDto.Id });
        }

        [HttpGet("{clinicId}")]
        public IActionResult Get(int clinicId)
        {

            ClinicsRepository repo = new ClinicsRepository();
            Clinic clinic = repo.GetAll().Find(c => c.Id == clinicId);

            EmployeeToClinicRepository empToClinicRepo = new EmployeeToClinicRepository();
            List<EmployeeToClinic> employeeToClinic = empToClinicRepo.GetAll().FindAll(emp => emp.ClinicId == clinic.Id);

            return Ok(new { clinic, employees = employeeToClinic });
        }

        // Create employee and assign it to a clinic
        [HttpPost("{clinicId}")]
        public IActionResult AddEmployeeToClinic([FromBody] User userDto, int clinicId)
        {

            UsersRepository usersRepo = new UsersRepository();
            EmployeeToClinicRepository employeeToClinicRepo = new EmployeeToClinicRepository();
            int userId = usersRepo.Add(userDto);

            employeeToClinicRepo.Add(new EmployeeToClinic { UserId = userId, ClinicId = clinicId });

            return Ok();
        }
    }
}