using System.Runtime.Serialization;
using static RoyalGameOfUr.Player;

namespace RoyalGameOfUr
{
    [DataContract]
    public class StrictGame : Game
    {
        [DataMember]
        public PlayerId CurrentPlayerId { get; private set; }

        public StrictGame() 
            : base()
        {
            CurrentPlayerId = random.NextDouble() < 0.5 ? PlayerId.A : PlayerId.B;
        }

        public override void Move(MoveInfo validMove)
        {
            if(validMove.PlayerId == CurrentPlayerId)
            {
                base.Move(validMove);

                CurrentPlayerId = CurrentPlayerId == PlayerId.A ? PlayerId.B : PlayerId.B;
            }
        }
    }
}
