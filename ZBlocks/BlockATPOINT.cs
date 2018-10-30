using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using AcAp = Autodesk.AutoCAD.ApplicationServices;

public class BlockATPOINT
{
    public Document AcDoc
    {
        get { return _document; }
    }

    private Document _document;

    public const string BlockName = "AT-POINT";

    private Database database;

    private Transaction transaction;

    private System.Data.DataTable _dataTable;

    public System.Data.DataTable DataTable { get { return _dataTable; } }

    public BlockATPOINT(Document document)
    {
        this._document = document;
        this.database = document.Database;
        this._dataTable = new System.Data.DataTable();
        this._dataTable.Columns.Add("ObjedID", typeof(string));
        this._dataTable.Columns.Add("BRefName", typeof(string));
        this._dataTable.Columns.Add("Type", typeof(string));
        this._dataTable.Columns.Add("AttRef", typeof(string));
        this._dataTable.Columns.Add("Nested", typeof(string));

    }

    public BlockATPOINT(string ExternalFile)
    {
        this._document = AcAp.Application.DocumentManager.MdiActiveDocument;
        this.database = _document.Database;
        CreateATPOINT();
    }

    public BlockReference ATPOINT
    {
        get
        {
            return CreateATPOINT();
        }
    }

    private BlockReference CreateATPOINT()
    {
        using (transaction = this.database.TransactionManager.StartTransaction())
        {
            BlockTable bt = transaction.GetObject(database.CurrentSpaceId, OpenMode.ForRead) as BlockTable;
            BlockTableRecord btr = transaction.GetObject(bt[""], OpenMode.ForRead) as BlockTableRecord;
            if (bt.Has(BlockName))
            {
                    
            }
        }
        return new BlockReference(new Autodesk.AutoCAD.Geometry.Point3d(0, 0, 0), new ObjectId());
    }

    /// <summary>
    /// Get Blocks From Current Drawing
    /// </summary>
    /// <returns></returns>
    internal List<AcBlockReferences> GetBlockReferences()
    {
        Document Cur_Doc = this._document;
        //this.vmethod_1(0, "Getting document blocks...", null, false);
        List<AcBlockReferences> ListBlockRef = new List<AcBlockReferences>();
        Transaction transaction = null;
        string[] arr = new string[3];
        try
        {
            using (DocumentLock documentLock = Cur_Doc.LockDocument())
            {
                Transaction transaction1 = Cur_Doc.TransactionManager.StartTransaction();
                transaction = transaction1;
                using (transaction1)
                {
                    foreach (ObjectId obj in transaction.GetObject(Cur_Doc.Database.BlockTableId, OpenMode.ForRead) as BlockTable)
                    {
                        BlockTableRecord blockTableRecords = transaction.GetObject(obj, OpenMode.ForRead) as BlockTableRecord;
                        if (blockTableRecords.IsLayout || blockTableRecords.IsAnonymous)
                        {
                            continue;
                        }
                        List<AttributeDefinition> attributeDefinitions = new List<AttributeDefinition>();
                        if (blockTableRecords.HasAttributeDefinitions)
                        {
                            foreach (ObjectId objectId in blockTableRecords)
                            {
                                DBObject dBObject = transaction.GetObject(objectId, OpenMode.ForRead, true, true);
                                if (dBObject.IsErased || !(dBObject is AttributeDefinition))
                                {
                                    continue;
                                }
                                attributeDefinitions.Add(dBObject as AttributeDefinition);
                            }
                        }
                        AcBlockReferences class0 = new AcBlockReferences(obj, blockTableRecords.Name, attributeDefinitions);
                        foreach (ObjectId blockReferenceId in blockTableRecords.GetBlockReferenceIds(true, true))
                        {
                            using (BlockReference blockReference = transaction.GetObject(blockReferenceId, OpenMode.ForRead, true, true) as BlockReference)
                            {
                                if (!blockReference.IsErased)
                                {
                                    List<AttributeReference> attributeReferences = new List<AttributeReference>();
                                    foreach (ObjectId attributeCollection in blockReference.AttributeCollection)
                                    {
                                        AttributeReference attributeReference = transaction.GetObject(attributeCollection, OpenMode.ForRead, true, true) as AttributeReference;
                                        if (attributeReference.IsErased || attributeReference.IsConstant)
                                        {
                                            continue;
                                        }
                                        attributeReferences.Add(attributeReference);
                                    }
                                    //List BlockReference
                                    AcBlockAttributes class3 = new AcBlockAttributes(blockReference, attributeReferences);
                                    if (!class3.bool_0)
                                    {
                                        class0.list_2.Add(class3);
                                    }
                                    else
                                    {
                                        class0.list_1.Add(class3);
                                    }
                                }
                            }
                        }
                        ListBlockRef.Add(class0);
                        arr[0] = class0.objectId_0.ToString();
                        arr[1] = class0.Bref_Name.ToString();
                        arr[2] = class0.GetType().ToString();
                        this._dataTable.Rows.Add(arr);
                    }
                    transaction.Abort();
                    // "Document blocks retrieval complete.", null, false);
                }
            }
        }
        catch (System.Exception exception1)
        {
            System.Exception exception = exception1;
            try
            {
                if (transaction != null && !transaction.IsDisposed)
                {
                    transaction.Abort();
                }
            }
            catch
            {
            }
        }
        return ListBlockRef;
    }
    
}
