using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using static RoyalGameOfUr.Board;
using static RoyalGameOfUr.Player;

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
    [DataContract]
    public class Game : IGame
    {
        public const int TotalPieces = 7;
        public const int Dices = 4;

        protected static readonly Random random = new Random();

        [DataMember]
        public Player PlayerA { get; private set; }

        [DataMember]
        public Player PlayerB { get; private set; }

        [DataMember]
        public Board Board { get; private set; }

        public event EventHandler<Player> Win;
        public event EventHandler<MoveInfo> Moved;

        private MoveInfo LastMove { get; set; }

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

        public virtual void Move(MoveInfo validMove)
        {
            var me = validMove.PlayerId == Player.PlayerId.A ? PlayerA : PlayerB;

            Board.Move(validMove);

            if (validMove.New)
            {
                me.PiecesNotYetOnBoard--;
            }

            if (validMove.Finish)
            {
                me.PiecesFinished++;
            }

            if (validMove.DoesKillEnemy)
            {
                var other = validMove.PlayerId == Player.PlayerId.A ? PlayerB : PlayerA;
                other.PiecesNotYetOnBoard++;
            }


            LastMove = validMove;
            OnMoved(validMove);

            if (validMove.Win)
            {
                OnWin(me);
            }
        }
        public IEnumerable<MoveInfo> GetPossibleMoves(PlayerId playerId, int score)
        {
            var moves = new List<MoveInfo>();

            //Score 0 means the move is skipped
            if(score == 0)
            {
                return moves;
            }

            //If the player lands on a rosette he is allowed another move with the same piece
            if (LastMove?.CanMoveAgain == true)
            {
                var move = CreateMove(playerId, LastMove.EndIndex, score);

                if(move != null)
                {
                    moves.Add(move);
                }
            }
            else
            {
                var player = GetPlayer(playerId);

                //Add a piece to the map
                if (player.PiecesNotYetOnBoard > 0)
                {
                    var endIndex = score - 1;
                    var occupied = Board.IsOccupied(playerId, endIndex);
                    if (occupied != OccupationState.Me)
                    {
                        moves.Add(new MoveInfo()
                        {
                            PlayerId = playerId,
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
                for (int i = 0; i < PathLength; i++)
                {
                    var move = CreateMove(playerId, i, score);

                    if(move != null)
                    {
                        moves.Add(move);
                    }
                }
            }
           

            return moves;
        }

        private MoveInfo CreateMove(in PlayerId playerId, in int startIndex, in int score)
        {
            var player = GetPlayer(playerId);
            if (Board.IsOccupied(playerId, startIndex) == OccupationState.Me)
            {
                int endIndex = startIndex + score;
                if (endIndex < PathLength) //Normal move
                {
                    var occupied = Board.IsOccupied(playerId, endIndex);
                    if (occupied != OccupationState.Me)
                    {
                        bool startSafe = Board.IsSafe(startIndex);
                        bool endSafe = Board.IsSafe(endIndex);
                        bool doesKill = occupied == OccupationState.Other;

                        if (!endSafe || !doesKill)
                        {
                            return new MoveInfo()
                            {
                                PlayerId = playerId,
                                StartIndex = startIndex,
                                EndIndex = endIndex,
                                NoLongerSafe = startSafe && !endSafe,
                                IsSafe = endSafe,
                                DoesKillEnemy = doesKill,
                                CanMoveAgain = Board.IsRosette(endIndex)
                            };
                        }
                    }
                }
                else if (endIndex == PathLength) //Finish piece
                {
                    return new MoveInfo()
                    {
                        PlayerId = playerId,
                        StartIndex = startIndex,
                        EndIndex = endIndex,
                        Finish = true,
                        Win = player.PiecesNotYetOnBoard == 0 && player.PiecesFinished == TotalPieces - 1, //if it's the last piece then it's a win
                        IsSafe = true
                    };
                }
            }

            return null;
        }


        protected virtual void OnMoved(MoveInfo info)
        {
            Moved?.Invoke(this, info);
        }

        protected virtual void OnWin(Player player)
        {
            Win?.Invoke(this, player);
        }

        public virtual int Roll()
        {
            int total = 0;
            for(int i = 0; i < Dices; i++)
            {
                total += RollOne();
            }

            return total;
        }

        protected int RollOne()
        {
            //each dice is a tetrahedron with a 2/4 chance of rolling a white edge
            return random.NextDouble() < 0.5 ? 1 : 0;
        }

        public Player GetPlayer(PlayerId id)
        {
            return id == PlayerId.A ? PlayerA : PlayerB;
        }
    }
}
