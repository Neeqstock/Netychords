using ArduinoDriver.SerialProtocol;
using ArduinoUploader.Hardware;

namespace NeeqDMIs.ATmega
{
    class ATmegaModule
    {
        ArduinoDriver.ArduinoDriver driver;
        public ATmegaModule()
        {
            driver = new ArduinoDriver.ArduinoDriver(ArduinoModel.Mega2560, true);
        }
    }
}
