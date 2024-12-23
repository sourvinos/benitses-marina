using System;
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
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Features.Expenses.Invoices {

    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository {

        private readonly IMapper mapper;
        private readonly TestingEnvironment testingEnvironment;

        public InvoiceRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
            this.testingEnvironment = testingEnvironment.Value;
        }

        public async Task<IEnumerable<InvoiceListVM>> GetAsync() {
            var invoices = await context.Invoices
                .AsNoTracking()
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Supplier)
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<Invoice>, IEnumerable<InvoiceListVM>>(invoices);
        }

        public async Task<Invoice> GetByIdAsync(string invoiceId, bool includeTables) {
            return includeTables
                ? await context.Invoices
                    .AsNoTracking()
                    .Include(x => x.DocumentType)
                    .Include(x => x.PaymentMethod)
                    .Include(x => x.Supplier)
                    .Where(x => x.Id.ToString() == invoiceId)
                    .SingleOrDefaultAsync()
               : await context.Invoices
                  .AsNoTracking()
                  .Where(x => x.Id.ToString() == invoiceId)
                  .SingleOrDefaultAsync();
        }

        public Invoice Update(Guid invoiceId, Invoice invoice) {
            using var transaction = context.Database.BeginTransaction();
            UpdateInvoice(invoice);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return invoice;
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        private void UpdateInvoice(Invoice invoice) {
            context.Invoices.Update(invoice);
        }

    }

}