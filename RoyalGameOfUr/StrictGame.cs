using System.Runtime.Serialization;
using static RoyalGameOfUr.Player;

namespace RoyalGameOfUr
{
    [DataContract]
    public class StrictGame : Game
    {
        [DataMember]
        public PlayerId CurrentPlayerId { get; private set; }

        [DataMember]
        public int LastRoll { get; private set; }

        public StrictGame() 
            : base()
        {
            LastRoll = -1;
            CurrentPlayerId = random.NextDouble() < 0.5 ? PlayerId.A : PlayerId.B;
        }

        public override void Move(MoveInfo validMove)
        {
            if(validMove.PlayerId == CurrentPlayerId && 
                validMove.EndIndex - validMove.StartIndex == LastRoll &&
                LastRoll >= 0)
            {
                base.Move(validMove);

                if (!validMove.CanMoveAgain)
                {
                    SwitchTurns();
                }

                LastRoll = -1;
            }
        }

        public override int Roll()
        {
            LastRoll = base.Roll();

            if(LastRoll == 0)
            {
                SwitchTurns();
            }

            return LastRoll;
        }

        private void SwitchTurns()
        {
            CurrentPlayerId = CurrentPlayerId == PlayerId.A ? PlayerId.B : PlayerId.A;
            LastRoll = -1;
        }
    }
}
