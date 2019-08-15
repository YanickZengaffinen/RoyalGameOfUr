using System;
using System.Runtime.Serialization;
using static RoyalGameOfUr.Player;

namespace RoyalGameOfUr
{
    [DataContract]
    public class Board
    {
        public enum OccupationState { Empty, Me, Other }

        public const int PathLength = 14;

        [DataMember]
        public bool[] A { get; private set; }

        [DataMember]
        public bool[] B { get; private set; }

        public event EventHandler Changed;

        /// <summary>
        /// A03 A02 A01 A00 --- --- A13 A12
        /// M04 M05 M06 M07 M08 M09 M10 M11
        /// B03 B02 B01 B00 --- --- B13 B12
        /// </summary>
        public PlayerId this[string field]
        {
            get
            {
                int index = int.Parse(field.Substring(1));
                switch(field[0])
                {
                    case 'A':
                        return A[index] ? PlayerId.A : PlayerId.None;
                    case 'B':
                        return B[index] ? PlayerId.B : PlayerId.None;
                    case 'M':
                        return A[index] ? PlayerId.A : B[index] ? PlayerId.B : PlayerId.None;
                }

                return PlayerId.None;
            }
        }

        public Board()
        {
            A = new bool[PathLength];
            B = new bool[PathLength];
        }

        private Board(bool[] a, bool[] b)
        {
            this.A = a;
            this.B = b;
        }

        public Board Clone()
        {
            return new Board((bool[])A.Clone(), (bool[])B.Clone());
        }

        /// <summary>
        /// Expects a valid/precomputed move and executes it.
        /// 
        /// Does not update the player
        /// </summary>
        public void Move(MoveInfo validMove)
        {
            var me = validMove.PlayerId == PlayerId.A ? A : B;

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
                var other = validMove.PlayerId == PlayerId.A ? B : A;
                other[validMove.EndIndex] = false;
            }

            OnChanged();
        }

        protected virtual void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
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

        public OccupationState IsOccupied(PlayerId viewId, int index)
        {
            var me = viewId == PlayerId.A ? A : B;
            var other = viewId == PlayerId.A ? B : A;

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
