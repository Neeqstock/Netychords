using NeeqDMIs;
using NeeqDMIs.ATmega;
using NeeqDMIs.Keyboard;
using NeeqDMIs.Music;
using System.Windows.Controls;

namespace Netytar
{
    /// <summary>
    /// DMIBox for Netytar, implementing the internal logic of the instrument.
    /// </summary>
    public class NetychordsDMIBox : DMIBox
    {
        public Eyetracker Eyetracker { get; set; } = Eyetracker.Tobii;
        public KeyboardModuleWPF KeyboardModule;
        public MainWindow MainWindow { get; set; }
    }
}
