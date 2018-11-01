using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using AcAp = Autodesk.AutoCAD.ApplicationServices;
using ZTools;

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

    public System.Data.DataSet DataSet;
    public System.Data.DataTable TableChild;

    public BlockATPOINT(Document document)
    {
        this._document = document;
        this.database = document.Database;
        this._dataTable = new System.Data.DataTable();
        this._dataTable.Columns.Add("ObjectID", typeof(Int64));
        this._dataTable.Columns.Add("BRefName", typeof(string));
        this._dataTable.Columns.Add("AcBref", typeof(AcBlockReferences));
        this._dataTable.Columns.Add("Type", typeof(string));
        this._dataTable.Columns.Add("AttRef", typeof(string));
        this._dataTable.Columns.Add("Nested", typeof(string));

        TableChild = new System.Data.DataTable();
        TableChild.Columns.Add("Id", typeof(Int64));
        TableChild.Columns.Add("ParentId", typeof(Int64));
        //TableChild.Columns.Add("BObjectId", typeof(ObjectId));
        //TableChild.Columns.Add("PObjectId", typeof(ObjectId));
        TableChild.Columns.Add("BlockName", typeof(string));
        TableChild.Columns.Add("Attr", typeof(string));
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
        object[] arr = new object[] { typeof(Int64), typeof(string), typeof(AcBlockReferences), typeof(string), typeof(string), typeof(string) };
        object[] arrChild = new object[] {
            typeof(Int64),
            typeof(Int64),
            typeof(string),
            typeof(string) };
        this.DataSet = new DataSet("BlockDataSet");
        bool _flagAttributes = false;
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

                        List<AttributeDefinition> attributeDefinitions = ListAttributeDefinitions(blockTableRecords, transaction1);


                        AcBlockReferences acBrefs = new AcBlockReferences(obj, blockTableRecords.Name, attributeDefinitions, blockTableRecords.HasAttributeDefinitions);
                        acBrefs.BlockTableId = blockTableRecords.Id;
                        string ids = acBrefs.BlockTableId.ToString().Trim(new[] { '{', '(', ')', '}' });

                        arrChild[0] = ids;
                        //arrChild[1] = null;
                        arrChild[2] = acBrefs.Bref_Name;
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
                                    AcBlockAttributes acBatt = new AcBlockAttributes(blockReference, attributeReferences);
                                    if (!acBatt.bool_0)
                                    {
                                        acBrefs.ListBlkAtt2.Add(acBatt);
                                    }
                                    else
                                    {
                                        acBrefs.ListBlkAtt1.Add(acBatt);
                                        //arrChild[1] = class3.BlockRefId.ToString().Trim(new[] { '{', '(', ')', '}' });
                                        arrChild[1] = blockReferenceId.ConvertObjectId();
                                        arrChild[3] = acBatt.ParseAttributeRefsValue(true);
                                        _flagAttributes = true;
                                    }
                                    TableChild.Rows.Add(arrChild);
                                }
                            }
                        }
                        ListBlockRef.Add(acBrefs);
                        arr[0] = acBrefs.objectId_0.ConvertObjectId();
                        arr[1] = string.Format("{0} {2}{1}{3}", acBrefs.Bref_Name, acBrefs.objectId_0.ConvertObjectId(), "{", "}");
                        arr[2] = acBrefs;
                        arr[3] = acBrefs.GetType().ToString();
                        this._dataTable.Rows.Add(arr);
                        if (!_flagAttributes)
                        {
                            arrChild[1] = null;
                        }
                        TableChild.Rows.Add(arrChild);
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
    
    List<AttributeDefinition> ListAttributeDefinitions(BlockTableRecord blockTableRecords)
    {
        List<AttributeDefinition> list = new List<AttributeDefinition>();
        if (blockTableRecords.HasAttributeDefinitions)
        {
            foreach (ObjectId objectId in blockTableRecords)
            {
                DBObject dBObject = transaction.GetObject(objectId, OpenMode.ForRead, true, true);
                if (dBObject.IsErased || !(dBObject is AttributeDefinition))
                {
                    continue;
                }
                list.Add(dBObject as AttributeDefinition);
            }
        }
        return list;
    }

    List<AttributeDefinition> ListAttributeDefinitions(BlockTableRecord blockTableRecords, Transaction transaction)
    {
        List<AttributeDefinition> list = new List<AttributeDefinition>();
        if (blockTableRecords.HasAttributeDefinitions)
        {
            foreach (ObjectId objectId in blockTableRecords)
            {
                DBObject dBObject = transaction.GetObject(objectId, OpenMode.ForRead, true, true);
                if (dBObject.IsErased || !(dBObject is AttributeDefinition))
                {
                    continue;
                }
                list.Add(dBObject as AttributeDefinition);
            }
        }
        return list;
    }

}
