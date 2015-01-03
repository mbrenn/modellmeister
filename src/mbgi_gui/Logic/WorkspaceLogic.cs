using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbgi_gui.Logic
{
    public class WorkspaceLogic
    {
        public static string WorkspacePath
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                    "workspace");
            }
        }
    }
}
