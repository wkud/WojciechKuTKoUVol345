using UnityEngine;
using TKOU.SimAI.Levels;
using TKOU.SimAI.Camera;
using System.Collections.Generic;
using TKOU.SimAI.Highlights;

namespace TKOU.SimAI
{
    /// <summary>
    /// Main controller of the game.
    /// </summary>
    public class GameController : MonoBehaviour, IGameController
    {
        #region Variables

        [SerializeField, Tooltip("If set to true will start the game automatically")]
        private bool autorun = true;

        [field:Header("Controllers")]
        [field: SerializeField]
        public PlayerController Player { get; private set; }

        [Header("Level")]
        [SerializeField, Tooltip("Level controller to use by the game.")]
        private LevelController levelController;

        [SerializeField, Tooltip("Level generator to use for the generation")]
        private LevelGenerator [] levelGenerators;

        [SerializeField]
        private int startLevelWidth = 32;

        [SerializeField]
        private int startLevelHeight = 32;

        [SerializeField]
        private Vector3 singleTileSize = new Vector3(30.0f, 5.0f, 30.0f);

        [field: SerializeField]
        public GameCamera GameCamera { get; private set; }

        /// <summary>
        /// All current game contents to use during gameplay.
        /// </summary>
        [field:Header("Game")]
        [field:SerializeField]
        public GameContents Contents { get; private set; }

        public bool IsGameRunning { get; private set;}

        #endregion Variables

        #region Events

        /// <summary>
        /// Called when the game is being run.
        /// </summary>
        public event System.Action OnGameRun;

        /// <summary>
        /// Called when the game is stopped.
        /// </summary>
        public event System.Action OnGameEnd;

        #endregion Events

        #region Unity methods

        private void Start()
        {
            if (autorun)
            {
                RunGame();
            }
        }

        #endregion Unity methods

        #region Public methods
        public void RunGame()
        {
            if (IsGameRunning)
            {
                Debug.LogError("Tried to run game that is already running!");
                return;
            }

            levelController.generationConfig = new LevelGeneratorConfig();
            levelController.generationConfig.tileSize = singleTileSize;
            levelController.generationConfig.tileOffset = -singleTileSize * 0.5f;

            levelController.generators = levelGenerators;

            Level level = levelController.GenerateLevel();
            levelController.RunLevel(level);

            GameCamera.SetBounds(levelController.generationConfig.levelMinPosition, levelController.generationConfig.levelMaxPosition);
            Player.Initialize(GameCamera);

            IsGameRunning = true;

            OnGameRun?.Invoke();
        }

        public void StopGame()
        {
            if (!IsGameRunning)
            {
                Debug.LogError("Tried to stop game that is not running!");
                return;
            }

            levelController.StopLevel();

            IsGameRunning = false;
            OnGameEnd?.Invoke();
        }

        #endregion Public methods
    }
}
