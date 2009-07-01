using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RogueSharper.BrowseToReflector
{
    class ReflectorFinder : IQueryReflectorLocation
    {
        public string GetReflectorLocation()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;

                dialog.Multiselect = false;

                dialog.Filter = "Reflector|Reflector.exe";
                dialog.AutoUpgradeEnabled = true;

                if (dialog.ShowDialog() == DialogResult.OK &&
                    dialog.SafeFileName.Equals("Reflector.exe",
                                               StringComparison.
                                                   InvariantCultureIgnoreCase))
                {

                    return dialog.FileName;
                }
                return null;
            }
        }
    }
}
