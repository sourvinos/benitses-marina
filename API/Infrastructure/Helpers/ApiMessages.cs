namespace API.Infrastructure.Helpers {

    public enum Icons {
        Success,
        Info,
        Warning,
        Error
    }

    public static class ApiMessages {

        #region Generic Messages

        public static string OK() { return "OK"; }
        public static string RecordInUse() { return "Record is used and can't be deleted"; }
        public static string AuthenticationFailed() { return "Authentication failed."; }
        public static string RecordNotFound() { return "Record not found"; }
        public static string UnknownError() { return "Something bad has happened."; }
        public static string EmailNotSent() { return "Email not sent."; }
        public static string NotUniqueUsernameOrEmail() { return "The username or the email are not unique"; }
        public static string ConcurrencyError() { return "Another user has already updated this record."; }

        #endregion

        #region Specific Messages

        public static string VatNumberIsDuplicate() { return "The Tax Number already exists."; }
        public static string CustomerDataIsInvalid() { return "Customer data is invalid."; }
        public static string InvoiceAlreadyExists() { return "Invoice already exists."; }
        public static string InvoiceAutoNumberingHasErrors() { return "Invoice auto-numbering has errors."; }

        #endregion

    }

}