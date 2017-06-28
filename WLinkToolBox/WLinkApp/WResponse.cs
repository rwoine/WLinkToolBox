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
