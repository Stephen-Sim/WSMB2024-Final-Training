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
    public partial class UserManagmentForm : Form
    {
        public WSC2022SE_Session1Entities ent { get; set; }

        public long UserID { get; set; }

        public UserManagmentForm(long loginUserID)
        {
            ent = new WSC2022SE_Session1Entities();

            InitializeComponent();

            UserID = loginUserID;

            loadListings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UserID = 0;
            Properties.Settings.Default.IsEmployee = false;
            Properties.Settings.Default.Save();

            this.Hide();

            new Form1().ShowDialog();
        }

        void loadListings()
        {
            var listings = ent.Items.ToList().Select(x => new {
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

            label1.Text = $"{listings.Count()} items found";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var listings = ent.Items.ToList().Where(x => x.Title.StartsWith(textBox1.Text) ||
                x.Area.Name.StartsWith(textBox1.Text) || 
                x.ItemAttractions.Any(y => y.Distance < 1 && y.Attraction.Name.StartsWith(textBox1.Text))).Select(x => new {
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

            label1.Text = $"{listings.Count()} items found";
        }

        void loadOwnLisitings()
        {
            var listings = ent.Items.ToList().Where(x => x.UserID == this.UserID)
                .Select(x => new
                {
                    x.ID,
                    x.Title,
                    x.Capacity,
                    Area = x.Area.Name,
                    ItemType = x.ItemType.Name
                }).ToList();

            dataGridView2.Rows.Clear();

            foreach (var item in listings)
            {
                dataGridView2.Rows.Add(item.ID, item.Title, item.Capacity, item.Area, item.ItemType, "Edit Details");
            }

            label1.Text = $"{listings.Count()} items found.";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                loadListings();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                loadOwnLisitings();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();

            new AddOrEditListingForm(this.UserID).ShowDialog();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                var ItemID = long.Parse(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());

                this.Hide();

                new AddOrEditListingForm(this.UserID, ItemID).ShowDialog();
            }
        }
    }
}
