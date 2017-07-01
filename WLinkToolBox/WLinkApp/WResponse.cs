using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLinkToolBox.WLinkApp
{
    class WResponse
    {
        byte[] _byteArray;
        int _length;
        Boolean _isEmpty;

        /* ************************************************************************************* */
        /* Constructor */
        /* ************************************************************************************* */
        public WResponse()
        {
            this._byteArray = new byte[256];
            this._length = 0;
            this._isEmpty = true;
        }

        public WResponse(byte[] byteArray, int length)
        {
            this._byteArray = new byte[length];
            this._byteArray = byteArray;
            this._length = length;
            this._isEmpty = false;
        }


        /* ************************************************************************************* */
        /* Methods */
        /* ************************************************************************************* */
        public override String ToString()
        {
            String retString = "";

            switch ((WCommand.WCMD_ID_ENUM)((this._byteArray[1]) & 0x7F))
            {
                case WCommand.WCMD_ID_ENUM.WCMD_GET_REVISION_ID: retString = "GetRevisionId"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_GPIO_READ: retString = "GpioRead"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_GPIO_WRITE: retString = "GpioWrite"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_GPIO_SET_BIT: retString = "GpioSetbit"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_GPIO_CLR_BIT: retString = "GpioClrbit"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_INDICATOR_GET_WEIGHT: retString = "IndicatorGetWeight"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_INDICATOR_GET_WEIGHT_ALIBI: retString = "IndicatorGetWeightAlibi"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_INDICATOR_SET_ZERO: retString = "IndicatorSetZero"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_INDICATOR_GET_WEIGHT_ASCII: retString = "IndicatorGetWeightAscii"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_BADGE_READER_GET_ID: retString = "BadgereaderGetBadgeId"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_LCD_WRITE: retString = "LcdWrite"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_LCD_READ: retString = "LcdRead"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_LCD_CLEAR: retString = "LcdClear"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_LCD_SET_BACKLIGHT: retString = "LcdSetBacklight"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_LCD_ENABLE_EXT_WRITE: retString = "LcdEnableExternalWrite"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_LCD_DISABLE_EXT_WRITE: retString = "LcdDisableExternalWrite"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_LCD_GET_EXT_WRITE_STATUS: retString = "LcdGetExternalWriteStatus"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_LCD_GET_EXT_WRITE_DATA: retString = "LcdGetExternalWriteData"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_EEPROM_WRITE: retString = "EepromWrite"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_EEPROM_READ: retString = "EepromRead"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_RTC_SET_DATETIME: retString = "RtcSetDateTime"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_RTC_GET_DATETIME: retString = "RtcGetDateTime"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_COMPORT_OPEN: retString = "ComPortOpen"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_COMPORT_CLOSE: retString = "ComPortClose"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_COMPORT_WRITE: retString = "ComPortWrite"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_COMPORT_ENABLE_TUNNEL: retString = "ComPortEnableTunnel"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_COMPORT_DISABLE_TUNNEL: retString = "ComPortDisableTunnel"; break;
                case WCommand.WCMD_ID_ENUM.WCMD_TEST_CMD: retString = "TestCommand"; break;
                default: retString = "N.A."; break;
            }

            return retString;
        }


        public Boolean isEmpty()
        {
            return this._isEmpty;
        }

        public Byte getIdValue()
        {
            return (Byte)((this._byteArray[1]) & 0x7F);     // Mask 'hasParam' bit
        }

        public Byte getStatus()
        {
            return (Byte)(this._byteArray[2]);
        }

        public Byte getAck()
        {
            if (!hasParam())
                return (Byte)(this._byteArray[3]);
            else
                return (Byte)(this._byteArray[getParamCount() + 4]);
        }

        public Boolean hasParam()
        {
            if (((this._byteArray[1]) & 0x80) == 0x80)
                return true;
            else
                return false;
        }

        public int getParamCount()
        {
            if (!hasParam())
                return 0;
            else
                return ((int)(this._byteArray[3]));
        }

        public byte[] getParam()
        {
            byte[] paramArray;

            if (!hasParam())
            {
                paramArray = new byte[1];
                paramArray[0] = 0x00;
            }
            else
            {
                paramArray = new byte[getParamCount()];
                for (int i = 0; i < getParamCount(); i++)
                    paramArray[i] = this._byteArray[i + 4];
            }

            return paramArray;
        }

    }
}
