using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AutoMapper;
using API.Features.Cashiers.Transactions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.IO;

namespace API.Features.Cashiers.Ledgers {

    public class CashierLedgerRepository : Repository<CashierRepository>, ICashierLedgerRepository {

        private readonly IMapper mapper;

        public CashierLedgerRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager, IMapper mapper) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CashierLedgerVM>> GetForLedger(int companyId, int safeId, string fromDate, string toDate) {
            var records = await context.Cashiers
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId && x.SafeId == safeId && x.Date <= Convert.ToDateTime(toDate) && x.IsDeleted == false)
                .Include(x => x.Company)
                .Include(x => x.Safe)
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<Cashier>, IEnumerable<CashierLedgerVM>>(records);
        }

        public IEnumerable<CashierLedgerVM> BuildBalanceForLedger(IEnumerable<CashierLedgerVM> records) {
            decimal balance = 0;
            foreach (var record in records) {
                balance = balance + record.Debit - record.Credit;
                record.Balance = balance;
            }
            return records;
        }

        public CashierLedgerVM BuildPrevious(IEnumerable<CashierLedgerVM> records, string fromDate) {
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

        public List<CashierLedgerVM> BuildRequested(IEnumerable<CashierLedgerVM> records, string fromDate) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            var requestedPeriod = new List<CashierLedgerVM> { };
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

        public CashierLedgerVM BuildTotal(IEnumerable<CashierLedgerVM> records) {
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

        public List<CashierLedgerVM> MergePreviousRequestedAndTotal(CashierLedgerVM previousPeriod, List<CashierLedgerVM> requestedPeriod, CashierLedgerVM total) {
            var final = new List<CashierLedgerVM> {
                previousPeriod
            };
            foreach (var record in requestedPeriod) {
                final.Add(record);
            }
            final.Add(total);
            return final;
        }

          public FileStreamResult OpenDocument(string filename) {
            var fullpathname = Path.Combine("Uploaded Cashiers" + Path.DirectorySeparatorChar + filename);
            byte[] byteArray = File.ReadAllBytes(fullpathname);
            MemoryStream memoryStream = new(byteArray);
            return new FileStreamResult(memoryStream, "application/pdf");
        }

        public async Task<IEnumerable<CashierLedgerVM>> GetForBalanceAsync(int companyId) {
            var records = await context.Cashiers
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId)
                .ToListAsync();
            return mapper.Map<IEnumerable<Cashier>, IEnumerable<CashierLedgerVM>>(records);
        }

        private static CashierLedgerVM BuildTotalLine(decimal debit, decimal credit, decimal balance, string label) {
            var total = new CashierLedgerVM {
                Date = "",
                Remarks = label,
                Debit = debit,
                Credit = credit,
                Balance = balance
            };
            return total;
        }

    }

}