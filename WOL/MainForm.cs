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
                    tabControl_main.TabPages.Add(tabpageNew);
                }
                else
                {
                    var tabpage = tabControl_main.TabPages[tabControl_main.TabPages.IndexOfKey(pc.GroupName)];


                }
            }
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
