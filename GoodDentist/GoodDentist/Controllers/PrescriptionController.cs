﻿using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Services;

namespace GoodDentist.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PrescriptionController : ControllerBase
	{
		private readonly IPrescriptionService _prescriptionService;
		private readonly IDistributedCache _distributedCache;
        public PrescriptionController(IPrescriptionService prescriptionService, IDistributedCache distributedCache)
        {
            this._prescriptionService = prescriptionService;
			this._distributedCache = distributedCache;
        }
		[HttpPost("new-prescription")]
		public async Task<ResponseDTO> AddPrescription([FromBody] PrescriptionCreateDTO prescriptionDTO)
		{
			ResponseDTO responseDTO = await _prescriptionService.AddPrescription(prescriptionDTO);

			return responseDTO;
		}

		[HttpGet("all-prescription")]
		public async Task<ResponseDTO> GetAllPrescription([FromQuery] int pageNumber, int rowsPerPage)
		{
			ResponseDTO responseDTO = await _prescriptionService.GetAllPrescription(pageNumber, rowsPerPage);
			return responseDTO;
		}

		[HttpGet("search-prescription")]
		public async Task<ResponseDTO> SearchPrescription([FromQuery] string searchValue)
		{
			ResponseDTO responseDTO = await _prescriptionService.SearchPrescription(searchValue);
			return responseDTO;
		}

		[HttpDelete("delete-prescription")]
		public async Task<ResponseDTO> DeletePrescription([FromQuery] int prescriptionId)
		{
			ResponseDTO responseDTO = await _prescriptionService.DeletePrescription(prescriptionId);
			return responseDTO;
		}

		[HttpPut("update-prescription")]
		public async Task<ResponseDTO> UpdatePrescription([FromBody] PrescriptionDTO prescriptionDTO)
		{
			ResponseDTO responseDTO = await _prescriptionService.UpdatePrescription(prescriptionDTO);

			return responseDTO;
		}
	}
}