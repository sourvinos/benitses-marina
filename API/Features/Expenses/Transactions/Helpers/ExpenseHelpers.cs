namespace API.Features.Expenses.Transactions {

    public static class ExpenseHelpers {

        public static bool HasDocument(Expense Expense) {
            DirectoryInfo directoryInfo = new(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploaded Expenses"))));
            return directoryInfo.GetFiles(Expense.ExpenseId + "*.pdf").Length != 0;
        }

        public static string DocumentName(Expense Expense) {
            DirectoryInfo directoryInfo = new(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploaded Expenses"))));
            var document = directoryInfo.GetFiles(Expense.ExpenseId + "*.pdf").FirstOrDefault();
            return document != null ? document.Name : "";
        }

    }

}
