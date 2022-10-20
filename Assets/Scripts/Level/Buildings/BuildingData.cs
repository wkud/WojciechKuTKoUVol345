using UnityEngine;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// Data of a single building.
    /// </summary>
    [CreateAssetMenu(fileName =nameof(BuildingData), menuName = nameof(SimAI)+"/"+nameof(SimAI.Levels)+"/"+nameof(BuildingData))]
    public class BuildingData : ScriptableObject, IAmData
    {
        [field: SerializeField]
        public Sprite BuildingSprite { get; private set; }

        [field: SerializeField]
        public BuildingEntity BuildingEntityPrefab { get; private set; }

        [field:SerializeField]
        public string BuildingName { get; private set; }

        public int BuildingCost { get; set; } 
        
        Sprite IAmData.DataIcon => BuildingSprite;

        string IAmData.DataName => BuildingName;

        IAmEntity IAmData.EntityPrefab => BuildingEntityPrefab;
    }
}
