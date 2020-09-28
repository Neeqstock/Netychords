using RawInputProcessor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace NeeqDMIs.Keyboard
{
    public sealed class KeyboardModuleWPF
    {
        private RawPresentationInput _rawinput;

        public KeyboardModuleWPF(IntPtr parentHandle)
        {
            _rawinput = new RawPresentationInput(HwndSource.FromHwnd(parentHandle), RawInputCaptureMode.Foreground);

            _rawinput.AddMessageFilter();

            _rawinput.KeyPressed += OnKeyPressed;
        }

        /// <summary>
        /// Contains all the behavior modules set.
        /// </summary>
        public List<AKeyboardBehavior> KeyboardBehaviors { get; set; } = new List<AKeyboardBehavior>();

        private void OnKeyPressed(object sender, RawInputEventArgs e)
        {
            foreach (AKeyboardBehavior behavior in KeyboardBehaviors)
            {
                behavior.ReceiveEvent(e);
            }
        }

    }

}
