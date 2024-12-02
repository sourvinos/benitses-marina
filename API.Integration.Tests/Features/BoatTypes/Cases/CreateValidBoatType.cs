using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace BoatTypes {

    public class CreateValidBoatType : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestBoatType {
                    Description = Helpers.CreateRandomString(5)
                }
            };
        }

    }

}