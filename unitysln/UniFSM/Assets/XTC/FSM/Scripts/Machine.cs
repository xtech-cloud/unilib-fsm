using System.Collections.Generic;

namespace XTC.FSM
{
    public class Machine
    {
        public Command startupCommand { get; private set; }
        private List<State> status = new List<State>();
        private Dictionary<string, Command> internalCommands = new Dictionary<string, Command>();
        private Dictionary<string, Command> externalCommands = new Dictionary<string, Command>();
        private Dictionary<string, Paramter> parameters = new Dictionary<string, Paramter>();
        private State activeState_ { get; set; }

        public Machine()
        {
            startupCommand = new Command("STARTUP");
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

        public Command NewInternalCommand(string _name)
        {
            if (internalCommands.ContainsKey(_name))
                return null;
            Command command = new Command(_name);
            internalCommands[_name] = command;
            return command;
        }

        public void DeleteInternalCommand(Command _command)
        {
            if (!internalCommands.ContainsKey(_command.Name))
                return;
            internalCommands.Remove(_command.Name);
        }

        public Command NewExternalCommand(string _name)
        {
            if (externalCommands.ContainsKey(_name))
                return null;
            Command command = new Command(_name);
            externalCommands[_name] = command;
            return command;
        }

        public void DeleteExternalCommand(Command _command)
        {
            if (!externalCommands.ContainsKey(_command.Name))
                return;
            externalCommands.Remove(_command.Name);
        }

        public void AddParameter(string _name, Paramter _paramter)
        {
            if (parameters.ContainsKey(_name))
                return;
            parameters[_name] = _paramter;
        }

        public void DeleteParameter(string _name)
        {
            if (!parameters.ContainsKey(_name))
                return;
            parameters.Remove(_name);
        }

        public Paramter FindParameter(string _name)
        {
            Paramter paramter;
            parameters.TryGetValue(_name, out paramter);
            return paramter;
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

        public void InvokeExternalCommand(string _command)
        {
            Command command;
            if (!externalCommands.TryGetValue(_command, out command))
                return;
            switchState(command.state);
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