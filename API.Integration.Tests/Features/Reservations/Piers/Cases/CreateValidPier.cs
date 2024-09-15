using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Piers {

    public class CreateValidPier : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestPier {
                    Description = Helpers.CreateRandomString(128)
                }
            };
        }

    }

}