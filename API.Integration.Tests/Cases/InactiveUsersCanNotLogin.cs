using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Cases {

    public class InactiveUsersCanNotLogin : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Inactive_Simple_Users_Can_Not_Login();
            yield return Inactive_Admins_Can_Not_Login();
        }

        private static object[] Inactive_Simple_Users_Can_Not_Login() {
            return new object[] {
                new Login {
                    Username = "michael",
                    Password = "A#ba439de-446e-4eef-8c4b-833f1b3e18aa",
                }
            };
        }

        private static object[] Inactive_Admins_Can_Not_Login() {
            return new object[] {
                new Login {
                    Username = "galini",
                    Password = "A#ba439de-446e-4eef-8c4b-833f1b3e18aa"
                }
            };
        }

    }

}
