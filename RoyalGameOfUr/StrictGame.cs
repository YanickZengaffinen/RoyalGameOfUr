namespace RoyalGameOfUr
{
    public class StrictGame : Game
    {
        public Player CurrentPlayer { get; private set; }

        public StrictGame() 
            : base()
        {
            CurrentPlayer = random.NextDouble() < 0.5 ? PlayerA : PlayerB;
        }

        public override void Move(MoveInfo validMove)
        {
            if(validMove.PlayerId == CurrentPlayer.Id)
            {
                base.Move(validMove);

                CurrentPlayer = CurrentPlayer.Id == Player.PlayerId.A ? PlayerB : PlayerA;
            }
        }
    }
}
