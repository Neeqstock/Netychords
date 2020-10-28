using System;
using NeeqDMIs.ATmega;
using NeeqDMIs.Utils;
using System.Globalization;
using System.Windows;

namespace HANDMIsTestSuite.Behaviors.BreathSensor
{
    public class BSbreathSensor : ISensorReaderBehavior
    {
        int value = 0;

        private ValueNormalizerLong normalizer;

        public BSbreathSensor()
        {
            normalizer = new ValueNormalizerLong(GlobalValues.BarWidth);
        }

        public void ReceiveSensorRead(string val)
        {
            value = 0;

            try
            {
                value = int.Parse(val, CultureInfo.InvariantCulture.NumberFormat);
            }
            finally { }

            Rack.DMIBox.BreathSensorValue = normalizer.Normalize(value, Rack.DMIBox.BreathSensorMax);

            if (Rack.DMIBox.ControlMode == ControlModes.BreathIntensity && Rack.DMIBox.TestStarted)
            {
                Rack.DMIBox.XCursorValue = Rack.DMIBox.BreathSensorCalib;

                if (Rack.DMIBox.PollingMode == Modules.Polling.PollingMode.Stopwatch)
                {
                    Rack.DMIBox.PollingModuleStopwatch.ReceiveStopwatchStamp();
                }
            }
            
        }
    }
}
