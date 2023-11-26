using Unity.Burst;

namespace IlanGreuter.Maze
{
    [BurstCompile]
    public static class ExtensionMethods
    {
        /// <summary> Is the nth bit of the integer set? The index of n is 0-based </summary>
        public static bool IsBitSet(this int i, int n) => (i & (1 << n)) != 0;

        /// <summary> Sets the nth bit of the integer to 1. The index of n is 0-based </summary>
        public static int SetBit(this int i, int n) => i | (1 << n);

        /// <summary> Sets the nth bit of the integer to 0. The index of n is 0-based </summary>
        public static int UnsetBit(this int i, int n) => i & ~(1 << n);
    }
}
