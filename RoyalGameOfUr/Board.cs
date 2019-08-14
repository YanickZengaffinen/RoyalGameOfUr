using static RoyalGameOfUr.Player;

namespace RoyalGameOfUr
{
    public class Board
    {
        public enum OccupationState { Empty, Me, Other }

        public const int PathLength = 13;

        private readonly bool[] a;
        private readonly bool[] b;

        public Board()
        {
            a = new bool[PathLength];
            b = new bool[PathLength];
        }

        private Board(bool[] a, bool[] b)
        {
            this.a = a;
            this.b = b;
        }

        public Board Clone()
        {
            return new Board((bool[])a.Clone(), (bool[])b.Clone());
        }

        /// <summary>
        /// Expects a valid/precomputed move and executes it.
        /// 
        /// Does not update the player
        /// </summary>
        public void Move(MoveInfo validMove)
        {
            var me = validMove.PlayerId == PlayerId.A ? a : b;

            if (validMove.New)
            {
                me[validMove.EndIndex] = true;
            }
            else if(validMove.Finish)
            {
                me[validMove.StartIndex] = false;
            }
            else
            {
                me[validMove.EndIndex] = true;
                me[validMove.StartIndex] = false;
            }

            if(validMove.DoesKillEnemy)
            {
                var other = validMove.PlayerId == PlayerId.A ? b : a;
                other[validMove.EndIndex] = false;
            }
        }

        public bool IsRosette(int index)
        {
            return
                index == 3 ||
                index == 07 ||
                index == 13;
        }

        public bool IsSafe(int index)
        {
            return index == 07;
        }

        public OccupationState IsOccupied(Player view, int index)
        {
            var me = view.Id == PlayerId.A ? a : b;
            var other = view.Id == PlayerId.A ? b : a;

            if (me[index])
            {
                return OccupationState.Me;
            }

            if (index >= 4 && index <= 11)
            {
                if (other[index])
                {
                    return OccupationState.Other;
                }
            }

            return OccupationState.Empty;
        }
    }
}
