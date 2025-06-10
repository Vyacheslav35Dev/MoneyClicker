using Components;
using Components.Level;
using Components.Reward;
using Components.Upgrades;
using Data;
using Leopotam.Ecs;
using Services;
using UnityComponents;

namespace Systems
{
    public class BoostersSystem : IEcsRunSystem 
    {
        private readonly EcsWorld _world = null;
        
        private readonly IPlayerService _playerService = null;
        private readonly GameView _gameView = null;
        
        private EcsFilter<BuyFirstUpgradeEvent,  UpgradeComponent, RewardComponent, LevelComponent> _filterFirstBoosters;
        private EcsFilter<BuySecondUpgradeEvent, UpgradeComponent, RewardComponent, LevelComponent> _filterSecondBoosters;

        public BoostersSystem(IPlayerService playerService, GameView gameView)
        {
            _playerService = playerService;
            _gameView = gameView;
        }
        
        void IEcsRunSystem.Run () 
        {
            foreach (var idx in _filterFirstBoosters) 
            {
                var eventComponent = _filterFirstBoosters.Get1(idx);
                ref var upgradeComponent = ref _filterFirstBoosters.Get2(idx);
                ref var rewardComponent = ref _filterFirstBoosters.Get3(idx);
                var levelComponent = _filterFirstBoosters.Get4(idx);
                var entity = _filterFirstBoosters.GetEntity(idx);
                
                if (levelComponent.Level != 0 && !upgradeComponent.Boosters[0].IsActivated)
                {
                    var currency = _playerService.GetCurrency();
                    var price = upgradeComponent.Boosters[0].Price;
                    if (currency >= price)
                    {
                        currency -= price;
                        _playerService.SetCurrency(currency);
                        _playerService.SaveBooster(upgradeComponent.Boosters[0].Key);
                        _gameView.UpdateCurrency(currency);
                        upgradeComponent.Boosters[0].IsActivated = true;
                        rewardComponent.CurrentReward = GetReward(upgradeComponent.Boosters[0],
                            upgradeComponent.Boosters[1], levelComponent.Level, rewardComponent.BaseReward);
                        if (!entity.Has<UpdateUpgradeEvent>())
                        {
                            entity.Get<UpdateUpgradeEvent>();
                        }

                        if (!entity.Has<UpdateRewardEvent>())
                        {
                            entity.Get<UpdateRewardEvent>(); 
                        }
                    }
                }
                entity.Del<BuyFirstUpgradeEvent>();
            }
            
            foreach (var idx in _filterSecondBoosters) 
            {
                var eventComponent = _filterSecondBoosters.Get1(idx);
                var upgradeComponent = _filterSecondBoosters.Get2(idx);
                ref var rewardComponent = ref _filterSecondBoosters.Get3(idx);
                var levelComponent = _filterSecondBoosters.Get4(idx);
                var entity = _filterSecondBoosters.GetEntity(idx);
                
                if (levelComponent.Level != 0 && !upgradeComponent.Boosters[1].IsActivated)
                {
                    var currency = _playerService.GetCurrency();
                    var price = upgradeComponent.Boosters[1].Price;
                    if (currency >= price)
                    {
                        currency -= price;
                        _playerService.SetCurrency(currency);
                        _playerService.SaveBooster(upgradeComponent.Boosters[1].Key);
                        upgradeComponent.Boosters[1].IsActivated = true;
                        rewardComponent.CurrentReward = GetReward(upgradeComponent.Boosters[0],
                            upgradeComponent.Boosters[1], levelComponent.Level, rewardComponent.BaseReward);
                        if (!entity.Has<UpdateUpgradeEvent>())
                        {
                            entity.Get<UpdateUpgradeEvent>();
                        }

                        if (!entity.Has<UpdateRewardEvent>())
                        {
                            entity.Get<UpdateRewardEvent>(); 
                        }
                    }
                }
                entity.Del<BuySecondUpgradeEvent>();
            }
        }
        
        private float GetReward(Booster firstBooster, Booster secondBooster, int level, float baseReward)
        {
            float firstBoosterCoof = 0;
            float secondBoosterCoof = 0;

            if (firstBooster.IsActivated)
            {
                firstBoosterCoof = firstBooster.Coof;
            }
            if (secondBooster.IsActivated)
            {
                secondBoosterCoof = secondBooster.Coof;
            }
            
            return level * baseReward * (1 + (baseReward/100 * firstBoosterCoof) + (baseReward/100 * secondBoosterCoof));
        }
    }
}