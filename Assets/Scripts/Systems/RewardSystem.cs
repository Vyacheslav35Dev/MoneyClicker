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
    sealed class RewardSystem : IEcsRunSystem 
    {
        private readonly EcsWorld _world = null;
        
        private readonly IPlayerService _playerService = null;
        private readonly GameView _gameView = null;
        
        private EcsFilter<RewardEvent, RewardComponent, LevelComponent, UpgradeComponent> _filter;
        
        public RewardSystem(IPlayerService playerService, GameView gameView)
        {
            _playerService = playerService;
            _gameView = gameView;
        }
        
        void IEcsRunSystem.Run () 
        {
            foreach (var idx in _filter) 
            {
                var eventComponent = _filter.Get1(idx);
                ref var rewardComponent = ref _filter.Get2(idx);
                var levelComponent = _filter.Get3(idx);
                var upgradeComponent = _filter.Get4(idx);
                if (levelComponent.Level != 0)
                {
                    var currency = _playerService.GetCurrency();
                    rewardComponent.CurrentReward = GetReward(upgradeComponent.Boosters[0],
                        upgradeComponent.Boosters[1], levelComponent.Level, rewardComponent.BaseReward);
                    _playerService.SetCurrency(currency + rewardComponent.CurrentReward);
                    _gameView.UpdateCurrency((currency + rewardComponent.CurrentReward));
                }
                
                var entity = _filter.GetEntity(idx);
                entity.Del<RewardEvent>();
            }
        }
        
        private float GetReward(Booster firstBooster, Booster secondBooster, int level, float baseReward)
        {
            float firstRiseCoof = 0;
            float secondRiseCoof = 0;

            if (firstBooster.IsActivated)
            {
                firstRiseCoof = firstBooster.Coof;
            }
            if (secondBooster.IsActivated)
            {
                secondRiseCoof = secondBooster.Coof;
            }
            
            return level * baseReward * (1 + baseReward/100 * firstRiseCoof + baseReward/100 * secondRiseCoof);
        }
    }
}