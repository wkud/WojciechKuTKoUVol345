using System;
using System.Collections.Generic;
using UnityEngine;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// Current level that contains all data.
    /// </summary>
    public class Level : IDisposable
    {
        #region Variables

        /// <summary>
        /// generators that were used to create this level.
        /// </summary>

        public List<IAmLevelGenerator> usedGenerators { get; private set; }

        /// <summary>
        /// All level entities.
        /// </summary>
        public List<IAmEntity> entities { get; private set; }

        /// <summary>
        /// Tiles that make this level.
        /// </summary>
        public Tile [] levelTiles { get; private set; }

        /// <summary>
        /// Config that is used to generate this level.
        /// </summary>
        public LevelGeneratorConfig configuration { get; private set; }

        #endregion Variables

        #region Constructors

        public Level(LevelGeneratorConfig usedConfig)
        {
            this.usedGenerators = new List<IAmLevelGenerator>();
            this.entities = new List<IAmEntity>();
            this.configuration = usedConfig;
            this.levelTiles = new Tile[usedConfig.width * usedConfig.height];
        }

        #endregion Constructors

        #region Public methods

        /// <summary>
        /// Adds a generator that was used to create this level.
        /// </summary>
        public void AddUsedGenerator(IAmLevelGenerator generator)
        {
            usedGenerators.Add(generator);
        }

        public Tile GetTile(int x, int y)
        {
            return levelTiles[x + y * configuration.width];
        }

        public Tile GetTile(int index)
        {
            return levelTiles[index];
        }

        public void SetTile(int x, int y, TileData tileData)
        {
            Vector3 positionVector = new Vector3(x, 0, y);
            positionVector.Scale(configuration.tileSize);

            Tile tile = new Tile(tileData, x, y, positionVector);

            levelTiles[x + y * configuration.width] = tile;
        }

        public void SetTile(int index, TileData tileData)
        {
            Vector3 positionVector = new Vector3(index % configuration.width,
                                            0,
                                            index / configuration.width);

            positionVector.Scale(configuration.tileSize);

            int xIndex = index % configuration.width;
            int yIndex = index / configuration.width;

            Tile tile = new Tile(tileData, xIndex, yIndex, positionVector);

            levelTiles[index] = tile;
        }

        public void GenerateEntities()
        {
            for (int i = 0; i < levelTiles.Length; i++)
            {
                entities.Add(TileEntity.SpawnTileEntity(levelTiles[i]));
            }
        }

        public void Dispose()
        {
            usedGenerators.Clear();
           
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Destroy();
            }

            entities.Clear();
            this.configuration = null;
            levelTiles = new Tile[0];
        }

        #endregion Constructors
    }
}