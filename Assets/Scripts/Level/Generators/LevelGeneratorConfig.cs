using UnityEngine;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// Configuration to be used by the <see cref="LevelGeneratorBasicLogic"/>
    /// </summary>
    [System.Serializable]
    public class LevelGeneratorConfig
    {
        public int width = 8;
        public int height = 8;

        public Vector3 tileSize = new Vector3(1.0f, 0, 1.0f);

        public Vector3 tileOffset = Vector3.zero;

        public Vector2 sizeInUnits
        {
            get
            {
                return new Vector2(tileSize.x * width, tileSize.z * height);
            }
        }

        /// <summary>
        /// Position where the level should start, in units.
        /// </summary>
        public Vector2 levelMinPosition
        {
            get
            {
                return new Vector2(tileOffset.x, tileOffset.z);
            }
        }

        /// <summary>
        /// Position where the level ends, in units.
        /// </summary>
        public Vector2 levelMaxPosition
        {
            get
            {
                return levelMinPosition + sizeInUnits;
            }
        }
    }
}
