using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using RazorpayBackend.Models;

namespace RazorpayBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RazorpayController : ControllerBase
{
    private readonly IConfiguration _config;

    public RazorpayController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("create-order")]
    public IActionResult CreateOrder([FromBody] OrderRequest request)
    {
        try
        {
            // Initialize Razorpay client with your test keys
            RazorpayClient client = new RazorpayClient(
                _config["Razorpay:KeyId"],
                _config["Razorpay:KeySecret"]
            );

            Console.WriteLine($"KeyId: {_config["Razorpay:KeyId"]}");
            Console.WriteLine($"KeySecret: {_config["Razorpay:KeySecret"]}");

            Dictionary<string, object> options = new()
            {
                { "amount", request.Amount * 100 }, // amount in paise
                { "currency", "INR" },
                { "payment_capture", 1 }
            };

            // ✅ Create the order and assign it to the 'order' variable
            Order order = client.Order.Create(options);

            return Ok(new
            {
                id = order["id"],
                amount = order["amount"],
                currency = order["currency"]
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
