using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RogueSharper.BrowseToReflector
{
    class ReflectorRunner
    {
        private readonly Process _process;

        public ReflectorRunner(string configFile, string location)
        {
            ProcessStartInfo psi = new ProcessStartInfo(location)
            {
                Arguments = string.Format("\"/configuration:{0}\"", configFile)
            };

            this._process = new Process {StartInfo = psi};
        }

        public void Run()
        {
            this._process.Start();
        }

        public void WaitForIdle()
        {
            this._process.WaitForInputIdle();
        }
    }
}
