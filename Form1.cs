using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace wr2_server
{
    public partial class Form1 : Form
    {
        string path = @"wr2_pc.exe";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random playerNumber = new Random();
            nicknameTextBox1.Text = "Player" + playerNumber.Next(1000, 9999);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = checkBox1.Checked ? true : false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = checkBox2.Checked ? true : false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Client
            if (nicknameTextBox1.Text == "" || textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please fill in the blanks!");
            }
            else
            {
                try
                {
                    //Start client with arguments
                    Process.Start(path, @"/host client /type LAN /ip " + textBox1.Text
                        + @" /port " + textBox2.Text
                        + @" /pname """ + nicknameTextBox1.Text
                        + @"""" + (textBox3.Text != "" ? @" /pwd """ + textBox3.Text + @"""" : ""));
                }
                catch (Exception) { MessageBox.Show("WR2_PC.EXE NOT FOUND!"); }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Host
            if (nicknameTextBox1.Text == "" || textBox5.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Please fill in the blanks!");
            }
            else
            {
                try
                {
                    //Start host with arguments
                    Process.Start(path, @"/host host /type LAN /mode Ded"
                        + (checkBox1.Checked ? @" /maxbeam " + numericUpDown1.Value : "")
                        + (checkBox2.Checked ? @" /maxshort " + numericUpDown2.Value : "")
                        + @" /maxplr " + numericUpDown3.Value
                        + @" /port " + textBox5.Text
                        + @" /sessname " + textBox6.Text
                        + @" /pname """ + nicknameTextBox1.Text
                        + @"""" + (textBox4.Text != "" ? @" /pwd """ + textBox4.Text + @"""" : ""));
                }
                catch (Exception) { MessageBox.Show("WR2_PC.EXE NOT FOUND!"); }
            }
        }

        private void sourceCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Berikai/wr2-server-connector");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
