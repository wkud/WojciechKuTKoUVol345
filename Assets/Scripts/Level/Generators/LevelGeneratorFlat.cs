using UnityEngine;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// Generates a flat tiled level.
    /// </summary>
    [CreateAssetMenu(fileName =nameof(LevelGeneratorFlat), menuName = nameof(SimAI)+"/"+nameof(SimAI.Levels)+"/"+nameof(LevelGeneratorFlat))]
    public class LevelGeneratorFlat : LevelGenerator, IAmLevelGenerator
    {
        #region Variables

        [SerializeField, HideInInspector]
        private LevelGeneratorBasicLogic generationBasicLogic;

        public override int minLevelWidth => 8;
        public override int minLevelHeight => 8;

        [Tooltip("Tile to use for the terrain")]
        public TileData tileData;

        #endregion Variables

        #region Unity methods

        private void Awake()
        {
            generationBasicLogic = new LevelGeneratorBasicLogic(minLevelWidth, minLevelHeight);
        }

        #endregion Unity methods

        #region Public methods

        public override void GenerateLevel(Level level)
        {
            Tile [] levelTiles = level.levelTiles;

            for (int i = 0; i < levelTiles.Length; i++)
            {
                level.SetTile(i, tileData);
            }
        }

        public override bool IsLevelValid(Level level)
        {
            return generationBasicLogic.IsLevelValid(level);
        }

        #endregion Public methods
    }
}
