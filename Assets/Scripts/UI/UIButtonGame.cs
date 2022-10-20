using System.Collections;
using System.Collections.Generic;
using TKOU.SimAI.Levels;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace TKOU.SimAI
{
    /// <summary>
    /// A generic game button.
    /// </summary>
    public class UIButtonGame : MonoBehaviour
    {
        #region Variables

        public IAmData Data { get; private set; }

        [SerializeField]
        private Button button;

        [SerializeField]
        private TextMeshProUGUI nameText;
        
        [SerializeField]
        private TextMeshProUGUI priceText;

        [SerializeField]
        private Image iconImage;

        private const string nullDataName = "Empty";
        private const string nullDataPrice = "0";
        private const Sprite nullDataSprite = null;

        #endregion Variables

        #region Events

        public event System.Action<UIButtonGame> OnClickE;

        #endregion Events

        #region Unity methods

        private void Awake()
        {
            button.onClick.AddListener(Button_OnClick);
        }

        #endregion Unity methods

        #region Public methods

        public void SetData(IAmData data)
        {
            this.Data = data;

            UpdateUI();
        }

        #endregion Public methods

        #region Private methods

        public void UpdateUI()
        {
            if(Data != null)
            {
                nameText.text = Data.DataName;
                priceText.text = (Data as BuildingData)?.BuildingCost.ToString() ?? string.Empty;
                iconImage.sprite = Data.DataIcon;

                nameText.gameObject.SetActive(string.IsNullOrEmpty(nameText.text) == false);
                priceText.gameObject.SetActive(string.IsNullOrEmpty(priceText.text) == false);
                iconImage.gameObject.SetActive(iconImage.sprite != null);
            }
            else
            {
                nameText.text = nullDataName;
                nameText.text = nullDataPrice;
                iconImage.sprite = nullDataSprite;

                nameText.gameObject.SetActive(true);
                iconImage.gameObject.SetActive(false);
            }
        }

        #endregion Private methods

        #region Event callbacks

        private void Button_OnClick()
        {
            OnClickE?.Invoke(this);
        }

        #endregion Event callbacks
    }
}
