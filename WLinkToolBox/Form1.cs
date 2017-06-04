using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WLinkToolBox
{

    public partial class Form1 : Form
    {

        String[] GL_pComPort_Str = SerialPort.GetPortNames();

        public Form1()
        {
            InitializeComponent();

            // Setup Debug interface
            comboBoxPortComList.Items.Clear();
            comboBoxPortComList.Items.AddRange(GL_pComPort_Str);
            if (GL_pComPort_Str.Length != 0)
            {
                comboBoxPortComList.SelectedIndex = 0;
                serialPort1.PortName = GL_pComPort_Str[comboBoxPortComList.SelectedIndex];
            }
            else
            {
                comboBoxPortComList.Text = "";
            }

            timerRefreshCom.Start();

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text == "Open")
            {
                if (GL_pComPort_Str.Length != 0)
                {
                    button1.Text = "Close";
                    serialPort1.PortName = GL_pComPort_Str[comboBoxPortComList.SelectedIndex];
                    comboBoxPortComList.Enabled = false;

                    serialPort1.Open();
                    timerRefreshCom.Stop();
                }

            } else
            {
                button1.Text = "Open";
                comboBoxPortComList.Enabled = true;

                serialPort1.Close();
                timerRefreshCom.Start();
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            Console.Write(sp.ReadExisting());
            //textBoxDebug.AppendText(sp.ReadExisting());
        }

        private void timerRefreshCom_Tick(object sender, EventArgs e)
        {
            if(!serialPort1.IsOpen)
            {
                if (GL_pComPort_Str.Length != SerialPort.GetPortNames().Length)
                {
                    // Refresh Port COM
                    GL_pComPort_Str = SerialPort.GetPortNames();

                    // Setup Debug interface
                    comboBoxPortComList.Items.Clear();
                    comboBoxPortComList.Items.AddRange(GL_pComPort_Str);
                    if (GL_pComPort_Str.Length != 0)
                    {
                        comboBoxPortComList.SelectedIndex = 0;
                        serialPort1.PortName = GL_pComPort_Str[comboBoxPortComList.SelectedIndex];
                    } else
                    {
                        comboBoxPortComList.Text = "";
                    }
                }
            }
        }
    }
}
