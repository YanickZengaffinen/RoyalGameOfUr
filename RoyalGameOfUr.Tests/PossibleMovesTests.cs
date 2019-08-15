using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static RoyalGameOfUr.Player;

namespace RoyalGameOfUr.Tests
{
    public class PossitbleMovesTests
    {
        [Fact]
        public void ShouldBeRequestedPathLength()
        {
            var game = new Game();
            var moves = game.GetPossibleMoves(PlayerId.A, 4);
            var move = moves.ElementAt(0);

            Assert.True(move.EndIndex - move.StartIndex == 4);
        }

        [Fact]
        public void ShouldBeAbleToAddNew()
        {
            var game = new Game();
            var moves = game.GetPossibleMoves(PlayerId.A, 1);

            Assert.True(moves.Count() == 1);
            Assert.True(moves.ElementAt(0).New);
        }

        [Fact]
        public void ShouldNotBeAbleToMoveOnFieldWithOwn()
        {
            var game = new Game();
            game.Board.A[0] = true;

            var moves = game.GetPossibleMoves(PlayerId.A, 1);

            Assert.DoesNotContain(moves, x => x.IsSame(CreateMove(-1, 0)));
        }

        [Fact]
        public void ShouldBeAbleToMovePastFieldWithOwn()
        {
            var game = new Game();
            game.Board.A[0] = true;

            var moves = game.GetPossibleMoves(PlayerId.A, 2);
            Assert.Contains(moves, x => x.IsSame(CreateMove(-1, 1)));
        }

        [Fact]
        public void ShouldBeAbleToKill()
        {
            var game = new Game();
            game.Board.B[6] = true;
            game.Board.A[5] = true;

            var moves = game.GetPossibleMoves(PlayerId.A, 1);
            Assert.Contains(moves, x => x.IsSame(CreateMove(5, 6)) && x.DoesKillEnemy);
        }

        [Fact]
        public void ShouldBeAbleToFinish()
        {
            var game = new Game();
            game.Board.A[13] = true;

            var moves = game.GetPossibleMoves(PlayerId.A, 1);
            Assert.Contains(moves, x => x.IsSame(CreateMove(13, 14)) && x.Finish);
        }

        [Fact]
        public void ShouldNotBeAbleToFinish()
        {
            var game = new Game();
            game.Board.A[13] = true;

            var moves = game.GetPossibleMoves(PlayerId.A, 2);
            Assert.DoesNotContain(moves, x => x.Finish);
        }

        [Fact]
        public void ShouldNotBeAbleToKillSafe()
        {
            var game = new Game();
            game.Board.B[Board.SafeIndex] = true;
            game.Board.A[Board.SafeIndex - 1] = true;

            var moves = game.GetPossibleMoves(PlayerId.A, 1);
            Assert.DoesNotContain(moves, x => x.IsSame(CreateMove(Board.SafeIndex - 1, Board.SafeIndex)));
        }

        [Fact]
        public void ShouldBeAbleToMovePastFieldWithEnemy()
        {
            var game = new Game();
            game.Board.B[6] = true;
            game.Board.A[5] = true;

            var moves = game.GetPossibleMoves(PlayerId.A, 2);
            Assert.Contains(moves, x => x.IsSame(CreateMove(5, 7)));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(12)]
        [InlineData(13)]

        public void ShouldBeSeparateField(int index)
        {
            var game = new Game();
            game.Board.B[index] = true;

            var moves = game.GetPossibleMoves(PlayerId.A, index + 1);
            Assert.Contains(moves, x => x.EndIndex == index);
        }

        private MoveInfo CreateMove(int start, int end)
        {
            return new MoveInfo() {
                PlayerId = PlayerId.A,
                StartIndex = start,
                EndIndex = end
            };
        }
    }
}
