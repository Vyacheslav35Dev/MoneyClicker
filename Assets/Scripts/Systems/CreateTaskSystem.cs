using Components;
using Leopotam.Ecs;
using UnityComponents;

namespace Systems 
{
    sealed class CreateTaskSystem : IEcsRunSystem 
    {
        private readonly EcsWorld _world = null;
        private readonly GameView _gameView;
        
        private EcsFilter<CreateTaskEvent> _filter;

        public CreateTaskSystem(GameView gameView)
        {
            _gameView = gameView;
        }
        
        void IEcsRunSystem.Run () 
        {
            foreach (var idx in _filter) 
            {
                var entity = _filter.GetEntity(idx);
                var view = _gameView.CreateTask(entity);
                entity.Get<TaskComponentView>().View = view;
                entity.Del<CreateTaskEvent>();
            }
        }
    }
}