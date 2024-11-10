using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace jogodavelha
{
    public class RealtimeService
    {
        private int _a;
        public int a
        {
            get { return _a; }
            set 
            { 
                _a = value;
                UpdateEvent?.Invoke();
            }
        }
        private int _b;

        public int b
        {
            get { return _b; }
            set 
            { 
                _b = value;
                UpdateEvent?.Invoke();
            }
        }
        public event Func<Task> UpdateEvent;

        
        public List<List<int[]>> WinningCombos = new List<List<int[]>>()
        {
            //First row
            new List<int[]>() {new int[] { 0,0 }, new int[] { 0, 1 }, new int[] { 0, 2} },
            //Second row
            new List<int[]>() {new int[] { 1,0 }, new int[] { 1, 1 }, new int[] { 1, 2} },
            //Third row
            new List<int[]>() {new int[] { 2,0 }, new int[] { 2, 1 }, new int[] { 2, 2} },

            //First column
            new List<int[]>() {new int[] { 0,0 }, new int[] { 1, 0 }, new int[] { 2, 0} },
            //Second column
            new List<int[]>() {new int[] { 0,1 }, new int[] { 1, 1 }, new int[] { 2, 1} },
            //Third column
            new List<int[]>() {new int[] { 0,2 }, new int[] { 1, 2 }, new int[] { 2, 2} },

            //Backward diagonal
            new List<int[]>() {new int[] { 0,0 }, new int[] { 1, 1 }, new int[] { 2, 2} },
            //Forward diagonal
            new List<int[]>() {new int[] { 0,2 }, new int[] { 1, 1 }, new int[] { 2, 0} },
        };

        private char[,] _Board = { { ' ', ' ', ' ' }, { ' ', ' ', ' ' }, { ' ', ' ', ' ' } };
        public char[,] Board
        {
            get { return _Board; }
            set { _Board = value; }
        }
        
        public event Func<Task> UpdateBoard;

        private char _NextPlayer = 'o';
        public char NextPlayer
        {
            get { return _NextPlayer; }
            private set { _NextPlayer = value; }
        }

        public async Task BoardAction()
        {
            _NextPlayer = _NextPlayer == 'o' ? 'x' : 'o';

            foreach (var combo in WinningCombos)
            {
                int[] first = combo[0];
                int[] second = combo[1];
                int[] third = combo[2];
                if (Board[first[0], first[1]] == ' ' || Board[second[0], second[1]] == ' ' || Board[third[0], third[1]] == ' ') continue;
                if (Board[first[0], first[1]] == Board[second[0], second[1]] && Board[second[0], second[1]] == Board[third[0], third[1]] && Board[first[0], first[1]] == Board[third[0], third[1]])
                {
                    if (_NextPlayer == 'o')
                    {
                        Winner = "Player X won!!";
                        Wins_X++;
                    }
                    else
                    {
                        Winner = "Player O won!!";
                        Wins_O++;
                    }
                    _ShowWinner = true;
                }
            }

            if (IsGameReset())
            {
                Draws++;
                Winner = TieMessage;
                _ShowWinner = true;
            }

            UpdateBoard?.Invoke();
        }

        private bool IsGameReset()
        {
            bool isReset = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if(Board[i, j] == ' ')
                    {
                        isReset = false;
                    }
                }
            }
            return isReset;
        }

        public async Task ResetGame()
        {
            ShowWinner = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Board[i, j] = ' ';
                }
            }
            UpdateBoard?.Invoke();
        }

        private static string TieMessage = "Go home, nobody won!";
        public string Winner = TieMessage;
        private bool _ShowWinner = false;
        public bool ShowWinner
        {
            get { return _ShowWinner; }
            set 
            { 
                _ShowWinner = value;
                UpdateBoard?.Invoke();
            }
        }
        
        public int _Wins_X = 0;
        public int Wins_X
        {
            get { return _Wins_X; }
            private set { _Wins_X = value; }
        }
        public int _Wins_O = 0;
        public int Wins_O
        {
            get { return _Wins_O; }
            private set { _Wins_O = value; }
        }
        public int _Draws = 0;
        public int Draws
        {
            get { return _Draws; }
            private set { _Draws = value; }
        }
    }
}
