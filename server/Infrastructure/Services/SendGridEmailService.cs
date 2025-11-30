namespace Infrastructure.Services
{
    using Application.Services.Contracts;
    using Application.Services.Contracts.Email;
    using Domain.Order;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class SendGridEmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SendGridEmailService> _logger;

        public SendGridEmailService(
            ISendGridClient sendGridClient,
            IConfiguration configuration,
            ILogger<SendGridEmailService> logger)
        {
            _sendGridClient = sendGridClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendTicketEmailAsync(
            string recipientEmail,
            string recipientName,
            Order order,
            List<TicketEmailData> tickets,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var from = new EmailAddress(
                    _configuration["SendGrid:FromEmail"],
                    _configuration["SendGrid:FromName"]);

                var to = new EmailAddress(recipientEmail, recipientName);

                var subject = $"Your Tickets - Order #{order.Id.ToString()[..8]}";

                var htmlContent = GenerateEmailHtmlContent(order, tickets);
                var plainTextContent = GeneratePlainTextContent(order, tickets);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                foreach (var ticket in tickets)
                {
                    var attachment = new Attachment
                    {
                        Content = Convert.ToBase64String(ticket.QRCodeImage),
                        Type = "image/png",
                        Filename = $"ticket-{ticket.TicketId}.png",
                        Disposition = "attachment",
                        ContentId = ticket.TicketId.ToString()
                    };
                    msg.AddAttachment(attachment);
                }

                var response = await _sendGridClient.SendEmailAsync(msg, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully sent ticket email to {Email} for order {OrderId}",
                        recipientEmail, order.Id);
                    return true;
                }
                else
                {
                    var body = await response.Body.ReadAsStringAsync();
                    _logger.LogError("Failed to send email. Status: {StatusCode}, Body: {Body}",
                        response.StatusCode, body);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending ticket email to {Email} for order {OrderId}",
                    recipientEmail, order.Id);
                return false;
            }
        }

        private string GenerateEmailHtmlContent(Order order, List<TicketEmailData> tickets)
        {
            var ticketRows = string.Join("", tickets.Select(t => $@"
                <tr>
                    <td style='padding: 10px; border-bottom: 1px solid #eee;'>{t.EventName}</td>
                    <td style='padding: 10px; border-bottom: 1px solid #eee;'>{t.VenueName}</td>
                    <td style='padding: 10px; border-bottom: 1px solid #eee;'>{t.EventDate:MMM dd, yyyy}</td>
                    <td style='padding: 10px; border-bottom: 1px solid #eee;'>{t.EventTime.ToString()}</td>
                    <td style='padding: 10px; border-bottom: 1px solid #eee;'>{t.TicketType}</td>
                    <td style='padding: 10px; border-bottom: 1px solid #eee;'>${t.Price:F2}</td>
                </tr>
            "));

            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Your Tickets</title>
            </head>
            <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                <h1 style='color: #333;'>Your Tickets Are Ready!</h1>
                <p>Thank you for your purchase. Here are your ticket details:</p>
                
                <div style='background-color: #f5f5f5; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                    <h3>Order Information</h3>
                    <p><strong>Order ID:</strong> {order.Id}</p>
                    <p><strong>Order Status:</strong> {order.Status}</p>
                    <p><strong>Total Amount:</strong> ${order.Payment.Amount:F2}</p>
                </div>

                <h3>Ticket Details</h3>
                <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
                    <thead>
                        <tr style='background-color: #f0f0f0;'>
                            <th style='padding: 10px; text-align: left; border-bottom: 2px solid #ddd;'>Event</th>
                            <th style='padding: 10px; text-align: left; border-bottom: 2px solid #ddd;'>Venue</th>
                            <th style='padding: 10px; text-align: left; border-bottom: 2px solid #ddd;'>Date</th>
                            <th style='padding: 10px; text-align: left; border-bottom: 2px solid #ddd;'>Time</th>
                            <th style='padding: 10px; text-align: left; border-bottom: 2px solid #ddd;'>Type</th>
                            <th style='padding: 10px; text-align: left; border-bottom: 2px solid #ddd;'>Price</th>
                        </tr>
                    </thead>
                    <tbody>
                        {ticketRows}
                    </tbody>
                </table>

                <div style='background-color: #e8f4fd; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                    <h3>Important Instructions</h3>
                    <ul>
                        <li>Please bring your QR code tickets (attached to this email)</li>
                        <li>Arrive at least 30 minutes before the event starts</li>
                        <li>Keep your tickets safe - they cannot be replaced if lost</li>
                        <li>Each QR code is unique and can only be used once</li>
                    </ul>
                </div>

                <p>If you have any questions, please contact our support team.</p>
                <p>Enjoy your event!</p>
            </body>
            </html>";
        }

        private string GeneratePlainTextContent(Order order, List<TicketEmailData> tickets)
        {
            var ticketDetails = string.Join("\n", tickets.Select(t =>
                   $"- {t.EventName} at {t.VenueName} on {t.EventDate:MMM dd, yyyy} at {t.EventTime:hh\\:mm} ({t.TicketType}) - ${t.Price:F2}"));

            return $@"
                    Your Tickets Are Ready!
                    
                    Thank you for your purchase. Here are your ticket details:
                    
                    Order Information:
                    - Order ID: {order.Id}
                    - Order Status: {order.Status}
                    - Total Amount: ${order.Payment.Amount:F2}
                    
                    Ticket Details:
                    {ticketDetails}
                    
                    Important Instructions:
                    - Please bring your QR code tickets (attached to this email)
                    - Arrive at least 30 minutes before the event starts
                    - Keep your tickets safe - they cannot be replaced if lost
                    - Each QR code is unique and can only be used once
                    
                    If you have any questions, please contact our support team.
                    Enjoy your event!
            ";
        }
    }
}