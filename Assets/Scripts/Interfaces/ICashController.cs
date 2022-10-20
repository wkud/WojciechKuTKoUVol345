namespace TKOU.SimAI
{
    public interface ICashController
    {
        bool CanAfford(int price);
        void NotifyOnBuildingBuilt(int buildingPrice);
        void GenerateIncome();
    }
}