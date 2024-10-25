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
                    BoatName = "Boat name",
                    Loa = "14.2",
                    FromDate = "2024-05-01",
                    ToDate = "2024-05-10",
                    Days = 9,
                    Email = "email@server.com",
                    Contact = "Test contact",
                    Remarks = "Test remarks",
                    FinancialRemarks = "Financial remarks",
                    IsDocked = false,
                    IsLongTerm = true,
                    IsAthenian = false,
                    ReservationLease = new TestReservationLease {
                        Customer = "Test customer",
                        InsuranceCompany = "AXA",
                        PolicyNo = "1234-56",
                        PolicyEnds = "2050-12-31"
                    },
                    Berths = new List<TestReservationBerth>() {
                        new() { Description = "A1" }
                    },
                }
            };
        }

    }

}
