using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.IO;

namespace API.Features.Cashiers.Transactions {

    public class CashierRepository : Repository<Cashier>, ICashierRepository {

        private readonly IMapper mapper;
        private readonly TestingEnvironment testingEnvironment;

        public CashierRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
            this.testingEnvironment = testingEnvironment.Value;
        }

        public async Task<IEnumerable<CashierListVM>> GetAsync(int? companyId) {
            var cashiers = await context.Cashiers
                .AsNoTracking()
                .Where(x => companyId == null || x.CompanyId == companyId)
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Company)
                .Include(x => x.Safe)
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<Cashier>, IEnumerable<CashierListVM>>(cashiers);
        }

        public async Task<Cashier> GetByIdAsync(string cashierId, bool includeTables) {
            return includeTables
                ? await context.Cashiers
                    .AsNoTracking()
                    .Include(x => x.Company)
                    .Include(x => x.Safe)
                    .Where(x => x.CashierId.ToString() == cashierId)
                    .SingleOrDefaultAsync()
               : await context.Cashiers
                  .AsNoTracking()
                  .Where(x => x.CashierId.ToString() == cashierId)
                  .SingleOrDefaultAsync();
        }

        public Cashier Update(Guid cashierId, Cashier cashier) {
            using var transaction = context.Database.BeginTransaction();
            UpdateCashier(cashier);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return cashier;
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        private void UpdateCashier(Cashier cashier) {
            context.Cashiers.Update(cashier);
        }

        public FileStreamResult OpenDocument(string filename) {
            var fullpathname = Path.Combine("Uploaded Cashiers" + Path.DirectorySeparatorChar + filename);
            byte[] byteArray = File.ReadAllBytes(fullpathname);
            MemoryStream memoryStream = new(byteArray);
            return new FileStreamResult(memoryStream, "application/pdf");
        }

    }

}