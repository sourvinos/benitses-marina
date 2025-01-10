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
using API.Features.Expenses.Transactions;
using API.Features.Expenses.Suppliers;

namespace API.Features.Expenses.BalanceSheet {

    public class BalanceSheetRepository : Repository<BalanceSheetRepository>, IBalanceSheetRepository {

        private readonly IMapper mapper;

        public BalanceSheetRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager, IMapper mapper) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BalanceSheetVM>> GetForBalanceSheet(string fromDate, string toDate, int supplierId, int companyId) {
            var records = await context.Transactions
                .AsNoTracking()
                .Include(x => x.Supplier)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Where(x => x.Date <= Convert.ToDateTime(toDate)
                    && (x.Company.Id == companyId)
                    && (x.SupplierId == supplierId)
                    && (x.IsDeleted == false))
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<TransactionsBase>, IEnumerable<BalanceSheetVM>>(records);
        }

        public IEnumerable<BalanceSheetVM> BuildBalanceForBalanceSheet(IEnumerable<BalanceSheetVM> records) {
            decimal balance = 0;
            foreach (var record in records) {
                balance = balance + record.Debit - record.Credit;
                record.Balance = balance;
            }
            return records;
        }

        public BalanceSheetVM BuildPrevious(SupplierListVM supplier, IEnumerable<BalanceSheetVM> records, string fromDate) {
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
            var total = BuildTotalLine(supplier, debit, credit, balance, "ΣΥΝΟΛΑ ΠΡΟΗΓΟΥΜΕΝΗΣ ΠΕΡΙΟΔΟΥ");
            return total;
        }

        public List<BalanceSheetVM> BuildRequested(SupplierListVM supplier, IEnumerable<BalanceSheetVM> records, string fromDate) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            var requestedPeriod = new List<BalanceSheetVM> { };
            foreach (var record in records) {
                if (Convert.ToDateTime(record.Date) >= Convert.ToDateTime(fromDate)) {
                    requestedPeriod.Add(record);
                    debit += record.Debit;
                    credit += record.Credit;
                    balance += record.Debit - record.Credit;
                }
            }
            var total = BuildTotalLine(supplier, debit, credit, balance, "ΣΥΝΟΛΑ ΖΗΤΟΥΜΕΝΗΣ ΠΕΡΙΟΔΟΥ");
            requestedPeriod.Add(total);
            return requestedPeriod;
        }

        public BalanceSheetVM BuildTotal(SupplierListVM supplier, IEnumerable<BalanceSheetVM> records) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            foreach (var record in records) {
                debit += record.Debit;
                credit += record.Credit;
                balance += record.Debit - record.Credit;
            }
            var total = BuildTotalLine(supplier, debit, credit, balance, "ΓΕΝΙΚΑ ΣΥΝΟΛΑ");
            return total;
        }

        public List<BalanceSheetVM> MergePreviousRequestedAndTotal(BalanceSheetVM previousPeriod, List<BalanceSheetVM> requestedPeriod, BalanceSheetVM total) {
            var final = new List<BalanceSheetVM> {
                previousPeriod
            };
            foreach (var record in requestedPeriod) {
                final.Add(record);
            }
            final.Add(total);
            return final;
        }

        public BalanceSheetSummaryVM Summarize(SupplierListVM supplier, IEnumerable<BalanceSheetVM> records) {
            var previousBalance = records.First().Balance;
            var requestedDebit = records.SkipLast(1).Last().Debit;
            var requestedCredit = records.SkipLast(1).Last().Credit;
            var requestedBalance = records.SkipLast(1).Last().Balance;
            var actualBalance = previousBalance + requestedBalance;
            var summary = new BalanceSheetSummaryVM {
                Supplier = new SimpleEntity {
                    Id = supplier.Id,
                    Description = supplier.Description
                },
                PreviousBalance = previousBalance,
                Debit = requestedDebit,
                Credit = requestedCredit,
                Balance = requestedDebit - requestedCredit,
                ActualBalance = actualBalance
            };
            return summary;
        }

        public async Task<IEnumerable<BalanceSheetVM>> GetForBalanceAsync(int supplierId) {
            var records = await context.Transactions
                .AsNoTracking()
                .Include(x => x.Supplier)
                .Include(x => x.DocumentType)
                .Where(x => x.SupplierId == supplierId)
                .ToListAsync();
            return mapper.Map<IEnumerable<TransactionsBase>, IEnumerable<BalanceSheetVM>>(records);
        }

        private static BalanceSheetVM BuildTotalLine(SupplierListVM supplier, decimal debit, decimal credit, decimal balance, string label) {
            var total = new BalanceSheetVM {
                Date = "",
                Supplier = new SimpleEntity {
                    Id = supplier.Id,
                    Description = supplier.Description
                },
                Debit = debit,
                Credit = credit,
                Balance = balance
            };
            return total;
        }

    }

}