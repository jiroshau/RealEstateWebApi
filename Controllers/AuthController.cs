using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebApi.Data;
using RealEstateWebApi.Models.DTOs;
using RealEstateWebApi.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RealEstateWebApi.Data.RealEstateContext _context;

    public AuthController(RealEstateWebApi.Data.RealEstateContext context)
    {
        _context = context;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto dto)
    {
        var user = await _context.users
            .FirstOrDefaultAsync(u => u.Username.Trim() == dto.Username.Trim() &&
                                      u.Password.Trim() == dto.Password.Trim());

        if (user == null)
            return Unauthorized("Invalid username or password");

        var response = new LoginResponseDto
        {
            UserID = user.UserID,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };

        return Ok(response);
    }
}
