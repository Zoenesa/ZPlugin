using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AcBlockReferences
{
    internal ObjectId objectId_0;

    internal ObjectId BlockTableId;

    internal string Bref_Name;

    internal List<AttributeDefinition> ListAttDefs;

    internal List<AcBlockAttributes> ListBlkAtt1;

    internal List<AcBlockAttributes> ListBlkAtt2;

    internal readonly bool HasAttribute;
    
    internal AcBlockReferences(ObjectId objectId_1, string string_1, List<AttributeDefinition> list_3)
    {
        this.objectId_0 = objectId_1;
        this.Bref_Name = string_1;
        this.ListAttDefs = list_3;
        this.ListBlkAtt1 = new List<AcBlockAttributes>();
        this.ListBlkAtt2 = new List<AcBlockAttributes>();
    }

    internal AcBlockReferences(ObjectId objectId_1, string string_1, List<AttributeDefinition> list_3, bool hasRefs)
    {
        this.objectId_0 = objectId_1;
        this.Bref_Name = string_1;
        this.ListAttDefs = list_3;
        this.ListBlkAtt1 = new List<AcBlockAttributes>();
        this.ListBlkAtt2 = new List<AcBlockAttributes>();
        this.HasAttribute = hasRefs;
    }

    internal AttributeDefinition method_0(string string_1)
    {
        AttributeDefinition attributeDefinition = null;
        foreach (AttributeDefinition list0 in this.ListAttDefs)
        {
            if (!string.Equals(string_1, list0.Tag, StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }
            attributeDefinition = list0;
            return attributeDefinition;
        }
        return attributeDefinition;
    }

    internal bool method_1(string string_1)
    {
        bool flag;
        List<AttributeDefinition>.Enumerator enumerator = this.ListAttDefs.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                if (!string.Equals(string_1, enumerator.Current.Tag, StringComparison.InvariantCultureIgnoreCase))
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

    internal int method_2(List<AcBlockReferences> list_3)
    {
        int num = 0;
        this.method_3(list_3, this, ref num);
        return num;
    }

    private int method_3(List<AcBlockReferences> list_3, AcBlockReferences class0_0, ref int int_0)
    {
        foreach (AcBlockAttributes list2 in class0_0.ListBlkAtt2)
        {
            AcBlockReferences class0 = list_3.FirstOrDefault<AcBlockReferences>((AcBlockReferences argument0) => string.Equals(argument0.Bref_Name, list2.BlockRefBlockName));
            if (class0 == null)
            {
                continue;
            }
            int_0 += class0.ListBlkAtt1.Count;
            this.method_3(list_3, class0, ref int_0);
        }
        return int_0;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is AcBlockReferences))
        {
            return false;
        }
        AcBlockReferences object0 = obj as AcBlockReferences;
        if (!this.objectId_0.Equals(object0.objectId_0))
        {
            return false;
        }
        return this.Bref_Name.Equals(object0.Bref_Name);
    }

    public override int GetHashCode()
    {
        return this.objectId_0.GetHashCode();
    }

    public override string ToString()
    {
        return this.Bref_Name;
    }
}
