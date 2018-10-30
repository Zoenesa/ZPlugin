using AcAp = Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ZTools;
using ZPointControls.ZCls;
using Telerik.WinControls.UI;

namespace ZPointControls
{
    public partial class formPointImport : ZControl.AcZForm
    {
        OpenFileDialog ofd = null;

        PointCSV ImportPts;

        AcAp.Document document;

        List<PNEZHelper> PointsCollections;

        public formPointImport()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(
                    Autodesk.AutoCAD.Runtime.SystemObjects.DynamicLinker.ProductLcid);

            ofd = radBrowseEditor.Dialog as OpenFileDialog;
            ofd.Filter = "CSV (Comma Separated File Contains Points Data)|*.csv| PNEZ (Point, Northing, Easting, Elevation Files)|*.PNEZ";
            ofd.Title = "Select CSV File";
            this.radLabelElement2.Text = "";
            document = AcAp.Application.DocumentManager.MdiActiveDocument;
            AcAp.Application.DocumentManager.DocumentActivated += document_Changed;
            this.radButtonImportPointIntoCurrentDrawing.Enabled = false;
        }

        void document_Changed(object sender, AcAp.DocumentCollectionEventArgs e)
        {
            string str = "Import Survey Point: ";
            if (e.Document == null)
            {
                this.Text = str;
                return;
            }
            this.document = e.Document;
        }

        private void radBrowseEditor_DialogClosed(object sender, Telerik.WinControls.UI.DialogClosedEventArgs e)
        {
            if (e.DialogResult == DialogResult.Cancel)
            {
                return;
            }
            try
            {
                datagrid.Rows.Clear();
                PointsCollections = new List<PNEZHelper>();
                //setting ddpt dari file blok

                ImportPts = new PointCSV(this.document, radBrowseEditor.Value);
                System.Windows.Forms.Application.UseWaitCursor = true;
                this.radButtonImportPointIntoCurrentDrawing.Enabled = false;
                Autodesk.AutoCAD.Runtime.ProgressMeter pm = new Autodesk.AutoCAD.Runtime.ProgressMeter();
                this.radLabelStatus.Text = "Please wait...";
                InsertPointToDataGrid(pm, ImportPts, this.datagrid);
                System.Windows.Forms.Application.UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                this.document.Editor.WriteMessage(ex.Message + "\n");
            }
            finally
            {
                this.radButtonImportPointIntoCurrentDrawing.Enabled = true;

                this.radLabelStatus.Text = string.Format("Survey Points Imported: {0}", PointsCollections.Count);
                
                this.radLabelElement2.Text = string.Format("Total Points {2}Valid: {0} | Invalid: {1}{3} ", iPointOk, iPointErr, '{', '}');
            }
        }

        int iPointOk = 0; int iPointErr = 0;

        void InsertPointToDataGrid(Autodesk.AutoCAD.Runtime.ProgressMeter progress, PointCSV Points, RadGridView grid)
        {
            iPointOk = 0; iPointErr = 0;
            progress.SetLimit(Points.TotalPoints);
            try
            {
                double x = 0.0;
                double y = 0.0;
                double z = 0.0;
                object[] arr = new object[6];
                string rmsg = "";

                progress.Start("Loading Points");
                radButtonImportPointIntoCurrentDrawing.Enabled = false;
                foreach (string _value in Points.ImportedPoints)
                {
                    object[] obj = _value.PointRow();
                    try
                    {
                        arr[0] = obj[0];

                        object[] xyz = new object[] { obj[2], obj[1], obj[3] };
                        double[] ArrXYZ = new double[3];

                        xyz.ParsePointFromCSV(System.Globalization.NumberStyles.AllowDecimalPoint |
                            System.Globalization.NumberStyles.AllowThousands |
                            System.Globalization.NumberStyles.Float, Thread.CurrentThread.CurrentCulture, out ArrXYZ, out rmsg);

                        x = ArrXYZ[0];
                        y = ArrXYZ[1];
                        z = ArrXYZ[2];

                        arr[1] = y;
                        arr[2] = x;
                        arr[3] = z;
                        arr[4] = (string)obj[4];
                        arr[5] = rmsg;

                        if (rmsg.Contains("Valid."))
                        {
                            goto LabelOk;
                        }

                        if (rmsg.Contains("Invalid "))
                        {
                            arr[5] = rmsg;
                            goto LabelErr;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        arr[5] = ex.Message;
                        grid.Rows.Add(arr);
                        goto LabelErr;
                    }

                    LabelOk:
                    iPointOk++;
                    System.Windows.Forms.Application.DoEvents();
                    progress.MeterProgress();
                    grid.Rows.Add(arr);
                    this.PointsCollections.Add(new PNEZHelper(arr[0].ToString(), y, x, z, arr[4].ToString()));
                    continue;

                    LabelErr:
                    iPointErr++;
                    System.Windows.Forms.Application.DoEvents();
                    progress.MeterProgress();
                    grid.Rows.Add(arr);
                }
            }
            catch (Exception ex)
            {
                this.document.Editor.WriteMessage(ex.Message + "\n");
            }
            finally
            {
                progress.Stop();
                radButtonImportPointIntoCurrentDrawing.Enabled = true;
            }
        }

        private void radButtonImportPointIntoCurrentDrawing_Click(object sender, EventArgs e)
        {
            this.radButtonImportPointIntoCurrentDrawing.Enabled = false;
            BlockImporter.CreateBlockintoCurrentDrawing(this.document, "AT-POINT", this.PointsCollections);
            this.radButtonImportPointIntoCurrentDrawing.Enabled = true;
        }

        private void datagrid_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (e.RowElement.RowInfo.Cells[5].Value.ToString() != "Valid.")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.BackColor = Color.Red;
                //e.RowElement.RowInfo.ViewTemplate.MasterTemplate.SelectionMode = GridViewSelectionMode.FullRowSelect;
            }
            else
            {
                if (e.RowElement.BackColor == Color.Red)
                {
                    e.RowElement.ResetValue(Telerik.WinControls.VisualElement.BackColorProperty);
                    e.RowElement.ResetValue(LightVisualElement.DrawFillProperty);
                    //e.RowElement.ResetValue(LightVisualElement.SetPropertyValueCommand);
                }
            }
        }

        private void formPointImport_Activated(object sender, EventArgs e)
        {
            this.Text = string.Format("Import Survey Points - {0}{1}{2}", "{", System.IO.Path.GetFileName(this.document.Name), "}");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {

                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
