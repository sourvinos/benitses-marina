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
                    BoatTypeId = 1,
                    BoatName = "Test Boat" ,
                    Length = 14.2M,
                    FromDate = "2024-05-01",
                    ToDate = "2024-05-10",
                    Days = 9,
                    Email = "test-email@test-server.com",
                    Remarks = "test-remarks",
                    IsConfirmed = true,
                    IsDocked = false,
                    IsPaid = false
                }
            };
        }

    }

}
