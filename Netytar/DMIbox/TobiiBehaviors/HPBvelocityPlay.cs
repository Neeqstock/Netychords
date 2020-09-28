using NeeqDMIs.Eyetracking.Filters;
using NeeqDMIs.Eyetracking.Tobii;
using NeeqDMIs.Mathematics;
using System;
using System.Drawing;
using System.Windows;
using Tobii.Interaction;

namespace Netytar.DMIbox.TobiiBehaviors
{
    public class HPBvelocityPlay : ITobiiHeadPoseBehavior
    {
        private int offThresh; // B value necessary for a note to be considered stopped
        private int onThresh; // B value necessary for a note to be considered played
        private float maxVelocity; // Biggest possible physical velocity value (how much head swing is taken into account)
        private float sensitivity; // Multiplies the differential value

        private HeadPoseData lastData;
        private Vector2 diffVector = new Vector2();
        private double vel = 0;
        private double dir = 0;

        private IFilter filter; // Filter to eliminate noise
        private System.Drawing.Point velPoint = new System.Drawing.Point();

        public double Niente
        {
            get { return vel; }
            set
            {
                if(value < 0) // Eliminates negative values (resulting from eyes not being tracked and other misc)
                {
                    vel = 0;
                }
                else if(value > maxVelocity) // Set 50 as a maximum value, corresponding to a swing at maxVelocity
                {
                    vel = 50;
                }
                else if(value == 0) { }
                else if (value < -50) { }
                else
                {
                    vel = value;
                }

            }
        }

        public HPBvelocityPlay(int offThresh, int onThresh, float sensitivity, float maxSwing, float filterSensitivity)
        {
            this.offThresh = offThresh;
            this.onThresh = onThresh;
            this.sensitivity = sensitivity;
            this.maxVelocity = maxSwing;
            filter = new ExpDecayingFilter(filterSensitivity);
        }



        public void ReceiveHeadPoseData(HeadPoseData data)
        {

            if (Rack.NetytarDMIBox.NetytarControlMode == NetytarControlModes.EyeVel)
            {
   
                if (data.HasHeadPosition)
                {
                    if (lastData == null)
                    {
                        lastData = data;
                    }
                    else
                    {
                        diffVector.X = ((data.HeadRotation.X - lastData.HeadRotation.X) * 5000);
                        diffVector.Y = ((data.HeadRotation.Y - lastData.HeadRotation.Y) * 3000);

                        double diffXq = Math.Pow(diffVector.X, 2);
                        double diffYq = Math.Pow(diffVector.Y, 2);

                        vel = Math.Sqrt(diffXq + diffYq);
                        dir = Math.Atan2(diffVector.Y, diffVector.X);

                        velPoint.X = (int)vel;

                        //filter.Push(velPoint);
                        //velPoint = filter.GetOutput();

                        int Bfinal = velPoint.X;

                        Rack.NetytarDMIBox.TestString = ("Vel: " + Bfinal.ToString() + "\n Dir: " + dir.ToString() + "\n X: " + data.HeadRotation.X + "\n Y: " + data.HeadRotation.Y);
                        Rack.NetytarDMIBox.NetytarMainWindow.BreathSensorValue = Bfinal;
                        Rack.NetytarDMIBox.Pressure = (int)(Bfinal * sensitivity);
                        // NetytarRack.DMIBox.Modulation = (int)(Bfinal / 16 * sensitivity);

                        if (Bfinal > onThresh && Rack.NetytarDMIBox.Blow == false)
                        {
                            Rack.NetytarDMIBox.Blow = true;
                            // NetytarRack.DMIBox.Pressure = 110;
                        }

                        if (Bfinal < offThresh)
                        {
                            Rack.NetytarDMIBox.Blow = false;
                        }

                        lastData = data;
                    }
                }
            }
        }
    }
}
