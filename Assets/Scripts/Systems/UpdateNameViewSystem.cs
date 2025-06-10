using Components;
using Leopotam.Ecs;

namespace Systems
{
    public class UpdateNameViewSystem: IEcsRunSystem 
    {
        private readonly EcsWorld _world = null;
        
        private EcsFilter<UpdateNameEvent, NameComponent, TaskComponentView> _filter;
        
        void IEcsRunSystem.Run () 
        {
            foreach (var idx in _filter) 
            {
                var eventComponent = _filter.Get1(idx);
                var nameComponent = _filter.Get2(idx);
                var taskComponentView = _filter.Get3(idx);
                
                taskComponentView.View.UpdateName(nameComponent.Value);
                var entity = _filter.GetEntity(idx);
                entity.Del<UpdateNameEvent>();
            }
        }
    }
}