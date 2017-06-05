using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLinkToolBox.Application
{
    class WCommand
    {
        Byte _id = 0x00;
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
            "ComPortWrite"
            });


        /* ************************************************************************************* */
        /* Constructor */
        /* ************************************************************************************* */
        public WCommand(Byte id)
        {
            this._id = id;
            _paramList = new List<Byte>();
        }

        /* ************************************************************************************* */
        /* Methpds */
        /* ************************************************************************************* */
        public void addParam(Byte param)
        {
            _paramList.Add(param);
        }

        public void addParam(List<Byte> paramList)
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
                returnList.Add((Byte)(this._id | 0x80));        // ID
                returnList.Add((Byte)(this.getParamCount()));   // Number of Param
                returnList.AddRange(this._paramList);           // Parameters
            } else
            {
                returnList.Add(this._id);   // ID
            }


            returnList.Add(0x03);       // ETX

            return returnList;
        }
    }
}
