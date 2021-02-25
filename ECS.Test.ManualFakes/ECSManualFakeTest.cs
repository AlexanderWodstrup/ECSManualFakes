using NSubstitute;
using NSubstitute.Exceptions;
using NUnit.Framework;

namespace ECS.Test.ManualFakes
{
    [TestFixture]
    public class EcsManualFakeTest
    {
        private ECS _uut;
        private IHeater _heater;
        private ITempSensor _tempSensor;

        [SetUp]
        public void SetUp()
        {
            _heater = Substitute.For<IHeater>();
            _tempSensor = Substitute.For<ITempSensor>();

            _uut = new ECS(_tempSensor, _heater, 25);
        }

        #region TempLow
        [Test]
        public void Regulate_TempIsLow_HeaterIsTurnedOn()
        {
            // Setup stub with desired response
            _tempSensor.GetTemp().Returns(20);
            // Act
            _uut.Regulate();
            // Assert on the mock - was the heater called correctly
            _heater.Received(1).TurnOn();
        }

        #endregion

        #region TempHigh
        [Test]
        public void Regulate_TempIsAboveUpperThreshold_HeaterIsTurnedOff()
        {
            // Setup the stub with desired response
            _fakeTempSensor.Temp = 27;
            _uut.Regulate();

            // Assert on the mock - was the heater called correctly
            Assert.That(_fakeHeater.TurnOffCalledTimes, Is.EqualTo(1));
        }

        #endregion

        #region SelfTest
        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public void RunSelfTest_CombinationOfInput_CorrectOutput(
            bool tempResult, bool heaterResult, bool expectedResult)
        {
            _fakeTempSensor.SelfTestResult = tempResult;
            _fakeHeater.SelfTestResult = heaterResult;

            Assert.That(_uut.RunSelfTest(), Is.EqualTo(expectedResult));
        }

        #endregion


        [Test]
        public void RunSelfTest_TempSensorFails_SelfTestFails()
        {
            _tempSensor.RunSelfTest().Returns(false);
            _heater.RunSelfTest().Returns(true);
            Assert.IsFalse(_uut.RunSelfTest());
        }

        [Test]
        public void Regulate_TempBelowThreshold_HeaterTurnedOn()
        {
            _tempSensor.GetTemp().Returns(15);
            _uut.Regulate();
            _heater.Received(1).TurnOn();
            
        }

        //HEJ PALLE!
        //HEYO

    }
}