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
        /// <summary> The tile from which this is accessed. Allows reconstructing paths </summary>
        public int PreviousTile;
        /// <summary> This tiles walls </summary>
        public Walls Walls;

        public bool IsVisited;

        public MazeTile(int2 pos, int index)
        {
            Pos = pos;
            Index = index;
            Walls = Walls.Full;
            PreviousTile = -1;
            IsVisited = false;
        }
        
        public void OpenWall(Walls.Sides wall) =>
            Walls.RemoveWall(wall);
        public void PlaceWall(Walls.Sides wall) =>
            Walls.PlaceWall(wall);

        public Vector3Int ToVec3Int() => new(Pos.x, Pos.y);
    }
}