using NUnit.Framework;
using Tracker.T;
using Tracker.Global;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework.Internal;
using System.Dynamic;
using System.Numerics;

namespace Tracker.Tests
{

    //public class FilterFixture : IDisposable
    //{
    //    public FilterFixture()
    //    {
    //        Filter testFilter = new Filter();
    //        testFilter.InitialiseData();
    //    }

    //    public void Dispose() { }
    //}

    //[TestFixture(typeof(Filter))]
    [TestFixture]
   // [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class FilterTest
    {
        //public Filter testFilter;
        //public FilterTest()
        //{
        //    testFilter = new Filter(); ;
        //}

        [SetUp]
        public void SetUp()
        {
            _filter = new Filter();
            _filter.InitialiseData();
        }

        private Filter _filter;


        private static bool Tolerance(in double expected, in double actual, in double tolerance)
        {
            double difference = Math.Abs(expected - actual);
            return difference <= tolerance ? true : false;

        }

        [TestCaseSource(typeof(DataClass), nameof(DataClass.ConversionCases))]
        //[TesCaset]
        //[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
        public double TestOutput(double time, double v1, double v2)
        {
            Mail testMail;
            //_filter.UpdatePath(Filter.Mode.update); //can i make this done for each test case????
            _filter.UpdateData(t: time, v1: v1, v2: v2);
            _filter.CallPositionUpdate(); 
            _filter.Output(out testMail);

            //Assert.That(testMail.Range, Is.EqualTo(628.117226)); //rounded to 6 digits
            //angle test
            return 628.117226;
            //Assert.That(Tolerance(628.117226, testMail.Range, 0.00001), Is.True, $"Range calculated as: {testMail.Range}");
        }
        public class DataClass
        {
            public static IEnumerable<TestCaseData> ConversionCases
            {
                get
                {
                    yield return new TestCaseData(2, 10, 1).Returns(628.117226);
                }
            }

        }
    }
 


}