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

namespace API.Features.Sales.Invoices {

    public class InvoiceReadRepository : Repository<Invoice>, IInvoiceReadRepository {

        private readonly IMapper mapper;

        public InvoiceReadRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<InvoiceistVM>> GetAsync() {
            var sales = await context.Invoices
                .AsNoTracking()
                .Where(x => x.DiscriminatorId == 1)
                .Include(x => x.Customer)
                .Include(x => x.DocumentType)
                .Include(x => x.Aade)
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<Invoice>, IEnumerable<InvoiceistVM>>(sales);
        }

        public async Task<Invoice> GetByIdAsync(string invoiceId, bool includeTables) {
            return includeTables
                ? await context.Invoices
                    .AsNoTracking()
                    .Include(x => x.Customer).ThenInclude(x => x.Nationality)
                    .Include(x => x.Customer).ThenInclude(x => x.TaxOffice)
                    .Include(x => x.DocumentType)
                    .Include(x => x.PaymentMethod)
                    .Include(x => x.Aade)
                    .Include(x => x.Items)
                    .Where(x => x.InvoiceId.ToString() == invoiceId)
                    .SingleOrDefaultAsync()
               : await context.Invoices
                    .AsNoTracking()
                    .Include(x => x.Aade)
                    .Include(x => x.Items)
                    .Where(x => x.InvoiceId.ToString() == invoiceId)
                    .SingleOrDefaultAsync();
        }

        public async Task<InvoiceAade> GetInvoiceAadeByIdAsync(string invoiceId) {
            return await context.SalesAade
                .AsNoTracking()
                .Where(x => x.InvoiceId.ToString() == invoiceId)
                .SingleOrDefaultAsync();
        }

    }

}