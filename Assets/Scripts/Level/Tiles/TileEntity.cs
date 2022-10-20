using System;
using TKOU.SimAI.Levels;
using UnityEngine;

namespace TKOU.SimAI
{
    /// <summary>
    /// Visualisation of the <see cref="Tile"/> object.
    /// </summary>
    public class TileEntity: MonoBehaviour, IAmEntity, IHavePosition
    {
        #region Variables

        public Tile Tile { get; private set; }

        public Vector3 Position => transform.position;

        #endregion Variables

        #region Public methods

        public static TileEntity SpawnTileEntity(Tile ownerTile)
        {
            TileData tileData = ownerTile.Data;

            if (tileData == null)
            {
                Debug.LogError($"Can't spawn a tile, {nameof(tileData.tileEntityPrefab)} is null!");
                return null;
            }

            TileEntity tileEntity = GameObject.Instantiate(tileData.tileEntityPrefab);
            tileEntity.Initialize(ownerTile);

            return tileEntity;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public override string ToString()
        {
            return Tile.ToString();
        }

        #endregion Public methods

        #region Private methods

        private void Initialize(Tile ownerTile)
        {
            Tile = ownerTile;
            transform.position = ownerTile.Position;
        }

        #endregion Private methods
    }
}
