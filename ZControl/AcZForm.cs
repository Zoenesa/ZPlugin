using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using System.Runtime.InteropServices;

namespace ZControl
{
    public partial class AcZForm : Telerik.WinControls.UI.ShapedForm
    {
        string thName = "";
        string ThemeResourceName = "";
        const int WM_NCRBUTTONUP = 0xa5;

        [Category("ZControls")]
        public RadTitleBarElement TitleBar { get { return AcZTitleBarElement.TitleBarElement; } }

        [Category("ZControls")]
        public RadDropDownButtonElement ElementMenu { get { return AcZTitleBarElement.AcZMenu.DropDownButtonElement; } }

        [Category("ZControls")]
        public RadDropDownButton SettingsMenu { get { return AcZTitleBarElement.AcZMenu; } }

        public AcZForm()
        {
            InitializeComponent();
            //acTitleBar1.SetPosition(this);
            AcZTitleBarElement.TitleBarElement.Text = base.Text;
  
            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            SetTheme("MetroDarkBlue");
            AcZTitleBarElement.SetTopMost.Click += SetOnTop;
            AcZTitleBarElement.Op1.Click += SetOpacity;
            AcZTitleBarElement.Op5.Click += SetOpacity;
            AcZTitleBarElement.Op7.Click += SetOpacity;
            AcZTitleBarElement.MetroDarkBlue.Click += SetTheme;
            AcZTitleBarElement.MetroLight.Click += SetTheme;
            AcZTitleBarElement.OfficeDark.Click += SetTheme;
            TitleLayout.SendToBack();

            AcZTitleBarElement.ContextMenu = null;
        }
        
        [Category("ZControl")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("Telerik.WinControls.UI.Design.RadItemCollectionEditor, Telerik.WinControls.UI.Design, Version=2018.3.911.40, Culture=neutral, PublicKeyToken=5bb2a467cbec794e", typeof(System.Drawing.Design.UITypeEditor))]
        [RadEditItemsAction]
        public RadItemOwnerCollection TitleMenuItems
        {
            get
            {
                ArrItems = new RadItemOwnerCollection();
                for (int i = 5; i < AcZTitleBarElement.AcZMenu.Items.Count; i++)
                {
                    ArrItems.Add(AcZTitleBarElement.AcZMenu.Items[i]);
                }
                return AcZTitleBarElement.AcZMenu.Items;
                //return ArrItems;
            }
            set
            {
                value = AcZTitleBarElement.AcZMenu.Items;
            }
        }

        protected RadItemOwnerCollection ArrItems = new RadItemOwnerCollection();

        [Category("Window Style")]
        [Browsable(true)]
        public bool ControlMaximizeButton
        {
            get
            {
                return this.MaximizeBox;
            }
            set
            {
                this.MaximizeBox = value;
                AcZTitleBarElement.SuspendLayout();
                if (value)
                {
                    AcZTitleBarElement.TitleBarElement.MaximizeButton.Visibility = ElementVisibility.Visible;
                }
                else
                {
                    AcZTitleBarElement.TitleBarElement.MaximizeButton.Visibility = ElementVisibility.Collapsed;
                }
                AcZTitleBarElement.ResumeLayout(true);
            }
        }

        [Category("Window Style")]
        [Browsable(true)]
        public bool ControlMinimizeButton
        {
            get
            {
                return this.MinimizeBox;
            }
            set
            {
                this.MinimizeBox = value;
                AcZTitleBarElement.SuspendLayout();
                if (this.MinimizeBox)
                {
                    AcZTitleBarElement.TitleBarElement.MinimizeButton.Visibility = ElementVisibility.Visible;
                }
                else
                {
                    AcZTitleBarElement.TitleBarElement.MinimizeButton.Visibility = ElementVisibility.Collapsed;
                }
                AcZTitleBarElement.ResumeLayout(true);
            }
        }

