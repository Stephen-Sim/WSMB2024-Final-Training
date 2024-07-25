using Open_Session4.Models;
using Open_Session4.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Open_Session4
{
    public partial class Form1 : Form
    {
        public List<Team> Teams { get; set; } = new List<Team>()
        {
            new Team() {rank = 1, name = "abc1", WDL = "3/6/0", point = 15, aggregatePoint = 1000},
            new Team() {rank = 2, name = "abc2", WDL = "3/6/0", point = 15, aggregatePoint = 1000},
            new Team() {rank = 3, name = "abc3", WDL = "3/6/0", point = 15, aggregatePoint = 1000},
            new Team() {rank = 4, name = "abc4", WDL = "3/6/0", point = 15, aggregatePoint = 1000},
            new Team() {rank = 5, name = "abc5", WDL = "3/6/0", point = 15, aggregatePoint = 1000},
            new Team() {rank = 6, name = "abc6", WDL = "3/6/0", point = 15, aggregatePoint = 1000},
            new Team() {rank = 7, name = "abc7", WDL = "3/6/0", point = 15, aggregatePoint = 1000},
            new Team() {rank = 8, name = "abc8", WDL = "3/6/0", point = 15, aggregatePoint = 1000},
            new Team() {rank = 9, name = "abc9", WDL = "3/6/0", point = 15, aggregatePoint = 1000},
            new Team() {rank = 10, name = "abc10", WDL = "3/6/0", point = 15, aggregatePoint = 1000},
        };

        public Form1()
        {
            InitializeComponent();
            loadData();
            loadPlayoff();
        }

        void loadData()
        {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < Teams.Count; i++) 
            {
                dataGridView1.Rows.Add(Teams[i].rank,
                    Teams[i].name, Teams[i].WDL,
                    Teams[i].point, Teams[i].aggregatePoint);

                if (Teams.Count < 16 && i >= 8)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                }
            }

            if (Teams.Count < 16)
            {
                label4.Text = "only 8 teams that continues to playoff";
            }
        }

        void loadPlayoff()
        {
            flowLayoutPanel1.Controls.Clear();
            var uc = new Playoff8Usercontrol();

            for (int i = 1; i <= 15; i++)
            {
                var c = uc.Controls.Find($"label{i}", true)[0];

                if (i % 2 == 1)
                {
                    c.Font = new Font("Arial", 8.75F, FontStyle.Bold);
                    c.Text = "Abc123   2";
                }
                else
                {
                    c.Font = new Font("Arial", 8.75F, FontStyle.Regular);
                    c.Text = "Abc123   0";
                }
            }

            flowLayoutPanel1.Controls.Add(uc);
        }
    }
}