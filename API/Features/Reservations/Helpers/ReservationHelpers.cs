using System;

namespace API.Features.Reservations {

    public static class ReservationHelpers {

        public static bool IsOverdue(DateTime toDate) {
            return (toDate.AddDays(1) < TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "E. Europe Standard Time"));
        }

    }

}
