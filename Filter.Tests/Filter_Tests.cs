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
            _filter = Filter.Instance;
            _filter.InitialiseData();
        }

        private Filter _filter;


        private static bool Tolerance(in double expected, in double actual, in double tolerance)
        {
            double difference = Math.Abs(expected - actual);
            return difference <= tolerance ? true : false;

        }

        [TestCaseSource(typeof(DataClass), nameof(DataClass.ConversionCases))]
        //[Test]
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
            return testMail.Range;
            //Assert.That(Tolerance(628.117226, testMail.Range, 0.00001), Is.True, $"Range calculated as: {testMail.Range}");
        }
        public class DataClass
        {
            public static IEnumerable<TestCaseData> ConversionCases
            {
                get
                {
                    yield return new TestCaseData(2, 10, 1).Returns(628.117226); //updated the update values so change
                    yield return new TestCaseData(0, 0, 0).Returns(0.000000);
                }
            }

        }

        [Test]
        public void TestNewMailInstance()
        {
            Mail testMail;
            _filter.Output(out testMail);
            Mail mail1 = testMail;

            _filter.Output(out testMail);
            Mail mail2 = testMail;

            Assert.AreNotEqual(mail1, mail2);
        }

    }

    [TestFixture]
    public class MatricesTest
    {

        [SetUp]
        public void Setup()
        {
            vector = new double[2];
        }

        private double[] vector;

        [TestCase(0, new double[] {0, 0}, new double[] { 0, 0 }, TestName = "ScalarMultiply0")]
        [TestCase(-1, new double[] { 0, 0 }, new double[] { 0, 0 }, TestName ="ScalarMultiplyNegative")] 
        public void TestMuliplyVectorScalar(double scalar, double[] vector, double[] expectedVector)
        {
            Assert.That(expectedVector, Is.EquivalentTo(Matrices.MultiplyVectorScalar(scalar, vector)));
        }


        //[Test]
        //public void TestAddVectors(
        //    [Values(-1, 0, 1)] double v1,
        //    [Range(-1, 1, 1)] double v2,
        //    [Values(-1, 0, 1)] double v3,
        //    [Range(-1, 0, 1)] double v4)
        //{
        //    double[] vector2 = new double[2];
        //    vector2[0] = v1;
        //    vector2[1] = v2;
        //    vector[0] = v3;
        //    vector[1] = v4;

        //    Matrices.AddVector(vector, vector2);
        //}
    }

}