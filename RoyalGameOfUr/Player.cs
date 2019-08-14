namespace RoyalGameOfUr
{
    public class Player
    {
        public enum PlayerId { A, B };

        public PlayerId Id { get; }
        public int PiecesNotYetOnBoard { get; set; }
        public int PiecesFinished { get; set; }

        public Player(PlayerId playerId)
        {
            this.Id = playerId;
        }
    }
}
