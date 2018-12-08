
/*
## Copyright (C) 2018 James Dervay <jamesdervay@gmail.com>
##
## This program is free software; you can redistribute it and/or modify
## it under the terms of the GNU General Public License as published by
## the Free Software Foundation; either version 3 of the License, or
## (at your option) any later version.
##
## This program is distributed in the hope that it will be useful,
## but WITHOUT ANY WARRANTY; without even the implied warranty of
## MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
## GNU General Public License for more details.
##
## You should have received a copy of the GNU General Public License
## along with this program; if not, see <http://www.gnu.org/licenses/>.
##
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;


namespace ADF5355_Interface
{
    public partial class Form1 : Form
    {
        bool isConnected = false;
        String[] ports;
        SerialPort port;
        String[] registers;
        String[] registersHop;
        int milliseconds = 50;
        string msDelay;
        string hopCycles;
        string txtRecieved;
        string txtStop = "STPH";
        string extTime;
        string extIntMin;

        public Form1()
        {
            InitializeComponent();
            disableControls();
            getAvailableComPorts();

            foreach(string port in ports)
            {
                comboBox1.Items.Add(port);
                Console.WriteLine(port);
                if (ports[0] != null)
                {
                    comboBox1.SelectedItem = ports[0];
                }
            }
        }
        private void connect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                connectToArduino();
            }
            else
            {
                disconnectFromArduino();
            }
        }

        void getAvailableComPorts()
        {
            ports = SerialPort.GetPortNames();
        }

        private void connectToArduino()
        {
            isConnected = true;
            string selectedPort = comboBox1.GetItemText(comboBox1.SelectedItem);
            port = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
            port.Open();
            port.Write("#STAR\n");
            Button_1.Text = "Disconnect";
            enableControls();
            textBox1.Text = "Connected";
        }

        private void disconnectFromArduino()
        {
            isConnected = false;
            port.Write("#STOP\n");
            port.Close();
            Button_1.Text = "Connect";
            disableControls();
            resetDefaults();
        }

        private void enableControls()
        {
            button1.Enabled = true;
            Button_1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;

        }

        private void disableControls()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
        }

        private void resetDefaults()
        {
            textBox1.Text = "";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                if (File.Exists(@"C:\Users\james\Desktop\ADFx35x_register_values.txt"))
                {
                    textBox1.Text = "Updating registers...";
                    registers = System.IO.File.ReadAllLines(@"C:\Users\james\Desktop\ADFx35x_register_values.txt");
                    port.Write("#REGS00" + registers[12] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS01" + registers[11] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS02" + registers[10] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS03" + registers[9] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS04" + registers[8] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS05" + registers[7] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS06" + registers[6] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS07" + registers[5] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS08" + registers[4] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS09" + registers[3] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS10" + registers[2] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS11" + registers[1] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#REGS12" + registers[0] + "#\n");
                    textBox1.Text = "Registers updated";
                }
                else
                {
                    textBox1.Text = "File not found";
                }
            }
        }
        private void button4_Click(object sender, EventArgs e) //Send initialize command
        {
            if (isConnected)
            {
                port.Write("#INIT\n");
                textBox1.Text = "Initialization sequence written";
            }
        }
        private void button5_Click(object sender, EventArgs e) // Send frequency update command
        {
            if (isConnected)
            {
                port.Write("#FREQ\n");
                textBox1.Text = "Frequency update sequence written";
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (isConnected)
            {
                if (File.Exists(@"C:\Users\james\Desktop\ADFx35x_register_values_hop.txt"))
                {
                    registersHop = System.IO.File.ReadAllLines(@"C:\Users\james\Desktop\ADFx35x_register_values_hop.txt");
                    port.Write("#HOPS00" + registersHop[12] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS01" + registersHop[11] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS02" + registersHop[10] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS03" + registersHop[9] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS04" + registersHop[8] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS05" + registersHop[7] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS06" + registersHop[6] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS07" + registersHop[5] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS08" + registersHop[4] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS09" + registersHop[3] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS10" + registersHop[2] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS11" + registersHop[1] + "#\n");
                    Thread.Sleep(milliseconds);
                    port.Write("#HOPS12" + registersHop[0] + "#\n");
                    textBox1.Text = "Hop registers updated";
                }
                else
                {
                    textBox1.Text = "File not found";
                }
            }
        }

        private void button6_Click(object sender, EventArgs e) // Send hop period value
        {
            if (isConnected)
            {
                msDelay = textBox2.Text;
                port.Write("#HOPD" + msDelay + "#\n");
                textBox1.Text = "Hop period (ms) set";
            }
        }

        private void button7_Click(object sender, EventArgs e) // Send hop cycles 
        {
            if (isConnected)
            {
                hopCycles = textBox3.Text;
                port.Write("#HOPC" + hopCycles + "#\n");
                textBox1.Text = "Number of hop cycles set";
            }
        }

        private void button3_Click_1(object sender, EventArgs e) // Begin bopping
        {
            if (isConnected)
            {
                textBox1.Text = "Hopping...";
                port.Write("#HOPB\n");
                txtRecieved = port.ReadExisting();
                while (!txtRecieved.Equals(txtStop))
                {
                    txtRecieved = port.ReadExisting();
                }
                textBox1.Text = "Hopping finished";
            }
        }

        private void button8_Click(object sender, EventArgs e) // Set ext sweep time
        {
            if (isConnected)
            {
                extTime = textBox4.Text;
                port.Write("#EXTT" + extTime + "#\n");
                textBox1.Text = "Sweep time duration set";
            }
        }

        private void button9_Click(object sender, EventArgs e) // Start ext sweep
        {
            if (isConnected)
            {
                textBox1.Text = "External sweeping...";
                port.Write("#EXTS\n");
                txtRecieved = port.ReadExisting();
                while (!txtRecieved.Equals(txtStop))             {
                    txtRecieved = port.ReadExisting();
                }
                textBox1.Text = "External sweeping finished";
            }
        }

        private void button10_Click(object sender, EventArgs e) //Set minimum integer of external sweep
        {
            if (isConnected)
            {
                extIntMin = textBox5.Text;
                port.Write("#EXTI" + extIntMin + "#\n");
                textBox1.Text = "Minimum integer of external sweep set";
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}