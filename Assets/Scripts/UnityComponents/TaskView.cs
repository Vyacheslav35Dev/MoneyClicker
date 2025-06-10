using Components.Level;
using Components.Upgrades;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponents
{
   public class TaskView : MonoBehaviour
   {
      private int _id;
      
      [Header("Tittle text")]
      [SerializeField]
      private TMP_Text tittleText;
   
      [Header("Level Tittle text")]
      [SerializeField]
      private TMP_Text lvlTittleText;
   
      [Header("Level text")]
      [SerializeField]
      private TMP_Text lvlText;
   
      [Header("LevelUp text")]
      [SerializeField]
      private TMP_Text levelUpText;
   
      [Header("LevelUp Price text")]
      [SerializeField]
      private TMP_Text levelUpPriceText;
   
      [Header("LevelUp Price text")]
      [SerializeField]
      private Button levelUpButton;
      
      [Header("Reward Tittle text")]
      [SerializeField]
      private TMP_Text rewardTittleText;
      
      [Header("Reward text")]
      [SerializeField]
      private TMP_Text rewardText;
      
      [Header("Reward slider")]
      [SerializeField]
      private Slider rewardSlider;
      
      [Header("First Booster view")]
      [SerializeField]
      private BoosterView firstBoosterView;
      
      [Header("Second Booster view")]
      [SerializeField]
      private BoosterView secondBoosterView;
      
      private EcsEntity _entity;

      public void Init(EcsEntity entity)
      {
         _entity = entity;
      }

      public void UpdateSlider(float revenue, float maxRevenue)  
      {
         rewardSlider.maxValue = maxRevenue;
         rewardSlider.value = revenue;
      }
      
      public void UpdateLvl(int lvl, string lvlTittle)
      {
         lvlText.text = lvl.ToString();
         lvlTittleText.text = lvlTittle;
      }
      
      public void UpdateLvlUpButton(string price, string lvlUpTittle)
      {
         levelUpText.text = lvlUpTittle;
         levelUpPriceText.text = price.ToString();
      }
      
      public void UpdateName(string name)
      {
         tittleText.text = name;
      }

      public void UpdateFirstBooster(string tittle, string description, string price)
      {
         firstBoosterView.Init(tittle, description, price);
      }
      
      public void UpdateSecondBooster(string tittle, string description, string price)
      {
         secondBoosterView.Init(tittle, description, price);
      }
      
      public void UpdateReward(float value, string rewardTittle)
      {
         rewardTittleText.text = rewardTittle;
         rewardText.text = value.ToString();
      }
   
      private void ClickLevelUpButton()
      {
         _entity.Get<LevelUpEvent>();
      }
   
      private void ClickBuyFirstBooster()
      {
         _entity.Get<BuyFirstUpgradeEvent>();
      }
   
      private void ClickBuySecondBooster()
      {
         _entity.Get<BuySecondUpgradeEvent>();
      }

      private void OnEnable()
      {
         levelUpButton.onClick.AddListener(ClickLevelUpButton);
         firstBoosterView.onClick += ClickBuyFirstBooster;
         secondBoosterView.onClick += ClickBuySecondBooster;
      }
   
      private void OnDisable()
      {
         levelUpButton.onClick.RemoveListener(ClickLevelUpButton);
         firstBoosterView.onClick -= ClickBuyFirstBooster;
         secondBoosterView.onClick -= ClickBuySecondBooster;
      }
   }
}
