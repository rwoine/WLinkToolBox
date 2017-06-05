using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WLinkToolBox
{

    public partial class Form1 : Form
    {

        String[] GL_pComPort_Str = SerialPort.GetPortNames();
        Queue<String> debugDataQueue = new Queue<String>();

        /* ************************************************************************************* */
        /* Constructor */
        /* ************************************************************************************* */
        public Form1()
        {
            InitializeComponent();

            debugDataQueue.Clear();
            timer1.Start();
        }

        /* ************************************************************************************* */
        /* Functions */
        /* ************************************************************************************* */

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Open & Close Serial Communication
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
                }

            } else
            {
                button1.Text = "Open";
                comboBoxPortComList.Enabled = true;

                serialPort1.Close();
            }
        }

        // Select the COM Port
        private void comboBoxPortComList_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
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
                }
                else
                {
                    comboBoxPortComList.Text = "";
                }
            }
        }

        // Get Data from Debug Source
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            debugDataQueue.Enqueue(sp.ReadExisting());
        }

        // Forward Data to Text Box
        private void timer1_Tick(object sender, EventArgs e)
        {
            while (debugDataQueue.Count != 0)
                this.textBoxDebug.AppendText(debugDataQueue.Dequeue());
        }

        // Clear Text Box
        private void button2_Click(object sender, EventArgs e)
        {
            this.textBoxDebug.Clear();
        }
    }
}
