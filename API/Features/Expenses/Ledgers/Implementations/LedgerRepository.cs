using System;
using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AutoMapper;
using System.Threading.Tasks;
using API.Features.Expenses.Invoices;

namespace API.Features.Expenses.Ledgers {

    public class LedgerRepository : Repository<LedgerRepository>, ILedgerRepository {

        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<UserExtended> userManager;
        private readonly IMapper mapper;

        public LedgerRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager, IMapper mapper) : base(appDbContext, httpContext, settings, userManager) {
            this.httpContext = httpContext;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<LedgerVM>> GetForLedger(string fromDate, string toDate, int supplierId) {
            var records = await context.Invoices
                .AsNoTracking()
                .Include(x => x.Supplier)
                .Include(x => x.DocumentType)
                .Where(x => x.Date <= Convert.ToDateTime(toDate) && x.SupplierId == supplierId)
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<Invoice>, IEnumerable<LedgerVM>>(records);
        }

        public IEnumerable<LedgerVM> BuildBalanceForLedger(IEnumerable<LedgerVM> records) {
            decimal balance = 0;
            foreach (var record in records) {
                balance = balance + record.Debit - record.Credit;
                record.Balance = balance;
            }
            return records;
        }

        public LedgerVM BuildPrevious(IEnumerable<LedgerVM> records, string fromDate) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            foreach (var record in records) {
                if (Convert.ToDateTime(record.Date) < Convert.ToDateTime(fromDate)) {
                    debit += record.Debit;
                    credit += record.Credit;
                    balance = balance + record.Debit - record.Credit;
                }
            }
            var total = BuildTotalLine(debit, credit, balance, "ΣΥΝΟΛΑ ΠΡΟΗΓΟΥΜΕΝΗΣ ΠΕΡΙΟΔΟΥ");
            return total;
        }

        public List<LedgerVM> BuildRequested(IEnumerable<LedgerVM> records, string fromDate) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            var requestedPeriod = new List<LedgerVM> { };
            foreach (var record in records) {
                if (Convert.ToDateTime(record.Date) >= Convert.ToDateTime(fromDate)) {
                    requestedPeriod.Add(record);
                    debit += record.Debit;
                    credit += record.Credit;
                    balance += record.Debit - record.Credit;
                }
            }
            var total = BuildTotalLine(debit, credit, balance, "ΣΥΝΟΛΑ ΖΗΤΟΥΜΕΝΗΣ ΠΕΡΙΟΔΟΥ");
            requestedPeriod.Add(total);
            return requestedPeriod;
        }

        public LedgerVM BuildTotal(IEnumerable<LedgerVM> records) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            foreach (var record in records) {
                debit += record.Debit;
                credit += record.Credit;
                balance += record.Debit - record.Credit;
            }
            var total = BuildTotalLine(debit, credit, balance, "ΓΕΝΙΚΑ ΣΥΝΟΛΑ");
            return total;
        }

        public List<LedgerVM> MergePreviousRequestedAndTotal(LedgerVM previousPeriod, List<LedgerVM> requestedPeriod, LedgerVM total) {
            var final = new List<LedgerVM> {
                previousPeriod
            };
            foreach (var record in requestedPeriod) {
                final.Add(record);
            }
            final.Add(total);
            return final;
        }

        public async Task<IEnumerable<LedgerVM>> GetForBalanceAsync(int supplierId) {
            var records = await context.Invoices
                .AsNoTracking()
                .Include(x => x.Supplier)
                .Include(x => x.DocumentType)
                .Where(x => x.SupplierId == supplierId)
                .ToListAsync();
            return mapper.Map<IEnumerable<Invoice>, IEnumerable<LedgerVM>>(records);
        }

        private static LedgerVM BuildTotalLine(decimal debit, decimal credit, decimal balance, string label) {
            var total = new LedgerVM {
                Date = "",
                Supplier = new SimpleEntity {
                    Id = 0,
                    Description = ""
                },
                DocumentType = new DocumentTypeVM {
                    Id = 0,
                    Description = label,
                    Batch = ""
                },
                InvoiceNo = "",
                Debit = debit,
                Credit = credit,
                Balance = balance
            };
            return total;
        }

    }

}