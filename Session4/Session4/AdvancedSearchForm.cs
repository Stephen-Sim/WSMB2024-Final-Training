using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Session4
{
    public partial class AdvancedSearchForm : Form
    {
        public AdvancedSearchForm()
        {
            InitializeComponent();

            ent = new WSC2022SE_Session4Entities();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            new Form1().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearForm();
        }

        void clearForm()
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;

            textBox1.Visible = true;
            textBox1.Text = string.Empty;
            textBox2.Visible = true;
            textBox2.Text = string.Empty;

            numericUpDown1.Value = 1;
            numericUpDown2.Value = 1;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;

            comboBox6.DataSource = null;
            comboBox7.DataSource = null;

            var amens = ent.Amenities.ToList();
            comboBox5.DataSource = amens;
            comboBox5.DisplayMember = "Name";
            comboBox5.ValueMember = "ID";
            comboBox5.SelectedIndex = -1;

            comboBox6.Enabled = false;
            comboBox7.Enabled = false;
        }

        public WSC2022SE_Session4Entities ent { get; set; }

        private void AdvancedSearchForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wSC2022SE_Session4DataSet.ItemTypes' table. You can move, or remove it, as needed.
            this.itemTypesTableAdapter.Fill(this.wSC2022SE_Session4DataSet.ItemTypes);
            // TODO: This line of code loads data into the 'wSC2022SE_Session4DataSet.Items' table. You can move, or remove it, as needed.
            this.itemsTableAdapter.Fill(this.wSC2022SE_Session4DataSet.Items);
            // TODO: This line of code loads data into the 'wSC2022SE_Session4DataSet.Attractions' table. You can move, or remove it, as needed.
            this.attractionsTableAdapter.Fill(this.wSC2022SE_Session4DataSet.Attractions);
            // TODO: This line of code loads data into the 'wSC2022SE_Session4DataSet.Areas' table. You can move, or remove it, as needed.
            this.areasTableAdapter.Fill(this.wSC2022SE_Session4DataSet.Areas);

            clearForm();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                textBox1.Visible = false;
                textBox2.Visible = false;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex != -1)
            {
                var items = comboBox5.Items.Cast<object>().ToList();
                items.Remove(comboBox5.SelectedItem);
                comboBox6.DataSource = items;
                comboBox6.DisplayMember = "Name";
                comboBox6.ValueMember = "ID";
                comboBox6.Enabled = true;
                comboBox6.SelectedIndex = -1;
            }
            else
            {
                comboBox6.Enabled = false;
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex != -1)
            {
                var items = comboBox6.Items.Cast<object>().ToList();
                items.Remove(comboBox6.SelectedItem);
                comboBox7.DataSource = items;
                comboBox7.DisplayMember = "Name";
                comboBox7.ValueMember = "ID";
                comboBox7.Enabled = true;
                comboBox7.SelectedIndex = -1;
            }
            else
            {
                comboBox7.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if (numericUpDown3.Value < 0 && numericUpDown4.Value < 0 
                && comboBox4.SelectedIndex != -1)
            {
                MessageBox.Show("price and type fields are required.");
                return;
            }

            if (dateTimePicker2.Value < dateTimePicker1.Value)
            {
                MessageBox.Show("from date could not greater than to date.");
                return;
            }

            if (numericUpDown3.Value > numericUpDown4.Value)
            {
                MessageBox.Show("staring price could not greater than the maximum price.");
                return;
            }

            var itemprices = ent.ItemPrices.Where(x => x.Item.ItemTypeID == (long)comboBox4.SelectedValue
                && x.Price >= numericUpDown3.Value && x.Price <= numericUpDown4.Value 
                && x.Date >= dateTimePicker1.Value && x.Date <= dateTimePicker2.Value
                && numericUpDown1.Value >= x.Item.MinimumNights
                && numericUpDown1.Value <= x.Item.MaximumNights
                && numericUpDown2.Value <= x.Item.Capacity).ToList();

            if (comboBox1.SelectedIndex != -1)
            {
                itemprices = itemprices.Where(x => x.Item.AreaID == (long)comboBox1.SelectedValue).ToList();
            }

            if (comboBox1.SelectedIndex == -1 && !string.IsNullOrEmpty(textBox1.Text)) 
            {
                itemprices = itemprices.Where(x => x.Item.ItemAttractions
                .Any(y => y.Attraction.Name.StartsWith(textBox1.Text))).ToList();
            }
            else if (textBox1.Visible == false && comboBox2.SelectedIndex != - 1)
            {
                itemprices = itemprices.Where(x => x.Item.ItemAttractions
                .Any(y => y.Attraction.ID == (long)comboBox2.SelectedValue)).ToList();
            }

            if (comboBox1.SelectedIndex == -1 && !string.IsNullOrEmpty(textBox2.Text))
            {
                itemprices = itemprices.Where(x => x.Item.Title
                .StartsWith(textBox2.Text)).ToList();
            }
            else if (textBox2.Visible == false && comboBox3.SelectedIndex != -1)
            {
                itemprices = itemprices.Where(x => x.Item.Title.StartsWith(textBox2.Text)).ToList();
            }

            if (comboBox5.SelectedIndex != -1)
            {
                itemprices = itemprices.Where(x => x.Item.ItemAmenities
               .Any(y => y.Amenity.ID == (long)comboBox5.SelectedValue)).ToList();
            }

            if (comboBox6.SelectedIndex != -1)
            {
                itemprices = itemprices.Where(x => x.Item.ItemAmenities
               .Any(y => y.Amenity.ID == (long)comboBox6.SelectedValue)).ToList();
            }

            if (comboBox7.SelectedIndex != -1)
            {
                itemprices = itemprices.Where(x => x.Item.ItemAmenities
               .Any(y => y.Amenity.ID == (long)comboBox7.SelectedValue)).ToList();
            }

            foreach (var item in itemprices)
            {
                dataGridView1.Rows.Add(item.Item.Title, item.Item.Area.Name,
                    new Func<string>(() =>
                    {
                        var itemScores = ent.ItemScores.Where(x => x.ItemID == item.Item.ID).ToList();

                        if (itemScores != null)
                        {
                            var avg = itemScores.Average(x => x.Value);

                            return avg.ToString("0.00");
                        }

                        return string.Empty;
                    })(),
                    ent.ItemPrices.Where(x => x.ItemID == item.ItemID
                        && x.Date < DateTime.Now
                        && x.BookingDetails.Count != 0).Count(),
                    item.Price.ToString("0.00 $"), item.Date.ToString("dd/MM/yyyy"));
            }

            label15.Text = $"Displaying {itemprices.Count} options from {itemprices.GroupBy(x => x.ItemID).Count()} properties.";
            this.Size = new Size(718, 558);
        }
    }
}
