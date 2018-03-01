using static System.Console;

namespace ECS
{
    public interface IHeater
    {
        void TurnOn();
        void TurnOff();
    }
}