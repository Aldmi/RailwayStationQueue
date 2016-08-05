using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Interfaces
{
    public interface IExchangeDataProviderBase
    {
        byte[] GetDataByte();          //сформировать буфер для отправки.
        bool SetDataByte(byte[] data);   //получить принятый буфер.

        int CountGetDataByte { get; }    //кол-во байт для отправки.
        int CountSetDataByte { get; }    //кол-во байт для приема.
    }
}
