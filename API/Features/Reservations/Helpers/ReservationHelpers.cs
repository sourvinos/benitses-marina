using System;

namespace API.Features.Reservations {

    public static class ReservationHelpers {

        public static bool IsOverdue(DateTime toDate) {
            var today = GetLocalDateTime();
            if (today > toDate) {
                return true;
            } else {
                return false;
            }
        }

        private static DateTime GetLocalDateTime() {
            var x = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "E. Europe Standard Time");
            return x;
        }

    }

}
