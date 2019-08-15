using System.Runtime.Serialization;

namespace RoyalGameOfUr
{
    [DataContract]
    public class Player
    {
        public enum PlayerId { A, B, None }

        [DataMember]
        public PlayerId Id { get; }

        [DataMember]
        public int PiecesNotYetOnBoard { get; set; }

        [DataMember]
        public int PiecesFinished { get; set; }

        public Player(PlayerId playerId)
        {
            this.Id = playerId;
        }
    }
}
