using System.Collections;
using System.Collections.Generic;

namespace Users {

    public class UpdateValidUserNotOwnRecord : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return AccountIsOwnedByAnotherUser();
        }

        private static object[] AccountIsOwnedByAnotherUser() {
            return new object[] {
                new TestUpdateUser {
                    StatusCode = 490,
                    Id = "e7e014fd-5608-4936-866e-ec11fc8c16da",
                    Username = "litourgis",
                    Displayname = "LITOURGIS TRAVEL",
                    Email = "litourgistravel@yahoo.gr",
                    IsAdmin = false,
                    IsActive = true
                }
            };
        }

    }

}
