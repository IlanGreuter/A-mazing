using Unity.Mathematics;
using UnityEngine;

namespace IlanGreuter.Maze
{
    public static class ExtensionMethods
    {
        public static int2 ToInt2(this Vector3Int vec) =>
            new(vec.x, vec.y);
        public static int2 ToInt2(this Vector2Int vec) =>
            new(vec.x, vec.y);

        public static Vector3Int ToVec3Int(this int2 vec) =>
            new(vec.x, vec.y);
        public static Vector2Int ToVec2Int(this int2 vec) =>
            new(vec.x, vec.y);
    }
}
