using NUnit.Framework;
using Tracker.T;
using Tracker.Global;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework.Internal;
using System.Dynamic;
using System.Numerics;
using System.Threading.Tasks;
using System.Runtime.Intrinsics;

namespace Tracker.Tests
{

    [TestFixture]
    public class FilterTest
    {

        [SetUp]
        public void SetUp()
        {
            _filter = Filter.Instance;
            _filter.InitialiseData();
        }

        private Filter _filter;


        //private static bool Tolerance(in double expected, in double actual, in double tolerance)
        //{
        //    double difference = Math.Abs(expected - actual);
        //    return difference <= tolerance ? true : false;

        //}

        [TestCaseSource(typeof(DataClass), nameof(DataClass.ConversionCases))]
        [Category("Mathematical")]
        public double TestOutput(double time, double v1, double v2)
        {
            Mail testMail;
            //_filter.UpdatePath(Filter.Mode.update); //can i make this done for each test case????
            _filter.UpdateData(t: time, v1: v1, v2: v2);
            _filter.CallPositionUpdate(); 
            _filter.Output(out testMail);

            return testMail.Range;
        }
        public class DataClass
        {
            public static IEnumerable<TestCaseData> ConversionCases
            {
                get
                {
                    yield return new TestCaseData(2, 10, 1).Returns(40.199502).SetName("Conversion_positive_vectors"); //updated the update values so change
                    yield return new TestCaseData(0, 0, 0).Returns(0.000000).SetName("Conversion_0_values");
                }
            }

        }

        [Test]
        [Category("Instances")]
        public void TestNewMailInstance()
        {
            Mail mail1;
            Mail mail2;
            Mail testMail;
            _filter.Output(out testMail);
            mail1 = testMail;

            _filter.Output(out testMail);
            mail2 = testMail;

            Assert.AreNotSame(mail1, mail2);
        }

        [Test]
        [Category("Threading")]
        public async Task TestThreadSafety()
        {
            Mail testMail;
            await Task.Run(() => { _filter.UpdatePath(Filter.Mode.update); _filter.CallPositionUpdate(); });
            await Task.Run(() => { _filter.UpdatePath(Filter.Mode.update); _filter.CallPositionUpdate(); });


            _filter.Output(out testMail);
            Assert.That(testMail.Range, Is.EqualTo(44.721360));

        }

    }

    [TestFixture]
    public class MatricesTest
    {

        [SetUp]
        public void Setup(){}

        [Category("Mathematics")]
        [TestCase(0, new double[] {0, 0}, new double[] { 0, 0 }, TestName = "ScalarMultiply0")]
        [TestCase(-1, new double[] { 0, 0 }, new double[] { 0, 0 }, TestName ="ScalarMultiplyNegative")] 
        public void TestMuliplyVectorScalar(double scalar, double[] vector, double[] expectedVector)
        {
            Assert.That(expectedVector, Is.EquivalentTo(Matrices.MultiplyVectorScalar(scalar, vector)));
        }


        //[Test]
        //[Category("Mathematics")]
        //public void TestAddVectors(
        //    [Values(-1.0, 0.0, 1.0)] double[] v1,
        //    [Random(-10.0, 10.0, 5)] double[] v2)
        //{
        //    Matrices.AddVector(v1, v2);
        //}
    }

}