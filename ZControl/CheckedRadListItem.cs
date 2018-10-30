using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ZControl
{
    public class CheckedRadListItem : RadListVisualItem
    {
        private RadCheckBoxElement m_checkBox = null;
        private RadLabelElement m_label = null;
        protected override Type ThemeEffectiveType
        {
            get
            {
                return typeof(RadListVisualItem);
            }
        } // ThemeEffectiveType

        static CheckedRadListItem()
        {
            RadListVisualItem.SynchronizationProperties.Add(RadListDataItem.SelectedProperty);
            RadListVisualItem.SynchronizationProperties.Add(RadListDataItem.ValueProperty);
        } // static c-tor

        protected override void PropertySynchronized(RadProperty i_property)
        {
            base.PropertySynchronized(i_property);
            Text = String.Empty;
            if (i_property == RadListDataItem.SelectedProperty && m_checkBox.Checked != Selected)
            {
                m_checkBox.ToggleStateChanged -= ToggleStateChanged; // a hack to prevent redundant looping
                m_checkBox.Checked = Selected;
                m_checkBox.ToggleStateChanged += ToggleStateChanged;
            }
            if (i_property == RadListDataItem.ValueProperty)
            {
                m_label.Text = Data.Text;
            }
        } // PropertySynchronized

        protected override void CreateChildElements()
        {
            base.CreateChildElements();
            m_checkBox = new RadCheckBoxElement();
            m_checkBox.Margin = new System.Windows.Forms.Padding(5, 2, 2, 2);
            m_checkBox.ToggleStateChanged += ToggleStateChanged;
            m_label = new RadLabelElement();
            m_label.StretchHorizontally = true;
            Telerik.WinControls.Layouts.StackLayoutPanel l_row = new Telerik.WinControls.Layouts.StackLayoutPanel();
            l_row.Orientation = System.Windows.Forms.Orientation.Horizontal;
            l_row.Children.Add(m_checkBox);
            l_row.Children.Add(m_label);
            Children.Add(l_row);
        } // CreateChildElements

        private void ToggleStateChanged(object i_sender, StateChangedEventArgs i_args)
        {
            Data.Selected = m_checkBox.Checked;
        } // ToggleStateChanged
    }

    public class CustomDataItem : RadListDataItem
    {
        public static readonly RadProperty CheckedProperty = RadProperty.Register("Checked", typeof(bool), typeof(CustomDataItem), new RadElementPropertyMetadata(false));
        public bool IsChecked
        {
            get
            {
                return (bool)this.GetValue(CustomDataItem.CheckedProperty);
            }
            set
            {
                this.SetValue(CustomDataItem.CheckedProperty, value);
            }
        }
    }
}
