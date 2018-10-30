using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ZControl
{
    public class AcZDropDown : RadDropDownButton
    {
        internal readonly RadFormTitleBarElement TitleBarElement;
        internal readonly RadForm FormOwner;

        public AcZDropDown()
        {
            _flagInTitlebar = false;
        }

        public AcZDropDown(RadForm Owner, RadFormTitleBarElement TitleBarElement, string Caption)
        {
            this.TitleBarElement = TitleBarElement;
            this.FormOwner = Owner;
            this.Text = Caption;
            _flagInTitlebar = true;
        }

        bool _flagInTitlebar;
        bool Added = false;

        public bool ShowInTitleBar
        {
            get { return _flagInTitlebar; }
            set
            {
                value = _flagInTitlebar;
            }
        }

        public void SetDropDownInTitleBar()
        {
            if (Added)
            {
                return;
            }
            if (_flagInTitlebar & FormOwner != null & TitleBarElement != null)
            {
                RadDropDownButtonElement elm = this.TitleBarElement.SystemButtons.Children[0] as RadDropDownButtonElement;
                foreach (RadElement element in TitleBarElement.SystemButtons.Children)
                {
                    RadDropDownButtonElement btnE = this.DropDownButtonElement;
                    if (element == btnE)
                    {
                        return;
                    }
                    TitleBarElement.SystemButtons.Children.Insert(0, this.DropDownButtonElement);
                    Added = true;
                    foreach (System.Windows.Forms.Control ctl in FormOwner.Controls)
                    {
                        if (ctl is AcZDropDown)
                        {
                            FormOwner.Controls.Remove(ctl);
                            return;
                        }
                    }
                    break;
                }
            }
        }
    }
}
