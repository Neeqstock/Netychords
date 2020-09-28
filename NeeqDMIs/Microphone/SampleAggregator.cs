using System.Collections.Generic;

namespace NeeqDMIs.Microphone
{
    internal class SampleAggregator
    {
        private List<AggregatorListener> listeners = new List<AggregatorListener>();

        private int size;
        private int index = 0;
        private float max;
        private float min;
        private float total;

        private float avg;

        public SampleAggregator(AggregatorListener firstListener, int size)
        {
            listeners.Add(firstListener);
            this.size = size;
        }

        public void push(float sample)
        {

            if(index == 0)
            {
                min = sample;
                max = sample;
                total = 0;
            }

            if(sample > max)
            {
                max = sample;
            }

            if(sample < min)
            {
                min = sample;
            }

            index++;
            total += sample;

            if (index == size - 1)
            {
                notifyListeners();
                index = 0;
            }

        }

        private void notifyListeners()
        {
            foreach(AggregatorListener listener in listeners)
            {
                avg = total / size;
                listener.aggregatorReached(min, max, avg);
            }
        }
        
         
    }
}
