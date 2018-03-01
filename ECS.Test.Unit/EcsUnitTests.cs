using System;
using NUnit.Framework;


namespace ECS.Test.Unit
{
    [TestFixture]
    public class EcsUnitTests
    {
        private FakeTempSensor _fakeTempSensor;
        private FakeHeater _fakeHeater;
        private ECS _uut;
        private FakeWindow _fakeWindow;

        [SetUp]
        public void Setup()
        {
            _fakeHeater = new FakeHeater();
            _fakeTempSensor = new FakeTempSensor();
            _fakeWindow = new FakeWindow();
            _uut = new ECS(_fakeTempSensor, _fakeHeater, _fakeWindow, 25, 28);
        }

        #region Threshold tests

        [Test]
        public void Thresholds_ValidUpperTemperatureThresholdSet_NoExceptionsThrown()
        {
            Assert.That(() => { _uut.UpperTemperatureThreshold = 27; }, Throws.Nothing);
        }

        [Test]
        public void Thresholds_ValidLowerTemperatureThresholdSet_NoExceptionsThrown()
        {
            Assert.That(() => { _uut.LowerTemperatureThreshold = 26; }, Throws.Nothing);
        }

        [Test]
        public void Thresholds_UpperSetToLower_NoExceptionsThrown()
        {
            Assert.That(() => { _uut.UpperTemperatureThreshold = _uut.LowerTemperatureThreshold; }, Throws.Nothing);
        }

        [Test]
        public void Thresholds_LowerSetToUpper_NoExceptionsThrown()
        {
            Assert.That(() => { _uut.LowerTemperatureThreshold = _uut.UpperTemperatureThreshold; }, Throws.Nothing);
        }


        public void Thresholds_InvalidUpperTemperatureThresholdSet_ArgumentExceptionThrown()
        {
            Assert.That(() => { _uut.UpperTemperatureThreshold = 24; }, Throws.TypeOf<ArgumentException>());
        }

        public void Thresholds_InvalidLowerTemperatureThresholdSet_ArgumentExceptionThrown()
        {
            Assert.That(() => { _uut.LowerTemperatureThreshold = 29; }, Throws.TypeOf<ArgumentException>());
        }

        #endregion

        #region Regulation tests

        #region T < Tlow

        [Test]
        public void Regulate_TempIsLow_HeaterIsTurnedOn()
        {
            _fakeTempSensor.Temp = 20;
            _uut.Regulate();

            Assert.That(_fakeHeater.TurnOnCalledTimes, Is.EqualTo(1));
        }


        [Test]
        public void Regulate_TempIsLow_WindowIsClosed()
        {
            _fakeTempSensor.Temp = 20;
            _uut.Regulate();

            Assert.That(_fakeWindow.CloseCalledTimes, Is.EqualTo(1));
        }

        #endregion

        #region T == Tlow

        [Test]
        public void Regulate_TempIsAtLowerThreshold_HeaterIsTurnedOff()
        {
            _fakeTempSensor.Temp = 25;
            _uut.Regulate();

            Assert.That(_fakeHeater.TurnOffCalledTimes, Is.EqualTo(1));
        }

        [Test]
        public void Regulate_TempIsAtLowerThreshold_WindowIsClosed()
        {
            _fakeTempSensor.Temp = 25;
            _uut.Regulate();

            Assert.That(_fakeWindow.CloseCalledTimes, Is.EqualTo(1));
        }

        #endregion

        #region Tlow < T < Thigh

        [Test]
        public void Regulate_TempIsBetweenLowerAndUpperThresholds_HeaterIsTurnedOff()
        {
            _fakeTempSensor.Temp = 27;
            _uut.Regulate();

            Assert.That(_fakeHeater.TurnOnCalledTimes, Is.EqualTo(0));
        }

        [Test]
        public void Regulate_TempIsBetweenLowerAndUpperThresholds_WindowIsClosed()
        {
            _fakeTempSensor.Temp = 27;
            _uut.Regulate();

            Assert.That(_fakeWindow.CloseCalledTimes, Is.EqualTo(1));
        }

        #endregion

        #region T == Thigh

        [Test]
        public void Regulate_TempIsAtUpperThreshold_HeaterIsTurnedOff()
        {
            _fakeTempSensor.Temp = 27;
            _uut.Regulate();

            Assert.That(_fakeHeater.TurnOffCalledTimes, Is.EqualTo(1));
        }

        [Test]
        public void Regulate_TempIsAtUpperThreshold_WindowIsClosed()
        {
            _fakeTempSensor.Temp = 27;
            _uut.Regulate();

            Assert.That(_fakeWindow.CloseCalledTimes, Is.EqualTo(1));
        }

        #endregion

        #region T > Thigh

        [Test]
        public void Regulate_TempIsAboveUpperThreshold_HeaterIsTurnedOff()
        {
            _fakeTempSensor.Temp = 27;
            _uut.Regulate();

            Assert.That(_fakeHeater.TurnOffCalledTimes, Is.EqualTo(1));
        }

        [Test]
        public void Regulate_TempIsAboveUpperThreshold_WindowIsOpened()
        {
            _fakeTempSensor.Temp = 29;
            _uut.Regulate();

            Assert.That(_fakeWindow.OpenCalledTimes, Is.EqualTo(1));
        }

        #endregion

        #endregion
    }
}
