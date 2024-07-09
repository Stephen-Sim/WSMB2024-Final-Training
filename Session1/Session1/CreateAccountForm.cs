using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Session1
{
    public partial class CreateAccountForm : Form
    {
        public WSC2022SE_Session1Entities ent { get; set; }

        public CreateAccountForm()
        {
            InitializeComponent();
            ent = new WSC2022SE_Session1Entities();

            radioButton1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();

            new Form1().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) ||
                string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text) ||
                string.IsNullOrEmpty(textBox4.Text) ||
                numericUpDown1.Value <= 0 ||
                !checkBox1.Checked)
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (textBox3.Text.Length < 5)
            {
                MessageBox.Show("The length of the password needs to be at least five characters.");
                return;
            }

            if (textBox3.Text != textBox4.Text)
            {
                MessageBox.Show("The retype password is not matched.");
                return;
            }

            if (ent.Users.Any(x => x.Username == textBox1.Text))
            {
                MessageBox.Show("The username is already register in the system.");
                return;
            }

            var user = new User()
            {
                Username = textBox1.Text,
                Password = textBox3.Text,
                FullName = textBox2.Text,
                BirthDate = dateTimePicker1.Value,
                FamilyCount = (int)numericUpDown1.Value,
                Gender = radioButton1.Checked,
                UserTypeID = 2,
                GUID = Guid.NewGuid(),
            };

            ent.Users.Add(user);
            ent.SaveChanges();

            MessageBox.Show("User account is created.");

            this.Hide();

            new UserManagmentForm(user.ID).ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string text = File.ReadAllText("Terms.txt");
            MessageBox.Show(text);
            isRead = true;
        }

        bool isRead = false;

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (!isRead)
            {
                MessageBox.Show("You have to at least view terms and conditions once");
                checkBox1.Checked = false;
            }
        }
    }
}
