using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session1
{
    public partial class EmployeeManagmentForm : Form
    {
        public long UserID { get; set; }

        public EmployeeManagmentForm(long userID)
        {
            InitializeComponent();
            this.UserID = userID;

            ent = new WSC2022SE_Session1Entities();

            var listings = ent.Items.ToList().Where(x => x.UserID == this.UserID)
               .Select(x => new
               {
                   x.ID,
                   x.Title,
                   x.Capacity,
                   Area = x.Area.Name,
                   ItemType = x.ItemType.Name
               }).ToList();

            dataGridView1.Rows.Clear();

            foreach (var item in listings)
            {
                dataGridView1.Rows.Add(item.Title, item.Capacity, item.Area, item.ItemType);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public WSC2022SE_Session1Entities ent { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UserID = 0;
            Properties.Settings.Default.IsEmployee = false;
            Properties.Settings.Default.Save();

            this.Hide();

            new Form1().ShowDialog();
        }

    }
}
