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
        List<PCInfo> Computers = new List<PCInfo>();
        string saveFileName = "data.json";
        public MainForm()
        {
            InitializeComponent();
            backgroundWorker_LoadData.RunWorkerAsync(saveFileName);
        }

        private void LoadData(string filename)
        {
            //Fake LoadData
            PCInfo NewComputer = new PCInfo //Fake info
            {
                Name = "PC1",
                Ip = "192.168.1.100",
                Mac = "E0-D5-5E-48-82-B9",
                IpStart = "192.168.1.1",
                IpEnd = "192.168.1.255",
                GroupName = "Dev"
            };
            PCInfo NewComputer2 = new PCInfo //Fake info
            {
                Name = "PC2",
                Ip = "192.168.1.100",
                Mac = "E0-D5-5E-48-82-B9",
                IpStart = "192.168.1.1",
                IpEnd = "192.168.1.255",
                GroupName = "QA"
            };
            Computers.Add(NewComputer);
            Computers.Add(NewComputer2);
        }

        private void backgroundWorker_LoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            string filename = e.Argument.ToString();
            LoadData(filename);
        }
        private void backgroundWorker_LoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (var pc in Computers)
            {
                if (!tabControl_main.TabPages.ContainsKey(pc.GroupName))
                {
                    TabPage tabpageNew = new TabPage();
                    tabpageNew.Text = pc.GroupName;
                    tabpageNew.Name = pc.GroupName;
                    
                    var listviewNew = CreateListView();
                    tabpageNew.Controls.Add(listviewNew);

                    tabControl_main.TabPages.Add(tabpageNew);
                }
                
                var tabpage = tabControl_main.TabPages[tabControl_main.TabPages.IndexOfKey(pc.GroupName)];
                ListView listview = (ListView)tabpage.Controls[0];
                var item = new ListViewItem(new string[] { listview.Items.Count.ToString(), pc.Name, pc.Ip, pc.Mac});
                listview.Items.Add(item);
            }
        }
        private ListView CreateListView()
        {
            ListView listview = new ListView();
            ColumnHeader column_STT         = new ColumnHeader() {Text = "STT",         TextAlign = HorizontalAlignment.Center, Width = 50 };
            ColumnHeader column_Name        = new ColumnHeader() {Text = "Name",        TextAlign = HorizontalAlignment.Center, Width = 150 };
            ColumnHeader column_IP          = new ColumnHeader() {Text = "IP",          TextAlign = HorizontalAlignment.Center, Width = 120 };
            ColumnHeader column_Mac         = new ColumnHeader() {Text = "Mac",         TextAlign = HorizontalAlignment.Center, Width = 120 };
            ColumnHeader column_IPBroadcast = new ColumnHeader() {Text = "Broadcast",   TextAlign = HorizontalAlignment.Center, Width = 120 };
            ColumnHeader column_Status      = new ColumnHeader() {Text = "Status",      TextAlign = HorizontalAlignment.Center, Width = 60 };
            ColumnHeader column_Start       = new ColumnHeader() {Text = "Start",       TextAlign = HorizontalAlignment.Center, Width = 100 };
            listview.Columns.AddRange(new ColumnHeader[]{
                column_STT, 
                column_Name , 
                column_IP , 
                column_Mac , 
                column_IPBroadcast, 
                column_Status, 
                column_Start });
            listview.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right)));
            listview.FullRowSelect = true;
            listview.GridLines = true;
            listview.HideSelection = false;
            listview.Location = new System.Drawing.Point(0, 0);
            listview.Name = "listView";
            listview.Size = new System.Drawing.Size(342, 534);
            listview.TabIndex = 0;
            listview.UseCompatibleStateImageBehavior = false;
            listview.View = View.Details;
            return listview;
        }
        private void backgroundWorker_SaveData_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        private void button_AddNew_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
