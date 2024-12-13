using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersRestApi.Repositories;
using UsersRestApi.Entities;
using System.Collections.Generic;
using System;

public class AssignEmployeeToClinicModel
{
    public int userId { get; set; }
    public int clinicId { get; set; }
}

public class EditClinicModel
{
    public int clinicId { get; set; }
    public string name { get; set; }
    public string address { get; set; }
}

namespace UsersRestApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {

        [HttpPut]
        public IActionResult EditField([FromBody]EditClinicModel editClinicModel)
        {

            ClinicsRepository repo = new ClinicsRepository();

            Clinic clinic = repo.GetAll().Find(c => c.Id == editClinicModel.clinicId);

            if (clinic == null)
            {
                NotFound(new { message = "Clinic not found " });
            }

            clinic.Address = editClinicModel.address;
            clinic.Name = editClinicModel.name;

            repo.Update(clinic);

            return Ok(new { clinic });
        }

        [HttpGet]
        public IActionResult GetClinicById([FromQuery(Name = "clinicId")] int clinicId)
        {

            ClinicsRepository repo = new ClinicsRepository();

            Clinic clinic = repo.GetAll().Find(c => c.Id == clinicId);

            return Ok(new { clinic });
        }

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

            EmployeeToClinic empToClinic = employeeToClinicRepo.GetAll().Find(emp => emp.ClinicId == employeeInfo.clinicId && emp.UserId == employeeInfo.userId);

            if (empToClinic != null)
            {
                return Conflict(new { message = "This user is already part of the clinic" });
            }

            employeeToClinicRepo.Add(new EmployeeToClinic { UserId = employeeInfo.userId, ClinicId = employeeInfo.clinicId });

            return Created();
        }
    }
}