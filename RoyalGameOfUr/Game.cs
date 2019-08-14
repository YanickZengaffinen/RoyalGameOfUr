using System;
using System.Collections.Generic;
using static RoyalGameOfUr.Board;

namespace RoyalGameOfUr
{
    /// <summary>
    /// Helper class that can be used to simulate games
    /// 
    /// Does not enforce game rules!
    /// 
    /// Board Layout:
    /// 03 02 01 00       13 12
    /// 04 05 06 07 08 09 10 11
    /// 03 02 01 00       13 12
    /// </summary>
    public class Game
    {
        public const int TotalPieces = 7;
        public const int Dices = 4;

        private static readonly Random random = new Random();

        public Player PlayerA { get; private set; }
        public Player PlayerB { get; private set; }

        public Board Board { get; private set; }

        public event EventHandler<Player> Win;
        public event EventHandler<MoveInfo> Moved;

        public Game()
        {
            Board = new Board();
            PlayerA = new Player(Player.PlayerId.A)
            {
                PiecesNotYetOnBoard = TotalPieces
            };

            PlayerB = new Player(Player.PlayerId.B)
            {
                PiecesNotYetOnBoard = TotalPieces
            };
        }

        public void Move(MoveInfo validMove)
        {
            var me = validMove.Player;

            Board.Move(validMove);

            if(validMove.New)
            {
                me.PiecesNotYetOnBoard--;
            }

            if(validMove.Finish)
            {
                me.PiecesFinished++;
            }

            if(validMove.DoesKillEnemy)
            {
                var other = validMove.Player.Id == Player.PlayerId.A ? PlayerB : PlayerA;
                other.PiecesNotYetOnBoard++;
            }

            OnMoved(validMove);

            if(validMove.Win)
            {
                OnWin(me);
            }
        }

        protected virtual void OnMoved(MoveInfo info)
        {
            Moved?.Invoke(this, info);
        }

        protected virtual void OnWin(Player player)
        {
            Win?.Invoke(this, player);
        }

        public IEnumerable<MoveInfo> GetPossibleMoves(Player player, int score)
        {
            var moves = new List<MoveInfo>();

            //Add a piece to the map
            if (player.PiecesNotYetOnBoard > 0)
            {
                var endIndex = score - 1;
                var occupied = Board.IsOccupied(player, endIndex);
                if (occupied != OccupationState.Me) 
                {
                    moves.Add(new MoveInfo()
                    {
                        Player = player,
                        StartIndex = -1,
                        EndIndex = score - 1,
                        CanMoveAgain = Board.IsRosette(score - 1),
                        IsSafe = Board.IsSafe(score - 1),
                        New = true,
                        DoesKillEnemy = occupied == OccupationState.Other
                    });
                }
            }

            //Move existing pieces
            for(int i = 0; i < PathLength; i++)
            {
                if(Board.IsOccupied(player, i) == OccupationState.Me)
                {
                    int endIndex = i + score;
                    if(endIndex < PathLength) //Normal move
                    {
                        var occupied = Board.IsOccupied(player, endIndex);
                        if(occupied != OccupationState.Me)
                        {
                            bool startSafe = Board.IsSafe(i);
                            bool endSafe = Board.IsSafe(endIndex);
                            moves.Add(new MoveInfo()
                            {
                                Player = player,
                                StartIndex = i,
                                EndIndex = endIndex,
                                NoLongerSafe = startSafe && !endSafe,
                                IsSafe = endSafe,
                                DoesKillEnemy = occupied == OccupationState.Other,
                                CanMoveAgain = Board.IsRosette(endIndex)
                            });
                        }
                    }
                    else if(endIndex == PathLength) //Finish piece
                    {
                        moves.Add(new MoveInfo()
                        {
                            Player = player,
                            StartIndex = i,
                            EndIndex = endIndex,
                            Finish = true,
                            Win = player.PiecesNotYetOnBoard == 0 && player.PiecesFinished == TotalPieces - 1, //if it's the last piece then it's a win
                            IsSafe = true
                        });
                    }
                }
            }

            return moves;
        }


        public int Roll()
        {
            int total = 0;
            for(int i = 0; i < Dices; i++)
            {
                total += RollOne();
            }

            return total;
        }

        private int RollOne()
        {
            //each dice is a tetrahedron with a 2/4 chance of rolling a white edge
            return random.NextDouble() < 0.5 ? 1 : 0;
        }
    }
}
