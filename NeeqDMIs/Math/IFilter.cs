using System.Drawing;

namespace NeeqDMIs.Mathematics
{
    /// <summary>
    /// An interface for a generic black-box point based filter.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Inserts a new point in the filter's array.
        /// </summary>
        void Push(double number);

        /// <summary>
        /// Returns the output value from the filter.
        /// </summary>
        /// <returns></returns>
        double GetOutput();
    }
}
