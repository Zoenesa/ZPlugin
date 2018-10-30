using Autodesk.AutoCAD.ApplicationServices;
using Acore = Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBlocks
{

    public static class AcModuls
    {
        static ZBlocks.formBlocksCollections formBlocks = null;

        public static void LoadFormBlocks()
        { 
            formBlocks = new formBlocksCollections();
           System.Windows.Forms.DialogResult result = 
                Application.ShowModalDialog(Acore.Core.Application.MainWindow as System.Windows.Forms.IWin32Window, formBlocks, true);
            formBlocks.Dispose();
            formBlocks = null;
        }

    }
}
