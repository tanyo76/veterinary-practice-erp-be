using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsersRestApi.Entities;
using UsersRestApi.Repositories;


public class MyModel
{
    public string username {get; set;}
    public string password {get; set;}
}


namespace UsersRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpPut]
        public IActionResult Put([FromBody] MyModel model)
        {
            User loggedUser = UsersRepository.Items.Find(u => u.Username == model.username &&
                                                              u.Password == model.password);

            if (loggedUser == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim("LoggedUserId", loggedUser.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("843y1rfhuewuiwqjsu9201jsaklsdnsafuieruig213b89"));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "vetmasters",
                "vetmasters.react.app",
                claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string jwt = tokenHandler.WriteToken(token);
            Console.WriteLine(jwt);

            return Ok(new { success = true, token = jwt });
        }
    }
}
