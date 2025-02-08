using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using zKassa_Server.Attributes;
using zKassa_Server.ControllerModels;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionController : ControllerBase
    {
        private readonly ZDbContext _dbContext;
        private readonly UserManager<Employee> _userManager;

        public TransactionController(
            [FromServices] ZDbContext dbContext,
            UserManager<Employee> userManager
        )
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [RoleCheck(Permission.SubmitTransaction)]
        [HttpPost]
        public ActionResult<Guid> CreateTransaction([FromBody] NewTransaction newTransaction)
        {
            // TODO: add reasoning for clarity
            //if (!newTransaction.IsValid())
            //    return BadRequest();

            Employee user = GetEmployee();
            Guid storeId =
                user.ShopId
                ?? throw new Exception(
                    "Should not be permitted to access this controller without associating with a shop"
                );
            Transaction transaction = newTransaction.ToTransaction(storeId);
            _dbContext.Transactions.Add(transaction);
            _dbContext.TransactionItems.AddRange(transaction.TransactionItems);
            _dbContext.SaveChanges();
            return Ok(transaction.Id);
        }

        [NonAction]
        private Employee GetEmployee()
        {
            Employee? employee = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

            return employee
                ?? throw new Exception("Unauthorized user in [Authorize] Controller???");
        }
    }
}
