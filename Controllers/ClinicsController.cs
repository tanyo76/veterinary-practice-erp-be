using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersRestApi.Repositories;
using UsersRestApi.Entities;
using System.Collections.Generic;

public class AssignEmployeeToClinicModel
{
    public int userId { get; set; }
    public int clinicId { get; set; }
}

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

        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {

            UsersRepository userRepo = new UsersRepository();
            User user = userRepo.GetAll().Find(u => u.Id == userId);

            EmployeeToClinicRepository empToClinicRepo = new EmployeeToClinicRepository();
            ClinicsRepository clinicsRepo = new ClinicsRepository();

            List<Clinic> clinics = [];

            if (user.Role == "Owner")
            {
                clinics = clinicsRepo.GetAll().FindAll(c => c.OwnerId == userId);
            }

            if (user.Role != "Owner")
            {
                List<EmployeeToClinic> employeeToClinics = empToClinicRepo.GetAll().FindAll(emp => emp.UserId == userId);

                foreach (var item in employeeToClinics)
                {
                    Clinic clinic = clinicsRepo.GetAll().Find(clinic => clinic.Id == item.ClinicId);
                    clinics.Add(clinic);
                }
            }



            return Ok(new { clinics });
        }


        // Assign employee to a clinic
        [HttpPost("{clinicId}")]
        public IActionResult AddEmployeeToClinic([FromBody] AssignEmployeeToClinicModel employeeInfo)
        {

            EmployeeToClinicRepository employeeToClinicRepo = new EmployeeToClinicRepository();

            employeeToClinicRepo.Add(new EmployeeToClinic { UserId = employeeInfo.userId, ClinicId = employeeInfo.clinicId });

            return Created();
        }
    }
}