using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Entity;

namespace Repositories
{
	public interface IPaymentRepo
	{
		Task<List<Payment>> GetAllPayment(int pageNumber, int rowsPerPage);
	}
}
