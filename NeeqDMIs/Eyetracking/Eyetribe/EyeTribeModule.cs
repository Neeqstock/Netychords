using EyeTribe.ClientSdk;
using EyeTribe.ClientSdk.Data;
using NeeqDMIs.Eyetracking.Filters;
using NeeqDMIs.Eyetracking.Utils;
using System.Collections.Generic;

namespace NeeqDMIs.Eyetracking.Eyetribe
{
    public class EyeTribeModule : IGazeListener
    {
        private List<IEyeTribeGazePointBehavior> gazePointBehaviors = new List<IEyeTribeGazePointBehavior>();

        public EyeTribeModule()
        {
            MouseEmulator = new MouseEmulator(new NoFilter());
        }
        public void Start()
        {
            // Connect client
            GazeManager.Instance.Activate(GazeManagerCore.ApiVersion.VERSION_1_0);

            // Register this class for events
            GazeManager.Instance.AddGazeListener(this);
        }

        public GazeMode MouseEmulatorGazeMode { get; set; } = GazeMode.Raw;

        public List<IEyeTribeGazePointBehavior> GazePointBehaviors { get => gazePointBehaviors; set => gazePointBehaviors = value; }

        public MouseEmulator MouseEmulator { get; set; }

        public void OnGazeUpdate(GazeData gazeData)
        {
            foreach (IEyeTribeGazePointBehavior behavior in gazePointBehaviors)
            {
                behavior.ReceiveGazePoint(gazeData);
            }

            if(MouseEmulator != null)
            {
                switch (MouseEmulatorGazeMode)
                {
                    case GazeMode.Raw:
                        MouseEmulator.ReceiveGazePointData(gazeData.RawCoordinates.X, gazeData.RawCoordinates.Y);
                        break;
                    case GazeMode.Smooth:
                        MouseEmulator.ReceiveGazePointData(gazeData.SmoothedCoordinates.X, gazeData.SmoothedCoordinates.Y);
                        break;

                }

            }

        }
    }
    public enum GazeMode
    {
        Raw,
        Smooth
    }
}
