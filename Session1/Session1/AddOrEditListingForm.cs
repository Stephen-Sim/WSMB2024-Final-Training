using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session1
{
    public partial class AddOrEditListingForm : Form
    {
        public long UserID { get; set; }

        public AddOrEditListingForm(long UserID)
        {
            // adding
            InitializeComponent();
            ent = new WSC2022SE_Session1Entities();

            this.UserID = UserID;

            this.Text = "Seoul Stay - Add Lisitng";

            this.button2.Text = "Cancel";
        }

        public long? ItemID { get; set; }

        public AddOrEditListingForm(long UserID, long ItemID)
        {
            InitializeComponent();
            ent = new WSC2022SE_Session1Entities();

            this.UserID = UserID;
            this.ItemID = ItemID;
            
            this.Text = "Seoul Stay - Edit Lisitng";

            this.button1.Visible = false;
            this.button2.Text = "Close";
        }

        public WSC2022SE_Session1Entities ent { get; set; }

        void loadEditLisingDetails()
        {
            var item = ent.Items.FirstOrDefault(x => x.ID == this.ItemID);

            comboBox1.SelectedValue = item.ItemTypeID;
            textBox1.Text = item.Title;
            numericUpDown1.Value = item.Capacity;
            numericUpDown2.Value = item.NumberOfBeds;
            numericUpDown3.Value = item.NumberOfBedrooms;
            numericUpDown4.Value = item.NumberOfBathrooms;

            textBox2.Text = item.ApproximateAddress;
            textBox3.Text = item.ExactAddress;
            textBox4.Text = item.Description;
            textBox5.Text = item.HostRules;

            numericUpDown5.Value = item.MinimumNights;
            numericUpDown6.Value = item.MaximumNights;
        }

        void loadTables()
        {
            var amens = ent.Amenities.ToList();

            dataGridView1.Rows.Clear();

            var attracs = ent.Attractions.ToList();
            dataGridView2.Rows.Clear();

            if (ItemID != null)
            {
                foreach (var amen in amens)
                {
                    dataGridView1.Rows.Add(amen.ID, amen.Name, 
                        ent.ItemAmenities.Any(x => x.ItemID == this.ItemID && x.AmenityID == amen.ID));
                }

                foreach (var attrac in attracs)
                {
                    if (ent.ItemAttractions.Any(x => x.ItemID == this.ItemID && x.AttractionID == attrac.ID))
                    {
                        var itemAttract = ent.ItemAttractions.First(x => x.ItemID == this.ItemID 
                            && x.AttractionID == attrac.ID);
                        
                        dataGridView2.Rows.Add(attrac.ID, attrac.Name, attrac.Area.Name, 
                            itemAttract.Distance, 
                            itemAttract.DurationOnFoot,
                            itemAttract.DurationByCar);
                    }
                    else
                    {
                        dataGridView2.Rows.Add(attrac.ID, attrac.Name, attrac.Area.Name, null, null, null);
                    }
                }

            }
            else
            {
                foreach (var amen in amens)
                {
                    dataGridView1.Rows.Add(amen.ID, amen.Name, false);
                }

                foreach (var attrac in attracs)
                {
                    dataGridView2.Rows.Add(attrac.ID, attrac.Name, attrac.Area.Name, null, null, null);
                }
            }

        }

        private void AddOrEditListingForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wSC2022SE_Session1DataSet.ItemTypes' table. You can move, or remove it, as needed.
            this.itemTypesTableAdapter.Fill(this.wSC2022SE_Session1DataSet.ItemTypes);

            comboBox1.SelectedIndex = -1;

            if (ItemID != null)
            {
                loadEditLisingDetails();
            }

            loadTables();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Cancel")
            {
                this.Close();

                new UserManagmentForm(this.UserID).ShowDialog();
            }
            else if (button2.Text == "Finish")
            {
                if (dataGridView2.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[3] != null).Count() < 2)
                {
                    MessageBox.Show("The client needs to at least enter two attractions.");
                    return;
                }

                var item = new Item();
                item.GUID = Guid.NewGuid();
                item.UserID = this.UserID;
                item.AreaID = 1;
                item.ItemTypeID = (long)comboBox1.SelectedValue;
                item.Title = textBox1.Text;
                item.Capacity = (int)numericUpDown1.Value;
                item.NumberOfBeds = (int)numericUpDown2.Value;
                item.NumberOfBedrooms = (int)numericUpDown3.Value;
                item.NumberOfBathrooms = (int)numericUpDown4.Value;
                item.MinimumNights = (int)numericUpDown5.Value;
                item.MaximumNights = (int)numericUpDown6.Value;

                item.ApproximateAddress = textBox2.Text;
                item.ExactAddress = textBox3.Text;
                item.Description = textBox4.Text;
                item.HostRules = textBox5.Text;

                ent.Items.Add(item);

                ent.SaveChanges();

                ent.ItemAmenities.RemoveRange(ent.ItemAmenities.Where(x => x.ItemID == this.ItemID));

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (bool.Parse(row.Cells[2].Value.ToString()))
                    {
                        var itemAmen = new ItemAmenity()
                        {
                            GUID = Guid.NewGuid(),
                            AmenityID = long.Parse(row.Cells[0].Value.ToString()),
                            ItemID = item.ID
                        };

                        ent.ItemAmenities.Add(itemAmen);
                        ent.SaveChanges();
                    }
                }

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[3].Value != null && !string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                    {
                        var itemattrac = new ItemAttraction()
                        {
                            GUID = Guid.NewGuid(),
                            Distance = decimal.Parse(row.Cells[3].Value.ToString()),
                            ItemID = item.ID,
                            AttractionID = long.Parse(row.Cells[0].Value.ToString())
                        };

                        itemattrac.DurationOnFoot = row.Cells[4].Value == null || string.IsNullOrEmpty(row.Cells[4].Value.ToString()) ? null : (long?)long.Parse(row.Cells[4].Value.ToString());
                        itemattrac.DurationByCar = row.Cells[5].Value == null || string.IsNullOrEmpty(row.Cells[5].Value.ToString()) ? null : (long?)long.Parse(row.Cells[5].Value.ToString());

                        ent.ItemAttractions.Add(itemattrac);
                        ent.SaveChanges();
                    }
                }

                MessageBox.Show("Item is added");

                this.Hide();

                new UserManagmentForm(this.UserID).ShowDialog();
            }
            else if (button2.Text == "Close")
            {
                if (string.IsNullOrEmpty(textBox1.Text) ||
                    string.IsNullOrEmpty(textBox2.Text) ||
                    string.IsNullOrEmpty(textBox3.Text) ||
                    string.IsNullOrEmpty(textBox4.Text) ||
                    string.IsNullOrEmpty(textBox5.Text) ||
                    numericUpDown1.Value <= 0 ||
                    numericUpDown2.Value <= 0 ||
                    numericUpDown3.Value <= 0 ||
                    numericUpDown4.Value <= 0 ||
                    numericUpDown5.Value <= 0 ||
                    numericUpDown6.Value <= 0 || 
                    comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("All the fields are required.");
                    return;
                }

                if (dataGridView2.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[3] != null).Count() < 2)
                {
                    MessageBox.Show("The client needs to at least enter two attractions.");
                    return;
                }

                var item = ent.Items.FirstOrDefault(x => x.ID == this.ItemID);
                item.ItemTypeID = (long)comboBox1.SelectedValue;
                item.Title = textBox1.Text;
                item.Capacity = (int)numericUpDown1.Value;
                item.NumberOfBeds = (int)numericUpDown2.Value;
                item.NumberOfBedrooms = (int)numericUpDown3.Value;
                item.NumberOfBathrooms = (int)numericUpDown4.Value;
                item.MinimumNights = (int)numericUpDown5.Value;
                item.MaximumNights = (int)numericUpDown6.Value;

                item.ApproximateAddress = textBox2.Text;
                item.ExactAddress = textBox3.Text;
                item.Description = textBox4.Text;
                item.HostRules = textBox5.Text;

                ent.SaveChanges();

                ent.ItemAmenities.RemoveRange(ent.ItemAmenities.Where(x => x.ItemID == this.ItemID));

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (bool.Parse(row.Cells[2].Value.ToString()))
                    {
                        var itemAmen = new ItemAmenity() { 
                            GUID = Guid.NewGuid(),
                            AmenityID = long.Parse(row.Cells[0].Value.ToString()),
                            ItemID = item.ID
                        };

                        ent.ItemAmenities.Add(itemAmen);
                        ent.SaveChanges();
                    }
                }

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[3].Value != null && !string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                    {
                        var itemattrac = new ItemAttraction()
                        {
                            GUID = Guid.NewGuid(),
                            Distance = decimal.Parse(row.Cells[3].Value.ToString()),
                            ItemID = item.ID,
                            AttractionID = long.Parse(row.Cells[0].Value.ToString())
                        };

                        itemattrac.DurationOnFoot = string.IsNullOrEmpty(row.Cells[4].Value.ToString()) ? null : (long?) long.Parse(row.Cells[4].Value.ToString());
                        itemattrac.DurationByCar = string.IsNullOrEmpty(row.Cells[5].Value.ToString()) ? null : (long?) long.Parse(row.Cells[5].Value.ToString());

                        ent.ItemAttractions.Add(itemattrac);
                        ent.SaveChanges();
                    }
                }

                MessageBox.Show("Item is edited");

                this.Hide();

                new UserManagmentForm(this.UserID).ShowDialog();
            }
        }

        int prev_tabPage = 0;

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != prev_tabPage)
            {
                MessageBox.Show("the client could not change the tab page.");
                tabControl1.SelectedIndex = prev_tabPage;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (string.IsNullOrEmpty(textBox1.Text) ||
                    string.IsNullOrEmpty(textBox2.Text) ||
                    string.IsNullOrEmpty(textBox3.Text) ||
                    string.IsNullOrEmpty(textBox4.Text) ||
                    string.IsNullOrEmpty(textBox5.Text) ||
                    numericUpDown1.Value <= 0 ||
                    numericUpDown2.Value <= 0 ||
                    numericUpDown3.Value <= 0 ||
                    numericUpDown4.Value <= 0 ||
                    numericUpDown5.Value <= 0 ||
                    numericUpDown6.Value <= 0 ||
                    comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("All the fields are required.");
                    return;
                }

                if (numericUpDown6.Value < numericUpDown5.Value)
                {
                    MessageBox.Show("Maximum night could not lesster than minimum night.");
                    return;
                }

                prev_tabPage++;
                tabControl1.SelectedIndex = prev_tabPage;
            }
            else
            {
                prev_tabPage++;
                tabControl1.SelectedIndex = prev_tabPage;

                this.button1.Visible = false;
                this.button2.Text = "Finish";
            }
        }
    }
}
