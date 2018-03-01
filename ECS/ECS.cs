using System;

namespace ECS
{
    public class ECS
    {
        private readonly ITempSensor _tempSensor;
        private readonly IHeater _heater;
        private readonly IWindow _window;
        private int _lowerTemperatureThreshold;
        private int _upperTemperatureThreshold;

        public int LowerTemperatureThreshold
        {
            get { return _lowerTemperatureThreshold; }
            set
            {
                if (value <= _upperTemperatureThreshold)
                    _lowerTemperatureThreshold = value;
                else throw new ArgumentException("Lower threshold must be <= upper threshold");
            }
        }


        public int UpperTemperatureThreshold
        {
            get { return _upperTemperatureThreshold; }
            set
            {
                if(value >=_lowerTemperatureThreshold)
                    _upperTemperatureThreshold = value;
                else throw new ArgumentException("Upper threshold must be <= lower threshold");
            }
        }

        public ECS(ITempSensor tempSensor, IHeater heater, IWindow window, int lowerTemperatureThreshold, int upperTemperatureThreshold)
        {
            _tempSensor = tempSensor;
            _heater = heater;
            _window = window;

            UpperTemperatureThreshold = upperTemperatureThreshold;
            LowerTemperatureThreshold = lowerTemperatureThreshold;
            
        }


        public void Regulate()
        {
            var curTemp = _tempSensor.GetTemp();
            if (curTemp < LowerTemperatureThreshold)
            {
                _heater.TurnOn();
                _window.Close();
            }
            else if (curTemp >= LowerTemperatureThreshold && curTemp <= UpperTemperatureThreshold)
            {
                _heater.TurnOff();
                _window.Close();
            }
            else
            {
                _heater.TurnOff();
                _window.Open();
            }
        }

        
    }
}
