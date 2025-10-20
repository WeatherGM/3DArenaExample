using Assets.Scripts.Battle;
using Assets.Scripts.Core;
using Assets.Scripts.Core.Enemies;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _waveDelay;
    [SerializeField] private int _waveValue = 10;
    [SerializeField] private int _waveWeightIncrease = 2;
    [SerializeField] private Transform _battleCenter;
    [SerializeField] private Transform _target;


    private int _waveIndex = 0;
    private int _alivesEnemies = 0;

    private Coroutine _waveCoroutine;
    private EnemySpawner _enemySpawner;
    private EnemyController _enemyController;
    private BattleView _battleView;
    private IDataService _dataService;
    private IGameInvoker _gameInvoker;
    private IGameEvents _gameEvents;

    private SO_GameSave _save;

    private readonly WaveData _waveData = new();
    private readonly List<IDataInitializer> _dataInitializer = new();

    [Inject]
    public void Initialize(
        EnemySpawner enemySpawner,
        EnemyFactory enemyFactory,
        IEnemyPoolManager enemyPoolManager,
        EnemyController enemyController,
        BattleView battleView,
        IDataService dataService,
        IGameInvoker gameInvoker,
        IGameEvents gameEvents)
    {
        _enemySpawner = enemySpawner;
        _enemyController = enemyController;
        _battleView = battleView;
        _dataService = dataService;
        _gameInvoker = gameInvoker;
        _gameEvents = gameEvents;

        enemyFactory.InitializeTarget(_target);
        _dataInitializer.Add(enemyPoolManager);
        _dataInitializer.Add(enemyFactory);
    }
    private void Start()
    {
        foreach (var item in _dataInitializer)
            item.InitializeData();


        _save = _dataService.GetGameSave();
        _waveIndex = _save.LastWave;

        _waveData.WaveIndex = _waveIndex;
        _waveData.WaveValue = CalculateWaveValue();

        _gameEvents.OnBonusChanged += ()=> StartCoroutine(WinRoutine());
        _enemySpawner.OnDie += () => _alivesEnemies--;
    }
    public void StartWave()
    {
        StopAllCoroutines();
        _save.LastWave = _waveIndex;

        _enemySpawner.InitWaveData(_waveData);

        _battleView.SetWaveIndex(_waveIndex);
        _battleView.SetEnemiesCounter(_waveData.WaveValue);


        StartCoroutine(DelayFromStart());
    }
    public void ForcedEndWave()
    {
        StopAllCoroutines();
        _waveCoroutine = null;
        _enemySpawner.ForcedEnd();
        _enemyController.KillAll();
        _waveData.WaveValue = CalculateWaveValue();
        StartWave();
    }
    private void EndWave()
    {
        StopAllCoroutines();
        _gameInvoker?.WaveWin();
    }
    private void CalculateWaveData()
    {
        _waveData.WaveIndex = _waveIndex++;
        _waveData.WaveValue = _waveValue += _waveWeightIncrease;
    }

    private int CalculateWaveValue()
    {
        return _waveValue + _save.LastWave * _waveWeightIncrease;
    }
    private IEnumerator WaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);


            var enemy = _enemySpawner.Spawn();

            if (enemy != null && _enemySpawner.HasWeightValue)
            {
                enemy.BattleCenter = _battleCenter;
                enemy.CurrentWave = _waveIndex;
                enemy.StartLiving();
                _enemyController.AddEnemy(enemy);
                _alivesEnemies++;
            }
            _battleView.SetEnemiesCounter(_alivesEnemies);
            if (_enemySpawner.AllDespawn && !_enemySpawner.HasWeightValue)
                EndWave();
                    
        }
    }    
    private IEnumerator WinRoutine()
    {
        CalculateWaveData();
        yield return new WaitForSeconds(1f);
        StartWave();
        _waveCoroutine = null;
    }
    private IEnumerator DelayFromStart()
    {
        float seconds = _waveDelay;
        while (seconds > 0)
        {
            _battleView.SetEnemiesCounter(0);
            _battleView.EnableTimer(true);
            _battleView.SetTimer(seconds);
            yield return new WaitForSeconds(seconds--);
        }
        _battleView.EnableTimer(false);
        _waveCoroutine ??= StartCoroutine(WaveRoutine());
    }


}
