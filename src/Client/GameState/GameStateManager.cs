using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ChessDemo.GameState
{
    public enum GameStates
    {
        Uninitialized,
        MainMenu,
        InGame
    }

    class GameStateManager
    {
        private static GameStateManager _instance = new GameStateManager(GameStates.InGame);
        public static GameStateManager Instance {
            get {
                return _instance;
            }
        }

        public GameStates CurrentState { get; private set; }
        public GameStates PreviousState { get; private set; }

        public event EventHandler StateChanged = new EventHandler((e, a) => { });

        private GameStates nextState;

        private GameStateManager(GameStates currentState)
        {
            nextState = currentState;
            CurrentState = PreviousState = GameStates.Uninitialized;
        }

        public void SetState(GameStates nextState)
        {
            Debug.Assert(nextState != GameStates.Uninitialized);
            this.nextState = nextState;
        }

        /// <summary>
        /// Called from the Game object to update after everything is done.
        /// </summary>
        public void Update()
        {
            if (nextState != CurrentState)
            {
                var previousState = CurrentState;
                CurrentState = nextState;
                StateChanged(this, new EventArgs());
            }
        }
    }
}
