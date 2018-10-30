using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ZControl
{
    public class AcTitleBar : Telerik.WinControls.UI.RadTitleBar
    {
        public System.Windows.Forms.Control OwnerForm { get; set; }

        public AcTitleBar()
        {
            SetDropDownMenu();

        }

        public void SetPosition(System.Windows.Forms.Control Owner)
        {
            OwnerForm = Owner;
            Inisial();
        }

        public RadDropDownButton AcZMenu;

        void SetDropDownMenu()
        {
            AcZMenu = new RadDropDownButton();
            MenuOpacity = new RadMenuItem();
            SetTopMost = new RadMenuItem();
            Separator2 = new RadMenuSeparatorItem();

            Op5 = new RadMenuItem();
            Op7 = new RadMenuItem();
            Op1 = new RadMenuItem();
            Separator1 = new RadMenuSeparatorItem();
            Theme = new RadMenuItem();
            MetroDarkBlue = new RadMenuItem();
            MetroLight = new RadMenuItem();
            OfficeDark = new RadMenuItem();

            ((System.ComponentModel.ISupportInitialize)(this.AcZMenu)).BeginInit();
            base.SuspendLayout();
            AcZMenu.Text = " Settings";
            AcZMenu.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);

            MenuOpacity.Text = "Opacity";
            MenuOpacity.Name = "MenuOpacity";

            Separator1.Name = "Separator1"; Separator2.Name = "Separator2";

            SetTopMost.Text = "Always On Top";
            SetTopMost.CheckOnClick = true;

            MetroDarkBlue.Text = "Metro Dark Blue"; MetroDarkBlue.Name = "MetroDarkBlue";
            MetroLight.Text = "Metro Light"; MetroLight.Name = "MetroLight";
            OfficeDark.Text = "XP Black"; OfficeDark.Name = "OfficeBlack";

            Theme.Text = "Theme";
            Theme.Name = "Theme";
            Theme.Items.AddRange(new RadItem[]
            {
                MetroDarkBlue, MetroLight
            });

            MenuOpacity.Items.AddRange(new RadItem[]
            { 
                Op5,Op7,Op1
            });

            Op5.Text = "Opacity  50%"; Op5.Name = "Op5"; Op5.CheckOnClick = true;
            Op7.Text = "Opacity  70%"; Op5.Name = "Op7"; Op7.CheckOnClick = true;
            Op1.Text = "Opacity 100%"; Op5.Name = "Op1"; Op1.CheckOnClick = true;

            AcZMenu.Items.AddRange(new RadItem[]
            {
                MenuOpacity, Separator1, SetTopMost, Separator2, Theme
            });

            base.TitleBarElement.SystemButtons.Children.Insert(0, AcZMenu.DropDownButtonElement);
            AcZMenu.DropDownButtonElement.Margin = new System.Windows.Forms.Padding(1, 0, 10, 1);
            ((System.ComponentModel.ISupportInitialize)(this.AcZMenu)).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        void Inisial()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();

            base.Size = new Size(OwnerForm.ClientSize.Width - 2, 30);
            this.Size = new Size(OwnerForm.ClientSize.Width - 2, 30);
            base.Location = new Point(1, 1);
            base.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
            this.BringToFront();
        }

        internal RadMenuItem Theme;
        internal RadMenuItem SetTopMost;
        internal RadMenuItem MetroDarkBlue;
        internal RadMenuItem OfficeDark;
        internal RadMenuItem MetroLight;
        internal RadMenuSeparatorItem Separator1;
        internal RadMenuSeparatorItem Separator2;

        internal RadMenuItem MenuOpacity;
        internal RadMenuItem Op5;
        internal RadMenuItem Op7;
        internal RadMenuItem Op1;
    }
}
