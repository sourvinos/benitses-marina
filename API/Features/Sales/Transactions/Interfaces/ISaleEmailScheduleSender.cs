namespace API.Features.Sales.Transactions {

    public interface ISaleEmailScheduleSender {

        void SendInvoicesToEmailScheduleAsync(object state);

    }

}