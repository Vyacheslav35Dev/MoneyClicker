using Components;
using Components.Upgrades;
using Data.SO;
using Leopotam.Ecs;

namespace Systems
{
    public class UpdateBoostersViewSystem: IEcsRunSystem 
    {
        private readonly EcsWorld _world = null;
        
        private EcsFilter<UpdateUpgradeEvent, UpgradeComponent, TaskComponentView> _filter;
        
        void IEcsRunSystem.Run () 
        {
            foreach (var idx in _filter) 
            {
                var eventComponent = _filter.Get1(idx);
                var upgradeComponent = _filter.Get2(idx);
                var taskComponentView = _filter.Get3(idx);
                
                var priceFirstBooster = upgradeComponent.Boosters[0].BoosterActivatedText;
                if (!upgradeComponent.Boosters[0].IsActivated)
                {
                    priceFirstBooster = upgradeComponent.Boosters[0].PriceLocalizedText.Replace("{0}",
                    upgradeComponent.Boosters[0].Price.ToString());    
                }
                taskComponentView.View.UpdateFirstBooster(upgradeComponent.Boosters[0].Name,
                    upgradeComponent.Boosters[0].Description, priceFirstBooster);
                
                var priceSecondBooster = upgradeComponent.Boosters[1].BoosterActivatedText;
                if (!upgradeComponent.Boosters[1].IsActivated)
                {
                    priceSecondBooster = upgradeComponent.Boosters[1].PriceLocalizedText.Replace("{0}",
                        upgradeComponent.Boosters[1].Price.ToString());    
                }
                taskComponentView.View.UpdateSecondBooster(upgradeComponent.Boosters[1].Name,
                    upgradeComponent.Boosters[1].Description, priceSecondBooster);
                
                var entity = _filter.GetEntity(idx);
                entity.Del<UpdateUpgradeEvent>();
            }
        }
    }
}