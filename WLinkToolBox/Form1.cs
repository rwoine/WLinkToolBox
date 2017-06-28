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
        WCommand WCommandCurrent;

        byte[] WResponseByteArray = new byte[256];
        int ResponseIndex = 0;
        int ResponseState = 0;

        WResponse WResponseCurrent = new WResponse();

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

            WCommandCurrent = new WCommand(WCommand.WCMD_ID_ENUM.WCMD_UNKNOWN);

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
            int i = 0;
            int length = serialPort1.BytesToRead;
            byte[] bytes = new byte[length];
            byte singleByte = 0x00;

            while (length != 0)
            {
                // Get one byte to test
                singleByte = (byte)serialPort1.ReadByte();

                // Check if response has been sent
                if ((ResponseState == 0) && (singleByte == 0x02))
                {
                    ResponseState = 1;
                    ResponseIndex = 0;
                }


                switch (ResponseState)
                {
                    // STX
                    case 1:
                        WResponseByteArray[ResponseIndex++] = singleByte;
                        ResponseState++;
                        break;

                    // ID
                    case 2:
                        WResponseByteArray[ResponseIndex++] = singleByte;
                        ResponseState++;
                        break;

                    // Status
                    case 3:
                        WResponseByteArray[ResponseIndex++] = singleByte;
                        if ((WResponseByteArray[1] & 0x80) == 0x80)
                            ResponseState++;    // Get Lentgh and Data
                        else
                            ResponseState = 6;  // Get ACK
                        break;

                    // Length
                    case 4:
                        WResponseByteArray[ResponseIndex++] = singleByte;
                        ResponseState++;
                        break;

                    // Data
                    case 5:
                        WResponseByteArray[ResponseIndex++] = singleByte;
                        if ((ResponseIndex - 4) == WResponseByteArray[3])
                            ResponseState++;
                        break;

                    // ACK
                    case 6:
                        WResponseByteArray[ResponseIndex++] = singleByte;
                        ResponseState++;
                        break;


                    // ETX
                    case 7:
                        WResponseByteArray[ResponseIndex++] = singleByte;

                        WResponseCurrent = new WResponse(WResponseByteArray, ResponseIndex);

                        //textBoxResponse.Text = "";
                        //for (int j = 0; j < ReponseIndex; j++)
                        //{
                        //    StringBuilder sb1 = new StringBuilder(2);
                        //    sb1.AppendFormat("0x{0:X2}", WResponseByteArray[j]);

                        //    textBoxResponse.Text += sb1.ToString();
                        //    textBoxResponse.Text += ' ';
                        //}

                        ResponseState = 0;  // Reset state machine
                        break;


                    default: bytes[i++] = singleByte;   break;
                }
                
                
                length--;
            }

            debugDataQueue.Enqueue(System.Text.Encoding.ASCII.GetString(bytes));
        }

        // Forward Data to Text Box
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Get Debug data
            while (debugDataQueue.Count != 0)
            {
                textBoxDebug.AppendText(debugDataQueue.Dequeue());
            }

            // Get Response data
            if(!WResponseCurrent.isEmpty())
            {
                textBoxResponse.Text = "";
                for (int j = 0; j < ResponseIndex; j++)
                {
                    StringBuilder sb1 = new StringBuilder(2);
                    sb1.AppendFormat("0x{0:X2}", WResponseByteArray[j]);

                    textBoxResponse.Text += sb1.ToString();
                    textBoxResponse.Text += ' ';
                }
            }

            
        }

        // Clear Text Box
        private void button2_Click(object sender, EventArgs e)
        {
            this.textBoxDebug.Clear();
        }


        // Command ID
        private void comboBoxCommandId_SelectedIndexChanged(object sender, EventArgs e)
        {
            WCommandCurrent = (WCommand)(comboBoxCommandId.SelectedItem);
            checkBox1.Enabled = true;
            StringBuilder sb = new StringBuilder(2);
            sb.AppendFormat("0x{0:x2}", WCommandCurrent.getIdValue());
            textBoxIdValue.Text = sb.ToString();

            UpdateCommand();
        }

        // Parameters check box
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(((CheckBox)(sender)).Checked)
            {
                textBoxParamNb.Text = "0x00";
                textBoxParam.Enabled = true;


            } else
            {
                textBoxParamNb.Text = "0x00";
                textBoxParam.Enabled = false;
                textBoxParam.Text = "";
                textBoxParam.BackColor = Color.White;
            }
        }

        private void textBoxParam_TextChanged(object sender, EventArgs e)
        {
            UpdateCommand();
        }



        private void buttonSendCommand_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(WCommandCurrent.toByteAray().ToArray(), 0, WCommandCurrent.toByteAray().Count);
                WResponseCurrent = new WResponse(); // empty response

                
            }
        }





        /* ************************************************************************************* */
        /* Inner Functions */
        /* ************************************************************************************* */
        private int GetHexVal(char hex)
        {
            int val = (int)hex;

            if((val >= 0x30) && (val <= 0x39))          // 0 - 9
            {
                return (val - 0x30);
            } else if((val >= 0x41) && (val <= 0x46))   // A - F
            {
                return (val - 0x41 + 10);
            } else if((val >= 0x61) && (val <= 0x66))   // a - f
            {
                return (val - 0x61 + 10);
            }
            else
            {
                return 0;
            }
        }

        private void UpdateCommand()
        {
            Boolean NoError_B = true;
            textBoxParam.BackColor = Color.White;

            Boolean HasParam_B = (textBoxParam.Text.Length > 1) ? true : false;


            string[] WParams = textBoxParam.Text.Split(' ');
            byte[] WParamByte = new byte[WParams.Length];


            if (HasParam_B)
            {
                int i = 0;
                foreach (string WParam in WParams)
                {
                    if (NoError_B)
                    {
                        if (WParam.Length == 2)
                        {
                            WParamByte[i] = (byte)((GetHexVal(WParam.ToCharArray()[0]) << 4) + (GetHexVal(WParam.ToCharArray()[1])));
                            i++;
                        }
                        else
                        {
                            NoError_B = false;
                        }

                    }
                }
            }

            if(NoError_B)
            {
                WCommandCurrent.clearAllParam();

                if (HasParam_B)
                {
                    WCommandCurrent.addParam(WParamByte);
                    StringBuilder sb = new StringBuilder(2);
                    sb.AppendFormat("0x{0:X2}", WCommandCurrent.getParamCount());
                    textBoxParamNb.Text = sb.ToString();
                }
                else
                {
                    textBoxParamNb.Text = "0x00";
                }

                textBoxWCommand.Text = "";
                foreach (byte Value in WCommandCurrent.toByteAray())
                {
                    StringBuilder sb1 = new StringBuilder(2);
                    sb1.AppendFormat("0x{0:X2}", Value);

                    textBoxWCommand.Text += sb1.ToString();
                    textBoxWCommand.Text += ' ';
                }
            }


            if (!NoError_B)
                textBoxParam.BackColor = Color.LightPink;
        }


    }
}
