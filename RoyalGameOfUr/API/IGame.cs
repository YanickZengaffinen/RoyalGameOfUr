using System;

namespace RoyalGameOfUr
{
    public interface IGame
    {
        Player PlayerA { get; }

        Player PlayerB { get; }

        Board Board { get; }

        event EventHandler<Player> Win;

        event EventHandler<MoveInfo> Moved;

        void Move(MoveInfo move);

        int Roll();
    }
}
