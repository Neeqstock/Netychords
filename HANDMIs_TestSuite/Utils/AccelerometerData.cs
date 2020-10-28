using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDMIs_TestSuite.Utils
{
    public class AccelerometerData
    {
        public int GyroBaseX { get; set; } = 0;
        public int GyroBaseY { get; set; } = 0;
        public int GyroBaseZ { get; set; } = 0;
        public int AccBaseX { get; set; } = 0;
        public int AccBaseY { get; set; } = 0;
        public int AccBaseZ { get; set; } = 0;
        public int GyroX { get; set; } = 0;
        public int GyroY { get; set; } = 0;
        public int GyroZ { get; set; } = 0;
        public int AccX { get; set; } = 0;
        public int AccY { get; set; } = 0;
        public int AccZ { get; set; } = 0;
        public int GyroCalibX { get => GyroX - GyroBaseX; }
        public int GyroCalibY { get => GyroY - GyroBaseY; }
        public int GyroCalibZ { get => GyroZ - GyroBaseZ; }
        public int AccCalibX { get => AccX - GyroBaseX; }
        public int AccCalibY { get => AccY - GyroBaseY; }
        public int AccCalibZ { get => AccZ - GyroBaseZ; }

        public void CalibrateGyroBase()
        {
            GyroBaseX = GyroX;
            GyroBaseY = GyroY;
            GyroBaseZ = GyroZ;
        }

        public void CalibrateAccBase()
        {
            AccBaseX = AccX;
            AccBaseY = AccY;
            AccBaseZ = AccZ;
        }
    }
}
