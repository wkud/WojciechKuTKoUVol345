using TKOU.SimAI.Levels;
using UnityEngine;

#if UNITY_EDITOR
using TKOU.SimAI.Editor;
#endif

namespace TKOU.SimAI
{
    /// <summary>
    /// Contains all game elements.
    /// </summary>
    [CreateAssetMenu(fileName =nameof(GameContents), menuName = nameof(SimAI) + "/" + nameof(GameContents))]
    public class GameContents : ScriptableObject
    {
        public TileData[] tiles;
        public BuildingData[] buildings;

        #if UNITY_EDITOR

        [ContextMenu(nameof(LoadAll))]
        public void LoadAll()
        {
            
        }

        #endif
    }
}