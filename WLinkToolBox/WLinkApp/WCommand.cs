using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLinkToolBox.WLinkApp
{
    class WCommand
    {
        WCMD_ID_ENUM _id = 0x00;
        List<Byte> _paramList;

        public static readonly IList<String> WCommandListString = new ReadOnlyCollection<string>(new List<String> {
            "GetRevisionId",
            "GpioRead",
            "GpioWrite",
            "GpioSetbit",
            "GpioClrbit",
            "IndicatorGetWeight",
            "IndicatorGetWeightAlibi",
            "IndicatorSetZero",
            "IndicatorGetWeightAscii",
            "BadgereaderGetBadgeId",
            "LcdWrite",
            "LcdRead",
            "LcdClear",
            "LcdSetBacklight",
            "LcdEnableExternalWrite",
            "LcdDisableExternalWrite",
            "LcdGetExternalWriteStatus",
            "LcdGetExternalWriteData",
            "EepromWrite",
            "EepromRead",
            "ComPortOpen",
            "ComPortClose",
            "ComPortWrite", 
            "ComPortEnableTunnel",
            "ComPortDisableTunnel"
            });

        public enum WCMD_ID_ENUM
        {
            WCMD_UNKNOWN = 0x7F,
            WCMD_GET_REVISION_ID = 0x00,
            WCMD_GPIO_READ = 0x01,
            WCMD_GPIO_WRITE = 0x02,
            WCMD_GPIO_SET_BIT = 0x03,
            WCMD_GPIO_CLR_BIT = 0x04,
            WCMD_INDICATOR_GET_WEIGHT = 0x11,
            WCMD_INDICATOR_GET_WEIGHT_ALIBI = 0x12,
            WCMD_INDICATOR_SET_ZERO = 0x13,
            WCMD_INDICATOR_GET_WEIGHT_ASCII = 0x14,
            WCMD_BADGE_READER_GET_ID = 0x21,
            WCMD_LCD_WRITE = 0x30,
            WCMD_LCD_READ = 0x31,
            WCMD_LCD_CLEAR = 0x32,
            WCMD_LCD_SET_BACKLIGHT = 0x33,
            WCMD_LCD_ENABLE_EXT_WRITE = 0x34,
            WCMD_LCD_DISABLE_EXT_WRITE = 0x35,
            WCMD_LCD_GET_EXT_WRITE_STATUS = 0x36,
            WCMD_LCD_GET_EXT_WRITE_DATA = 0x37,
            WCMD_EEPROM_WRITE = 0x40,
            WCMD_EEPROM_READ = 0x41,
            WCMD_RTC_SET_DATETIME = 0x48,
            WCMD_RTC_GET_DATETIME = 0x49,
            WCMD_COMPORT_OPEN = 0x50,
            WCMD_COMPORT_CLOSE = 0x51,
            WCMD_COMPORT_WRITE = 0x52,
            WCMD_COMPORT_ENABLE_TUNNEL = 0x55,
            WCMD_COMPORT_DISABLE_TUNNEL = 0x56,
            WCMD_TEST_CMD = 0x70
        };


        /* ************************************************************************************* */
        /* Constructor */
        /* ************************************************************************************* */
        public WCommand(WCMD_ID_ENUM id)
        {
            this._id = id;
            _paramList = new List<Byte>();
        }

        /* ************************************************************************************* */
        /* Methods */
        /* ************************************************************************************* */
        public override String ToString()
        {
            String retString = "";

            switch(this._id)
            {
                case WCMD_ID_ENUM.WCMD_GET_REVISION_ID:                 retString = "GetRevisionId";                break;
                case WCMD_ID_ENUM.WCMD_GPIO_READ:                       retString = "GpioRead";                     break;
                case WCMD_ID_ENUM.WCMD_GPIO_WRITE:                      retString = "GpioWrite";                    break;
                case WCMD_ID_ENUM.WCMD_GPIO_SET_BIT:                    retString = "GpioSetbit";                   break;
                case WCMD_ID_ENUM.WCMD_GPIO_CLR_BIT:                    retString = "GpioClrbit";                   break;
                case WCMD_ID_ENUM.WCMD_INDICATOR_GET_WEIGHT:            retString = "IndicatorGetWeight";           break;
                case WCMD_ID_ENUM.WCMD_INDICATOR_GET_WEIGHT_ALIBI:      retString = "IndicatorGetWeightAlibi";      break;
                case WCMD_ID_ENUM.WCMD_INDICATOR_SET_ZERO:              retString = "IndicatorSetZero";             break;
                case WCMD_ID_ENUM.WCMD_INDICATOR_GET_WEIGHT_ASCII:      retString = "IndicatorGetWeightAscii";      break;
                case WCMD_ID_ENUM.WCMD_BADGE_READER_GET_ID:             retString = "BadgereaderGetBadgeId";        break;
                case WCMD_ID_ENUM.WCMD_LCD_WRITE:                       retString = "LcdWrite";                     break;
                case WCMD_ID_ENUM.WCMD_LCD_READ:                        retString = "LcdRead";                      break;
                case WCMD_ID_ENUM.WCMD_LCD_CLEAR:                       retString = "LcdClear";                     break;
                case WCMD_ID_ENUM.WCMD_LCD_SET_BACKLIGHT:               retString = "LcdSetBacklight";              break;
                case WCMD_ID_ENUM.WCMD_LCD_ENABLE_EXT_WRITE:            retString = "LcdEnableExternalWrite";       break;
                case WCMD_ID_ENUM.WCMD_LCD_DISABLE_EXT_WRITE:           retString = "LcdDisableExternalWrite";      break;
                case WCMD_ID_ENUM.WCMD_LCD_GET_EXT_WRITE_STATUS:        retString = "LcdGetExternalWriteStatus";    break;
                case WCMD_ID_ENUM.WCMD_LCD_GET_EXT_WRITE_DATA:          retString = "LcdGetExternalWriteData";      break;
                case WCMD_ID_ENUM.WCMD_EEPROM_WRITE:                    retString = "EepromWrite";                  break;
                case WCMD_ID_ENUM.WCMD_EEPROM_READ:                     retString = "EepromRead";                   break;
                case WCMD_ID_ENUM.WCMD_RTC_SET_DATETIME:                retString = "RtcSetDateTime";               break;
                case WCMD_ID_ENUM.WCMD_RTC_GET_DATETIME:                retString = "RtcGetDateTime";               break;
                case WCMD_ID_ENUM.WCMD_COMPORT_OPEN:                    retString = "ComPortOpen";                  break;
                case WCMD_ID_ENUM.WCMD_COMPORT_CLOSE:                   retString = "ComPortClose";                 break;
                case WCMD_ID_ENUM.WCMD_COMPORT_WRITE:                   retString = "ComPortWrite";                 break;
                case WCMD_ID_ENUM.WCMD_COMPORT_ENABLE_TUNNEL:           retString = "ComPortEnableTunnel";          break;
                case WCMD_ID_ENUM.WCMD_COMPORT_DISABLE_TUNNEL:          retString = "ComPortDisableTunnel";         break;
                case WCMD_ID_ENUM.WCMD_TEST_CMD:                        retString = "TestCommand";                  break;
                default:                                                retString = "N.A.";                         break;
            }

            return retString;
        }

        public Byte getIdValue()
        {
            return (Byte)(this._id);
        }

        public void addParam(Byte param)
        {
            _paramList.Add(param);
        }

        public void addParam(List<Byte> paramList)
        {
            _paramList.AddRange(paramList);
        }

        public void addParam(byte[] paramList)
        {
            _paramList.AddRange(paramList);
        }

        public void clearAllParam()
        {
            _paramList.Clear();
        }

        public Boolean hasParam()
        {
            if (this._paramList.Count != 0)
                return true;
            else
                return false;
        }

        public int getParamCount()
        {
            return this._paramList.Count;
        }

        public List<Byte> toByteAray()
        {
            List<Byte> returnList = new List<Byte>();

            returnList.Add(0x02);       // STX
            if (this.hasParam())
            {
                returnList.Add((Byte)((Byte)(this._id) | 0x80));        // ID
                returnList.Add((Byte)(this.getParamCount()));   // Number of Param
                returnList.AddRange(this._paramList);           // Parameters
            } else
            {
                returnList.Add((Byte)(this._id));   // ID
            }


            returnList.Add(0x03);       // ETX

            return returnList;
        }
    }
}
