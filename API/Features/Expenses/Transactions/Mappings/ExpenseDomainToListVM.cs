using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using Microsoft.CodeAnalysis.CSharp;

namespace API.Features.Expenses.Transactions {

    public static class ReservationDomainToListVM {

        public static List<ExpenseListVM> Read(List<ExpenseListVM> expenses) {
            var x = new List<ExpenseListVM>();
            foreach (var expense in expenses) {
                var z = new ExpenseListVM {
                    ExpenseId = expense.ExpenseId,
                    Date = expense.Date,
                    Company = new SimpleEntity {
                        Id = expense.Company.Id,
                        Description = expense.Company.Description
                    },
                    DocumentType = new SimpleEntity {
                        Id = expense.DocumentType.Id,
                        Description = expense.DocumentType.Description
                    },
                    PaymentMethod = new SimpleEntity {
                        Id = expense.PaymentMethod.Id,
                        Description = expense.PaymentMethod.Description
                    },
                    Supplier = new SimpleEntity {
                        Id = expense.Supplier.Id,
                        Description = expense.Supplier.Description
                    },
                    PutAt = expense.PutAt[..10],
                };
                x.Add(z);
            }
            return x;
        }

    }

}