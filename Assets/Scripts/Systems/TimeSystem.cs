using Components;
using Components.Reward;
using Leopotam.Ecs;
using Services;
using UnityEngine;

namespace Systems 
{
    sealed class TimeSystem : IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsWorld _world = null;
        private readonly IPlayerService _playerService = null;
        
        private EcsFilter<TimerComponent> _filters = null;
        
        public TimeSystem(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        
        void IEcsRunSystem.Run () 
        {
            foreach (var idx in _filters) 
            {
                ref var timer = ref _filters.Get1(idx);
                var entity = _filters.GetEntity(idx);
                if (timer.IsRunning)
                {
                    timer.Elapsed += Time.deltaTime;
                    if (timer.Elapsed >= timer.Duration)
                    {
                        timer.IsRunning = false;
                        entity.Get<RewardEvent>();
                        timer.Elapsed = 0;
                        timer.IsRunning = true;
                    }
                }
            }
        }

        public void Destroy()
        {
            foreach (var idx in _filters) 
            {
                ref var timer = ref _filters.Get1(idx);
                if (timer.IsRunning)
                {
                    _playerService.SetProgress(timer.Id, timer.Elapsed);
                }
            }
        }
    }
}