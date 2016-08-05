using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Communication.Annotations;
using Communication.Interfaces;
using Communication.Settings;
using Library.Async;
using Library.Convertion;
using Library.Library;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;




namespace Communication.SerialPort
{
    /// <summary>
    /// Перед началом работы с портом проверять флаг занятости порта IsBusy.
    /// И если порт свободен и нужно с ним работать то выставить это флаг.
    /// </summary>
    public class MasterSerialPort : INotifyPropertyChanged, IDisposable
    {
        #region fields

        private string _statusString;
        private bool _isConnect;
        private bool _isRunDataExchange;

        private readonly System.IO.Ports.SerialPort _port; //COM порт

        #endregion




        #region ctor

        public MasterSerialPort(string portName, int baudRate, int dataBits, StopBits stopBits)
        {
            _port = new System.IO.Ports.SerialPort("COM" + portName)
            {
                BaudRate = baudRate,
                DataBits = dataBits,
                Parity = Parity.None,
                StopBits = stopBits
            };
        }

        public MasterSerialPort(XmlSerialSettings xmlSerial) :
            this(xmlSerial.Port, xmlSerial.BaudRate, xmlSerial.DataBits, xmlSerial.StopBits)
        {
        }

        #endregion




        #region prop   

        public List<Func<MasterSerialPort, CancellationToken, Task>> Funcs { get; set; }= new List<Func<MasterSerialPort, CancellationToken, Task>>();

        public string StatusString
        {
            get { return _statusString; }
            set
            {
                if (value == _statusString) return;
                _statusString = value;
                OnPropertyChanged();
            }
        }
        public bool IsConnect
        {
            get { return _isConnect; }
            set
            {
                if (value == _isConnect) return;
                _isConnect = value;
                OnPropertyChanged();
            }
        }
        public bool IsRunDataExchange
        {
            get { return _isRunDataExchange; }
            set
            {
                if (value == _isRunDataExchange) return;
                _isRunDataExchange = value;
                OnPropertyChanged();
            }
        }

        public CancellationTokenSource Cts { get; set; } = new CancellationTokenSource();

        #endregion




        #region Methode

        public async Task<bool> CycleReConnect(int period)
        {
            bool res = false;
            while (!res)
            {
                res = ReConnect();
                if(!res)
                  await Task.Delay(period, Cts.Token);
            }

            return true;
        }


        public bool ReConnect()
        {
            Dispose();

            IsConnect = false;

            try
            {
                _port.Open();
            }
            catch (Exception ex)
            {
                IsConnect = false;
                StatusString = $"Ошибка открытия порта: {_port.PortName}. ОШИБКА: {ex}";
                return false;
            }

            IsConnect = true;
            return true;
        }


        public void ReOpen()
        {
            if (_port.IsOpen)
                _port.Close();

            if (!_port.IsOpen)
                _port.Open();
        }


        public void AddFunc(Func<MasterSerialPort, CancellationToken, Task> action)
        {
            if(action!= null)
               Funcs.Add(action);
        }


        public async Task RunExchange()
        {
            while (!Cts.IsCancellationRequested)
            {
                foreach (var func in Funcs)
                {
                    await func(this, Cts.Token);
                }
            }
        }



        public async Task DataExchangeAsync(int timeRespoune, IExchangeDataProviderBase dataProvider, CancellationToken ct)
        {
            if (!IsConnect)
                return;

            if (dataProvider == null)
                return;

            IsRunDataExchange = true;
            try
            {
                byte[] writeBuffer = dataProvider.GetDataByte();
                if (writeBuffer != null && writeBuffer.Any())
                {
                    var readBuff = await RequestAndRespawnInstantlyAsync(writeBuffer, dataProvider.CountSetDataByte, timeRespoune,  ct);
                    dataProvider.SetDataByte(readBuff);
                }
            }
            catch (OperationCanceledException)
            {
                StatusString = "Операция обмена данными с портом отменена";
                //ReConnect();
            }
            catch (TimeoutException ex)
            {
                StatusString = ex.ToString();
                //ReOpen();                       //TODO: проблема!!!
            }
            IsRunDataExchange = false;
        }


        /// <summary>
        /// Функция посылает запрос в порт, потом отсчитывает время readTimeout и проверяет буфер порта на чтение.
        /// Таким образом обеспечивается одинаковый промежуток времени между запросами в порт.
        /// </summary>
        public async Task<byte[]> RequestAndRespawnConstPeriodAsync(byte[] writeBuffer, int nBytesRead, int readTimeout, CancellationToken ct)
        {
            if (!_port.IsOpen)
                return await Task<byte[]>.Factory.StartNew(() => null, ct);

            //очистили буферы порта
            _port.DiscardInBuffer();
            _port.DiscardOutBuffer();

            //отправили данные в порт
            _port.WriteTimeout = 500;
            _port.Write(writeBuffer, 0, writeBuffer.Length);

            //ждем ответа....
            await Task.Delay(readTimeout, ct);

            //проверяем ответ
            var buffer = new byte[nBytesRead];
            if (_port.BytesToRead == nBytesRead)
            {
                _port.Read(buffer, 0, nBytesRead);
                return buffer;
            }
            throw new TimeoutException("Время на ожидание ответа вышло");
        }



        /// <summary>
        /// Функция посылает запрос в порт, и как только в буфер порта приходят данные сразу же проверяет их кол-во.
        /// Как только накопится нужное кол-во байт сразу же будет возвращен ответ не дожедаясь вермени readTimeout.
        /// Таким образом период опроса не фиксированный, а определяется скоростью ответа slave устройства.
        /// </summary>
        public async Task<byte[]> RequestAndRespawnInstantlyAsync(byte[] writeBuffer, int nBytesRead, int readTimeout, CancellationToken ct)
        {
            if (!_port.IsOpen)
                return await Task<byte[]>.Factory.StartNew(() => null, ct);

            //очистили буферы порта
            _port.DiscardInBuffer();
            _port.DiscardOutBuffer();

            //отправили данные в порт
            _port.WriteTimeout = 500;
            _port.Write(writeBuffer, 0, writeBuffer.Length);

            //ждем ответа....
            TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();

            var handler= new SerialDataReceivedEventHandler((o, e) =>
            {
                if (_port.BytesToRead >= nBytesRead)
                {
                    var buffer = new byte[nBytesRead];
                    _port.Read(buffer, 0, nBytesRead);
                    
                    tcs.SetResult(buffer);
                }
            });

            _port.DataReceived += handler;
            try
            {
                var buff = await AsyncHelp.WithTimeout(tcs.Task, readTimeout, ct);
                return buff;
            }
            catch (TimeoutException)
            {
                tcs.SetCanceled();
                throw;
            }
            finally
            {
                _port.DataReceived -= handler;
            }
        }

        #endregion




        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion




        #region Disposable

        public void Dispose()
        {
            if (_port == null)
                return;

            if (_port.IsOpen)
            {
                Cts.Cancel();
                _port.DiscardInBuffer();
                _port.DiscardOutBuffer();
                _port.Close();
            }

            _port.Dispose();
        }

        #endregion
    }
}