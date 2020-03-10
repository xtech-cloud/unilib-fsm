using System.Collections.Generic;

namespace XTC.FSM
{
    public class Machine
    {
        public Command startupCommand { get; private set; }
        private List<State> status = new List<State>();
        private Dictionary<string, Command> commands = new Dictionary<string, Command>();
        private State activeState_ { get; set; }

        public Machine()
        {
            startupCommand = new Command(this, "STARTUP");
        }

        public State NewState()
        {
            State state = new State(this);
            status.Add(state);
            return state;
        }

        public void DeleteState(State _state)
        {
            if (!status.Contains(_state))
                return;
            status.Remove(_state);
        }

        public Command NewCommand(string _name)
        {
            if(commands.ContainsKey(_name))
                return null;
            Command command = new Command(this, _name);
            commands[_name] = command;
            return command;
        }

        public void DeleteCommand(Command _command)
        {
            if (!commands.ContainsKey(_command.Name))
                return;
            commands.Remove(_command.Name);
        }

        public void Run()
        {
            activeState_ = startupCommand.state;
            if (null == activeState_)
            {
                throw new System.ArgumentNullException("None state is active");
            }
            activeState_.doEnter();
        }

        public void Update()
        {
            if (null == activeState_)
                return;
            activeState_.doUpdate();
        }

        internal void switchState(State _state)
        {
            if (null == _state)
                return;
            if (null != activeState_)
                activeState_.doExit();
            activeState_ = _state;
            activeState_.doEnter();
        }
    }
}