using System;
using System.Collections.Generic;
using System.Dynamic;
using Server.Model;

namespace Server.Service
{
    public class TicketFactory
    {
        #region field

        private const uint MaxTicketNumber = 999;
        private readonly string _ticketPrefix;
        private uint _ticketNumber;

        #endregion




        #region ctor

        public TicketFactory(string ticketPrefix)
        {
            _ticketPrefix = ticketPrefix;
        }

        #endregion






        #region prop

        public uint GetCurrentTicketNumber
        {
            get { return _ticketNumber; }
        }

        #endregion




        public TicketItem Create(ushort countElement)
        {
            if (++_ticketNumber > MaxTicketNumber)
                _ticketNumber = 0;

            return new TicketItem() {NumberElement = _ticketNumber, CountElement = countElement, AddedTime = DateTime.Now, Prefix = _ticketPrefix, Сashbox =null, CountTryHandling = 0};
        }
    }
}