using AcAp = Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZControl;
using Telerik.WinControls.UI;
using Autodesk.AutoCAD.DatabaseServices;
using ZTools;

namespace ZBlocks
{
    public partial class formBlocksCollections : AcZForm
    {
        private AcAp.Document acDoc;

        public AcAp.Document document
        {
            get { return acDoc; }
        }

        public formBlocksCollections()
        {
            InitializeComponent();
            this.acDoc = AcAp.Application.DocumentManager.MdiActiveDocument;
            Init();
        }

        public formBlocksCollections(AcAp.Document document)
        {
            InitializeComponent();
            this.acDoc = document;
            Init();
        }

        void Init()
        {
            this.Activated += FormBlocksCollections_Activated;
            this.radTextSearchBlocks.Text = string.Empty;
            this.radTextSearchBlocks.NullText = "Search Block";
            this.radTextSearchBlocks.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.radTextSearchBlocks.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.radTextSearchBlocks.TextChanged += RadTextSearchBlocks_TextChanged;

            this.radTextSearchBlocks.ShowClearButton = true;
            this.radTextSearchBlocks.TextBoxElement.ShowClearButton = true;
        }
        private void RadCheckedListBlocks_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
        }

        void Cancel_Command()
        {
            this.acDoc.Editor.WriteMessage("*Cancel*\n");
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape | this.radTextSearchBlocks.Focused)
            {
                this.radTextSearchBlocks.SelectAll();
                //Cancel_Command();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        BlockATPOINT block = null;
        List<AcBlockReferences> list_BlkRefs;
        List<ObjectId> objectIds = new List<ObjectId>();
        
        private void FormBlocksCollections_Activated(object sender, EventArgs e)
        {
            if (this.acDoc != null && !this.acDoc.IsDisposed)
            {
                this.acDoc = AcAp.Application.DocumentManager.MdiActiveDocument;
                this.Text = string.Format("Block Collections - {0}{1}{2}", "{", System.IO.Path.GetFileName(this.document.Name), "}");
                
                try
                {
                    string[] str = (from name in list_BlkRefs select name.Bref_Name).ToArray();
                    radTextSearchBlocks.AutoCompleteCustomSource.AddRange(str);

                    radCheckedListBlocks.DataSource = block.DataTable;
                    radCheckedListBlocks.DisplayMember = "BrefName";
                    radCheckedListBlocks.ValueMember = "AcBref";

                    
                }
                catch
                {
                }
            }
        }

        private void formBlocksCollections_Load(object sender, EventArgs e)
        {
            if (this.acDoc != null && !this.acDoc.IsDisposed)
            {

                block = new BlockATPOINT(this.acDoc);
                list_BlkRefs = new List<AcBlockReferences>();
                list_BlkRefs = block.GetBlockReferences();

                using (Transaction tr = this.acDoc.TransactionManager.StartTransaction())
                {
                    BlockTableRecord[] btrs
                    = this.acDoc.Database.GetBlocksWithAttribute();
                    
                    List<string> btrsName = (from br in btrs select br.Name).ToList<string>();
                    
                }
                block.DataTable.TableName = "DataBlockDef";
                block.TableChild.TableName = "BlockRefs";
                block.DataSet.Tables.Add(block.DataTable);
                block.DataSet.Tables.Add(block.TableChild);
                //block.DataSet.Relations.Add("Blocks", block.DataTable.Columns["ObjectID"], block.TableChild.Columns["parentId"]);
                this.datagrid.MasterTemplate.AutoGenerateHierarchy = true;
                this.datagrid.MasterTemplate.AutoGenerateColumns = false;
                
                this.datagrid.MasterTemplate.DataSource = CustomSource("BlocksCollections");
                this.datagrid.Relations.AddSelfReference(this.datagrid.MasterTemplate, "column1", "column2");
                this.datagrid.Columns[0].IsVisible = false;
                this.datagrid.Columns[1].IsVisible = false;
                this.datagrid.MasterTemplate.EnableAlternatingRowColor = true;
                this.datagrid.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);
            }
        }

