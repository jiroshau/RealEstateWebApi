using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebApi.Data;
using RealEstateWebApi.Models;
using RealEstateWebApi.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AccountantController : ControllerBase
{
    private readonly RealEstateWebApi.Data.RealEstateContext _context;

    public AccountantController(RealEstateWebApi.Data.RealEstateContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountantDto>>> GetAccountants()
    {
        var accountants = await _context.accountants
    .Include(a => a.User)
    .Include(a => a.Payments) // <-- include payments
    .Select(a => new AccountantDto
    {
        AccountantID = a.AccountantID,
        Name = a.Name,
        Phone = a.Phone,
        Address = a.Address,
        Username = a.User.Username,
        Email = a.User.Email,
        Payments = a.Payments.Select(p => new PaymentDto
        {
            PaymentID = p.PaymentID,
            Amount = p.Amount,
            PaymentDate = p.PaymentDate,
            Status = p.Status,
            LeaseID = p.LeaseID
        }).ToList()
    }).ToListAsync();

        return Ok(accountants);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<AccountantDto>> GetAccountant(int id)
    {
        var accountant = await _context.accountants
            .Include(a => a.User)
            .Where(a => a.AccountantID == id)
            .Select(a => new AccountantDto
            {
                AccountantID = a.AccountantID,
                Name = a.Name,
                Phone = a.Phone,
                Address = a.Address,
                Username = a.User.Username,
                Email = a.User.Email
            }).FirstOrDefaultAsync();

        if (accountant == null) return NotFound();
        return Ok(accountant);
    }


    [HttpPost]
    public async Task<ActionResult<AccountantDto>> CreateAccountant(CreateAccountantDto dto)
    {
        var accountant = new Accountant
        {
            UserID = dto.UserID,
            Name = dto.Name,
            Phone = dto.Phone,
            Address = dto.Address
        };

        _context.accountants.Add(accountant);
        await _context.SaveChangesAsync();

        var accountantDto = new AccountantDto
        {
            AccountantID = accountant.AccountantID,
            Name = accountant.Name,
            Phone = accountant.Phone,
            Address = accountant.Address,
            Username = accountant.User.Username,
            Email = accountant.User.Email
        };

        return CreatedAtAction(nameof(GetAccountant), new { id = accountant.AccountantID }, accountantDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccountant(int id, CreateAccountantDto dto)
    {
        var accountant = await _context.accountants.FindAsync(id);
        if (accountant == null) return NotFound();

        accountant.Name = dto.Name;
        accountant.Phone = dto.Phone;
        accountant.Address = dto.Address;

        _context.Entry(accountant).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccountant(int id)
    {
        var accountant = await _context.accountants.FindAsync(id);
        if (accountant == null) return NotFound();

        _context.accountants.Remove(accountant);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
