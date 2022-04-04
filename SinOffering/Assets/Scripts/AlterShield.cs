using UnityEngine;

/// <summary>
/// shield behavior for alters that can kill player until shield is removed
/// by completing required killcount / _killRequired /
/// </summary>

public class AlterShield : MonoBehaviour
{
    private GameManager _gameManager;
    private int _curKillCount;
    private int _difference;
    private int _killRequired = 3;
    
    void Awake()
    {
        _gameManager = GameManager.Instance;
        _killRequired = GetComponentInParent<Crates>().EnemyKilledMAX;
        _difference = _gameManager.CurEnemyKills;
    }

    private void Update()
    {
        _curKillCount = (_gameManager.CurEnemyKills - _difference);
        if (_curKillCount >= _killRequired)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            if (_curKillCount < _killRequired)
            {
                Debug.Log("killing player from altershield.cs");
                col.gameObject.GetComponent<PlayerController>().Killed();
            }
        }
    }
}
