using System.Collections;
using System.Collections.Generic;

namespace Reservations {

    public class CreateValidReservation : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestReservation {
                    PaymentStatusId = 1,
                    FromDate = "2024-05-01",
                    ToDate = "2024-05-10",
                    Remarks = "Test remarks",
                    FinancialRemarks = "Financial remarks",
                    IsDocked = false,
                    IsLongTerm = true,
                    IsAthenian = false,
                    Boat = new TestReservationBoat {
                        Name = "Boat name",
                        Loa = "12.5",
                        Beam = "7.8",
                        Draft = "2.2",
                        TypeId = 1,
                        UsageId = 1
                    },
                    Insurance = new TestReservationInsurance {
                        InsuranceCompany = "",
                        PolicyNo = "",
                        PolicyEnds = "2024-12-31"
                    },
                    Owner = new TestReservationOwner {
                        Name = "Owner",
                        Address = "Address",
                        TaxNo = "TaxNo",
                        TaxOffice = "TaxOffice",
                        PassportNo = "PassportNo",
                        Phones = "Phones",
                        Email = "Email"
                    },
                    Billing = new TestReservationBilling {
                        Name = "Owner",
                        Address = "Address",
                        TaxNo = "TaxNo",
                        TaxOffice = "TaxOffice",
                        PassportNo = "PassportNo",
                        Phones = "Phones",
                        Email = "Email"
                    },
                    Fee = new TestReservationFee {
                        NetAmount = 1234.56M,
                        VatPercent = 24M,
                        VatAmount = 296.29M,
                        GrossAmount = 1530.85M
                    },
                    Berths = new List<TestReservationBerth>() {
                        new() { Description = "A1" }
                    },
                }
            };
        }

    }

}
