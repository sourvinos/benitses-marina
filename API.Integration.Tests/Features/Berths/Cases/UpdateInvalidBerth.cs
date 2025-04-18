using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Berths {

    public class UpdateInvalidBerth : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Berth_Must_Not_Be_Already_Updated();
        }

        private static object[] Berth_Must_Not_Be_Already_Updated() {
            return new object[] {
                new TestBerth {
                    StatusCode = 415,
                    Id = 1,
                    Description = Helpers.CreateRandomString(128),
                    PutAt = "2023-09-07 09:55:02"
                }
            };
        }

    }

}
