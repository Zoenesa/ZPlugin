using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using AcAP = Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.EditorInput;

namespace AcZPlugins
{
    public class AcCommands : IExtensionApplication
    {
        Document document;
        Editor editor;

        public void Initialize()
        {
            this.document = AcAP.Application.DocumentManager.MdiActiveDocument;
            this.editor = document.Editor;
            LoadAssembly();
            string[] strInit = new string[]
            {
                "\n",
                "================== ZPlugins Successfully Loaded ==================", "\n",
                "========= .NET Add-Ins for Handlings Blocks & Attributes =========", "\n",
                "===== Type: \"ZIBP\" to Invoke (Insert Block from Survey Data) =====", "\n",
                "================== ZPlugins.dll | Version 2.1.0 ==================", "\n",
                "===================== ©2018 ZnsZ™ www.znsz.id ====================", "\n"
            };
            editor.WriteMessage(string.Concat(strInit));
        }

        void LoadAssembly()
        {
            int i;
            Environment.SpecialFolder specialFolder = 
                (Environment.Is64BitOperatingSystem ? 
                Environment.SpecialFolder.ProgramFilesX86 : 
                Environment.SpecialFolder.ProgramFiles);

            string[] Files = new string[] {
                System.IO.Path.Combine(Environment.GetFolderPath(specialFolder), "AcZLib"),
                System.IO.Path.Combine(Environment.GetFolderPath(specialFolder), "AcZLib\\UI")};

            bool flag = true;

            string[] ArrayFiles = Files;

            for (i = 0; i < (int)ArrayFiles.Length; i++)
            {
                if (!System.IO.Directory.Exists(ArrayFiles[i]))
                {
                    flag = false;
                }
            }
            if (!flag)
            {
                this.editor.WriteMessage(string.Concat(new string[] {
                    "The required startup components were not found.",
                    Environment.NewLine, Environment.NewLine, "AcZPlugins", " may not run correctly." }), "Startup Error");
                return;
            }
            try
            {
                List<string> ListAssemblyName = new List<string>();
                ArrayFiles = Files;
                for (i = 0; i < (int)ArrayFiles.Length; i++)
                {
                    string[] files = System.IO.Directory.GetFiles(ArrayFiles[i], "*.dll", System.IO.SearchOption.TopDirectoryOnly);
                    for (int j = 0; j < (int)files.Length; j++)
                    {
                        string str = files[j];
                        ListAssemblyName.Add(str);
                    }
                }
                this.LoadAssemblyFromPath(ListAssemblyName);
            }
            catch (System.Exception exception1)
            {
                System.Exception exception = exception1;
                this.editor.WriteMessage(string.Format("An error occurred when loading {0} startup items: {1}", Environment.NewLine, Environment.NewLine, "{0} may not run correctly."));
            }
        }

        private void LoadAssemblyFromPath(List<string> Files)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string list0 in Files)
            {
                try
                {
                    System.Reflection.Assembly.LoadFrom(list0);
                }
                catch (System.Exception exception1)
                {
                    System.Exception exception = exception1;
                    stringBuilder.AppendLine(string.Concat("        ", list0, " - ", exception.Message));
                }
            }
            if (stringBuilder.Length > 0)
            {
                this.editor.WriteMessage(string.Concat("Errors occurred when loading required assemblies: \r\n", stringBuilder.ToString()));
            }
        }

        public void Terminate()
        {
        }

        [CommandMethod("ZBLOCK IMPORTER", "ZIBP", CommandFlags.Modal | CommandFlags.Session)]
        public void ExecuteCommand()
        {
            ZPointControls.ZCls.AcModuls.LoadFormImporter();
        }

        [CommandMethod("ZBLOCK COLLECTIONS", "ZBLK", CommandFlags.Modal | CommandFlags.Session)]
        public void ExecuteZblk()
        {
            ZBlocks.AcModuls.LoadFormBlocks();
        }
    }
}
