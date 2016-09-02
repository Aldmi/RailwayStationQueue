﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using Server.Entitys;
using Server.Infrastructure;

namespace Server.Service
{
    public class CashierExchangeService
    {
        #region field

        private readonly List<Сashier> _cashiers;
        private readonly ushort _timeRespone;

        private int _lastSyncLabel;

        #endregion



        #region ctor

        public CashierExchangeService(List<Сashier> cashiers, ushort timeRespone)
        {
            _cashiers = cashiers;
            _timeRespone = timeRespone;
        }

        #endregion



        #region Methode

        public async Task ExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            if (port == null)
                return;

            foreach (var cashier in _cashiers)              //Запуск опроса кассиров
            {
                var readProvider = new Server2CashierReadDataProvider { InputData = cashier.Id };
                await port.DataExchangeAsync(_timeRespone, readProvider, ct);                        //TODO: можно добавить кассирам свойство IsConnect. И выставлять его как резульат обмена.

                if (readProvider.IsOutDataValid)
                {
                    TicketItem item;
                    var cashierInfo = readProvider.OutputData;

                    if (!cashierInfo.IsWork)
                        continue;

                    switch (cashierInfo.Handling)
                    {
                        case CashierHandling.IsSuccessfulHandling:
                            cashier.SuccessfulHandling();
                            break;

                        case CashierHandling.IsErrorHandling:
                            cashier.ErrorHandling();
                            break;

                        case CashierHandling.IsStartHandling:
                            item = cashier.StartHandling();
                            var writeProvider = new Server2CashierWriteDataProvider { InputData = item };
                            await port.DataExchangeAsync(_timeRespone, writeProvider, ct);
                            if (writeProvider.IsOutDataValid)                //завершение транзакции ( успешная передача билета кассиру)
                            {
                                cashier.SuccessfulStartHandling();
                            }
                            break;

                        case CashierHandling.IsSuccessfulAndStartHandling:
                            cashier.SuccessfulHandling();

                            item = cashier.StartHandling();
                            writeProvider = new Server2CashierWriteDataProvider { InputData = item };
                            await port.DataExchangeAsync(_timeRespone, writeProvider, ct);
                            if (writeProvider.IsOutDataValid)                //завершение транзакции ( успешная передача билета кассиру)
                            {
                                cashier.SuccessfulStartHandling();
                            }
                            break;

                        case CashierHandling.IsErrorAndStartHandling:
                            cashier.ErrorHandling();

                            item = cashier.StartHandling();
                            writeProvider = new Server2CashierWriteDataProvider { InputData = item };
                            await port.DataExchangeAsync(_timeRespone, writeProvider, ct);
                            if (writeProvider.IsOutDataValid)                //завершение транзакции ( успешная передача билета кассиру)
                            {
                                cashier.SuccessfulStartHandling();
                            }
                            break;

                        default:
                            item = null;
                            break;
                    }
                }
            }

            //Отправка запроса синхронизации времени раз в час
            if (_lastSyncLabel != DateTime.Now.Hour)
            {
                _lastSyncLabel = DateTime.Now.Hour;

                var syncTimeProvider = new Server2CashierSyncTimeDataProvider();
                await port.DataExchangeAsync(_timeRespone, syncTimeProvider, ct);
            }
        }

        #endregion
    }
}