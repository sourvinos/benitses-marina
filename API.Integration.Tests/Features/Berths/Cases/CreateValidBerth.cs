using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Berths {

    public class CreateValidBerth : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestBerth {
                    Description = Helpers.CreateRandomString(5)
                }
            };
        }

    }

}