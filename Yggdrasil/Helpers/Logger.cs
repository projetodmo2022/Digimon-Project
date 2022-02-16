using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;

namespace Yggdrasil
{
    public class Logger : TextWriter
    {
        private TextBox m_output;
        public Logger(TextBox textbox)
        {
            m_output = textbox;
            Console.SetOut(this);
        }

        public override void Write(char value)
        {
            base.Write(value);
            m_output.Dispatcher.BeginInvoke(new Action(() =>
            {
                m_output.AppendText(value.ToString());
            }
            ));
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
