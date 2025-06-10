using Components;
using Components.Reward;
using Leopotam.Ecs;

namespace Systems 
{
    sealed class UpdateRewardViewSystem : IEcsRunSystem 
    {
        private readonly EcsWorld _world = null;
        
        private EcsFilter<UpdateRewardEvent, RewardComponent, TaskComponentView> _filter;
        
        void IEcsRunSystem.Run () 
        {
            foreach (var idx in _filter) 
            {
                var eventComponent = _filter.Get1(idx);
                var rewardComponent = _filter.Get2(idx);
                var taskComponentView = _filter.Get3(idx);
                
                taskComponentView.View.UpdateReward(rewardComponent.CurrentReward, rewardComponent.RewardTittle);
                var entity = _filter.GetEntity(idx);
                entity.Del<UpdateRewardEvent>();
            }
        }
    }
}