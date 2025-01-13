using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace FlySafe
{
    public class SimConnectHandler
    {
        private IntPtr hSimConnect;
        private bool quit = false;

        // Дефиниции и константы SimConnect
        private const uint SIMCONNECT_DATATYPE_FLOAT32 = 1;
        private const uint DEFINITION_PDR = 1;
        private const uint DATA_VERTICAL_SPEED = 1;
        private const uint DATA_PITOT_HEAT = 2;
        private const uint EVENT_SIM_START = 1;

        // Делегат для обработки событий
        private delegate void SimConnectDispatchCallback(IntPtr data, uint dataSize, uint dataType, uint definitionID);

        // Событие SimStart
        public event Action? SimStartEvent;

        /*
        [DllImport("SimConnect.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int SimConnect_Open(ref IntPtr hSimConnect, string szName, IntPtr hwnd, uint dwNotifyFlags, uint dwCallback, uint dwUserData);

        [DllImport("SimConnect.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int SimConnect_Close(IntPtr hSimConnect);
        */

        [DllImport("SimConnect.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int SimConnect_AddToDataDefinition(IntPtr hSimConnect, uint definitionID, string datumName, string units, uint dataType, uint unitsSpecifier, uint requestID);

        [DllImport("SimConnect.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int SimConnect_SubscribeToSystemEvent(IntPtr hSimConnect, uint eventID, string eventName);

        [DllImport("SimConnect.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int SimConnect_CallDispatch(IntPtr hSimConnect, SimConnectDispatchCallback dispatchCallback, IntPtr userData);

        public SimConnectHandler(IntPtr simConnectHandle)
        {
            hSimConnect = simConnectHandle;

            // Проверим, подписано ли событие
            //Console.WriteLine("Подписка на событие SimStart...");

            int result = SimConnect_SubscribeToSystemEvent(hSimConnect, EVENT_SIM_START, "SimStart");

            /*
            if (result == 0)
            {
                Console.WriteLine("Подписка на событие SimStart успешна.");
            }
            else
            {
                Console.WriteLine($"Ошибка подписки на событие SimStart. Код ошибки: {result}");
            }
            */
        }

        // Метод для начала обработки данных и запросов от SimConnect
        public void Start()
        {
            try
            {
                //Console.WriteLine("Попытка установить соединение с SimConnect...");

                // Настройка данных для запроса
                SimConnect_AddToDataDefinition(hSimConnect, DEFINITION_PDR, "Vertical Speed", "Feet per second", SIMCONNECT_DATATYPE_FLOAT32, 0, DATA_VERTICAL_SPEED);
                SimConnect_AddToDataDefinition(hSimConnect, DEFINITION_PDR, "Pitot Heat", "Bool", SIMCONNECT_DATATYPE_FLOAT32, 0, DATA_PITOT_HEAT);

                // Логируем перед подпиской
                //Console.WriteLine("Подписка на событие SimStart...");
                int result = SimConnect_SubscribeToSystemEvent(hSimConnect, EVENT_SIM_START, "SimStart");

                /*
                if (result == 0)
                {
                    Console.WriteLine("Подписка на событие SimStart успешна.");
                }
                else
                {
                    Console.WriteLine($"Ошибка подписки на событие SimStart. Код ошибки: {result}");
                }
                */

                // Бесконечный цикл для обработки событий
                while (!quit)
                {
                    // Проверим, что цикл продолжает работать
                    //Console.WriteLine("Цикл обработки событий...");

                    SimConnect_CallDispatch(hSimConnect, MyDispatchProcPDR, IntPtr.Zero);
                    Thread.Sleep(1); // Задержка, чтобы избежать 100% загрузки процессора
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        // Метод для обработки событий от SimConnect
        private void MyDispatchProcPDR(IntPtr data, uint dataSize, uint dataType, uint definitionID)
        {
            //Console.WriteLine($"Получено событие с definitionID: {definitionID}");

            if (definitionID == EVENT_SIM_START)
            {
                //Console.WriteLine("Событие SimStart получено!");
                SimStartEvent?.Invoke();
            }
        }

        // Метод для завершения работы с SimConnect
        public void Disconnect()
        {
            quit = true;
        }
    }
}
