using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WOL
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            tabControl.DrawItem += TabControl_DrawItem;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // If the last TabPage is selected then Create a new TabPage
                if (tabControl.SelectedIndex == tabControl.TabPages.Count - 1)
                    CreateTabPage(tabControl.SelectedIndex);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void CreateTabPage(int index)
        {
            var tabPageNew = new TabPage();

            tabPageNew.Location = new System.Drawing.Point(4, 22);
            tabPageNew.Name = $"tabPage{index}";
            tabPageNew.Padding = new System.Windows.Forms.Padding(3);
            tabPageNew.Size = new System.Drawing.Size(767, 345);
            tabPageNew.TabIndex = 0;
            tabPageNew.Text = $"tabPage{index}";
            tabPageNew.UseVisualStyleBackColor = true;

            tabControl.Controls.Add(tabPageNew);
            tabControl.TabPages[index].Text = $"tabPage{index}";
        }

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                var tabPage = this.tabControl.TabPages[e.Index];
                var tabRect = this.tabControl.GetTabRect(e.Index);
                tabRect.Inflate(-2, -2);
                if (e.Index == this.tabControl.TabCount - 1) // Add button to the last TabPage only
                {
                    //var addImage = new Bitmap(addButtonFullPath);
                    //e.Graphics.DrawImage(addImage,
                    //    tabRect.Left + (tabRect.Width - addImage.Width) / 2,
                    //    tabRect.Top + (tabRect.Height - addImage.Height) / 2);
                    TextRenderer.DrawText(e.Graphics, "Add", tabPage.Font,
                        tabRect, tabPage.ForeColor, TextFormatFlags.HorizontalCenter);
                    
                }
                else // draw Close button to all other TabPages
                {
                    //var closeImage = new Bitmap(closeButtonFullPath);
                    //e.Graphics.DrawImage(closeImage,
                    //    (tabRect.Right - closeImage.Width),
                    //    tabRect.Top + (tabRect.Height - closeImage.Height) / 2);
                    TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font,
                        tabRect, tabPage.ForeColor, TextFormatFlags.Left);
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
