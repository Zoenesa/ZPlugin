using Autodesk.AutoCAD.ApplicationServices;
using Acore = Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZPointControls.ZCls
{
    public static class AcModuls
    {
        static formPointImport form;
        
        public static void LoadFormImporter()
        {
            Application.DocumentManager.DocumentActivated += Document_Changed;
            Application.EnterModal += Show_Modal;
            if (form != null && !form.IsDisposed)
            {
                form.Activate();
                form.ShowDialog(Acore.Application.MainWindow as System.Windows.Forms.IWin32Window);
                form = null;
                return;
            }
            form = new formPointImport();
            Application.ShowModalDialog(Acore.Core.Application.MainWindow as System.Windows.Forms.IWin32Window, form, true);
        }

        static void Document_Changed(object sender, Autodesk.AutoCAD.ApplicationServices.DocumentCollectionEventArgs e)
        {
            
        }

        static void Show_Modal(object sender, EventArgs e)
        {
            
        }
    }
}
