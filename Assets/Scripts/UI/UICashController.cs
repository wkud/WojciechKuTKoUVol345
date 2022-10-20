using System;
using TKOU.SimAI;
using TKOU.SimAI.Levels;
using TMPro;
using UnityEngine;

namespace TKOU.SimAI.UI
{
    public class UICashController : MonoBehaviour, ICashController
    {
        [SerializeField] private TMP_Text cashText; 
        [SerializeField] private TMP_Text cashSpentOnBuildingsText; 
        [SerializeField] private TMP_Text incomeRateText; 
        [SerializeField] private GameObject winPanel; 
        private const int INCOME_RATE_PER_BUILDING = 10;
        private const int INCOME_INTERVAL_IN_SECONDS = 5;        
        private const int WIN_THRESHOLD = 500;

        private const string CASH_SUFFIX = "$";
        private const string INCOME_PREFIX = "+";
        private const string EXPENDITURE_PREFIX = "-";
        private int totalMoneySpent = 0;
        private int buildingCount = 0;

        public int Cash { get; private set; } = 200;

        private Timer timer;

        private void Awake()
        {
            timer = new Timer(GenerateIncome, INCOME_INTERVAL_IN_SECONDS);
            UpdateUI();
        }

        private void Update()
        {
            timer.Update();
        }

        public void UpdateUI()
        {
            cashText.text = Cash + CASH_SUFFIX;
            incomeRateText.text = INCOME_PREFIX + (buildingCount * INCOME_RATE_PER_BUILDING) + CASH_SUFFIX;
            cashSpentOnBuildingsText.text = EXPENDITURE_PREFIX + totalMoneySpent + CASH_SUFFIX;
        }

        public bool CanAfford(int price)
        {
            return Cash >= price;
        }

        public void NotifyOnBuildingBuilt(int buildingPrice)
        {
            Cash -= buildingPrice;
            if (Cash < 0)
            {
                Debug.LogWarning("Make sure to check whether player can afford a building before building one");
            }

            buildingCount++;
            totalMoneySpent += buildingPrice;
            UpdateUI();
        }

        public void GenerateIncome()
        {
            Cash += buildingCount * INCOME_RATE_PER_BUILDING;
            UpdateUI();
            if (Cash > WIN_THRESHOLD)
            {
                winPanel.SetActive(true);
                timer.Disable();
            }
        }

    }
}