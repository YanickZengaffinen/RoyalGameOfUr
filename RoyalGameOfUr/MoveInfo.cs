using static RoyalGameOfUr.Player;

namespace RoyalGameOfUr
{
    public class MoveInfo
    {
        /// <summary>
        /// The player executing the move
        /// </summary>
        public PlayerId PlayerId { get; set; }

        /// <summary>
        /// Where does the piece lay at the beginning of the move
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// Where does the piece lay at the end of the move
        /// </summary>
        public int EndIndex { get; set; }

        /// <summary>
        /// Is an enemy piece killed/moved back to the start
        /// </summary>
        public bool DoesKillEnemy { get; set; }

        /// <summary>
        /// Can the player do another move after this one
        /// </summary>
        public bool CanMoveAgain { get; set; }

        /// <summary>
        /// Is a piece currently safe
        /// </summary>
        public bool IsSafe { get; set; }

        /// <summary>
        /// Is a piece no longer safe
        /// </summary>
        public bool NoLongerSafe { get; set; }

        /// <summary>
        /// Does a piece finish its route
        /// </summary>
        public bool Finish { get; set; }
        
        /// <summary>
        /// Is a new piece added to the board
        /// </summary>
        public bool New { get; set; }

        /// <summary>
        /// Is the game won
        /// </summary>
        public bool Win { get; set; }

        /// <summary>
        /// Compare the moves origin and destination 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSame(MoveInfo other)
        {
            return 
                other.PlayerId == PlayerId && 
                other.StartIndex == StartIndex && 
                other.EndIndex == EndIndex;
        }
    }
}
