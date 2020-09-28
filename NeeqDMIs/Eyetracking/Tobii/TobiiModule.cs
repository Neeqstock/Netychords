using NeeqDMIs.Eyetracking.Filters;
using NeeqDMIs.Eyetracking.Utils;
using System.Collections.Generic;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace NeeqDMIs.Eyetracking.Tobii
{
    public class TobiiModule
    {
        private TobiiBlinkProcessor blinkProcessor;
        private MouseEmulator mouseEmulator;
        public MouseEmulator MouseEmulator { get => mouseEmulator; }



        private GazePointData lastGazePointData;
        private EyePositionData lastEyePositionData;
        private HeadPoseData lastHeadPoseData;
        public GazePointData LastGazePointData { get => lastGazePointData; set => lastGazePointData = value; }
        public EyePositionData LastEyePositionData { get => lastEyePositionData; set => lastEyePositionData = value; }
        public HeadPoseData LastHeadPoseData { get => lastHeadPoseData; set => lastHeadPoseData = value; }

        #region Host and streams
        private GazePointDataStream gazePointDataStream;
        private EyePositionStream eyePositionStream;
        private HeadPoseStream headPoseStream;

        private Host tobiiHost;
        public Host TobiiHost { get => tobiiHost; set => tobiiHost = value; }
        #endregion

        #region Behaviors lists
        private List<ATobiiBlinkBehavior> blinkBehaviors = new List<ATobiiBlinkBehavior>();
        private List<ITobiiGazePointBehavior> gazePointBehaviors = new List<ITobiiGazePointBehavior>();
        private List<ITobiiEyePositionBehavior> eyePositionBehaviors = new List<ITobiiEyePositionBehavior>();
        private List<ITobiiHeadPoseBehavior> headPoseBehaviors = new List<ITobiiHeadPoseBehavior>();

        public List<ATobiiBlinkBehavior> BlinkBehaviors { get => blinkBehaviors; set => blinkBehaviors = value; }
        public List<ITobiiGazePointBehavior> GazePointBehaviors { get => gazePointBehaviors; set => gazePointBehaviors = value; }
        public List<ITobiiEyePositionBehavior> EyePositionBehaviors { get => eyePositionBehaviors; set => eyePositionBehaviors = value; }
        public List<ITobiiHeadPoseBehavior> HeadPoseBehaviors { get => headPoseBehaviors; set => headPoseBehaviors = value; }



        #endregion

        public TobiiModule(GazePointDataMode gazePointDataMode)
        {
            TobiiHost = new Host();

            gazePointDataStream = TobiiHost.Streams.CreateGazePointDataStream(gazePointDataMode);
            eyePositionStream = TobiiHost.Streams.CreateEyePositionStream(true);
            headPoseStream = TobiiHost.Streams.CreateHeadPoseStream(true);
            
            gazePointDataStream.Next += GazePointDataStreamNext;
            eyePositionStream.Next += EyePositionStreamNext;
            headPoseStream.Next += HeadPoseStreamNext;

            blinkProcessor = new TobiiBlinkProcessor(this);
            mouseEmulator = new MouseEmulator(new NoFilter());
        }

        private void HeadPoseStreamNext(object sender, StreamData<HeadPoseData> e)
        {
            LastHeadPoseData = e.Data;

            foreach (ITobiiHeadPoseBehavior behavior in HeadPoseBehaviors)
            {
                behavior.ReceiveHeadPoseData(e.Data);
            }
        }

        private void EyePositionStreamNext(object sender, StreamData<EyePositionData> e)
        {
            LastEyePositionData = e.Data;

            blinkProcessor.ReceiveEyePositionData(e.Data);
            foreach (ITobiiEyePositionBehavior behavior in eyePositionBehaviors)
            {
                behavior.ReceiveEyePositionData(e.Data);
            }
        }

        private void GazePointDataStreamNext(object sender, StreamData<GazePointData> e)
        {
            LastGazePointData = e.Data;

            MouseEmulator.ReceiveGazePointData(e.Data.X, e.Data.Y);
            foreach (ITobiiGazePointBehavior behavior in gazePointBehaviors)
            {
                behavior.ReceiveGazePoint(e.Data);
            }
        }

        public void Start()
        {
            //tobiiHost.InitializeWpfAgent();
        }

        public void Dispose()
        {
            TobiiHost.Dispose();
        }
    }
}