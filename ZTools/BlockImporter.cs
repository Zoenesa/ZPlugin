using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTools
{
    public static class BlockImporter
    {
        public static void CreateBlockintoCurrentDrawing(Document doc, string BlockName, List<PNEZHelper> Kordinat)
        {
            InsertATPoint(doc, BlockName, Kordinat);
        }

        static void ExtractBlock()
        {
            System.Reflection.Assembly asmZ = System.Reflection.Assembly.GetExecutingAssembly();
            List<string[]> AsmNames = new List<string[]>();
            AsmNames.Add(asmZ.GetManifestResourceNames());
            string DirPath = System.IO.Path.GetDirectoryName(asmZ.Location);
            System.IO.FileInfo fi = new System.IO.FileInfo(System.IO.Path.Combine(DirPath, "BLOCKS", "ATPOINT", "AT-POINT.zblk"));
            if (!fi.Exists)
            {
                string AsmName = asmZ.FullName;
                using (System.IO.Stream stream = asmZ.GetManifestResourceStream("AcZTools.BLOCKS.ATPOINT.AT-POINT.dwg"))
                {
                    byte[] fbyte = new byte[stream.Length];
                    stream.Read(fbyte, 0, (int)stream.Length);
                    IntPtr ptr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fbyte.Length);
                    string path = System.IO.Path.Combine(DirPath, "BLOCKS", "ATPOINT", "AT-POINT.zblk");
                    System.IO.File.WriteAllBytes(path, fbyte);
                }
                fi = new System.IO.FileInfo(System.IO.Path.Combine(DirPath, "BLOCKS", "ATPOINT", "AT-POINT.zblk"));
            }
            BlockExtensions.InsertBlock(Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument, fi.FullName, "AT-POINT");
        }

        public static void InsertATPoint(Document acDoc, string BlockName, List<PNEZHelper> Kordinat)
        {
            Autodesk.AutoCAD.Runtime.ProgressMeter pm =
                new Autodesk.AutoCAD.Runtime.ProgressMeter();
            pm.SetLimit(Kordinat.Count);
            try
            {
                using (acDoc.LockDocument())
                {
                    Database database = acDoc.Database;
                    using (Transaction tr = acDoc.TransactionManager.StartTransaction())
                    {
                        BlockTable blockTable = database.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;
                        if (!blockTable.Has(BlockName))
                        {
                            acDoc.Editor.WriteMessage("\nBlock \"{0}\" does not exist try loading from stream", BlockName);
                            ExtractBlock();
                        }

                        ObjectId blockID = blockTable[BlockName];
                        BlockTableRecord btr = tr.GetObject(database.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;
                        BlockTableRecord btRec = tr.GetObject(blockID, OpenMode.ForRead) as BlockTableRecord;
                        System.Windows.Forms.Application.UseWaitCursor = true;
                        pm.Start("Importing Block with Coordinate Points");
                        
                        foreach (PNEZHelper coordinatHelper in Kordinat)
                        {
                            BlockReference bRef = new
                                BlockReference(coordinatHelper.POINT3D,
                                blockID);
                            bRef.SetDatabaseDefaults();
                            if (bRef.Annotative == AnnotativeStates.True)
                            {
                                ObjectContextManager ObjContext = database.ObjectContextManager;
                                ObjectContextCollection ObjContextColl = ObjContext.GetContextCollection("ACDB_ANNOTATIONSCALES");
                                Autodesk.AutoCAD.Internal.ObjectContexts.AddContext(bRef, ObjContextColl.CurrentContext);
                            }

                            Matrix3d m3d = Matrix3d.Identity;
                            bRef.TransformBy(m3d);
                            btr.AppendEntity(bRef);
                            tr.AddNewlyCreatedDBObject(bRef, true);

                            if (btRec.HasAttributeDefinitions)
                            {
                                btRec.UpgradeOpen();
                                ObjectId BlockObjectId = new ObjectId();

                                foreach (ObjectId objId in btRec)
                                {
                                    DBObject dBObject = objId.GetObject(OpenMode.ForRead);

                                    if (dBObject is DBPoint)
                                    {
                                        BlockObjectId = bRef.ObjectId;
                                        DBPoint dB = (DBPoint)dBObject;
                                        continue;
                                    }
                                    AttributeDefinition attDef = dBObject as AttributeDefinition;
                                    string strValue = "%<\\AcObjProp Object(%<\\_ObjId {0}>%).InsertionPoint \\f \"%lu6%pt{1}\">%";
                                    string fieldValue = "";

                                    if (attDef != null)
                                    {
                                        if (dBObject is AttributeDefinition)
                                        {
                                            AttributeReference attRef = new AttributeReference();
                                            string AttributeValue = "";
                                            switch (attDef.Tag)
                                            {
                                                case "ID":
                                                    AddAttributeReference(acDoc, attRef, tr, bRef, attDef, false, coordinatHelper.Point);
                                                    break;
                                                case "NORTHING":
                                                    fieldValue = string.Format(strValue, BlockObjectId.OldIdPtr.ToString(), "2");
                                                    AttributeValue = fieldValue;
                                                    AddAttributeReference(acDoc, attRef, tr, bRef, attDef, true, AttributeValue);
                                                    break;
                                                case "EASTING":
                                                    fieldValue = string.Format(strValue, BlockObjectId.OldIdPtr.ToString(), "1");
                                                    AttributeValue = fieldValue;
                                                    AddAttributeReference(acDoc, attRef, tr, bRef, attDef, true, AttributeValue);
                                                    break;
                                                case "ELEVASI":
                                                    fieldValue = string.Format(strValue, BlockObjectId.OldIdPtr.ToString(), "4");
                                                    AttributeValue = fieldValue;
                                                    AddAttributeReference(acDoc, attRef, tr, bRef, attDef, true, AttributeValue);
                                                    break;
                                                case "CODE":
                                                    AddAttributeReference(acDoc, attRef, tr, bRef, attDef, false, coordinatHelper.Code);
                                                    break;
                                            }
                                            tr.AddNewlyCreatedDBObject(attRef, true);
                                        }
                                    }
                                }
                            }
                            System.Windows.Forms.Application.DoEvents();
                            System.Threading.Thread.Sleep(80);
                            pm.MeterProgress();
                            Autodesk.AutoCAD.ApplicationServices.Application.UpdateScreen();
                        }
                        tr.Commit();
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                acDoc.Editor.WriteMessage(ex.Message);
            }
            finally
            {
                pm.Stop();
                System.Windows.Forms.Application.UseWaitCursor = false;
            }
        }

        static void SetFieldOnAttributeValue(Document acDoc, Transaction transaction, AttributeReference attributeReference, bool useField, string _Value)
        {
            try
            {
                if (!useField)
                {
                    attributeReference.TextString = _Value;
                }
                else
                {
                    Field field = new Field(_Value);
                    field.Evaluate();
                    FieldEvaluationStatusResult evaluationResult = field.EvaluationStatus;
                    if (evaluationResult.Status != FieldEvaluationStatus.Success)
                    {
                        transaction.Abort();
                        return;
                    }
                    else
                    {
                        attributeReference.SetField(field);
                        string _fValue = field.Value.ToString();
                        transaction.AddNewlyCreatedDBObject(field, true);
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                acDoc.Editor.WriteMessage("\nFailed to Set Field on Attribute Error{0}", ex.Message);
            }
            finally
            {
                //transaction.AddNewlyCreatedDBObject(attributeReference, true);
            }
        }

        public static void AddAttributeReference(Document acDoc, AttributeReference attRef, Transaction transaction, BlockReference blockReference, AttributeDefinition attributeDefinition, bool useField, string value)
        {
            try
            {
                attRef.SetDatabaseDefaults();
                attRef.SetAttributeFromBlock(attributeDefinition, blockReference.BlockTransform);
                attRef.Tag = attributeDefinition.Tag;
                attRef.AdjustAlignment(acDoc.Database);
                blockReference.AttributeCollection.AppendAttribute(attRef);
                SetFieldOnAttributeValue(acDoc, transaction, attRef, useField, value);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                acDoc.Editor.WriteMessage("\nFailed Adding AttRef Error{0}", ex.Message);
            }
        }

    }
}
