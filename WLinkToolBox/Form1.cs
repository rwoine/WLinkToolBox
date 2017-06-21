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
using WLinkToolBox.WLinkApp;

namespace WLinkToolBox
{

    public partial class Form1 : Form
    {

        String[] GL_pComPort_Str = SerialPort.GetPortNames();
        Queue<String> debugDataQueue = new Queue<String>();

        List<WCommand> WCommandList = new List<WCommand>();

        /* ************************************************************************************* */
        /* Constructor */
        /* ************************************************************************************* */
        public Form1()
        {
            InitializeComponent();

            debugDataQueue.Clear();
            timer1.Start();

            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_GET_REVISION_ID));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_GPIO_READ));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_GPIO_WRITE));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_GPIO_SET_BIT));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_GPIO_CLR_BIT));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_INDICATOR_GET_WEIGHT));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_INDICATOR_GET_WEIGHT_ALIBI));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_INDICATOR_SET_ZERO));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_INDICATOR_GET_WEIGHT_ASCII));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_BADGE_READER_GET_ID));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_LCD_WRITE));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_LCD_READ));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_LCD_CLEAR));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_LCD_SET_BACKLIGHT));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_LCD_ENABLE_EXT_WRITE));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_LCD_DISABLE_EXT_WRITE));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_LCD_GET_EXT_WRITE_STATUS));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_LCD_GET_EXT_WRITE_DATA));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_EEPROM_WRITE));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_EEPROM_READ));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_RTC_SET_DATETIME));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_RTC_GET_DATETIME));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_COMPORT_OPEN));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_COMPORT_CLOSE));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_COMPORT_WRITE));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_COMPORT_ENABLE_TUNNEL));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_COMPORT_DISABLE_TUNNEL));
            WCommandList.Add(new WCommand(WCommand.WCMD_ID_ENUM.WCMD_TEST_CMD));

            for (int i = 0; i < WCommandList.Count; i++)
                comboBoxCommandId.Items.Add(WCommandList[i]);
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


        // Command ID
        private void comboBoxCommandId_SelectedIndexChanged(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(2);
            sb.AppendFormat("0x{0:x2}", ((WCommand)(comboBoxCommandId.SelectedItem)).getIdValue());
            textBoxIdValue.Text = sb.ToString();
        }
    }
}
