
namespace XTC.FSM
{

    public class Invoker
    {
        public void Invoke(Command _command)
        {
            if(null == _command.state)
                return;
            _command.state.machine.switchState(_command.state);
        }
    }
}