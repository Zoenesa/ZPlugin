using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class AcBlockAttributes
    {
        internal List<AttributeReference> List_AttRef;

        internal bool bool_0;

        internal ObjectId BlockRefId;

        internal ObjectId BlockRefObjId;

        internal string BlockRefName;

        internal string BlockRefBlockName;

        internal bool isBlockRefVisible;

        internal string BlockRefLayerName;

        internal AcBlockAttributes(BlockReference blockReference_0, List<AttributeReference> list_1)
        {
            this.BlockRefId = blockReference_0.Id;
            this.BlockRefObjId = blockReference_0.ObjectId;
            this.BlockRefName = blockReference_0.Name;
            this.BlockRefBlockName = blockReference_0.BlockName;
            this.List_AttRef = list_1;
            this.bool_0 = blockReference_0.BlockName.Equals(BlockTableRecord.ModelSpace, StringComparison.InvariantCultureIgnoreCase);
            this.BlockRefLayerName = blockReference_0.Layer;
            this.isBlockRefVisible = blockReference_0.Visible;
        }

        internal string method_0(bool bool_2)
        {
            string empty = string.Empty;
            foreach (AttributeReference attref in this.List_AttRef)
            {
                string str = (string.IsNullOrEmpty(empty) ? string.Empty : "; ");
                if (bool_2)
                {
                    str = string.Concat(str, attref.Tag, " = ");
                }
                str = string.Concat(str, attref.TextString);
                empty = string.Concat(empty, str);
            }
            return empty;
        }

        internal bool method_1(string string_3)
        {
            bool flag;
            List<AttributeReference>.Enumerator enumerator = this.List_AttRef.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    if (!string.Equals(string_3, enumerator.Current.Tag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    flag = true;
                    return flag;
                }
                return false;
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }

        internal string GetAttRefsTagName(string stringTag)
        {
            string empty = string.Empty;
            foreach (AttributeReference attref in this.List_AttRef)
            {
                if (string.Equals(stringTag, attref.Tag, StringComparison.InvariantCultureIgnoreCase))
                {
                    empty = attref.TextString;
                }
                if (string.IsNullOrEmpty(empty))
                {
                    continue;
                }
                return empty;
            }
            return empty;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AcBlockAttributes))
            {
                return false;
            }
            return this.BlockRefObjId.Equals((obj as AcBlockAttributes).BlockRefObjId);
        }

        public override int GetHashCode()
        {
            return this.BlockRefObjId.GetHashCode();
        }

        public override string ToString()
        {
            return this.BlockRefName;
        }
    }
