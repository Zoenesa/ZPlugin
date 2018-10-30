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
            if (keyData == Keys.Escape)
            {
                //Cancel_Command();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        BlockATPOINT block = null;
        List<AcBlockReferences> list_BlkRefs;

        private void formBlocksCollections_Load(object sender, EventArgs e)
        {
            block = new BlockATPOINT(this.acDoc);
            list_BlkRefs = new List<AcBlockReferences>();
            list_BlkRefs = block.GetBlockReferences();
            try
            {
                string[] str = (from name in list_BlkRefs select name.Bref_Name).ToArray();
                radTextSearchBlocks.AutoCompleteCustomSource.AddRange(str);

                radCheckedListBlocks.DataSource = block.DataTable;
                radCheckedListBlocks.DisplayMember = "BrefName";
            }
            catch
            {
            }
        }

        private void RadTextSearchBlocks_TextChanged(object sender, EventArgs e)
        {
            block.DataTable.DefaultView.RowFilter = string.Format("BrefName LIKE '%{0}%'", radTextSearchBlocks.Text);
        }
        
        private void radButtonZoomSelections_Click(object sender, EventArgs e)
        {
            List<ObjectId> objectIds = (from bref in list_BlkRefs select bref.objectId_0).ToList();
            List<List<ObjectId>> objectIds1 = new List<List<ObjectId>>();
            objectIds = new List<ObjectId>();
            foreach (ListViewDataItem item in radCheckedListBlocks.Items)
            {
                if (item.Selected)
                {
                    objectIds.InsertRange(0, (from bref in list_BlkRefs where bref.Bref_Name == item.Text select bref.objectId_0).ToList());
                    objectIds1.Add((from bref in list_BlkRefs where bref.Bref_Name == item.Text select bref.objectId_0).ToList());
                }
            }
            foreach (List<ObjectId> item in objectIds1)
            {

            }
        }

        private void radButtonDeleteBlocks_Click(object sender, EventArgs e)
        {

        }
    }
}
