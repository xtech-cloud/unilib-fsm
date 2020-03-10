using System;
using System.Collections.Generic;

namespace XTC.FSM
{
    public abstract class Action
    {
        public enum Status
        {
            STOP,
            RUN,
            FINISH
        }

        public Status status{ get; private set;}
        public Command finishCommand {get;private set;}
        internal State state {get;set;}
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

        protected Paramter findParamter(string _name)
        {
            return state.machine.FindParameter(_name);
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