        [Category("Window Style")]
        [Browsable(true)]
        public bool ControlBoxButton
        {
            get
            {
                return this.ControlBox;
            }
            set
            {
                this.ControlBox = value;
                AcZTitleBarElement.SuspendLayout();
                if (this.ControlBox)
                {
                    AcZTitleBarElement.TitleBarElement.CloseButton.Visibility = ElementVisibility.Visible;
                    AcZTitleBarElement.TitleBarElement.MaximizeButton.Visibility = ElementVisibility.Visible;
                    AcZTitleBarElement.TitleBarElement.MinimizeButton.Visibility = ElementVisibility.Visible;
                }
                else
                {
                    AcZTitleBarElement.TitleBarElement.CloseButton.Visibility = ElementVisibility.Collapsed;
                    AcZTitleBarElement.TitleBarElement.MaximizeButton.Visibility = ElementVisibility.Collapsed;
                    AcZTitleBarElement.TitleBarElement.MinimizeButton.Visibility = ElementVisibility.Collapsed;
                }
                AcZTitleBarElement.ResumeLayout(true);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetBorder(true, Color.FromKnownColor(KnownColor.Highlight), Color.FromArgb(43, 43, 43), 1);
            this.BackColor = GetColorFromTheme();
            this.radContextMenu.DropDownOpening += RadContextMenu_DropDownOpening;
        }

        private void RadContextMenu_DropDownOpening(object sender, CancelEventArgs e)
        {
            if (!ControlMaximizeButton)
            {
                AcZContextMenuMax.Enabled = false;
            }
            if (!ControlMinimizeButton)
            {
                AcZContextMenuMin.Enabled = false;
            }
            if (!ControlBoxButton)
            {
                AcZContextMenuMin.Visibility = ElementVisibility.Collapsed;
                AcZContextMenuMax.Visibility = ElementVisibility.Collapsed;
            }
            else
            {
                AcZContextMenuMin.Visibility = ElementVisibility.Visible;
                AcZContextMenuMax.Visibility = ElementVisibility.Visible;
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            SetBorder(true, Color.FromKnownColor(KnownColor.Highlight), Color.FromArgb(43, 43, 43), 1);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            SetBorder(false, Color.FromKnownColor(KnownColor.Gray), Color.FromArgb(58, 58, 58), 1);
        }
        
        protected void SetOnTop(object sender, EventArgs e)
        {
            bool _isTopMost = AcZTitleBarElement.SetTopMost.IsChecked;
            this.TopMost = _isTopMost;
        }

        protected void SetOpacity(object sender, EventArgs e)
        {
            if (sender == AcZTitleBarElement.Op1)
            {
                this.Opacity = 1;
                AcZTitleBarElement.Op1.IsChecked = true;
                AcZTitleBarElement.Op5.IsChecked = false;
                AcZTitleBarElement.Op7.IsChecked = false;
            } else if (sender == AcZTitleBarElement.Op5)
            {
                this.Opacity = 0.6;
                AcZTitleBarElement.Op1.IsChecked = false;
                AcZTitleBarElement.Op5.IsChecked = true;
                AcZTitleBarElement.Op7.IsChecked = false;
            } else if (sender == AcZTitleBarElement.Op7)
            {
                this.Opacity = 0.8;
                AcZTitleBarElement.Op1.IsChecked = false;
                AcZTitleBarElement.Op5.IsChecked = false;
                AcZTitleBarElement.Op7.IsChecked = true;
            }
        }

        protected void SetTheme(object sender, EventArgs e)
        {
            if (sender == AcZTitleBarElement.MetroDarkBlue)
            {
                SetTheme("MetroDarkBlue");
            } else if (sender == AcZTitleBarElement.MetroLight)
            {
                SetTheme("MetroLight");
            } else if (sender == AcZTitleBarElement.OfficeDark)
            {
                SetTheme("OfficeBlack");
            }
        }

        protected void SetBorder(bool onActivated, Color BorderColor, Color FormBackColor, int BorderWidth)
        {
            if (onActivated)
            {
                this.BorderColor = BorderColor;
                //this.BorderWidth = BorderWidth;
            }
            else
            {
                this.BorderColor = BorderColor;
                //this.BorderWidth = BorderWidth;
            }
            this.BackColor = GetColorFromTheme();
            this.Refresh();
        }

        protected void SetBorder(Color BorderColor, Color FormBackColor, int BorderWidth)
        {
            this.BorderColor = BorderColor;
            //this.BorderWidth = BorderWidth;
            this.BackColor = GetColorFromTheme();
            this.Refresh();
        }

        protected Color GetColorFromTheme()
        {
            Color clr = new Color();
            Theme ThemeRes = ThemeResolutionService.GetTheme(thName);
            StyleGroup styleGroup = ThemeRes.FindStyleGroup("Telerik.WinControls.UI.RadForm");
            foreach (PropertySettingGroup psg in styleGroup.PropertySettingGroups)
            {
                if (styleGroup.Registrations[0].ControlType == "Telerik.WinControls.UI.RadForm" && psg.Selector.Value == "Telerik.WinControls.RootRadElement")
                {
                    foreach (PropertySetting propertySetting in psg.PropertySettings)
                    {
                        if (propertySetting.Name == "BackColor")
                        {
                            clr = (Color)propertySetting.Value;
                        }
                    }
                }
            }
            return clr;
        }

        protected void SetTheme(string theme)
        {
            switch (theme)
            {
                case "MetroDarkBlue":
                    ThemeResourceName = "ZControl.Resources.FluentDarkBlue.tssp";
                    thName = "FluentDarkBlue";
                    break;
                case "MetroLight":
                    ThemeResourceName = "ZControl.Resources.FluentBlue.tssp";
                    thName = "FluentBlue";
                    break;
                case "OfficeBlack":
                    ThemeResourceName = "ZControl.Resources.Custom_Office2010Black.tssp";
                    thName = "Custom_Office2010Black";
                    break;
            }
            ThemeResolutionService.LoadPackageResource(ThemeResourceName);
            ThemeResolutionService.ApplicationThemeName = thName;
            SetBorder(Color.FromKnownColor(KnownColor.Highlight), this.AcZTitleBarElement.TitleBarElement.TitleBarFill.BackColor, 1);
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args)
        {
            this.BackColor = GetColorFromTheme();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCRBUTTONUP)
            {
                return;
            }
            base.WndProc(ref m);
        }

        private void OnFormResize(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case FormWindowState.Normal:
                    AcZContextMenuRestore.Enabled = false;
                    AcZContextMenuMax.Enabled = true;
                    break;
                case FormWindowState.Minimized:
                    break;
                case FormWindowState.Maximized:
                    AcZContextMenuRestore.Enabled = true;
                    AcZContextMenuMax.Enabled = false;
                    break;
            }
        }

        private void ContextMenuMaximizeOnClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void ContextMenuMinimizeOnClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ContextMenuRestoreOnClick(object sender, EventArgs e)
        {
            if ((this.WindowState == FormWindowState.Maximized))
            {
                this.WindowState = FormWindowState.Normal;
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void ContextMenuCloseOnClick(object sender, EventArgs e)
        {
            this.Close();
        }

        protected void radMenuItemAbout_Click(object sender, EventArgs e)
        {

        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (AcZTitleBarElement != null)
            {
                AcZTitleBarElement.TitleBarElement.Text = base.Text;
            }
        }
         
    }

}
