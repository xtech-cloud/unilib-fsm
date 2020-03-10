
namespace XTC.FSM
{

    public class Invoker
    {
        private Machine machine_ {get;set;}

        internal Invoker(Machine _machine)
        {
            machine_ = _machine;
        }
        public void InvokeInternal(Command _command)
        {
            if(null == _command.state)
                return;
            machine_.switchState(_command.state);
        }

        public void InvokeExternal(string _command)
        {
            machine_.InvokeExternalCommand(_command);
        }
    }
}