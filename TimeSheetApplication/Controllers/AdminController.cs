using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace Time.Sheet.Final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Typically, an API endpoint wouldn't have a method just to return a view.
            return Ok("API is running.");
        }

        // Action method to handle sending email
        [HttpPost("send-email")]
        public IActionResult SendEmail([FromForm] string email)
        {
            try
            {
                // Generate unique link with expiration time
                string uniqueLink = GenerateUniqueLink(email);
                // Send email with unique link
                SendEmailToUser(email, uniqueLink);
                return Ok(new { Message = "Email sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "Error sending email: " + ex.Message });
            }
        }

        // Method to generate unique link (you can implement your own logic)
        private string GenerateUniqueLink(string email)
        {
            DateTime expirationTime = DateTime.Now.AddDays(1); // Link expires after one day
            string timestamp = expirationTime.ToString("yyyyMMddHHmmss");
            string uniqueLink = "https://localhost:7269/user/create?email=" + email + "&timestamp=" + timestamp;
            return uniqueLink;
        }

        // Method to send email
        private void SendEmailToUser(string email, string uniqueLink)
        {
            string smtpHost = _configuration["SmtpSettings:Host"];
            int smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
            string smtpUsername = _configuration["SmtpSettings:Username"];
            string smtpPassword = _configuration["SmtpSettings:Password"];

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(smtpUsername); // Sender's email address
                message.To.Add(email); // Recipient's email address
                message.Subject = "Create User Account";
                message.Body = "Click the following link to create your user account: " + uniqueLink;

                using (var smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }
            }
        }
    }
}