        System.Data.DataTable CustomSource(string Name)
        {
            System.Data.DataTable dt = new System.Data.DataTable(Name);
            dt.Columns.Add("Id", typeof(Int64));
            dt.Columns.Add("ParentId", typeof(Int64));
            dt.Columns.Add("BlockName", typeof(string));
            dt.Columns.Add("Attr", typeof(string));
            object[] arr = new object[] { typeof(Int64), typeof(Int64), typeof(string), typeof(string) };
            bool _ParentAdded = false;
            foreach (AcBlockReferences refs in list_BlkRefs)
            {
                if (refs.HasAttribute)
                {
                    if (!_ParentAdded)
                    {
                        arr[0] = refs.objectId_0.ConvertObjectId();
                        arr[1] = null;
                        arr[2] = refs.Bref_Name;
                        string str = "";
                        foreach (AttributeDefinition def in refs.ListAttDefs)
                        {
                            str = string.Format("{0} {1}; ", str, def.Tag);
                        }
                        str = str.Trim(new[] { ' ', ';' });
                        arr[3] = str;
                        dt.Rows.Add(arr);
                        _ParentAdded = true;
                    }

                    foreach (AcBlockAttributes bat in refs.ListBlkAtt1)
                    {
                        arr[0] = bat.BlockRefId.ConvertObjectId();
                        arr[1] = refs.objectId_0.ConvertObjectId();
                        arr[2] = bat.BlockRefName;
                        arr[3] = bat.ParseAttributeRefsValue(true);
                        dt.Rows.Add(arr);
                    }
                }
                else
                {
                    arr[0] = refs.objectId_0.ConvertObjectId();
                    arr[1] = null;
                    arr[2] = refs.Bref_Name;
                    arr[3] = "";
                    dt.Rows.Add(arr);
                }
                _ParentAdded = false;
            }
            return dt;
        }

        private void RadTextSearchBlocks_TextChanged(object sender, EventArgs e)
        {
            block.DataTable.DefaultView.RowFilter = string.Format("BrefName LIKE '%{0}%'", radTextSearchBlocks.Text);
        }
        
        private void radButtonZoomSelections_Click(object sender, EventArgs e)
        {
            List<AcBlockReferences> list = new List<AcBlockReferences>();
            objectIds = new List<ObjectId>();
            try
            {
                foreach (ListViewDataItem item in radCheckedListBlocks.CheckedItems)
                {
                    AcBlockReferences br = (AcBlockReferences)item.Value;
                    list.Add(br);
                }
                List<string> batts = new List<string>();

                foreach (AcBlockReferences acbr in list)
                {
                    foreach (AcBlockAttributes attr in acbr.ListBlkAtt1)
                    {
                        objectIds.Add(attr.BlockRefId);
                        batts.Add(attr.GetAttRefsTagName("ID"));
                    }
                }
                new ZTools.ZoomTools.Zooms(this.acDoc.Editor).ZoomObjects(objectIds);
            }
            catch (Exception ex)
            {
                this.acDoc.Editor.WriteMessage(ex.Message + "\n");
            }
        }

        private void radButtonDeleteBlocks_Click(object sender, EventArgs e)
        {

        }

        private void radMenuItemCek_Click(object sender, EventArgs e)
        {
            if (radCheckedListBlocks.Items.Count > 0)
            {
                radCheckedListBlocks.CheckAllItems();
            }
        }

        private void radMenuItemUncek_Click(object sender, EventArgs e)
        {
            if (radCheckedListBlocks.Items.Count > 0)
            {
                radCheckedListBlocks.UncheckAllItems();
            }
        }

        private void radMenuUncekSelected_Click(object sender, EventArgs e)
        {
            if (radCheckedListBlocks.SelectedItems.Count > 0)
            {
                radCheckedListBlocks.UncheckSelectedItems();
            }
        }

        private void radCheckedListBlocks_SelectedItemChanged(object sender, EventArgs e)
        {
        }

        private List<ObjectId> ListSelectedBlocks(List<AcBlockReferences> blockReferences)
        {
            List<ObjectId> items = new List<ObjectId>();
            if (radCheckedListBlocks.SelectedItems.Count > 0)
            {
                try
                {
                    foreach (ListViewDataItem item in this.radCheckedListBlocks.SelectedItems)
                    {
                        object obj = item.Value.ToString().Trim(new[] { '(', ')' });
                        ObjectId id = (ObjectId)item.Value;
                        items.Add(id);
                    }
                }
                catch(Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    this.acDoc.Editor.WriteMessage("Failed List ObjectId {0}", ex.Message);
                }
                catch(System.Exception ex)
                {
                    this.acDoc.Editor.WriteMessage("Failed List ObjectId {0}", ex.Message);
                }
            }
            return items;
        }
    }
}
