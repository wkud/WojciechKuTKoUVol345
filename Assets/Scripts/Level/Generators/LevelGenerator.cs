using UnityEngine;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// Base class for level generators.
    /// </summary>
    public abstract class LevelGenerator : ScriptableObject, IAmLevelGenerator
    {
        public abstract int minLevelWidth { get; }
        public abstract int minLevelHeight { get; }

        public abstract void GenerateLevel(Level level);
        public abstract bool IsLevelValid(Level level);

    }
}