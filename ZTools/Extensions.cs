using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTools
{
    public static class Extensions
    {
        public static object[] PointRow(this string ImportedPoint)
        {
            return ImportedPoint.Split(new[] { ',' });
        }

        public static object[] PointRow(this string ImportedPoint, string Delimiter)
        {
            return ImportedPoint.Split(Delimiter.ToCharArray());
        }

        public static double ParsePointFromCSV(this object Value, IFormatProvider Provider)
        {
            try
            {
                return Convert.ToDouble(Value, Provider);
            }
            catch (System.Exception)
            {
                throw new System.Exception("Invalid Value.");
            }
        }

        public static void ParsePointFromCSV(this object[] xyz, System.Globalization.NumberStyles numberStyles, IFormatProvider Provider, out double[] OutValue, out string refMsg)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                double[] _value = new double[xyz.Length];
                bool fx = double.TryParse(xyz[0].ToString(), numberStyles, Provider, out _value[0]);
                bool fy = double.TryParse(xyz[1].ToString(), numberStyles, Provider, out _value[1]);
                bool fz = double.TryParse(xyz[2].ToString(), numberStyles, Provider, out _value[2]);
                OutValue = _value;
                if (fx & fy & fz)
                {
                    sb.Append("Valid.");
                }
                else
                {
                    sb.Append("Invalid ");
                }
                if (!fx)
                {
                    sb.Append("X ");
                }
                if (!fy)
                {
                    sb.Append("Y ");
                }
                if (!fz)
                {
                    sb.Append("Z ");
                }
                refMsg = sb.ToString();
            }
            catch (System.Exception)
            {
                throw new System.Exception("Invalid Value.");
            }
        }

        public static object ConvertObjectId(this ObjectId objectId)
        {
            return objectId.ToString().Trim(new[] { '{', '(', ')', '}' });
        }

        // Opens a DBObject in ForRead mode (kaefer @ TheSwamp)
        public static T GetObject<T>(this ObjectId id) where T : DBObject
        {
            return id.GetObject<T>(OpenMode.ForRead);
        }

        // Opens a DBObject in the given mode (kaefer @ TheSwamp)
        public static T GetObject<T>(this ObjectId id, OpenMode mode) where T : DBObject
        {
            return id.GetObject(mode) as T;
        }

        // Opens a collection of DBObject in ForRead mode (kaefer @ TheSwamp)       
        public static IEnumerable<T> GetObjects<T>(this IEnumerable ids) where T : DBObject
        {
            return ids.GetObjects<T>(OpenMode.ForRead);
            //return source.GetObjects<T>(OpenMode.ForRead, false, false);
        }

        // Opens a collection of DBObject in the given mode (kaefer @ TheSwamp)
        public static IEnumerable<T> GetObjects<T>(this IEnumerable ids, OpenMode mode) where T : DBObject
        {
            return ids
                .Cast<ObjectId>()
                .Select(id => id.GetObject<T>(mode))
                .Where(res => res != null);
        }

        // returns the object (opened in the specified mode)
        public static T GetObject<T>(
            this ObjectId id,
            OpenMode mode = OpenMode.ForRead,
            bool openErased = false,
            bool forceOpenOnLockedLayer = false) where T : DBObject
        {
            if (id == ObjectId.Null)
                throw new ArgumentNullException(nameof(id));

            var tr = id.Database.TransactionManager.TopTransaction;
            if (tr == null)
                throw new Autodesk.AutoCAD.Runtime.Exception(ErrorStatus.NoActiveTransactions);

            return (T)tr.GetObject(id, mode, openErased, forceOpenOnLockedLayer);
        }

        // gets all references of the block (opened for read)
        public static IEnumerable<BlockReference> GetBlockReferences(this Database db, string blockName)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            var tr = db.TransactionManager.TopTransaction;
            if (tr == null)
                throw new Autodesk.AutoCAD.Runtime.Exception(ErrorStatus.NoActiveTransactions);

            var bt = db.BlockTableId.GetObject<BlockTable>();
            if (bt.Has(blockName))
            {
                var btr = bt[blockName].GetObject<BlockTableRecord>();
                foreach (ObjectId id in btr.GetBlockReferenceIds(true, false))
                {
                    yield return id.GetObject<BlockReference>();
                }
                if (btr.IsDynamicBlock)
                {
                    foreach (ObjectId btrId in btr.GetAnonymousBlockIds())
                    {
                        var anonBtr = btrId.GetObject<BlockTableRecord>();
                        foreach (ObjectId id in anonBtr.GetBlockReferenceIds(true, false))
                        {
                            yield return id.GetObject<BlockReference>();
                        }
                    }
                }
            }
        }

            // Applies the given Action to each element of the collection (mimics the F# Seq.iter function).
        public static void Iterate<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection) action(item);
        }

        // Applies the given Action to each element of the collection (mimics the F# Seq.iteri function).
        // The integer passed to the Action indicates the index of element.
        public static void Iterate<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            int i = 0;
            foreach (T item in collection) action(item, i++);
        }

        // Gets the block effective name (anonymous dynamic blocs).
        public static string GetEffectiveName(this BlockReference br)
        {
            if (br.IsDynamicBlock)
                return br.DynamicBlockTableRecord.GetObject<BlockTableRecord>().Name;
            return br.Name;
        }

        public static TextInfo GetTextInfo(this DBText text)
        {
            return new TextInfo(text.Position, text.AlignmentPoint, text.Justify != AttachmentPoint.BaseLeft);
        }

        public static string GetAreaOfBoundaries(this BlockTableRecord btr, Autodesk.AutoCAD.EditorInput.Editor ed, Autodesk.AutoCAD.EditorInput.PromptPointResult pRes)
        {


            return "#";
        }

        public static Point3d GetAreaOfBoundaries(this Autodesk.AutoCAD.EditorInput.PromptEntityResult pEntRes)
        {
            Point3d p3d = new Point3d();
            ObjectId objId = pEntRes.ObjectId;
            return p3d;
        }

        // Creates a System.Data.DataTable from a BlockAttribute collection.
        public static System.Data.DataTable ToDataTable(this IEnumerable<BlockAttribute> blockAtts, string name)
        {
            System.Data.DataTable dTable = new System.Data.DataTable(name);
            dTable.Columns.Add("Name", typeof(string));
            dTable.Columns.Add("Quantity", typeof(int));
            blockAtts
                .GroupBy(blk => blk, (blk, blks) => new { Block = blk, Count = blks.Count() }, new BlockAttributeEqualityComparer())
                .Iterate(row =>
                {
                    System.Data.DataRow dRow = dTable.Rows.Add(row.Block.Name, row.Count);
                    row.Block.Attributes.Iterate(att =>
                    {
                        if (!dTable.Columns.Contains(att.Key))
                            dTable.Columns.Add(att.Key);
                        dRow[att.Key] = att.Value;
                    });
                });
            return dTable;
        }

        // Gets the column names collection of the datatable
        public static IEnumerable<string> GetColumnNames(this System.Data.DataTable dataTbl)
        {
            return dataTbl.Columns.Cast<System.Data.DataColumn>()
                .Select(col => col.ColumnName);
        }

        // Writes a csv file from the datatable.
        public static void WriteCsv(this System.Data.DataTable dataTbl, string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(dataTbl.GetColumnNames().Aggregate((s1, s2) => string.Format("{0},{1}", s1, s2)));
                dataTbl.Rows
                    .Cast<System.Data.DataRow>()
                    .Select(row => row.ItemArray.Aggregate((s1, s2) => string.Format("{0},{1}", s1, s2)))
                    .Iterate(line => writer.WriteLine(line));
            }
        }

        // Creates an AutoCAD Table from the datatable.
        public static Table ToAcadTable(this System.Data.DataTable dataTbl, double rowHeight, double columnWidth)
        {
            //return dataTbl.Rows.Cast<DataRow>().ToAcadTable(dataTbl.TableName, dataTbl.GetColumnNames(), rowHeight, columnWidth);
            Table tbl = new Table();
            tbl.Rows[0].Height = rowHeight;
            tbl.Columns[0].Width = columnWidth;
            tbl.InsertColumns(0, columnWidth, dataTbl.Columns.Count - 1);
            tbl.InsertRows(0, rowHeight, dataTbl.Rows.Count + 1);
            tbl.Cells[0, 0].Value = dataTbl.TableName;
            dataTbl.GetColumnNames()
                .Iterate((name, i) => tbl.Cells[1, i].Value = name);
            dataTbl.Rows
                .Cast<System.Data.DataRow>()
                .Iterate((row, i) =>
                    row.ItemArray.Iterate((item, j) =>
                        tbl.Cells[i + 2, j].Value = item));
            return tbl;
        }

        //public static Dictionary<string, DBText> GetAttributesToDictionaryKeyByTag(this BlockReference blockref)
        //{
        //    return blockref.GetAttributes().ToDictionary(a => GetTag(a), StringComparer.OrdinalIgnoreCase);
        //}

        public static IEnumerable<AttributeReference> GetAttributes(this BlockReference source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var tr = source.Database.TransactionManager.TopTransaction;
            if (tr == null)
                throw new Autodesk.AutoCAD.Runtime.Exception(ErrorStatus.NoActiveTransactions);

            foreach (ObjectId id in source.AttributeCollection)
            {
                yield return id.GetObject<AttributeReference>();
            }
        }

        public static AttributeReference AddAttributeReferences(this BlockReference br, int index, string value)
        {
            BlockTableRecord obj = br.BlockTableRecord.GetObject<BlockTableRecord>();
            Transaction topTransaction = br.Database.TransactionManager.TopTransaction;
            AttributeReference attributeReference = null;
            AttributeDefinition[] array = obj.GetObjects<AttributeDefinition>().ToArray<AttributeDefinition>();
            for (int i = 0; i < (int)array.Length; i++)
            {
                AttributeDefinition attributeDefinition = array[i];
                AttributeReference attributeReference1 = new AttributeReference();
                attributeReference1.SetAttributeFromBlock(attributeDefinition, br.BlockTransform);
                Point3d position = attributeDefinition.Position;
                attributeReference1.Position = position.TransformBy(br.BlockTransform);
                if (attributeDefinition.Justify != AttachmentPoint.BaseLeft)
                {
                    position = attributeDefinition.AlignmentPoint;
                    attributeReference1.AlignmentPoint = position.TransformBy(br.BlockTransform);
                    attributeReference1.AdjustAlignment(br.Database);
                }
                if (attributeReference1.IsMTextAttribute)
                {
                    attributeReference1.UpdateMTextAttribute();
                }
                if (i == index)
                {
                    attributeReference1.TextString = value;
                    attributeReference = attributeReference1;
                }
                br.AttributeCollection.AppendAttribute(attributeReference1);
                topTransaction.AddNewlyCreatedDBObject(attributeReference1, true);
            }
            return attributeReference;
        }

        public static List<TextInfo> GetAttributesTextInfos(this BlockTableRecord btr)
        {
            return (
                from att in btr.GetObjects<AttributeDefinition>()
                select att.GetTextInfo()).ToList<TextInfo>();
        }

        public static ObjectId GetBlock(this BlockTable blockTable, string blockName)
        {
            string str;
            ObjectId @null;
            Database database = blockTable.Database;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(blockName);
            if (blockTable.Has(fileNameWithoutExtension))
            {
                return blockTable[fileNameWithoutExtension];
            }
            try
            {
                if (Path.GetExtension(blockName) == "")
                {
                    blockName = string.Concat(blockName, ".dwg");
                }
                str = (!File.Exists(blockName) ? HostApplicationServices.Current.FindFile(blockName, database, FindFileHint.Default) : blockName);
                blockTable.UpgradeOpen();
                using (Database database1 = new Database(false, true))
                {
                    database1.ReadDwgFile(str, FileShare.Read, true, null);
                    @null = blockTable.Database.Insert(Path.GetFileNameWithoutExtension(blockName), database1, true);
                }
            }
            catch
            {
                @null = ObjectId.Null;
            }
            return @null;
        }

        public static BlockTableRecord[] GetBlocksWithAttribute(this Database db)
        {
            Func<ObjectId, bool> func2 = null;
            RXClass @class = RXObject.GetClass(typeof(AttributeDefinition));
            return db.BlockTableId.GetObject<BlockTable>().GetObjects<BlockTableRecord>().Where<BlockTableRecord>((BlockTableRecord btr) => {
                if (btr.IsLayout || btr.IsAnonymous || btr.IsFromExternalReference || btr.IsFromOverlayReference)
                {
                    return false;
                }
                IEnumerable<ObjectId> objectIds = btr.Cast<ObjectId>();
                Func<ObjectId, bool> f = func2;
                if (f == null)
                {
                    Func<ObjectId, bool> func = (ObjectId id) => id.ObjectClass.IsDerivedFrom(@class);
                    Func<ObjectId, bool> func1 = func;
                    func2 = func;
                    f = func1;
                }
                return objectIds.Any<ObjectId>(f);
            }).OrderBy<BlockTableRecord, string>((BlockTableRecord btr) => btr.Name).ToArray<BlockTableRecord>();
        }

        public static BlockTableRecord[] GetBlocksWithAttribute(this Database db, bool SelectXRef)
        {
            Func<ObjectId, bool> func2 = null;
            RXClass @class = RXObject.GetClass(typeof(AttributeDefinition));
            return db.BlockTableId.GetObject<BlockTable>().GetObjects<BlockTableRecord>().Where<BlockTableRecord>((BlockTableRecord btr) => {
                if (SelectXRef)
                {
                    if (btr.IsLayout || btr.IsAnonymous)
                    {
                        return false;
                    }
                }
                else
                {
                    if (btr.IsLayout || btr.IsAnonymous || btr.IsFromExternalReference || btr.IsFromOverlayReference)
                    {
                        return false;
                    }
                }

                IEnumerable<ObjectId> objectIds = btr.Cast<ObjectId>();
                Func<ObjectId, bool> f = func2;
                if (f == null)
                {
                    Func<ObjectId, bool> func = (ObjectId id) => id.ObjectClass.IsDerivedFrom(@class);
                    Func<ObjectId, bool> func1 = func;
                    func2 = func;
                    f = func1;
                }
                return objectIds.Any<ObjectId>(f);
            }).OrderBy<BlockTableRecord, string>((BlockTableRecord btr) => btr.Name).ToArray<BlockTableRecord>();
        }

        public static int IndexOf(this BlockTableRecord btr, AttributeDefinition attDef)
        {
            return (
                from att in btr.GetObjects<AttributeDefinition>()
                where !att.Constant
                select att).ToList<AttributeDefinition>().FindIndex((AttributeDefinition x) => x == attDef);
        }

        public static void SetAttributeValue(this BlockReference br, int index, string value)
        {
            br.AttributeCollection[index].GetObject<AttributeReference>(OpenMode.ForWrite).TextString = value;
        }
        
        public class TextInfo
        {
            public Point3d Alignment
            {
                get;
                set;
            }

            public bool IsAligned
            {
                get;
                set;
            }

            public Point3d Position
            {
                get;
                set;
            }

            public TextInfo(Point3d position, Point3d alignment, bool aligned)
            {
                this.Position = position;
                this.Alignment = alignment;
                this.IsAligned = aligned;
            }
        }

        public class BlockAttribute
        {
            private string _name;
            private Dictionary<string, string> _atts;

            // Public read only properties
            public string Name
            {
                get { return _name; }
            }

            public Dictionary<string, string> Attributes
            {
                get { return _atts; }
            }

            public string this[string key]
            {
                get { return _atts[key.ToUpper()]; }
            }

            // Constructors
            public BlockAttribute(BlockReference br)
            {
                SetProperties(br);
            }

            public BlockAttribute(ObjectId id)
            {
                Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                using (Transaction tr = doc.TransactionManager.StartTransaction())
                {
                    SetProperties(tr.GetObject(id, OpenMode.ForRead) as BlockReference);
                }
            }

            // Public method
            new public string ToString()
            {
                if (_atts != null && _atts.Count > 0)
                    return string.Format("{0}: {1}",
                        _name,
                        _atts.Select(a => string.Format("{0}={1}", a.Key, a.Value))
                            .Aggregate((a, b) => string.Format("{0}; {1}", a, b)));
                return _name;
            }

            // Private method
            private void SetProperties(BlockReference br)
            {
                if (br == null) return;
                _name = br.GetEffectiveName();
                _atts = new Dictionary<string, string>();
                br.AttributeCollection
                    .GetObjects<AttributeReference>()
                    .Iterate(att => _atts.Add(att.Tag.ToUpper(), att.TextString));
            }
        }

        public class BlockAttributeEqualityComparer : IEqualityComparer<BlockAttribute>
        {
            public bool Equals(BlockAttribute x, BlockAttribute y)
            {
                return
                    x.Name.Equals(y.Name, StringComparison.CurrentCultureIgnoreCase) &&
                    x.Attributes.SequenceEqual(y.Attributes);
            }

            public int GetHashCode(BlockAttribute obj)
            {
                return base.GetHashCode();
            }
        }

    }
}


