using Components;
using Components.Level;
using Components.Reward;
using Components.Upgrades;
using Data;
using Data.SO;
using Leopotam.Ecs;
using Services;
using Systems;
using UnityComponents;
using UnityEngine;

sealed class Bootstrap : MonoBehaviour 
{
    EcsWorld _world;
    EcsSystems _systems;
    [Header("Game view")]
    [SerializeField]
    private GameView gameView;
        
    [Header("Config")]
    [SerializeField]
    private Config config;
        
    [Header("Localizations")]
    [SerializeField]
    private Localized configLocalized;
        
    private IPlayerService _playerService;

    void Start () 
    {
        _playerService = new PlayerService ();

        _world = new EcsWorld ();
        _systems = new EcsSystems (_world);
            
        gameView.Init(configLocalized);
        gameView.UpdateCurrency(_playerService.GetCurrency());
#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
#endif
        _systems
            .Add(new CreateTaskSystem(gameView))
                
            .Add (new UpdateNameViewSystem ())
            .Add (new BoostersSystem (_playerService, gameView))
            .Add (new TimeSystem (_playerService))
            .Add (new UpdateRewardSliderViewSystem ())
            .Add (new LevelUpSystem (_playerService, gameView))
            .Add (new UpdateLevelViewSystem (configLocalized))
            .Add (new RewardSystem (_playerService, gameView))
            .Add (new UpdateRewardViewSystem ())
            .Add (new UpdateBoostersViewSystem ())
            .Init ();
        RunGame();
    }

    private void RunGame()
    {
        foreach (var task in config.Tasks)
        {
            var entity = _world.NewEntity();
            
            entity.Get<UpgradeComponent>().Boosters = new Booster[task.Boosters.Length];
            for (int i = 0; i < task.Boosters.Length; i++)
            {
                var booster = new Booster();
                booster.Key = task.Boosters[i].NameId;
                booster.Name = configLocalized.GetLocalized(task.Boosters[i].NameId);
                booster.Description = configLocalized.GetLocalized("boosterDescription").Replace("{0}",
                    task.Boosters[i].Coof.ToString());
                booster.Price = task.Boosters[i].Price;
                booster.Coof = task.Boosters[i].Coof;
                booster.IsActivated = _playerService.GetStateBooster(task.Boosters[i].NameId);
                booster.BoosterActivatedText = configLocalized.GetLocalized("boosterActivated");
                booster.PriceLocalizedText = configLocalized.GetLocalized("boosterPrice");
                entity.Get<UpgradeComponent>().Boosters[i] = booster;
            }
            entity.Get<UpdateUpgradeEvent>();

            entity.Get<LevelComponent>().Id = task.Id;
            entity.Get<LevelComponent>().Level = _playerService.GetLvl(task.Id);
            entity.Get<LevelComponent>().LvlTittle = configLocalized.GetLocalized("lvl");
            entity.Get<LevelComponent>().LevelBasePrice = task.BasePrice;
            entity.Get<LevelComponent>().LevelUpPrice = (entity.Get<LevelComponent>().Level + 1) * task.BasePrice;
            entity.Get<LevelComponent>().LevelUpTittle = configLocalized.GetLocalized("lvlUp");
            entity.Get<UpdateLevelEvent>();
            
            entity.Get<RewardComponent>().DelayReward = task.DelayReward;
            entity.Get<RewardComponent>().BaseReward = task.BaseReward;
            entity.Get<RewardComponent>().CurrentReward = GetReward(entity, entity.Get<LevelComponent>().Level);
            entity.Get<RewardComponent>().RewardTittle = configLocalized.GetLocalized("revenue");
            entity.Get<UpdateRewardEvent>();
                
            entity.Get<NameComponent>().Value = configLocalized.GetLocalized(task.NameId);
            entity.Get<UpdateNameEvent>();

            if (entity.Get<LevelComponent>().Level != 0)
            {
                entity.Get<TimerComponent>().Id = task.Id;
                entity.Get<TimerComponent>().Duration = task.DelayReward;
                entity.Get<TimerComponent>().Elapsed = _playerService.GetProgress(task.Id);
                entity.Get<TimerComponent>().IsRunning = true;
            }

            entity.Get<CreateTaskEvent>();
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

    void Update () 
    {
        _systems?.Run ();
    }

    void OnDestroy () 
    {
        _playerService = null;
        
        if (_systems != null) {
            _systems.Destroy ();
            _systems = null;
            _world.Destroy ();
            _world = null;
        }
    }
}