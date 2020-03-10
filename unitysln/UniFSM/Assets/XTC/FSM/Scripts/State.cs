using System;
using System.Collections.Generic;

namespace XTC.FSM
{
    public class State
    {
        public FinishAction finishAction { get; private set; }
        internal Machine machine { get; set; }

        internal State(Machine _machine)
        {
            machine = _machine;
            finishAction = new FinishAction();
        }

        protected State()
        {

        }

        private List<Action> actions = new List<Action>();


        public T NewAction<T>() where T : Action, new()
        {
            T action = new T();
            action.joinState(this);
            actions.Add(action);
            return action;
        }

        internal void doEnter()
        {
            actions.ForEach((_item) =>
            {
                _item.doEnter();
            });
        }

        internal void doExit()
        {
            actions.ForEach((_item) =>
            {
                _item.doExit();
            });
        }

        internal void doUpdate()
        {
            int finishCount = 0;
            actions.ForEach((_item) =>
            {
                if(Action.Status.RUN == _item.status) 
                {
                    _item.doUpdate();
                }
                else if (Action.Status.FINISH == _item.status)
                {
                    finishCount += 1;
                }
            });

            //all actions finish
            if (actions.Count == finishCount)
            {
                machine.switchState(finishAction.state);
            }
        }
    }

}//namespace

