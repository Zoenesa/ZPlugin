namespace ZControl
{
    public partial class AcZForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AcZForm));
            this.TitleLayout = new System.Windows.Forms.TableLayoutPanel();
            this.AcZTitleBarElement = new ZControl.AcTitleBar();
            this.radContextMenu = new Telerik.WinControls.UI.RadContextMenu(this.components);
            this.AcZContextMenuRestore = new Telerik.WinControls.UI.RadMenuItem();
            this.AcZContextMenuMin = new Telerik.WinControls.UI.RadMenuItem();
            this.AcZContextMenuMax = new Telerik.WinControls.UI.RadMenuItem();
            this.AcZContextSeparator1 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.AcZContextMenuClose = new Telerik.WinControls.UI.RadMenuItem();
            this.AcZContextSeparator2 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.AcZContextMenuAbout = new Telerik.WinControls.UI.RadMenuItem();
            this.radContextMenuManager = new Telerik.WinControls.UI.RadContextMenuManager();
            this.TitleLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AcZTitleBarElement)).BeginInit();
            this.SuspendLayout();
            // 
            // TitleLayout
            // 
            this.TitleLayout.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.TitleLayout, "TitleLayout");
            this.TitleLayout.Controls.Add(this.AcZTitleBarElement, 0, 0);
            this.TitleLayout.Name = "TitleLayout";
            // 
            // AcZTitleBarElement
            // 
            this.TitleLayout.SetColumnSpan(this.AcZTitleBarElement, 2);
            resources.ApplyResources(this.AcZTitleBarElement, "AcZTitleBarElement");
            this.AcZTitleBarElement.Name = "AcZTitleBarElement";
            this.AcZTitleBarElement.OwnerForm = this;
            this.radContextMenuManager.SetRadContextMenu(this.AcZTitleBarElement, this.radContextMenu);
            this.AcZTitleBarElement.TabStop = false;
            // 
            // radContextMenu
            // 
            this.radContextMenu.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.AcZContextMenuRestore,
            this.AcZContextMenuMin,
            this.AcZContextMenuMax,
            this.AcZContextSeparator1,
            this.AcZContextMenuClose,
            this.AcZContextSeparator2,
            this.AcZContextMenuAbout});
            this.radContextMenu.ThemeName = "FluentDarkBlue";
            // 
            // AcZContextMenuRestore
            // 
            this.AcZContextMenuRestore.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.AcZContextMenuRestore.Image = global::ZControl.Properties.Resources.VSRestore;
            this.AcZContextMenuRestore.Name = "AcZContextMenuRestore";
            resources.ApplyResources(this.AcZContextMenuRestore, "AcZContextMenuRestore");
            this.AcZContextMenuRestore.Click += new System.EventHandler(this.ContextMenuRestoreOnClick);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.AcZContextMenuRestore.GetChildAt(0))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding")));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.AcZContextMenuRestore.GetChildAt(0))).Margin = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Margin")));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.AcZContextMenuRestore.GetChildAt(1))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding1")));
            ((Telerik.WinControls.UI.RadMenuItemLayout)(this.AcZContextMenuRestore.GetChildAt(2))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding2")));
            // 
            // AcZContextMenuMin
            // 
            this.AcZContextMenuMin.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.AcZContextMenuMin.Image = global::ZControl.Properties.Resources.VSMinNorm;
            this.AcZContextMenuMin.Name = "AcZContextMenuMin";
            resources.ApplyResources(this.AcZContextMenuMin, "AcZContextMenuMin");
            this.AcZContextMenuMin.Click += new System.EventHandler(this.ContextMenuMinimizeOnClick);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.AcZContextMenuMin.GetChildAt(0))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding3")));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.AcZContextMenuMin.GetChildAt(0))).Margin = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Margin1")));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.AcZContextMenuMin.GetChildAt(1))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding4")));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.AcZContextMenuMin.GetChildAt(1))).Margin = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Margin2")));
            ((Telerik.WinControls.UI.RadMenuItemLayout)(this.AcZContextMenuMin.GetChildAt(2))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding5")));
            ((Telerik.WinControls.UI.RadMenuItemLayout)(this.AcZContextMenuMin.GetChildAt(2))).Margin = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Margin3")));
            // 
            // AcZContextMenuMax
            // 
            this.AcZContextMenuMax.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.AcZContextMenuMax.Image = global::ZControl.Properties.Resources.VSMax;
            this.AcZContextMenuMax.Name = "AcZContextMenuMax";
            resources.ApplyResources(this.AcZContextMenuMax, "AcZContextMenuMax");
            this.AcZContextMenuMax.Click += new System.EventHandler(this.ContextMenuMaximizeOnClick);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.AcZContextMenuMax.GetChildAt(0))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding6")));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.AcZContextMenuMax.GetChildAt(1))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding7")));
            ((Telerik.WinControls.UI.RadMenuItemLayout)(this.AcZContextMenuMax.GetChildAt(2))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding8")));
            // 
            // AcZContextSeparator1
            // 
            this.AcZContextSeparator1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.AcZContextSeparator1.LineOffset = 3F;
            this.AcZContextSeparator1.LineWidth = 1;
            resources.ApplyResources(this.AcZContextSeparator1, "AcZContextSeparator1");
            this.AcZContextSeparator1.Name = "AcZContextSeparator1";
            // 
            // AcZContextMenuClose
            // 
            this.AcZContextMenuClose.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            resources.ApplyResources(this.AcZContextMenuClose, "AcZContextMenuClose");
            this.AcZContextMenuClose.Image = global::ZControl.Properties.Resources.VSCloseNorm;
            this.AcZContextMenuClose.Name = "AcZContextMenuClose";
            this.AcZContextMenuClose.Click += new System.EventHandler(this.ContextMenuCloseOnClick);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.AcZContextMenuClose.GetChildAt(0))).Margin = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Margin4")));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.AcZContextMenuClose.GetChildAt(1))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding9")));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.AcZContextMenuClose.GetChildAt(1))).Margin = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Margin5")));
            ((Telerik.WinControls.UI.RadMenuItemLayout)(this.AcZContextMenuClose.GetChildAt(2))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding10")));
            ((Telerik.WinControls.UI.RadMenuItemLayout)(this.AcZContextMenuClose.GetChildAt(2))).Margin = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Margin6")));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.AcZContextMenuClose.GetChildAt(2).GetChildAt(1))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding11")));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.AcZContextMenuClose.GetChildAt(2).GetChildAt(1))).Margin = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Margin7")));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.AcZContextMenuClose.GetChildAt(2).GetChildAt(1))).Alignment = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.Alignment")));
            // 
            // AcZContextSeparator2
            // 
            this.AcZContextSeparator2.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.AcZContextSeparator2.Name = "AcZContextSeparator2";
            resources.ApplyResources(this.AcZContextSeparator2, "AcZContextSeparator2");
            ((Telerik.WinControls.Primitives.LinePrimitive)(this.AcZContextSeparator2.GetChildAt(0))).FitToSizeMode = Telerik.WinControls.RadFitToSizeMode.FitToParentPadding;
            ((Telerik.WinControls.Primitives.LinePrimitive)(this.AcZContextSeparator2.GetChildAt(0))).Margin = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Margin8")));
            // 
            // AcZContextMenuAbout
            // 
            this.AcZContextMenuAbout.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.AcZContextMenuAbout.Name = "AcZContextMenuAbout";
            resources.ApplyResources(this.AcZContextMenuAbout, "AcZContextMenuAbout");
            this.AcZContextMenuAbout.Click += new System.EventHandler(this.radMenuItemAbout_Click);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.AcZContextMenuAbout.GetChildAt(0))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding12")));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.AcZContextMenuAbout.GetChildAt(1))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding13")));
            ((Telerik.WinControls.UI.RadMenuItemLayout)(this.AcZContextMenuAbout.GetChildAt(2))).Padding = ((System.Windows.Forms.Padding)(resources.GetObject("resource.Padding14")));
            // 
            // AcZForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TitleLayout);
            this.DoubleBuffered = true;
            this.Name = "AcZForm";
            this.Resize += new System.EventHandler(this.OnFormResize);
            this.TitleLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AcZTitleBarElement)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal AcTitleBar AcZTitleBarElement;
        private Telerik.WinControls.UI.RadContextMenu radContextMenu;
        private Telerik.WinControls.UI.RadContextMenuManager radContextMenuManager;
        public Telerik.WinControls.UI.RadMenuSeparatorItem AcZContextSeparator2;
        public Telerik.WinControls.UI.RadMenuItem AcZContextMenuAbout;
        public Telerik.WinControls.UI.RadMenuItem AcZContextMenuRestore;
        public Telerik.WinControls.UI.RadMenuSeparatorItem AcZContextSeparator1;
        public Telerik.WinControls.UI.RadMenuItem AcZContextMenuClose;
        public Telerik.WinControls.UI.RadMenuItem AcZContextMenuMax;
        public Telerik.WinControls.UI.RadMenuItem AcZContextMenuMin;
        public System.Windows.Forms.TableLayoutPanel TitleLayout;
    }
}
