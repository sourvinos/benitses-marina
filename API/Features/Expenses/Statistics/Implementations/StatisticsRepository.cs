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

namespace API.Features.Expenses.Statistics {

    public class StatisticsRepository : Repository<StatisticsRepository>, IStatisticsRepository {

        private readonly IMapper mapper;

        public StatisticsRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager, IMapper mapper) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<StatisticVM>> GetForStatisticsAsync(string fromDate, string toDate, int supplierId, int companyId) {
            var records = await context.Transactions
                .AsNoTracking()
                .Include(x => x.Supplier)
                .Include(x => x.DocumentType)
                .Where(x => x.Date <= Convert.ToDateTime(toDate)
                    && (x.Company.Id == companyId)
                    && (x.SupplierId == supplierId)
                    && (x.DocumentType.DiscriminatorId == 1))
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<TransactionsBase>, IEnumerable<StatisticVM>>(records);
        }

        public IEnumerable<StatisticVM> BuildBalanceForStatistics(IEnumerable<StatisticVM> records) {
            decimal balance = 0;
            foreach (var record in records) {
                balance = balance + record.Debit - record.Credit;
                record.Balance = balance;
            }
            return records;
        }

        public StatisticVM BuildPrevious(SupplierListVM supplier, IEnumerable<StatisticVM> records, string fromDate) {
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

        public List<StatisticVM> BuildRequested(SupplierListVM supplier, IEnumerable<StatisticVM> records, string fromDate) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            var requestedPeriod = new List<StatisticVM> { };
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

        public StatisticVM BuildTotal(SupplierListVM supplier, IEnumerable<StatisticVM> records) {
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

        public List<StatisticVM> MergePreviousRequestedAndTotal(StatisticVM previousPeriod, List<StatisticVM> requestedPeriod, StatisticVM total) {
            var final = new List<StatisticVM> {
                previousPeriod
            };
            foreach (var record in requestedPeriod) {
                final.Add(record);
            }
            final.Add(total);
            return final;
        }

        public StatisticsSummaryVM Summarize(SupplierListVM supplier, IEnumerable<StatisticVM> records) {
            var previousBalance = records.First().Balance;
            var requestedDebit = records.SkipLast(1).Last().Debit;
            var requestedCredit = records.SkipLast(1).Last().Credit;
            var requestedBalance = records.SkipLast(1).Last().Balance;
            var actualBalance = previousBalance + requestedBalance;
            var summary = new StatisticsSummaryVM {
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

        public async Task<IEnumerable<StatisticVM>> GetForBalanceAsync(int supplierId) {
            var records = await context.Transactions
                .AsNoTracking()
                .Include(x => x.Supplier)
                .Include(x => x.DocumentType)
                .Where(x => x.SupplierId == supplierId)
                .ToListAsync();
            return mapper.Map<IEnumerable<TransactionsBase>, IEnumerable<StatisticVM>>(records);
        }

        private static StatisticVM BuildTotalLine(SupplierListVM supplier, decimal debit, decimal credit, decimal balance, string label) {
            var total = new StatisticVM {
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