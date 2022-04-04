using UnityEngine;

/// <summary>
/// crates that are instantiated into random locations in arena and
/// gives the player a random weapon.
/// </summary>

public class Crates : MonoBehaviour 
{
    public Weapon[] Weapons;
    public int SpawnLoc;
    public AudioClip CrateObtainedClip;

    private PlayerController _player;
    private GameManager _gameManager;

    [SerializeField] bool _isAlter;
    private int _maxWeaponIndex;
    public int EnemyKilledMAX;
    private int _curkill;
    private int _difference;

    private int _randWeapon;
    private int _prevWeapon;


    private void Awake()
    {
        _player = PlayerController.instance;
        _gameManager = GameManager.Instance;
        _difference = _gameManager.CurEnemyKills;
    }

    private void Start()
    {
        Invoke("AddCrateToCamTargets", .1f);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (_isAlter)
            {
                _curkill = (_gameManager.CurEnemyKills - _difference);
                if (_curkill >= EnemyKilledMAX)
                {
                    SoundManager.PlaySound(CrateObtainedClip);
                    _gameManager.AddPoint();
                    GenerateWeapon();
                }
            }
            else
            {
                SoundManager.PlaySound(CrateObtainedClip);
                _gameManager.AddPoint();
                GenerateWeapon();
            }
        }
    }

    private void GenerateWeapon()
    {
        _maxWeaponIndex = _player.weaponManager.GetComponent<WeaponManager>().Weapons.Length - 1;
        _randWeapon = Random.Range(0, _maxWeaponIndex);

        while (_player.weaponManager.GetComponent<WeaponManager>().CurWeapon == _randWeapon)
            _randWeapon = Random.Range(0, _maxWeaponIndex);

        _prevWeapon = _player.weaponManager.GetComponent<WeaponManager>().CurWeapon;
        _player.weaponManager.GetComponent<WeaponManager>().EquipWeapon(_randWeapon);
        Destroy(gameObject);
    }

    private void AddCrateToCamTargets()
    {
        if (_gameManager.camManager == null)
            return;
        _gameManager.camManager.AddCameraTargets(gameObject.transform, 40f);
    }
}
