using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// Single tile of the level.
    /// </summary>
    public class Tile
    {
        public TileData Data { get; private set;}

        public int X { get; private set; }

        public int Y { get; private set; }

        public Vector3 Position { get; private set; }

        private List<IAmTileObject> tileObjects;

        public Tile(TileData tileData, int x, int y, Vector3 position)
        {
            this.Data = tileData;
            this.X = x;
            this.Y = y;
            this.Position = position;
            tileObjects = new List<IAmTileObject>();
        }

        public void AddObject(IAmTileObject tileObject)
        {
            tileObjects.Add(tileObject);
        }

        public void RemoveObject(IAmTileObject tileObject)
        {
            if (!tileObjects.Remove(tileObject))
            {
                Debug.LogError($"Failed to remove object : {tileObject.GetType()}. It doesn't exist on this tile!");
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"data:{Data} x:{X} y:{Y} pos:{Position} tileObjects:");
            
            for(int i = 0; i < tileObjects.Count; i++)
            {
                sb.AppendLine($"[{i}] - {tileObjects[i]}");
            }

            return sb.ToString();
        }
    }
}
