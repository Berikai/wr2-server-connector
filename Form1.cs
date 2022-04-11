using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

        public void checkConn()
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    tcpClient.Connect(textBox1.Text, int.Parse(textBox2.Text));
                    label10.Invoke((MethodInvoker)(() => label10.Visible = true));
                }
                catch (Exception) { }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var MplIni = new IniFile("mpl.ini");

            var Hostname = MplIni.Read("Hostname", "Host");
            var Playername = MplIni.Read("Playername", "Host");
            var Port = MplIni.Read("Port", "Host");
            var MaxShortcut = MplIni.Read("MaxShortcut", "Host");
            var MaxBeam = MplIni.Read("MaxBeam", "Host");
            var MaxPlayer = MplIni.Read("MaxPlayer", "Host");
            var MaxShortCheck = MplIni.Read("MaxShortCheck", "Host");
            var MaxBeamCheck = MplIni.Read("MaxBeamCheck", "Host");
            var HostClient = MplIni.Read("Host", "Client");
            var PortClient = MplIni.Read("Port", "Client");

            Random playerNumber = new Random();

            textBox6.Text = Hostname;
            nicknameTextBox1.Text = (Playername == "") ? "Player" + playerNumber.Next(1000, 9999) : Playername;
            textBox5.Text = (Port == "") ? "54323" : Port;
            numericUpDown1.Value = (MaxShortcut == "") ? 2 : (int.Parse(MaxShortcut) < 2 || int.Parse(MaxShortcut) > 30) ? 2 : int.Parse(MaxShortcut);
            numericUpDown2.Value = (MaxBeam == "") ? 2 : (int.Parse(MaxBeam) < 2 || int.Parse(MaxBeam) > 10) ? 2 : int.Parse(MaxBeam);
            numericUpDown3.Value = (MaxPlayer == "") ? 6 : (int.Parse(MaxPlayer) < 2 || int.Parse(MaxPlayer) > 6) ? 6 : int.Parse(MaxPlayer);
            checkBox1.Checked = (MaxShortCheck == "") ? false : (MaxShortCheck != "0" || MaxShortCheck != "1") ? false : Convert.ToBoolean(int.Parse(MaxShortCheck));
            checkBox2.Checked = (MaxBeamCheck == "") ? true : (MaxBeamCheck != "0" || MaxBeamCheck != "1") ? true : Convert.ToBoolean(int.Parse(MaxShortCheck));
            textBox1.Text = HostClient;
            textBox2.Text = (PortClient == "") ? "54323" : PortClient;

            label10.Visible = false;

            Thread checkPort = new Thread(checkConn);
            checkPort.Start();
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
            try
            {
                var MplIni = new IniFile("mpl.ini");
                MplIni.Write("Host", textBox1.Text, "Client");
                MplIni.Write("Port", textBox2.Text, "Client");
            } catch (Exception) { }

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
            try
            {
                var MplIni = new IniFile("mpl.ini");
                MplIni.Write("Hostname", textBox6.Text, "Host");
                MplIni.Write("Playername", nicknameTextBox1.Text, "Host");
                MplIni.Write("Port", textBox5.Text, "Host");
                MplIni.Write("MaxShortcut", numericUpDown1.Value.ToString(), "Host");
                MplIni.Write("MaxBeam", numericUpDown2.Value.ToString(), "Host");
                MplIni.Write("MaxPlayer", numericUpDown3.Value.ToString(), "Host");
                MplIni.Write("MaxShortCheck", checkBox1.Checked ? "1" : "0", "Host");
                MplIni.Write("MaxBeamCheck", checkBox2.Checked ? "1" : "0", "Host");
            } catch (Exception) { }

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
                        + (checkBox2.Checked ? @" /maxbeam " + numericUpDown2.Value : "")
                        + (checkBox1.Checked ? @" /maxshort " + numericUpDown1.Value : "")
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var MplIni = new IniFile("mpl.ini");
                MplIni.Write("Hostname", textBox6.Text, "Host");
                MplIni.Write("Playername", nicknameTextBox1.Text, "Host");
                MplIni.Write("Port", textBox5.Text, "Host");
                MplIni.Write("MaxShortcut", numericUpDown1.Value.ToString(), "Host");
                MplIni.Write("MaxBeam", numericUpDown2.Value.ToString(), "Host");
                MplIni.Write("MaxPlayer", numericUpDown3.Value.ToString(), "Host");
                MplIni.Write("MaxShortCheck", checkBox1.Checked ? "1" : "0", "Host");
                MplIni.Write("MaxBeamCheck", checkBox2.Checked ? "1" : "0", "Host");
                MplIni.Write("Host", textBox1.Text, "Client");
                MplIni.Write("Port", textBox2.Text, "Client");

                MessageBox.Show("Settings has been saved successfuly!");
            } catch (Exception err) 
            {
                MessageBox.Show("Couldn't save the settings. Error: " + err.Message);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                textBox5.Text = "";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox2.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                textBox2.Text = "";
            } else
            {
                label10.Visible = false;

                Thread checkPort = new Thread(checkConn);
                checkPort.Start();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label10.Visible = false;

            Thread checkPort = new Thread(checkConn);
            checkPort.Start();
        }
    }
}
