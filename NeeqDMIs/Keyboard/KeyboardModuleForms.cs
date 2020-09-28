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
    public sealed class KeyboardModuleForms
    {
        private RawFormsInput _rawinput;

        public KeyboardModuleForms(Form form)
        {
            _rawinput = new RawFormsInput(form.Handle, RawInputCaptureMode.Foreground);

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
