using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace API.Features.Sales.Transactions {

    public class InvoiceReadRepository : Repository<Sale>, ISaleReadRepository {

        private readonly IMapper mapper;

        public InvoiceReadRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SaleListVM>> GetAsync() {
            var invoices = await context.Sales
                .AsNoTracking()
                .Where(x => x.DiscriminatorId == 1)
                .Include(x => x.Customer)
                .Include(x => x.DocumentType)
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<Sale>, IEnumerable<SaleListVM>>(invoices);
        }

        public async Task<IEnumerable<SaleListVM>> GetForPeriodAsync(SaleListCriteriaVM criteria) {
            var invoices = await context.Sales
                .AsNoTracking()
                .Where(x => x.DiscriminatorId == 1)
                .Include(x => x.Customer)
                .Include(x => x.DocumentType)
                .Include(x => x.Aade)
                .Where(x => x.Date >= Convert.ToDateTime(criteria.FromDate) && x.Date <= Convert.ToDateTime(criteria.ToDate))
                .OrderBy(x => x.Date).ThenBy(x => x.InvoiceNo)
                .ToListAsync();
            return mapper.Map<IEnumerable<Sale>, IEnumerable<SaleListVM>>(invoices);
        }

        public SalePdfVM GetFirstWithEmailPending() {
            var invoice = context.Sales
                .AsNoTracking()
                .Include(x => x.Customer).ThenInclude(x => x.TaxOffice)
                .Include(x => x.Customer).ThenInclude(x => x.Nationality)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Aade)
                .Where(x => x.IsEmailPending)
                .FirstOrDefault();
            return mapper.Map<Sale, SalePdfVM>(invoice);
        }

        public async Task<Sale> GetByIdAsync(string invoiceId, bool includeTables) {
            return includeTables
                ? await context.Sales
                    .AsNoTracking()
                    .Include(x => x.Customer).ThenInclude(x => x.TaxOffice)
                    .Include(x => x.Customer).ThenInclude(x => x.Nationality)
                    .Include(x => x.DocumentType)
                    .Include(x => x.PaymentMethod)
                    .Include(x => x.Aade)
                    .Where(x => x.InvoiceId.ToString() == invoiceId)
                    .SingleOrDefaultAsync()
               : await context.Sales
                    .AsNoTracking()
                    .Include(x => x.Aade)
                    .Where(x => x.InvoiceId.ToString() == invoiceId)
                    .SingleOrDefaultAsync();
        }

        public async Task<Sale> GetByIdForPatchEmailSent(string invoiceId) {
            return await context.Sales
                .AsNoTracking()
                .Where(x => x.InvoiceId.ToString() == invoiceId)
                .SingleOrDefaultAsync();
        }

        public async Task<Sale> GetByIdForPdfAsync(string invoiceId) {
            return await context.Sales
                .AsNoTracking()
                .Include(x => x.Customer).ThenInclude(x => x.TaxOffice)
                .Include(x => x.Customer).ThenInclude(x => x.Nationality)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Aade)
                .Where(x => x.InvoiceId.ToString() == invoiceId)
                .SingleOrDefaultAsync();
        }

        public async Task<SaleAade> GetInvoiceAadeByIdAsync(string invoiceId) {
            return await context.SalesAade
                .AsNoTracking()
                .Where(x => x.InvoiceId.ToString() == invoiceId)
                .SingleOrDefaultAsync();
        }

        public async Task<Sale> GetByIdForXmlAsync(string invoiceId) {
            return await context.Sales
                .AsNoTracking()
                .Include(x => x.Customer).ThenInclude(x => x.Nationality)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Aade)
                .SingleOrDefaultAsync(x => x.InvoiceId.ToString() == invoiceId);
        }

    }

}