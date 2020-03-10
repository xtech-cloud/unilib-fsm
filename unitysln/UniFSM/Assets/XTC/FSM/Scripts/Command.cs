using System;

namespace XTC.FSM
{
    public class Command
    {
        public string Name {get; private set;}
        public State state {get;set;}

        protected Command()
        {

        }

        internal Command(string _name)
        {
            Name = _name;
        }
    } 

}//namespace

