using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// UI responsible for building menu.
    /// </summary>
    public class UIBuildingController : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private Button quitButton;

        [SerializeField]
        private GameObject buildingMenu;

        [SerializeField]
        private UIButtonGame buttonPrefab;

        [SerializeField]
        private Transform buttonsParent;

        [SerializeField]
        private int buttonsSiblingOffset = 1;

        private List<UIButtonGame> buttons;

        public IHaveBuildSelection buildSelectionTarget;

        #endregion Variables

        #region Unity metehods

        private void Awake()
        {
            buttons = new List<UIButtonGame>();

            gameController.OnGameRun += GameController_OnGameRun;
            gameController.OnGameEnd += GameController_OnGameEnd;

            quitButton.onClick.AddListener(QuitButton_OnClick);
        }

        #endregion Unity methods

        #region Private methods

        private void UpdateVisibility()
        {
            buildingMenu.SetActive(gameController.IsGameRunning);
        }

        private void UpdateUI()
        {
            GameContents contents = gameController.Contents;
            BuildingData[] buildings = contents.buildings;

            int currentButtonIndex = 0;

            List<UIButtonGame> buildingButtons = GetOrCreateButtons(ref currentButtonIndex, buildings.Length);

            for(int i = 0; i < buildings.Length; i++)
            {
                buildingButtons[i].SetData(buildings[i]);
            }
        }

        private void ClearUI()
        {
            for(int i = 0; i < buttons.Count; i++)
            {
                Destroy(buttons[i].gameObject);
            }

            buttons.Clear();
        }

        /// <summary>
        /// Gets the given amount of buttons, starting from the currentIndex.
        /// If required will instantiate more buttons.
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private List<UIButtonGame> GetOrCreateButtons(ref int currentIndex, int amount)
        {
            List<UIButtonGame> selectedButtons;

            if(amount <= 0)
            {
                Debug.LogError($"Tried to get {amount} buttons!");
                selectedButtons = new List<UIButtonGame>();

                return selectedButtons;
            }

            int neededAmount = (currentIndex + amount);
            int currentAmount = buttons.Count;

            int amountDifference = neededAmount - currentAmount;

            if (amountDifference > 0)
            {
                for (int i = 0; i < amountDifference; i++)
                {
                    CreateButton();
                }
            }

            selectedButtons = buttons.GetRange(currentIndex, amount);
            currentIndex += amount;

            return selectedButtons;
        }

        /// <summary>
        /// Creates a single button using default settings.
        /// </summary>
        /// <returns></returns>
        private UIButtonGame CreateButton()
        {
            UIButtonGame buttonGame = Instantiate(buttonPrefab);
            Transform transform = buttonGame.transform;
            transform.SetParent(buttonsParent, false);
            transform.SetSiblingIndex(buttonsSiblingOffset);
            buttonGame.OnClickE += ButtonGame_OnClickE;

            buttons.Add(buttonGame);

            return buttonGame;
        }

        #endregion Private methods

        #region Event callbacks

        private void GameController_OnGameRun()
        {
            UpdateUI();
            UpdateVisibility();
        }

        private void GameController_OnGameEnd()
        {
            UpdateVisibility();
            ClearUI();
        }

        private void QuitButton_OnClick()
        {
            gameController.StopGame();
        }

        private void ButtonGame_OnClickE(UIButtonGame button)
        {
            if(button.Data == null)
            {
                Debug.LogError("Button data is null!");
                return;
            }

            if(buildSelectionTarget == null)
            {
                Debug.LogError("Build selection target is null!");
                return;
            }

            buildSelectionTarget.BuildSelection = button.Data;
        }

        #endregion Event callbacks
    }
}