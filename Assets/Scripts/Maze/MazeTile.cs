using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace IlanGreuter.Maze
{
    [BurstCompile]
    public struct MazeTile
    {
        /// <summary> The position in the tilemap this tile represents </summary>
        public readonly int2 Pos;
        /// <summary> The index in the algorithm's list this tile represents </summary>
        public readonly int Index;

        private byte Walls;

        public MazeTile(int2 pos, int index, byte walls = byte.MaxValue)
        {
            Pos = pos;
            Index = index;

            //Default values
            Walls = walls;
        }

        public Vector3Int ToVec3Int() => new(Pos.x, Pos.y);
    }
}
