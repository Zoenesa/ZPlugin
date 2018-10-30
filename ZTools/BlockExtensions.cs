using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTools
{
    static class BlockExtensions
    {
        /// <summary>
        /// source: https://adndevblog.typepad.com/autocad/2012/05/insert-block-from-a-different-dwg-using-net-.html
        /// Using the WblockCloneObjects() method, 
        /// its possible to copy a particular block from a drawing in to the other drawing.
        /// </summary>
        public static void InsertBlock(Document doc, string filename, string BlockName)
        {
            try
            {
                using (Database OpenDb = new Database(false, true))
                {
                    OpenDb.ReadDwgFile(filename,
                        System.IO.FileShare.ReadWrite, true, "");
                    ObjectIdCollection ids = new ObjectIdCollection();
                    using (Transaction tr = OpenDb.TransactionManager.StartTransaction())
                    {
                        BlockTable bt = (BlockTable)tr.GetObject(OpenDb.BlockTableId, OpenMode.ForRead);
                        if (bt.Has(BlockName))
                        {
                            ids.Add(bt[BlockName]);
                        }
                        tr.Commit();
                    }

                    //if found, add the block
                    if (ids.Count != 0)
                    {
                        //get the current drawing database
                        Database destdb = doc.Database;
                        IdMapping iMap = new IdMapping();
                        destdb.WblockCloneObjects(ids, destdb.BlockTableId, iMap, DuplicateRecordCloning.Ignore, false);
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                doc.Editor.WriteMessage("Faild Insert Block Error: {0}\n", ex.Message);
            }
        }

        /// <summary>
        /// load method InsertDrawingAsBlock
        /// </summary>
        public static void testInsertExm(string fileName)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            string dwgname = fileName;
            string blockname = System.IO.Path.GetFileNameWithoutExtension(dwgname);
            Database db = doc.Database;
            Point3d pt = new Point3d(100, 200, 0);
            InsertDrawingAsBlock(doc, dwgname, blockname, pt);
        }

        /// <summary>
        ///            * Insert Drawing As Block *      [A2010 version]
        ///             the source drawigs should be drawn as number of
        ///             separate entites with or without attribute entities
        ///             https://forums.augi.com/showthread.php?145986-Insert-External-DWG-file-with-attributes
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path"></param>
        /// <param name="blocname"></param>
        /// <param name="ipt"></param>
        public static void InsertDrawingAsBlock(Document doc, string path, string blockname, Point3d ipt)
        {
            Database curdb = doc.Database;
            Editor ed = doc.Editor;
            DocumentLock loc = doc.LockDocument();
            using (loc)
            {
                ObjectId blkid = ObjectId.Null;
                Database db = new Database(false, true);
                using (db)
                {
                    db.ReadDwgFile(path, System.IO.FileShare.Read, true, "");
                    blkid = curdb.Insert(path, db, true);
                    using (Transaction tr = doc.TransactionManager.StartTransaction())
                    {
                        BlockTable bt = (BlockTable)tr.GetObject(curdb.BlockTableId, OpenMode.ForRead);
                        if (bt.Has(blockname))
                        {
                            ed.WriteMessage(string.Format("\nBlock {0} does already exist\nTry another block or Exit ", blockname));
                            return;
                        }
                        bt.UpgradeOpen();
                        BlockTableRecord btrec = (BlockTableRecord)blkid.GetObject(OpenMode.ForRead);
                        btrec.UpgradeOpen();
                        btrec.Name = blockname;
                        btrec.DowngradeOpen();
                        //---> debug only
                        // this code block is written in the good programming manner (remember that)
                        foreach (ObjectId index in btrec)
                        {
                            Entity en = (Entity)index.GetObject(OpenMode.ForRead);

                            AttributeDefinition adef = en as AttributeDefinition;

                            if (adef != null)
                            {
                                ed.WriteMessage("\n" + adef.Tag);
                            }
                        }//<--- debug only

                        BlockTableRecord btr = (BlockTableRecord)curdb.CurrentSpaceId.GetObject(OpenMode.ForWrite);
                        using (btr)
                        {
                            using (BlockReference bref = new BlockReference(ipt, blkid))
                            {
                                Matrix3d mat = Matrix3d.Identity;
                                bref.TransformBy(mat);
                                bref.ScaleFactors = new Scale3d(1, 1, 1);
                                btr.AppendEntity(bref);
                                tr.AddNewlyCreatedDBObject(bref, true);
                                using (BlockTableRecord btAttRec = (BlockTableRecord)bref.BlockTableRecord.GetObject(OpenMode.ForRead))
                                {
                                    Autodesk.AutoCAD.DatabaseServices.AttributeCollection atcoll = bref.AttributeCollection;
                                    foreach (ObjectId subid in btAttRec)
                                    {
                                        Entity ent = (Entity)subid.GetObject(OpenMode.ForRead);
                                        AttributeDefinition attDef = ent as AttributeDefinition;
                                        if (attDef != null)
                                        {
                                            // ed.WriteMessage("\nValue: " + attDef.TextString);
                                            AttributeReference attRef = new AttributeReference();
                                            attRef.SetPropertiesFrom(attDef);
                                            attRef.Visible = attDef.Visible;
                                            attRef.SetAttributeFromBlock(attDef, bref.BlockTransform);
                                            attRef.HorizontalMode = attDef.HorizontalMode;
                                            attRef.VerticalMode = attDef.VerticalMode;
                                            attRef.Rotation = attDef.Rotation;
                                            attRef.TextStyleId = attDef.TextStyleId;
                                            attRef.Position = attDef.Position + ipt.GetAsVector();
                                            attRef.Tag = attDef.Tag;
                                            attRef.FieldLength = attDef.FieldLength;
                                            attRef.TextString = attDef.TextString;
                                            attRef.AdjustAlignment(curdb);//?
                                            atcoll.AppendAttribute(attRef);
                                            tr.AddNewlyCreatedDBObject(attRef, true);
                                        }
                                    }
                                }
                                bref.DowngradeOpen();
                            }
                        }
                        btrec.DowngradeOpen();
                        bt.DowngradeOpen();
                        ed.Regen();
                        tr.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// Import blocks from an external DWG file using .NET
        /// a "side database" - a drawing that is loaded in memory, 
        /// but not into the AutoCAD editor - to import the blocks from another drawing into the one active in the editor.
        /// https://through-the-interface.typepad.com/through_the_interface/2006/08/import_blocks_f.html
        /// </summary>
        public static void ImportBlocks()
        {
            DocumentCollection dm =
                Application.DocumentManager;

            Editor ed = dm.MdiActiveDocument.Editor;

            Database destDb = dm.MdiActiveDocument.Database;

            Database sourceDb = new Database(false, true);

            PromptResult sourceFileName;
            try
            {
                // Get name of DWG from which to copy blocks
                sourceFileName =
                  ed.GetString("\nEnter the name of the source drawing: ");
                // Read the DWG into a side database
                sourceDb.ReadDwgFile(sourceFileName.StringResult,
                                    System.IO.FileShare.Read,
                                    true,
                                    "");
                
                // Create a variable to store the list of block identifiers
                ObjectIdCollection blockIds = new ObjectIdCollection();
                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm =
                  sourceDb.TransactionManager;
                using (Transaction myT = tm.StartTransaction())
                {
                    // Open the block table
                    BlockTable bt =
                        (BlockTable)tm.GetObject(sourceDb.BlockTableId,
                                                OpenMode.ForRead,
                                                false);
                    // Check each block in the block table
                    foreach (ObjectId btrId in bt)
                    {
                        BlockTableRecord btr =
                          (BlockTableRecord)tm.GetObject(btrId,
                                                        OpenMode.ForRead,
                                                        false);
                        // Only add named & non-layout blocks to the copy list
                        if (!btr.IsAnonymous && !btr.IsLayout)
                            blockIds.Add(btrId);
                        btr.Dispose();
                    }
                }
                // Copy blocks from source to destination database
                IdMapping mapping = new IdMapping();
                sourceDb.WblockCloneObjects(blockIds,
                                            destDb.BlockTableId,
                                            mapping,
                                            DuplicateRecordCloning.Replace,
                                            false);
                ed.WriteMessage("\nCopied "
                                + blockIds.Count.ToString()
                                + " block definitions from "
                                + sourceFileName.StringResult
                                + " to the current drawing.");
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ed.WriteMessage("\nError during copy: " + ex.Message);
            }
            sourceDb.Dispose();
        }
        
    }
}
