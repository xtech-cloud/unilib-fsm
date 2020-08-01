using System;
using System.Collections.Generic;

namespace XTC.FSM
{
    public abstract class Action
    {
        internal enum Status
        {
            STOP,
            RUN,
            FINISH
        }

        internal Status status{ get; private set;}
        public Command finishCommand {get;private set;}
        public State state {get;set;}
        protected Invoker invoker {get; private set;}

        protected Action()
        {

        }

        protected abstract void onEnter();
        protected abstract void onExit();
        protected abstract void onUpdate();


        internal void joinState(State _state)
        {
            status = Status.STOP;
            state = _state;
            finishCommand = new Command("FINISH");
            invoker = new Invoker(state.machine);
        }

        internal void doEnter()
        {
            status = Status.RUN;
            onEnter();
        }
        internal void doExit()
        {
            status = Status.STOP;
            onExit();
        }
        internal void doUpdate()
        {
            onUpdate();
        }

        protected void doFinish()
        {
            status = Status.FINISH;
        }

        protected Parameter getParameter(string _name)
        {
            return state.machine.GetParameter(_name);
        }
        protected void setParameter(string _name, Parameter _parameter)
        {
            state.machine.SetParameter(_name, _parameter);
        }
    }

    public class FinishAction : Action
    {
        protected override void onEnter()
        {

        }
        protected override void onExit()
        {

        }
        protected override void onUpdate()
        {

        }
    }

}//namespace

