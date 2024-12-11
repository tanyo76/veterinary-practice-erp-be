using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersRestApi.Entities;
using UsersRestApi.Repositories;


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

        [HttpDelete("{userId}/{clinicId}")]
        public IActionResult Delete(int clinicId, int userId)
        {

            EmployeeToClinicRepository empRepo = new EmployeeToClinicRepository();

            EmployeeToClinic empToClinicRecord = empRepo.GetAll()
            .Find(emp => emp.ClinicId == clinicId && emp.UserId == userId);

            empRepo.Delete(empToClinicRecord);

            return Ok();
        }
    }
}