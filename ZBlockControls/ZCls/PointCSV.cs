using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZPointControls.ZCls
{
    public class PointCSV
    {
        Document document;

        readonly FileInfo fileInfo;

        public int TotalPoints { get { return ImportedPoints.Count; } }

        public List<string> ImportedPoints { get; private set; }

        public PointCSV(Document AcDoc, string fileName)
        {
            this.document = AcDoc;
            fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("file doesnt exists.");
            }
            ImportedPoints = new List<string>();
            ProgressMeter pm = new ProgressMeter();
            ImportPoint(pm);
        }

        void ImportPoint(ProgressMeter progressMeter)
        {
            ImportedPoints = new List<string>();
            progressMeter.SetLimit(PointLength());
            try
            {
                using (StreamReader reader = new StreamReader(this.fileInfo.FullName))
                {
                    int num = PointLength();
                    string strLine;
                    progressMeter.Start("Reading Points From File");
                    while ((strLine = reader.ReadLine()) != null)
                    {
                        ImportedPoints.Add(strLine);
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(25);
                        progressMeter.MeterProgress();
                    }
                }
            }
            catch (System.Exception ex)
            {
                this.document.Editor.WriteMessage("Error: {0}\n", ex.Message);
            }
            finally
            {
                progressMeter.Stop();
            }
        }

        int PointLength ()
        {
            return File.ReadAllLines(fileInfo.FullName).Length;
        }


    }
}
