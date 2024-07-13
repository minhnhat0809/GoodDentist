using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Services;
using Services.Impl;

namespace GoodDentist.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService paymentService;
		public ResponseDTO responseDTO;
		public PaymentController(IPaymentService _paymentService)
		{
			paymentService = _paymentService;
		}
		[HttpGet]
		public async Task<ResponseDTO> GetAllPayment([FromQuery] int pageNumber, int rowsPerPage)
		{
			ResponseDTO responseDTO = await paymentService.GetAllPayment(pageNumber,rowsPerPage);
			return responseDTO;
		}
	}
}
