using System;
using System.Text;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// Single building that can be placed on a tile
    /// </summary>
    public class Building: IAmTileObject
    {
        public Type Type => typeof(Building);

        public Tile Tile {get; private set;}

        public BuildingData buildingData { get; private set; }

        public Building(BuildingData buildingData, Tile tile)
        {
            this.buildingData = buildingData;
            this.Tile = tile;
        }

        public override string ToString()
        {
            return $"data:{buildingData} ";
        }
    }
}
