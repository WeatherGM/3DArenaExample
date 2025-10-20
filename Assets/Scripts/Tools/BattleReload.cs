
using Assets.Scripts.Player;
using UnityEngine;

public class BattleReload : MonoBehaviour
{
    [SerializeField] private BattleManager _battleManager;
    [SerializeField] private PlayerEntity _playerEntity;
    [SerializeField] private GameObject _loseScreen;

    public void Reload()
    {
        _battleManager.ForcedEndWave();
        _playerEntity.StartLiving();
        _loseScreen.SetActive(false);
    }
}
