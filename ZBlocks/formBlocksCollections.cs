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

        private void FormBlocksCollections_Activated(object sender, EventArgs e)
        {
            if (this.acDoc != null && !this.acDoc.IsDisposed)
            {
                this.acDoc = AcAp.Application.DocumentManager.MdiActiveDocument;
                this.Text = string.Format("Block Collections - {0}{1}{2}", "{", System.IO.Path.GetFileName(this.document.Name), "}");
                
                block = new BlockATPOINT(this.acDoc);
                list_BlkRefs = new List<AcBlockReferences>();
                list_BlkRefs = block.GetBlockReferences();
                try
                {
                    string[] str = (from name in list_BlkRefs select name.Bref_Name).ToArray();
                    radTextSearchBlocks.AutoCompleteCustomSource.AddRange(str);

                    radCheckedListBlocks.DataSource = block.DataTable;
                    radCheckedListBlocks.DisplayMember = "BrefName";
                    radCheckedListBlocks.ValueMember = "ObjectID";
                }
                catch
                {
                }
            }
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
            if (keyData == Keys.Escape)
            {
                //Cancel_Command();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        BlockATPOINT block = null;
        List<AcBlockReferences> list_BlkRefs;
        List<ObjectId> objectIds = new List<ObjectId>();

        private void formBlocksCollections_Load(object sender, EventArgs e)
        {
        }

        private void RadTextSearchBlocks_TextChanged(object sender, EventArgs e)
        {
            block.DataTable.DefaultView.RowFilter = string.Format("BrefName LIKE '%{0}%'", radTextSearchBlocks.Text);
        }
        
        private void radButtonZoomSelections_Click(object sender, EventArgs e)
        {
            new ZTools.ZoomTools.Zooms(this.acDoc.Editor).ZoomObjects(objectIds);

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
            objectIds = ListSelectedBlocks(this.list_BlkRefs);
        }

        private List<ObjectId> ListSelectedBlocks(List<AcBlockReferences> blockReferences)
        {
            List<ObjectId> items = new List<ObjectId>();
            if (radCheckedListBlocks.SelectedItems.Count > 0)
            {
                try
                {
                    foreach (ListViewDataItem item in this.radCheckedListBlocks.Items)
                    {
                        var obj = (ObjectId)item.Value;
                        items.Add(obj);
                    }
                }
                catch
                {
                    this.acDoc.Editor.WriteMessage("Failed List ObjectId");
                }
            }
            return items;
        }
    }
}
