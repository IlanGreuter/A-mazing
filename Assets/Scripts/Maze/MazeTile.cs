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

        public Walls Walls { get; private set; }

        public MazeTile(int2 pos, int index, Walls walls = new())
        {
            Pos = pos;
            Index = index;

            //Default values
            Walls = walls;
        }

        public Vector3Int ToVec3Int() => new(Pos.x, Pos.y);
    }
}
