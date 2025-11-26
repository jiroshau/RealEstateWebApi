using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebApi.Data;
using RealEstateWebApi.Models;
using RealEstateWebApi.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly RealEstateWebApi.Data.RealEstateContext _context;

    public PaymentController(RealEstateWebApi.Data.RealEstateContext context)
    {
        _context = context;
    }

    // GET: api/Payment
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments()
    {
        var payments = await _context.Payments
            .Include(p => p.Accountant).ThenInclude(a => a.User)
            .Include(p => p.Lease).ThenInclude(l => l.Tenant)
            .Include(p => p.Lease).ThenInclude(l => l.Property)
            .Select(p => new PaymentDto
            {
                PaymentID = p.PaymentID,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status,

                Accountant = new AccountantDto
                {
                    AccountantID = p.Accountant.AccountantID,
                    Name = p.Accountant.Name,
                    Phone = p.Accountant.Phone,
                    Address = p.Accountant.Address,
                    Username = p.Accountant.User.Username,
                    Email = p.Accountant.User.Email
                },

                Lease = new LeaseDto
                {
                    LeaseID = p.Lease.LeaseID,
                    MonthlyRent = p.Lease.MonthlyRent,
                    StartDate = p.Lease.StartDate,
                    EndDate = p.Lease.EndDate,
                    Status = p.Lease.Status,

                    Tenant = new TenantDto
                    {
                        TenantID = p.Lease.Tenant.TenantID,
                        Name = p.Lease.Tenant.Name,
                        Phone = p.Lease.Tenant.Phone,
                        Address = p.Lease.Tenant.Address
                    },

                    Property = new PropertyDto
                    {
                        PropertyID = p.Lease.Property.PropertyID,
                        PropertyName = p.Lease.Property.PropertyName,
                        Address = p.Lease.Property.Address,
                        Type = p.Lease.Property.Type,
                        MonthlyRent = p.Lease.Property.MonthlyRent,
                        Status = p.Lease.Property.Status,
                        Description = p.Lease.Property.Description,
                        LandlordID = p.Lease.Property.LandlordID,
                        AgentID = p.Lease.Property.AgentID
                    }
                }
            })
            .ToListAsync();

        return Ok(payments);
    }

    // GET: api/Payment/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDto>> GetPayment(int id)
    {
        var payment = await _context.Payments
            .Include(p => p.Accountant).ThenInclude(a => a.User)
            .Include(p => p.Lease).ThenInclude(l => l.Tenant)
            .Include(p => p.Lease).ThenInclude(l => l.Property)
            .Where(p => p.PaymentID == id)
            .Select(p => new PaymentDto
            {
                PaymentID = p.PaymentID,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status,

                Accountant = new AccountantDto
                {
                    AccountantID = p.Accountant.AccountantID,
                    Name = p.Accountant.Name,
                    Phone = p.Accountant.Phone,
                    Address = p.Accountant.Address,
                    Username = p.Accountant.User.Username,
                    Email = p.Accountant.User.Email
                },

                Lease = new LeaseDto
                {
                    LeaseID = p.Lease.LeaseID,
                    MonthlyRent = p.Lease.MonthlyRent,
                    StartDate = p.Lease.StartDate,
                    EndDate = p.Lease.EndDate,
                    Status = p.Lease.Status,

                    Tenant = new TenantDto
                    {
                        TenantID = p.Lease.Tenant.TenantID,
                        Name = p.Lease.Tenant.Name,
                        Phone = p.Lease.Tenant.Phone,
                        Address = p.Lease.Tenant.Address
                    },

                    Property = new PropertyDto
                    {
                        PropertyID = p.Lease.Property.PropertyID,
                        PropertyName = p.Lease.Property.PropertyName,
                        Address = p.Lease.Property.Address,
                        Type = p.Lease.Property.Type,
                        MonthlyRent = p.Lease.Property.MonthlyRent,
                        Status = p.Lease.Property.Status,
                        Description = p.Lease.Property.Description,
                        LandlordID = p.Lease.Property.LandlordID,
                        AgentID = p.Lease.Property.AgentID
                    }
                }
            })
            .FirstOrDefaultAsync();

        if (payment == null) return NotFound();
        return Ok(payment);
    }

    // POST: api/Payment
    [HttpPost]
    public async Task<ActionResult<Payment>> CreatePayment(CreatePaymentDto dto)
    {
        var payment = new Payment
        {
            LeaseID = dto.LeaseID,
            AccountantID = dto.AccountantID,
            Amount = dto.Amount,
            PaymentMethod = dto.PaymentMethod,
            PaymentDate = dto.PaymentDate,
            Status = dto.Status
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentID }, payment);
    }

    // PUT: api/Payment/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayment(int id, CreatePaymentDto dto)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null) return NotFound();

        payment.LeaseID = dto.LeaseID;
        payment.AccountantID = dto.AccountantID;
        payment.Amount = dto.Amount;
        payment.PaymentMethod = dto.PaymentMethod;
        payment.PaymentDate = dto.PaymentDate;
        payment.Status = dto.Status;

        _context.Entry(payment).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Payment/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null) return NotFound();

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
