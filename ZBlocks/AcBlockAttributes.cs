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

        internal ObjectId objectId_0;

        internal ObjectId objectId_1;

        internal string string_0;

        internal string BlockRefName;

        internal bool bool_1;

        internal string string_2;

        internal AcBlockAttributes(BlockReference blockReference_0, List<AttributeReference> list_1)
        {
            this.objectId_0 = blockReference_0.Id;
            this.objectId_1 = blockReference_0.ObjectId;
            this.string_0 = blockReference_0.Name;
            this.BlockRefName = blockReference_0.BlockName;
            this.List_AttRef = list_1;
            this.bool_0 = blockReference_0.BlockName.Equals(BlockTableRecord.ModelSpace, StringComparison.InvariantCultureIgnoreCase);
            this.string_2 = blockReference_0.Layer;
            this.bool_1 = blockReference_0.Visible;
        }

        internal string method_0(bool bool_2)
        {
            string empty = string.Empty;
            foreach (AttributeReference list0 in this.List_AttRef)
            {
                string str = (string.IsNullOrEmpty(empty) ? string.Empty : "; ");
                if (bool_2)
                {
                    str = string.Concat(str, list0.Tag, " = ");
                }
                str = string.Concat(str, list0.TextString);
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

        internal string GetAttributeReference(string stringTag)
        {
            string empty = string.Empty;
            foreach (AttributeReference list0 in this.List_AttRef)
            {
                if (string.Equals(stringTag, list0.Tag, StringComparison.InvariantCultureIgnoreCase))
                {
                    empty = list0.TextString;
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
            return this.objectId_1.Equals((obj as AcBlockAttributes).objectId_1);
        }

        public override int GetHashCode()
        {
            return this.objectId_1.GetHashCode();
        }

        public override string ToString()
        {
            return this.string_0;
        }
    }
