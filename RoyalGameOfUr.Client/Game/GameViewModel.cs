using RoyalGameOfUr.Client.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static RoyalGameOfUr.Player;

namespace RoyalGameOfUr.Client.Game
{
    public class GameViewModel : ViewModelBase
    {
        public ICommand ButtonClickedCommand { get; private set; }

        public ICommand RollCommand { get; private set; }

        public StrictGame Game { get; private set; }

        public int ANew
            => Game.PlayerA.PiecesNotYetOnBoard;

        public int BNew
            => Game.PlayerB.PiecesNotYetOnBoard;

        public int AFinished
            => Game.PlayerA.PiecesFinished;

        public int BFinished
            => Game.PlayerB.PiecesFinished;

        public bool Rolled
            => Game.LastRoll >= 0;

        public int LastRoll
            => Game.LastRoll;

        public PlayerId CurrentPlayerId
            => Game.CurrentPlayerId;

        public GameViewModel(StrictGame game)
        {
            this.Game = game;

            ButtonClickedCommand = new RelayCommand(OnButtonClicked);
            RollCommand = new RelayCommand(OnRoll);
        }

        public void OnButtonClicked(object param)
        {
            if (!Rolled)
                return;

            var field = param as Field;
            var possibleMoves = Game.GetPossibleMoves(Game.CurrentPlayerId, LastRoll);
            int index = int.Parse(field.FieldId.Substring(1));
            var moveProto = new MoveInfo()
            {
                PlayerId = Game.CurrentPlayerId,
                StartIndex = index,
                EndIndex = index + LastRoll
            };

            var move = possibleMoves.FirstOrDefault(x => x.IsSame(moveProto));
            if(move != null)
            {
                Game.Move(move);
                OnMoved();
            }
            else if(possibleMoves.Count() == 0)
            {
                OnMoved();
            }
        }

        private void OnMoved()
        {
            OnPropertyChanged(nameof(CurrentPlayerId));
            OnPropertyChanged(nameof(LastRoll));
            OnPropertyChanged(nameof(Rolled));
            OnPropertyChanged(nameof(ANew));
            OnPropertyChanged(nameof(BNew));
            OnPropertyChanged(nameof(AFinished));
            OnPropertyChanged(nameof(BFinished));
        }

        public void OnRoll(object param)
        {
            Game.Roll();

            OnPropertyChanged(nameof(Rolled));
            OnPropertyChanged(nameof(LastRoll));
        }
    }
}
