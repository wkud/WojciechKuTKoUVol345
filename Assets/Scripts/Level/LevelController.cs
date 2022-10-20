using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// High-level controller for game levels, which uses <see cref="LevelGeneratorBasicLogic"/> 
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        #region Variables

        /// <summary>
        /// Level that is currently running.
        /// </summary>
        [field:Header("State")]
        public Level currentLevel { get; private set; }

        /// <summary>
        /// Current level generator to be used when a new level is created.
        /// </summary>
        [field:Header("Generation")]
        public IAmLevelGenerator [] generators { get; set;}

        public LevelGeneratorConfig generationConfig { get; set; }

        private const int minLevelWidth = 1;
        private const int minLevelHeight = 1;

        /// <summary>
        /// TRUE if a level is running. FALSE otherwise.
        /// </summary>
        public bool isLevelRunning => currentLevel != null;

        #endregion Variables

        #region Public methods

        /// <summary>
        ///  Creates a level using the configured <see cref="generationConfig"/>
        ///  and <see cref="generators"/>
        /// </summary>
        /// <returns>FALSE if generation failed. TRUE if it succeed</returns>
        public Level GenerateLevel()
        {
            if (generators == null)
            {
                Debug.LogError($"{nameof(generators)} can't be null!");
                return null;
            }

            Level level = CreateLevelBase(generationConfig);

            if(level == null)
            {
                Debug.LogError("Failed to create a level!");
                return null;
            }

            for(int i = 0; i < generators.Length; i++)
            {
                IAmLevelGenerator generator = generators[i];

                if (generator.IsLevelValid(level))
                {
                    generator.GenerateLevel(level);
                }
                else
                {
                    Debug.LogError($"current Level is not valid for the generator: {generator} !");
                }
            }

            return level;
        }

        public void RunLevel(Level level)
        {
            if (currentLevel != null) 
            {
                Debug.LogError($"Can't run a level when one is already running! Call {nameof(StopLevel)} first.");
                return;
            }

            currentLevel = level;
            level.GenerateEntities();
        }


        public void StopLevel()
        {
            if(currentLevel == null)
            {
                Debug.LogError("Tried to stop level that is not running!");
                return;
            }

            currentLevel.Dispose();

            currentLevel = null;
        }

        #endregion Public methods

        #region Private methods

        private Level CreateLevelBase(LevelGeneratorConfig configuration)
        {
            if (IsConfigurationValid(configuration))
            {
                Level level = new Level(configuration);
                return level;
            }
            else
            {
                Debug.LogError($"{nameof(LevelGeneratorConfig)} is not valid!");
                return null;
            }
        }

        private bool IsConfigurationValid(LevelGeneratorConfig configuration)
        {
            if (configuration.width < minLevelWidth)
            {
                Debug.LogError($"{nameof(configuration.width)} must be at least {minLevelWidth}");
                return false;
            }

            if (configuration.height < minLevelHeight)
            {
                Debug.LogError($"{nameof(configuration.height)} must be at least {minLevelHeight}");
                return false;
            }

            return true;
        }

        #endregion Private methods
    }
}