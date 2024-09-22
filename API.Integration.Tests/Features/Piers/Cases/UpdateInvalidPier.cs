using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Piers {

    public class UpdateInvalidPier : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Pier_Must_Not_Be_Already_Updated();
        }

        private static object[] Pier_Must_Not_Be_Already_Updated() {
            return new object[] {
                new TestPier {
                    StatusCode = 415,
                    Id = 1,
                    Description = Helpers.CreateRandomString(128),
                    PutAt = "2023-09-07 09:55:02"
                }
            };
        }

    }

}
