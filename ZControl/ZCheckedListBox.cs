using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ZControl
{
    public partial class ZCheckedListBox : UserControl
    {
        public ZCheckedListBox()
        {
            InitializeComponent();
            listObjects = new List<object>();
        }

        List<object> listObjects;

        private void CheckAll_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (CheckAll.Checked)
            {
                UncheckAll.Checked = false;
                CheckedBox.CheckAllItems();
            }
        }

        private void UncheckAll_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (UncheckAll.Checked)
            {
                CheckAll.Checked = false;
                CheckedBox.UncheckAllItems();
            }
        }

        private void FilterTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string lower = this.FilterTextBox.Text.ToLower();
                List<object> list = this.listObjects.Where<object>((object itemAsObject) =>
                {
                    if (string.IsNullOrWhiteSpace(lower))
                    {
                        return true;
                    }
                    return this.CheckedBox.GetItemText(itemAsObject).ToLower().Contains(lower);
                }).ToList<object>();
                for (int i = CheckedBox.Items.Count - 1; i > -1; i--)
                {
                    if (!list.Contains(this.CheckedBox.Items[i]))
                    {
                        this.CheckedBox.Items.RemoveAt(i);
                    }
                }
                foreach (object obj in list)
                {
                    if (this.CheckedBox.Items.Contains(obj))
                    {
                        this.CheckedBox.Items[this.CheckedBox.Items.IndexOf(obj)] = obj as ListViewDataItem;
                    }
                    else
                    {
                        this.CheckedBox.Items.Add(obj);
                    }
                }
            }
            catch
            {
            }
        }

        public void SetItems<T>(List<T> items)
        {
            foreach (T obj in items)
            {
                listObjects.Add(obj);
            }
            this.FilterTextBox_TextChanged(null, null);
        }
    }

    public static class CheckedBoxExtensions
    {
        public static string GetItemText(this RadCheckedListBox checkedListBox, object itemAsObject)
        {
            string _Value = "";            
            foreach (ListViewDataItem item in checkedListBox.Items)
            {
                if (item.Value.Equals(itemAsObject))
                {
                    _Value = item.Value.ToString();
                    break;
                }
            }
            return _Value;
        }
    }
}
