namespace Game.Core
{
    public class State
    {
        public delegate void OnUpdate();
        public OnUpdate onUpdate;

        public delegate void OnStateEnter();
        public OnStateEnter onStateEnter;

        public delegate void OnStateExit();
        public OnStateExit onStateExit;

        public State(OnUpdate onUpdate, OnStateEnter onStateEnter, OnStateExit onStateExit)
        {
            this.onUpdate = onUpdate;
            this.onStateEnter = onStateEnter;
            this.onStateExit = onStateExit;
        }
    }
}
