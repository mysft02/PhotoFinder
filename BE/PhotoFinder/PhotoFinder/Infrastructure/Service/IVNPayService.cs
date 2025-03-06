using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.VNPay;
using PhotoFinder.Entity;
using PhotoFinder.Infrastructure.Data;
using System.Security.Cryptography;

namespace PhotoFinder.Infrastructure.Service
{
    public interface IVNPayService
    {
        public Task<IActionResult> HandleCreateVNPayUrl(HttpContext context, VnPayRequestDTO vnPayRequestDTO, string userId, string url);
        //public Task<IActionResult> HandleGetTransactions(string currentUserId);
        public Task<IActionResult> HandleVNPay(string url, string userId);
    }


    public class VnPayService : ControllerBase, IVNPayService
    {
        private readonly VNPayData _vnPay;
        private readonly string? _vnpVersion;
        private readonly string? _vnpCommand;
        private readonly string? _vnpTmnCode;
        private readonly string? _vnpCurrCode;
        private readonly string? _vnpLocale;
        private readonly string? _vnpBaseUrl;
        private readonly string? _vnpHashSecret;
        private readonly PhotoFinderContext _context;

        public VnPayService(IConfiguration config, PhotoFinderContext context)
        {
            _vnPay = new VNPayData();
            _vnpVersion = config["VNPay:Version"];
            _vnpCommand = config["VNPay:Command"];
            _vnpTmnCode = config["VNPay:TmnCode"];
            _vnpCurrCode = config["VNPay:CurrCode"];
            _vnpLocale = config["VNPay:Locale"];
            _vnpBaseUrl = config["VNPay:BaseUrl"];
            _vnpHashSecret = config["VNPay:HashSecret"];
            _context = context;
        }

        public async Task<IActionResult> HandleCreateVNPayUrl(HttpContext context, VnPayRequestDTO vnPayRequestDTO, string userId, string url)
        {
            try
            {
                var tick = DateTime.Now.Ticks.ToString();

                var user = _context.Users.FirstOrDefault(c => c.UserId.ToString() == userId);
                var booking = _context.Bookings.FirstOrDefault(c => c.BookingId == vnPayRequestDTO.BookingId);

                var vnpReturnUrl = url;

                RandomNumberGenerator rng = RandomNumberGenerator.Create();
                byte[] buffer = new byte[10];
                rng.GetBytes(buffer);

                string chuoi = "";
                for (int i = 0; i < 10; i++)
                {
                    char kyTu;
                    if (buffer[i] % 2 == 0)
                    {
                        // Chọn chữ cái ngẫu nhiên
                        kyTu = (char)('a' + buffer[i] % 26);
                    }
                    else
                    {
                        // Chọn số ngẫu nhiên
                        kyTu = (char)('0' + buffer[i] % 10);
                    }
                    chuoi += kyTu;
                }

                _vnPay.AddRequestData("vnp_Version", _vnpVersion);
                _vnPay.AddRequestData("vnp_Command", _vnpCommand);
                _vnPay.AddRequestData("vnp_TmnCode", _vnpTmnCode);
                _vnPay.AddRequestData("vnp_Amount", (booking.TotalPrice * 100).ToString());
                _vnPay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                _vnPay.AddRequestData("vnp_CurrCode", _vnpCurrCode);
                _vnPay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
                _vnPay.AddRequestData("vnp_Locale", _vnpLocale);
                _vnPay.AddRequestData("vnp_OrderInfo", user.Email + "_" + booking.BookingId.ToString());
                _vnPay.AddRequestData("vnp_OrderType", "other"); //default value: other
                _vnPay.AddRequestData("vnp_ReturnUrl", vnpReturnUrl);
                _vnPay.AddRequestData("vnp_TxnRef", chuoi);

                var paymentUrl = _vnPay.CreateRequestUrl(_vnpBaseUrl, _vnpHashSecret);

                return new OkObjectResult(new { Url = paymentUrl });

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleVNPay(string url, string userId )
        {
            try
            {
                var user = _context.Users
                    .FirstOrDefault(c => c.UserId.ToString() == userId);
                
                var bookingId = _vnPay.GetResponseData("vnp_OrderInfo").Split("_")[1];
                var booking = _context.Bookings.FirstOrDefault(c => c.BookingId.ToString() == bookingId);

                var payment = new Payment
                {
                    BookingId = booking.BookingId,
                    PaymentMethod = "VNPAY",
                    PaymentDate = DateTime.Now,
                    PaymentStatus = "completed",
                    Amount = booking.TotalPrice,
                };

                _context.SaveChanges();

                return Ok("Update Balance Success!");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
