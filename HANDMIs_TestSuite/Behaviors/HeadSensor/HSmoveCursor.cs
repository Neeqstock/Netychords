using HANDMIsTestSuite.Modules.Polling;
using NeeqDMIs.ATmega;
using NeeqDMIs.Eyetracking.Filters;
using NeeqDMIs.Utils;
using System;
using System.Globalization;
using Tobii.Interaction;

namespace HANDMIsTestSuite.Behaviors.HeadSensor
{
    public class HSmoveCursor : ISensorReaderBehavior
    {
        private string[] split;

        private ValueNormalizerLong yawNormalizer;
        private ValueNormalizerLong pitchNormalizer;
        private ValueNormalizerLong rollNormalizer;
        private ValueNormalizerLong velocityNormalizer;

        //
        private Vector3 diffVector;

        private double lastYaw = 0;
        private double lastPitch = 0;
        private double lastRoll = 0;

        private double vel = 0;
        private int Bfinal = 0;

        private double diffXq = 0;
        private double diffYq = 0;
        private double diffZq = 0;

        private IFilter filter = new ExpDecayingFilter(0.1f); // Filter to eliminate noise
        private System.Drawing.Point velPoint = new System.Drawing.Point();
        //

        public HSmoveCursor()
        {
            yawNormalizer = new ValueNormalizerLong();
            yawNormalizer.ChannelMax = GlobalValues.HeadYawRange * 200;
            yawNormalizer.ParamMax = GlobalValues.BarWidth;

            pitchNormalizer = new ValueNormalizerLong();
            pitchNormalizer.ChannelMax = GlobalValues.HeadPitchRange * 200;
            pitchNormalizer.ParamMax = GlobalValues.BarWidth;

            rollNormalizer = new ValueNormalizerLong();
            rollNormalizer.ChannelMax = GlobalValues.HeadRollRange * 200;
            rollNormalizer.ParamMax = GlobalValues.BarWidth;

            velocityNormalizer = new ValueNormalizerLong();
            velocityNormalizer.ChannelMax = GlobalValues.VelocityRange;
            velocityNormalizer.ParamMax = GlobalValues.BarWidth;
        }

        public void RecordLasts()
        {
            lastPitch = Rack.DMIBox.HeadTrackerData.Pitch;
            lastYaw = Rack.DMIBox.HeadTrackerData.Yaw;
            lastRoll = Rack.DMIBox.HeadTrackerData.Roll;
        }

        public void ReceiveSensorRead(string val)
        {
            RecordLasts();

            //MessageBox.Show("Ommioddio");
            if (val.Contains("%"))
            {
                //MessageBox.Show("HeadTracker sends write request");
                Rack.DMIBox.HeadTrackerModule.Write("R");
            }
            if (val.StartsWith("$"))
            {
                //MessageBox.Show("Head tracker sends this values string:" + val);
                val = val.Replace("$", string.Empty);
                split = val.Split('!');

                Rack.DMIBox.HeadTrackerData.Yaw = -double.Parse(split[0], CultureInfo.InvariantCulture);
                if (!Rack.DMIBox.InvertPitchRoll)
                {
                    Rack.DMIBox.HeadTrackerData.Pitch = double.Parse(split[1], CultureInfo.InvariantCulture) * Rack.DMIBox.PitchInverter;
                    Rack.DMIBox.HeadTrackerData.Roll = double.Parse(split[2], CultureInfo.InvariantCulture) * Rack.DMIBox.RollInverter;
                }
                else
                {
                    Rack.DMIBox.HeadTrackerData.Pitch = double.Parse(split[2], CultureInfo.InvariantCulture) * Rack.DMIBox.PitchInverter;
                    Rack.DMIBox.HeadTrackerData.Roll = double.Parse(split[1], CultureInfo.InvariantCulture) * Rack.DMIBox.RollInverter;
                }
                
                //CalculateVelocity();
            }

            Rack.DMIBox.Str_HeadTrackerRaw = "Yaw: " + Rack.DMIBox.HeadTrackerData.Yaw + "\nPitch: " + Rack.DMIBox.HeadTrackerData.Pitch + "\nRoll: " + Rack.DMIBox.HeadTrackerData.Roll; ;
            Rack.DMIBox.Str_HeadTrackerCalib = "Yaw: " + Rack.DMIBox.HeadTrackerData.TranspYaw + "\nPitch: " + Rack.DMIBox.HeadTrackerData.TranspPitch + "\nRoll: " + Rack.DMIBox.HeadTrackerData.TranspRoll;

            if (Rack.DMIBox.TestStarted)
            {
                switch (Rack.DMIBox.ControlMode)
                {
                    case ControlModes.HeadYaw:
                        Rack.DMIBox.XCursorValue = yawNormalizer.Normalize((long)((Rack.DMIBox.HeadTrackerData.TranspYaw + GlobalValues.HeadYawRange) * 100));
                        Stamp();
                        break;
                    case ControlModes.HeadPitch:
                        Rack.DMIBox.XCursorValue = pitchNormalizer.Normalize((long)((Rack.DMIBox.HeadTrackerData.TranspPitch + GlobalValues.HeadPitchRange) * 100));
                        Stamp();
                        break;
                    case ControlModes.HeadRoll:
                        Rack.DMIBox.XCursorValue = rollNormalizer.Normalize((long)((Rack.DMIBox.HeadTrackerData.TranspRoll + GlobalValues.HeadRollRange) * 100));
                        Stamp();
                        break;
                    case ControlModes.HeadVelocity:
                        Rack.DMIBox.XCursorValue = velocityNormalizer.Normalize((long)Rack.DMIBox.HeadTrackerData.Velocity);
                        Stamp();
                        break;
                }

            }


        }

        private static void Stamp()
        {
            if (Rack.DMIBox.PollingMode == PollingMode.Stopwatch)
            {
                Rack.DMIBox.PollingModuleStopwatch.ReceiveStopwatchStamp();
            }
        }

        private void CalculateVelocity()
        {
            diffVector.X = ((Rack.DMIBox.HeadTrackerData.Yaw - lastYaw) * 4000);
            diffVector.Y = ((Rack.DMIBox.HeadTrackerData.Pitch - lastPitch) * 8000);
            //diffVector.Z = ((Rack.DMIBox.HeadTrackerData.Roll - lastRoll) * 1000);

            diffXq = Math.Pow(diffVector.X, 2);
            diffYq = Math.Pow(diffVector.Y, 2);
            //diffZq = Math.Pow(diffVector.Z, 2);

            vel = Math.Sqrt(diffXq + diffYq); //Math.Sqrt(diffXq + diffYq + diffZq);
            //dir = Math.Atan2(diffVector.Y, diffVector.X);

            velPoint.X = (int)vel;

            filter.Push(velPoint);
            velPoint = filter.GetOutput();
            Bfinal = velPoint.X;

            Rack.DMIBox.HeadTrackerData.Velocity = Bfinal;
        }
    }
}
