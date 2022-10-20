using System.Collections;
using System.Collections.Generic;
using TKOU.SimAI.Levels;
using UnityEngine;
using UnityEngine.Events;

namespace TKOU.SimAI
{
    /// <summary>
    /// Handles building in the game
    /// </summary>
    public class BuildHandler: IHaveBuildSelection
    {
        private readonly ICashController cashController;
        
        private IAmData buildSelection;

        private GameObject ghostGameObject;

        private IAmEntity buildTarget;

        public IAmEntity BuildTarget 
        {
            get
            {
                return buildTarget;
            }

            set
            {
                if(buildTarget == value)
                {
                    return;
                }

                buildTarget = value;
                
                if (buildTarget is IHavePosition havePosition)
                {
                    UpdateGhostPosition(havePosition);
                }
            }
        }

        public IAmData BuildSelection
        {
            get => buildSelection;
            set
            {
                if(buildSelection == value)
                {
                    return;
                }

                buildSelection = value;

                Debug.Log($"BuildSelection changed: {buildSelection}");
                UpdateGhostGO();
            }
        }

        public BuildHandler(ICashController cashController)
        {
            this.cashController = cashController;
        }
        
        public void AttemptToBuildSelection()
        {
            if(ghostGameObject == null)
            {
                return;
            }

            if(BuildTarget == null)
            {
                return;
            }

            if(BuildSelection is BuildingData buildingData 
                && BuildTarget is TileEntity tileEntity
                && cashController.CanAfford(buildingData.BuildingCost))
            {
                Building building = new Building(buildingData, tileEntity.Tile);
                tileEntity.Tile.AddObject(building);
                BuildingEntity.SpawnEntity(building);
                cashController.NotifyOnBuildingBuilt(buildingData.BuildingCost);
                
                BuildSelection = null;
            }
        }

        private void UpdateGhostPosition(IHavePosition havePosition)
        {
            if (ghostGameObject == null)
            {
                return;
            }

            ghostGameObject.transform.position = havePosition.Position;
        }

        private void UpdateGhostGO()
        {
            if (ghostGameObject != null)
            {
                GameObject.Destroy(ghostGameObject);
            }

            if (buildSelection == null)
            {
                return;
            }

            CreateGhostGameObject(buildSelection);
        }

        private void CreateGhostGameObject(IAmData sourceData)
        {
            if (sourceData.EntityPrefab == null)
            {
                return;
            }

            Object prefab = sourceData.EntityPrefab as Object;

            if(prefab == null)
            {
                return;
            }

            ghostGameObject = new GameObject();
            ghostGameObject.SetActive(false);
            Object instantiatedObject = GameObject.Instantiate(prefab, ghostGameObject.transform);

            Behaviour[] behaviours = ghostGameObject.GetComponentsInChildren<Behaviour>();

            for(int i = 0; i < behaviours.Length; i++)
            {
                GameObject.DestroyImmediate(behaviours[i]);
            }

            ghostGameObject.SetActive(true);
        }
    }
}