namespace ZControl
{
    partial class ZCheckedListBox
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CheckedBox = new Telerik.WinControls.UI.RadCheckedListBox();
            this.FilterTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.ZtblLayout = new System.Windows.Forms.TableLayoutPanel();
            this.radLblSearch = new Telerik.WinControls.UI.RadLabel();
            this.CheckAll = new Telerik.WinControls.UI.RadCheckBox();
            this.UncheckAll = new Telerik.WinControls.UI.RadCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.CheckedBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterTextBox)).BeginInit();
            this.ZtblLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLblSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CheckAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UncheckAll)).BeginInit();
            this.SuspendLayout();
            // 
            // CheckedBox
            // 
            this.ZtblLayout.SetColumnSpan(this.CheckedBox, 5);
            this.CheckedBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckedBox.Location = new System.Drawing.Point(3, 32);
            this.CheckedBox.Name = "CheckedBox";
            this.ZtblLayout.SetRowSpan(this.CheckedBox, 2);
            this.CheckedBox.Size = new System.Drawing.Size(330, 250);
            this.CheckedBox.TabIndex = 0;
            // 
            // FilterTextBox
            // 
            this.FilterTextBox.AutoSize = false;
            this.ZtblLayout.SetColumnSpan(this.FilterTextBox, 4);
            this.FilterTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilterTextBox.Location = new System.Drawing.Point(63, 3);
            this.FilterTextBox.Name = "FilterTextBox";
            this.FilterTextBox.NullText = "Search Block";
            this.FilterTextBox.ShowClearButton = true;
            this.FilterTextBox.ShowNullText = true;
            this.FilterTextBox.Size = new System.Drawing.Size(270, 23);
            this.FilterTextBox.TabIndex = 1;
            this.FilterTextBox.TextChanged += new System.EventHandler(this.FilterTextBox_TextChanged);
            // 
            // ZtblLayout
            // 
            this.ZtblLayout.BackColor = System.Drawing.Color.Transparent;
            this.ZtblLayout.ColumnCount = 5;
            this.ZtblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ZtblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.ZtblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.ZtblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ZtblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ZtblLayout.Controls.Add(this.CheckedBox, 0, 1);
            this.ZtblLayout.Controls.Add(this.FilterTextBox, 1, 0);
            this.ZtblLayout.Controls.Add(this.radLblSearch, 0, 0);
            this.ZtblLayout.Controls.Add(this.CheckAll, 0, 3);
            this.ZtblLayout.Controls.Add(this.UncheckAll, 2, 3);
            this.ZtblLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZtblLayout.Location = new System.Drawing.Point(0, 0);
            this.ZtblLayout.Name = "ZtblLayout";
            this.ZtblLayout.RowCount = 4;
            this.ZtblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.ZtblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ZtblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.ZtblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.ZtblLayout.Size = new System.Drawing.Size(336, 314);
            this.ZtblLayout.TabIndex = 2;
            // 
            // radLblSearch
            // 
            this.radLblSearch.AutoSize = false;
            this.radLblSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLblSearch.ForeColor = System.Drawing.Color.White;
            this.radLblSearch.Location = new System.Drawing.Point(3, 3);
            this.radLblSearch.Name = "radLblSearch";
            this.radLblSearch.Size = new System.Drawing.Size(54, 23);
            this.radLblSearch.TabIndex = 2;
            this.radLblSearch.Text = "Search:";
            // 
            // CheckAll
            // 
            this.CheckAll.AutoSize = false;
            this.ZtblLayout.SetColumnSpan(this.CheckAll, 2);
            this.CheckAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckAll.ForeColor = System.Drawing.Color.White;
            this.CheckAll.Location = new System.Drawing.Point(3, 288);
            this.CheckAll.Name = "CheckAll";
            this.CheckAll.Size = new System.Drawing.Size(92, 23);
            this.CheckAll.TabIndex = 3;
            this.CheckAll.Text = "CheckAll";
            this.CheckAll.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.CheckAll_ToggleStateChanged);
            // 
            // UncheckAll
            // 
            this.UncheckAll.AutoSize = false;
            this.UncheckAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UncheckAll.ForeColor = System.Drawing.Color.White;
            this.UncheckAll.Location = new System.Drawing.Point(101, 288);
            this.UncheckAll.Name = "UncheckAll";
            this.UncheckAll.Size = new System.Drawing.Size(92, 23);
            this.UncheckAll.TabIndex = 4;
            this.UncheckAll.Text = "Uncheck All";
            this.UncheckAll.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.UncheckAll_ToggleStateChanged);
            // 
            // ZCheckedListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ZtblLayout);
            this.Name = "ZCheckedListBox";
            this.Size = new System.Drawing.Size(336, 314);
            ((System.ComponentModel.ISupportInitialize)(this.CheckedBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterTextBox)).EndInit();
            this.ZtblLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radLblSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CheckAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UncheckAll)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadCheckedListBox CheckedBox;
        private Telerik.WinControls.UI.RadTextBox FilterTextBox;
        private System.Windows.Forms.TableLayoutPanel ZtblLayout;
        private Telerik.WinControls.UI.RadLabel radLblSearch;
        private Telerik.WinControls.UI.RadCheckBox CheckAll;
        private Telerik.WinControls.UI.RadCheckBox UncheckAll;
    }
}
