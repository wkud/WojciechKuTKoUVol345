using TKOU.SimAI.Levels;
using UnityEngine;

namespace TKOU.SimAI
{
    /// <summary>
    /// Visualisation of the <see cref="Building"/> object.
    /// </summary>
    public class BuildingEntity : MonoBehaviour, IAmEntity
    {
        public Building Building { get; private set; }
        
        public static BuildingEntity SpawnEntity(Building ownerBuilding)
        {
            BuildingData data = ownerBuilding.buildingData;

            if (data == null)
            {
                Debug.LogError($"Can't spawn a building, {nameof(data.BuildingEntityPrefab)} is null!");
                return null;
            }

            BuildingEntity buildingEntity = Instantiate(data.BuildingEntityPrefab);
            buildingEntity.Initialize(ownerBuilding);

            return buildingEntity;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void Initialize(Building ownerBuilding)
        {
            Building = ownerBuilding;
            transform.position = ownerBuilding.Tile.Position;
        }
    }
}
