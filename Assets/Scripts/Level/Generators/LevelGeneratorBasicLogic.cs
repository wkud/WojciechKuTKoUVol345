using UnityEngine;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// Base class for level generators that contains some of the basic functionalities.
    /// Feel free to add it as a component to your custom level generators and use in your methods.
    /// </summary>
    [System.Serializable]
    public class LevelGeneratorBasicLogic: IAmLevelGenerator
    {
        #region Variables

        [field:SerializeField]
        public int minLevelWidth { get; set; } = 8;

        [field: SerializeField]
        public int minLevelHeight { get; set; } = 8;

        #endregion Variables

        #region Constructors

        public LevelGeneratorBasicLogic()
        {

        }

        public LevelGeneratorBasicLogic(int minWidth, int minHeight)
        {
            this.minLevelWidth = minWidth;
            this.minLevelHeight = minHeight;
        }

        #endregion Constructors

        #region Public methods

        /// <summary>
        /// Generates the level using this generator.
        /// When overriden make sure to call 
        /// the base method first to create the <see cref="Level"/> for usage.
        /// </summary>
        /// <param name="width">Width of the level</param>
        /// <param name="height">Height of the level</param>
        /// <returns><see cref="Level"/> base or NULL if not all entry conditions are fulfilled. </returns>
        public void GenerateLevel(Level level)
        {
            if (level == null)
            {
                Debug.LogError($"{nameof(level)} is null!");
                return;
            }
            else
            {
                if (!IsLevelValid(level))
                {
                    Debug.LogError($"{nameof(level)} is not valid!");
                    return;
                }
            }
        }

        /// <summary>
        /// Checks if the provided configuration is valid.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool IsLevelValid(Level level)
        {
            LevelGeneratorConfig config = level.configuration;

            if (config.width < minLevelWidth)
            {
                Debug.LogError($"{nameof(config.width)} must be at least {minLevelWidth}");
                return false;
            }

            if (config.height < minLevelHeight)
            {
                Debug.LogError($"{nameof(config.height)} must be at least {minLevelHeight}");
                return false;
            }

            return true;
        }

        #endregion Public methods
    }
}