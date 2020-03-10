using System;

namespace XTC.FSM
{
    public class Command
    {
        public string Name {get; private set;}
        internal State state {get;set;}
        private Machine machine_ {get;set;}

        internal Command(Machine _machine, string _name)
        {
            machine_ = _machine;
            Name = _name;
        }
    } 

}//namespace

