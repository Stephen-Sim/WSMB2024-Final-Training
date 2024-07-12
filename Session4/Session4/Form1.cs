using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session4
{
    public partial class Form1 : Form
    {
        public WSC2022SE_Session4Entities ent { get; set; }
       
        public Form1()
        {
            InitializeComponent();

            dateTimePicker1.MinDate = DateTime.Today.Date;

            ent = new WSC2022SE_Session4Entities();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 3)
            {
                listView1.Items.Clear();

                var areas = ent.Areas.ToList().Where(x => x.Name.StartsWith(textBox1.Text)).ToList();

                foreach (var item in areas)
                {
                    ListViewItem listViewItem = new ListViewItem(item.Name);
                    listViewItem.SubItems.Add("Area");

                    listView1.Items.Add(listViewItem);
                }

                var attracs = ent.Attractions.ToList().Where(x => x.Name.StartsWith(textBox1.Text)).ToList();

                foreach (var item in attracs)
                {
                    ListViewItem listViewItem = new ListViewItem(item.Name);
                    listViewItem.SubItems.Add("Attraction");

                    listView1.Items.Add(listViewItem);
                }

                var listings = ent.Items.ToList().Where(x => x.Title.StartsWith(textBox1.Text)).ToList();

                foreach (var item in listings)
                {
                    ListViewItem listViewItem = new ListViewItem(item.Title);
                    listViewItem.SubItems.Add("Listing");

                    listView1.Items.Add(listViewItem);
                }

                var listingTypes = ent.ItemTypes.ToList().Where(x => x.Name.StartsWith(textBox1.Text)).ToList();

                foreach (var item in listingTypes)
                {
                    ListViewItem listViewItem = new ListViewItem(item.Name);
                    listViewItem.SubItems.Add("Listing Type");

                    listView1.Items.Add(listViewItem);
                }

                var amens = ent.Amenities.ToList().Where(x => x.Name.StartsWith(textBox1.Text)).ToList();

                foreach (var item in amens)
                {
                    ListViewItem listViewItem = new ListViewItem(item.Name);
                    listViewItem.SubItems.Add("Amenity");

                    listView1.Items.Add(listViewItem);
                }

                listView1.Visible = true;
            }
            else 
            {
                listView1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Text field is required.");
                return;
            }

            dataGridView1.Rows.Clear();

            var items = ent.ItemPrices.Where(x => x.Date == dateTimePicker1.Value.Date
                && numericUpDown1.Value >= x.Item.MinimumNights 
                && numericUpDown1.Value <= x.Item.MaximumNights
                && numericUpDown2.Value <= x.Item.Capacity).OrderBy(x => x.Item.Title).ToList();

            switch (typeID)
            {
                case 1:
                    items = items.Where(x => x.Item.Area.Name == textBox1.Text).ToList();
                    break;
                case 2:
                    items = items.Where(x => x.Item.ItemAttractions.Any(y => y.Attraction.Name == textBox1.Text)).ToList();
                    break;
                case 3:
                    items = items.Where(x => x.Item.Title == textBox1.Text).ToList();
                    break;
                case 4:
                    items = items.Where(x => x.Item.ItemType.Name == textBox1.Text).ToList();
                    break;
                case 5:
                    items = items.Where(x => x.Item.ItemAmenities.Any(y => y.Amenity.Name == textBox1.Text)).ToList();
                    break;
                default:
                    break;
            }

            foreach (var item in items)
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
                    item.Price.ToString("0.00 $"));
            }

            label6.Text = $"Displaying {items.Count} options.";

            this.Size = new Size(658, 429);
        }

        public int? typeID { get; set; }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = listView1.SelectedItems[0];

            textBox1.Text = item.Text;

            switch (item.SubItems[0].Text)
            {
                case "Area":
                    typeID = 1;
                    break;
                case "Attraction":
                    typeID = 2;
                    break;
                case "Listing":
                    typeID = 3;
                    break;
                case "Listing Type":
                    typeID = 4;
                    break;
                case "Amenity":
                    typeID = 5;
                    break;
                default:
                    break;
            }

            listView1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();

            new AdvancedSearchForm().ShowDialog();
        }
    }
}
