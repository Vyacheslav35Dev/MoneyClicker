using Components;
using Components.Level;
using Components.Reward;
using Components.Upgrades;
using Leopotam.Ecs;
using Services;
using UnityComponents;

namespace Systems
{
    sealed class LevelUpSystem : IEcsRunSystem 
    {
        private readonly EcsWorld _world = null;
        
        private readonly IPlayerService _playerService = null;
        private readonly GameView _gameView = null;
        
        private EcsFilter<LevelUpEvent, LevelComponent, RewardComponent> _filter;

        public LevelUpSystem(IPlayerService playerService, GameView gameView)
        {
            _playerService = playerService;
            _gameView = gameView;
        }
        
        void IEcsRunSystem.Run () 
        {
            foreach (var idx in _filter) 
            {
                var eventComponent = _filter.Get1(idx);
                ref var levelComponent = ref _filter.Get2(idx);
                ref var rewardComponent = ref _filter.Get3(idx);
                
                var entity = _filter.GetEntity(idx);
                
                var lvl = levelComponent.Level + 1;
                var lvlPrice = (lvl) * levelComponent.LevelBasePrice;
                
                var currency = _playerService.GetCurrency();
                
                if (currency >= lvlPrice)
                {
                    var newCurrency = currency - lvlPrice;
                    _playerService.SetCurrency(newCurrency);
                    
                    levelComponent.Level = lvl;
                    levelComponent.LevelUpPrice = (lvl + 1) * levelComponent.LevelBasePrice;
                    _playerService.SetLvl(levelComponent.Id, levelComponent.Level);
                    entity.Get<UpdateLevelEvent>();
                    
                    rewardComponent.CurrentReward = GetReward(entity, levelComponent.Level);
                    entity.Get<UpdateRewardEvent>();
                    
                    _gameView.UpdateCurrency((int)newCurrency);
                    
                    if (!entity.Has<TimerComponent>())
                    {
                        entity.Get<TimerComponent>().Duration = rewardComponent.DelayReward;
                        entity.Get<TimerComponent>().Elapsed = 0;
                        entity.Get<TimerComponent>().IsRunning = true;
                    }
                }
                entity.Del<LevelUpEvent>();
            }
        }
        
        private float GetReward(EcsEntity entity, int currentLvl)
        {
            var firstBooster = entity.Get<UpgradeComponent>().Boosters[0];
            var secondBooster = entity.Get<UpgradeComponent>().Boosters[1];
            var data = entity.Get<RewardComponent>();
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

            if (currentLvl == 0)
            {
                return data.BaseReward;
            }
            return currentLvl * data.BaseReward * (1 + data.BaseReward/100 * firstRiseCoof + data.BaseReward/100 * secondRiseCoof);
        }
    }
}