using UnityEngine;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// Data of a single tile.
    /// </summary>
    [CreateAssetMenu(fileName =nameof(TileData), menuName = nameof(SimAI)+"/"+nameof(SimAI.Levels)+"/"+nameof(TileData))]
    public class TileData : ScriptableObject, IAmData
    {
        [field: SerializeField]
        public Sprite tileSprite { get; private set; }

        [field: SerializeField]
        public TileEntity tileEntityPrefab { get; private set; }

        [field:SerializeField]
        public string tileName { get; private set; }

        IAmEntity IAmData.EntityPrefab => tileEntityPrefab;

        Sprite IAmData.DataIcon => tileSprite;

        string IAmData.DataName => tileName;
    }
}
