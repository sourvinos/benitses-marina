using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Berths {

    public class UpdateValidBerth : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestBerth {
                    Id = 1,
                    Description = Helpers.CreateRandomString(5),
                    PutAt = "2024-09-01 00:00:00"
                }
            };
        }

    }

}