using Components;
using Components.Level;
using Data.SO;
using Leopotam.Ecs;

namespace Systems
{
    public class UpdateLevelViewSystem : IEcsRunSystem 
    {
        private readonly EcsWorld _world = null;
        private readonly Localized _localized = null;
        
        private EcsFilter<UpdateLevelEvent, LevelComponent, TaskComponentView> _filter;
        
        public UpdateLevelViewSystem(Localized localized) 
        {
            _localized = localized;
        }
        
        void IEcsRunSystem.Run () 
        {
            foreach (var idx in _filter) 
            {
                var eventComponent = _filter.Get1(idx);
                var levelComponent = _filter.Get2(idx);
                var viewComponent = _filter.Get3(idx);
                
                viewComponent.View.UpdateLvl(levelComponent.Level, levelComponent.LvlTittle);
                
                var tempString = _localized.GetLocalized("lvlUpPrice");
                var priceString = tempString.Replace("{0}", levelComponent.LevelUpPrice.ToString());
                viewComponent.View.UpdateLvlUpButton(priceString, levelComponent.LevelUpTittle);
                
                var entity = _filter.GetEntity(idx);
                entity.Del<UpdateLevelEvent>();
            }
        }
    }
}