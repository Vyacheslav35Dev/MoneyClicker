using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponents
{
    public class BoosterView : MonoBehaviour
    {
        [Header("Tittle text")]
        [SerializeField]
        private TMP_Text tittleText;
   
        [Header("Description text")]
        [SerializeField]
        private TMP_Text descriptionText;
   
        [Header("Price text")]
        [SerializeField]
        private TMP_Text priceText;
    
        [Header("Button")]
        [SerializeField]
        private Button activateButton;

        public Action onClick;

        public void Init(string tittle, string desctiption, string price)
        {
            tittleText.text = tittle;
            descriptionText.text = desctiption;
            priceText.text = price;
        }

        public void UpdatePrice(string price)
        {
            priceText.text = price;
        }
    
        public void UpdateDescription(string desctiption)
        {
            descriptionText.text = desctiption;
        }
    
        private void Click()
        {
            onClick?.Invoke();
        }

        private void OnEnable()
        {
            activateButton.onClick.AddListener(Click);
        }
    
        private void OnDisable()
        {
            activateButton.onClick.RemoveListener(Click);
        }
    }
}
