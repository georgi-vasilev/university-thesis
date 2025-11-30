namespace Infrastructure.Services
{
    using Application.Services.Contracts.QRCode;
    using Microsoft.Extensions.Configuration;
    using QRCoder;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Json;

    public class QRCodeService : IQRCodeService
    {
        private readonly string _secretKey;

        public QRCodeService(IConfiguration configuration)
        {
            _secretKey = configuration["QRCode:SecretKey"] ?? "your-secret-key-here";
        }

        public byte[] GenerateQRCodeForTicket(Guid ticketId, Guid orderId, Guid eventId)
        {
            var validationString = GenerateTicketValidationString(ticketId, orderId, eventId);

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(validationString, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            return qrCodeImage;
        }

        public string GenerateTicketValidationString(Guid ticketId, Guid orderId, Guid eventId)
        {
            var ticketData = new
            {
                TicketId = ticketId,
                OrderId = orderId,
                EventId = eventId,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            var json = JsonSerializer.Serialize(ticketData);
            var hash = GenerateHash(json);

            var validationData = new
            {
                Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json)),
                Hash = hash
            };

            return JsonSerializer.Serialize(validationData);
        }

        private string GenerateHash(string input)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
    }
}