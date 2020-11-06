using NeeqDMIs.ATmega;
using System;
using System.Globalization;

namespace Netytar.Behaviors.Sensor
{
    public class HSreadSerial : ISensorReaderBehavior
    {
        private string[] split;
        private double lastYaw;


        public void ReceiveSensorRead(string val)
        {
            if (val.Contains("%"))
            {
                //MessageBox.Show("HeadTracker sends write request");
                Rack.NetychordsDMIBox.HeadTrackerModule.Write("A");
            }
            else if (val.StartsWith("$"))
            {
                //MessageBox.Show("Head tracker sends this values string:" + val);
                val = val.Replace("$", string.Empty);
                split = val.Split('!');

                Rack.NetychordsDMIBox.HeadTrackerData.Yaw = double.Parse(split[0], CultureInfo.InvariantCulture);
                if (Rack.NetychordsDMIBox.calibrateStarted && !Rack.NetychordsDMIBox.calibrateEnded)
                {
                    Rack.NetychordsDMIBox.HeadTrackerData.CalibrationYaw = double.Parse(split[0], CultureInfo.InvariantCulture);
                }
                Rack.NetychordsDMIBox.HeadTrackerData.Pitch = double.Parse(split[1], CultureInfo.InvariantCulture);
                Rack.NetychordsDMIBox.HeadTrackerData.Roll = double.Parse(split[2], CultureInfo.InvariantCulture);

                if (Rack.NetychordsDMIBox.isCalibrated && Rack.NetychordsDMIBox.MainWindow.NetychordsStarted)
                {
                    if (Rack.NetychordsDMIBox.HeadTrackerData.Yaw <= Rack.NetychordsDMIBox.maxYaw && Rack.NetychordsDMIBox.HeadTrackerData.Yaw >= Rack.NetychordsDMIBox.minYaw)
                    {
                        Rack.NetychordsDMIBox.startStrum = Rack.NetychordsDMIBox.HeadTrackerData.Yaw;
                    }
                    else if (!Rack.NetychordsDMIBox.isStartedStrum)
                    {
                        if (Rack.NetychordsDMIBox.HeadTrackerData.Yaw < 0)
                        {
                            Rack.NetychordsDMIBox.dirStrum = NetychordsDMIBox.directionStrum.Left;
                            Rack.NetychordsDMIBox.startingTime = DateTime.Now;
                            Rack.NetychordsDMIBox.isStartedStrum = true;
                            Rack.NetychordsDMIBox.isEndedStrum = false;
                            lastYaw = Rack.NetychordsDMIBox.HeadTrackerData.Yaw;
                        }
                        else if (Rack.NetychordsDMIBox.HeadTrackerData.Yaw > 0)
                        {
                            Rack.NetychordsDMIBox.dirStrum = NetychordsDMIBox.directionStrum.Right;
                            Rack.NetychordsDMIBox.startingTime = DateTime.Now;
                            Rack.NetychordsDMIBox.isStartedStrum = true;
                            Rack.NetychordsDMIBox.isEndedStrum = false;
                            lastYaw = Rack.NetychordsDMIBox.HeadTrackerData.Yaw;
                        }
                    }
                    else if (!Rack.NetychordsDMIBox.isEndedStrum)
                    {
                        switch (Rack.NetychordsDMIBox.dirStrum)
                        {
                            case NetychordsDMIBox.directionStrum.Left:
                                if (Rack.NetychordsDMIBox.HeadTrackerData.Yaw > lastYaw)
                                {
                                    Rack.NetychordsDMIBox.endStrum = lastYaw;
                                    Rack.NetychordsDMIBox.endingTime = DateTime.Now;
                                    double distance = Math.Abs(Rack.NetychordsDMIBox.endStrum - Rack.NetychordsDMIBox.startStrum);
                                    TimeSpan time = Rack.NetychordsDMIBox.endingTime - Rack.NetychordsDMIBox.startingTime;
                                    int speed = (int)(distance / time.Seconds);
                                    int midiVelocity;
                                    if (speed < 1)
                                    {
                                        midiVelocity = 127 * speed;
                                    }
                                    else
                                    {
                                        midiVelocity = 127 * (speed + 90) / (speed + 100);
                                    }
                                    Rack.NetychordsDMIBox.isEndedStrum = true;
                                    Rack.NetychordsDMIBox.isStartedStrum = false;
                                    //Rack.NetychordsDMIBox.Velocity = 60;
                                    /*if (Rack.NetychordsDMIBox.lastChord != null && Rack.NetychordsDMIBox.lastChord != Rack.NetychordsDMIBox.Chord)
                                    {
                                        Rack.NetychordsDMIBox.StopChord(Rack.NetychordsDMIBox.lastChord);
                                    }*/
                                    Rack.NetychordsDMIBox.PlayChord(Rack.NetychordsDMIBox.Chord);
                                    
                                }
                                break;

                            case NetychordsDMIBox.directionStrum.Right:
                                if (Rack.NetychordsDMIBox.HeadTrackerData.Yaw < lastYaw)
                                {
                                    Rack.NetychordsDMIBox.endStrum = lastYaw;
                                    Rack.NetychordsDMIBox.endingTime = DateTime.Now;
                                    double distance = Math.Abs(Rack.NetychordsDMIBox.endStrum - Rack.NetychordsDMIBox.startStrum);
                                    TimeSpan time = Rack.NetychordsDMIBox.endingTime - Rack.NetychordsDMIBox.startingTime;
                                    int speed = (int)(distance / time.Seconds);
                                    int midiVelocity;
                                    if (speed < 1)
                                    {
                                        midiVelocity = 127 * speed;
                                    }
                                    else
                                    {
                                        midiVelocity = 127 * (speed + 90) / (speed + 100);
                                    }
                                    Rack.NetychordsDMIBox.isEndedStrum = true;
                                    Rack.NetychordsDMIBox.isStartedStrum = false;
                                    //Rack.NetychordsDMIBox.Velocity = 60;
                                    /*if (Rack.NetychordsDMIBox.lastChord != null && Rack.NetychordsDMIBox.lastChord != Rack.NetychordsDMIBox.Chord)
                                    {
                                        Rack.NetychordsDMIBox.StopChord(Rack.NetychordsDMIBox.lastChord);

                                    }*/
                                    Rack.NetychordsDMIBox.PlayChord(Rack.NetychordsDMIBox.Chord);

                                }
                                break;
                        }
                    }
                }
            }
            Rack.NetychordsDMIBox.Str_HeadTrackerRaw = Rack.NetychordsDMIBox.HeadTrackerData.Yaw + "\n" + Rack.NetychordsDMIBox.HeadTrackerData.Pitch + "\n" + Rack.NetychordsDMIBox.HeadTrackerData.Roll + "\n";
            Rack.NetychordsDMIBox.Str_HeadTrackerCalib = Rack.NetychordsDMIBox.HeadTrackerData.TranspYaw + "\n" + Rack.NetychordsDMIBox.HeadTrackerData.TranspPitch + "\n" + Rack.NetychordsDMIBox.HeadTrackerData.TranspRoll + "\n";
        }

    }
}