using UnityEngine;

namespace IlanGreuter.Maze
{
    public struct Walls
    {
        //Using ints instead of bytes leaves about 28 bits unused
        //Sadly Bitwise operatiors dont work with bytes so a lot of casting wouldve been needed
        private int sides;

        /// <summary> A data structure to keep 4 walls </summary>
        /// <param name="sides"> The 1st four bits represent the walls, starting from up and rotating counter clockwise. </param>
        public Walls(int sides = 15)
        {
            this.sides = sides;
        }

        /// <summary> A set that is walled on every side </summary>
        public static Walls Full => new Walls(15);
        /// <summary> A set that has no walls </summary>
        public static Walls Empty => new Walls(0);

        /// <summary> Check whether there is a wall to this side </summary>
        public bool IsWallInDirection(Sides side) =>
            IsWallInDirection((int)side);
        
        private bool IsWallInDirection(int side) =>
            (sides & (1 << side)) != 0;

        /// <summary> Remove the wall on this side </summary>
        public void RemoveWall(Sides side) =>
            sides &= ~(1 << (int)side);

        /// <summary> Place the wall on this side </summary>
        public void PlaceWall(Sides side) =>
            sides |= (1 << (int)side);

        /// <summary> Count the number of walls </summary>
        public int CountWalls()
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
                if (IsWallInDirection(i))
                    count++;
            return count;
        }

        /// <summary> Is this side and the one opposite open? </summary>
        public bool IsStraight(Sides side) => !IsWallInDirection(side) && !IsWallInDirection((int)Mathf.Repeat((int)side + 2, 4));
        /// <summary> Is this side and a side next to it open </summary>
        public bool IsCorner(Sides side) => IsCornerCW(side) || IsCornerCounterCW(side);
        /// <summary> Is this side and the side next to it (rotating clockwise) open </summary>
        public bool IsCornerCW(Sides side) => !IsWallInDirection(side) && !IsWallInDirection((int)Mathf.Repeat((int)side + 3, 4));
        /// <summary> Is this side and the side next to it (rotating counterclockwise) open </summary>
        public bool IsCornerCounterCW(Sides side) => !IsWallInDirection(side) && !IsWallInDirection((int)Mathf.Repeat((int)side + 1, 4));

        /// <summary> Gets the first side that is open (checking counter clockwise) </summary>
        public Sides? GetOpenSide()
        {
            int i = GetOpen();
            return i >= 0 ? (Sides)i : null;
        }

        private int GetOpen()
        {
            for (int i = 0; i < 4; i++)
                if (!IsWallInDirection(i))
                    return i;
            return -1;
        }

        /// <summary> Gets the first side that is closed (checking counter clockwise) </summary>
        public Sides? GetClosedSide()
        {
            int i = GetClosed();
            return i >= 0 ? (Sides)i : null;
        }

        private int GetClosed()
        {
            for (int i = 0; i < 4; i++)
                if (IsWallInDirection(i))
                    return i;
            return -1;
        }

        //In this order two opposites will always be 2 apart
        public enum Sides
        {
            Top = 0,
            Left = 1,
            Bottom = 2,
            Right = 3,
        }
    }
}
