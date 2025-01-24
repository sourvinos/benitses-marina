using API.Features.Sales.Customers;
using API.Features.Sales.Transactions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Features.Sales.Invoices {

    [Route("api/[controller]")]
    public class SalesController : ControllerBase {

        #region variables

        private readonly ICustomerRepository customerRepo;
        private readonly IInvoiceReadRepository invoiceReadRepo;
        private readonly IMapper mapper;

        #endregion

        public SalesController(ICustomerRepository customerRepo, IInvoiceReadRepository invoiceReadRepo, IMapper mapper) {
            this.customerRepo = customerRepo;
            this.invoiceReadRepo = invoiceReadRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<InvoiceistVM>> GetAsync() {
            return await invoiceReadRepo.GetAsync();
        }

    }

}