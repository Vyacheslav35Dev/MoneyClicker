using Components;
using Leopotam.Ecs;

namespace Systems 
{
    sealed class UpdateRewardSliderViewSystem : IEcsRunSystem 
    {
        private readonly EcsWorld _world = null;
        
        private EcsFilter<TimerComponent, TaskComponentView> _filter;

        void IEcsRunSystem.Run () 
        {
            foreach (var idx in _filter) 
            {
                var eventComponent = _filter.Get1(idx);
                var taskComponentView = _filter.Get2(idx);
                taskComponentView.View.UpdateSlider(eventComponent.Elapsed, 
                    eventComponent.Duration);
            }
        }
    }
}