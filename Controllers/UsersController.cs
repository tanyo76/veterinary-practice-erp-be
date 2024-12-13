using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersRestApi.Repositories;
using UsersRestApi.Entities;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersRestApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetEmployees()
        {
            UsersRepository repo = new UsersRepository();
            List<User> items = repo.GetAll().FindAll(u => u.Role != "Owner");

            return Ok(items);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User userDto)
        {
            UsersRepository repo = new UsersRepository();

            User user = repo.GetAll().Find(u => u.Email == userDto.Email);

            if (user != null)
            {
                return Conflict(new { message = "User with this email already exists" });
            }

            int userId = repo.Add(userDto);

            return Ok(new { userId });
        }


        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId)
        {
            EmployeeToClinicRepository empToClinicRepo = new EmployeeToClinicRepository();

            EmployeeToClinic emptToClinicRecord = empToClinicRepo.GetAll().Find(emp => emp.UserId == userId);

            if (emptToClinicRecord != null)
            {
                empToClinicRepo.Delete(emptToClinicRecord);
                return Ok();
            }

            return NotFound(emptToClinicRecord);
        }
    }
}
