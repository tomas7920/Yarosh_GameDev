using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_controller : MonoBehaviour
{
    private ServiceManager _serviceManager;
    [SerializeField] private int _maxHP;
    private int _currentHP;
    [SerializeField] private int _maxMP;
    private int _currentMP;
    private int _counter1 = 0;
    private int _counter2 = 0;
    private int _timeToRegenHP = 150;
    private int _timeToRegenMP = 50;

    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _mpSlider;

    Movement_controller _playerMovement;
    Vector2 _startPosition;

    private bool _canBeDamaged = true;
    void Start()
    {
        _playerMovement = GetComponent<Movement_controller>();
        _playerMovement.OnGetHurt += OnGetHurt;
        _currentHP = _maxHP;
        _currentMP = _maxMP;
        _hpSlider.maxValue = _maxHP;
        _hpSlider.value = _maxHP;
        _mpSlider.maxValue = _maxMP;
        _mpSlider.value = _maxMP;
        _startPosition = transform.position;
        _serviceManager = ServiceManager.Instanse;
    }

    public void TakeTamage(int damage, DamageType type = DamageType.Casual, Transform enemy = null)
    {
        if (!_canBeDamaged)
            return;

        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            OnDeath();
        }

        switch (type)
        {
            case DamageType.PowerStrike:
                _playerMovement.GetHurt(enemy.position);
                break;
        }
        _hpSlider.value = _currentHP;
    }

    private void OnGetHurt(bool canBeDamaged)
    {
        _canBeDamaged = canBeDamaged;
    }

    public void RestoreHP(int hp)
    {
        _currentHP += hp;
        if (_currentHP > _maxHP)
        {
            _currentHP = _maxHP;
        }
        _hpSlider.value = _currentHP;
    }

    public void RestoreMP(int mp)
    {
        _currentMP += mp;
        if (_currentMP > _maxMP)
        {
            _currentMP = _maxMP;
        }
        _mpSlider.value = _currentMP;
    }

    public bool ChangeMP(int value)
    {
        if (value < 0 && _currentMP < Mathf.Abs(value))
            return false;

        _currentMP += value;
        if (_currentMP > _maxMP)
            _currentMP = _maxMP;
        _mpSlider.value = _currentMP;
        return true;
    }

    public void OnDeath()
    {
        _serviceManager.Restart();
    }

    private void RegenerationHP()
    {
        if (_maxHP > _currentHP)
        {
            GetComponent<Player_controller>().RestoreHP(1);
        }
    }
    private void RegenerationMP()
    {
        if (_maxMP > _currentMP)
        {
            GetComponent<Player_controller>().RestoreMP(1);
        }
    }
    private void FixedUpdate()
    {
        if (_counter1 < _timeToRegenHP) 
        {
            _counter1++;
        }
        if (_counter1 == _timeToRegenHP) 
        {
            RegenerationHP();
        }
        if (_counter1 >= _timeToRegenHP) 
        {
            _counter1 = 0;
        }
        if (_counter2 < _timeToRegenMP)
        {
            _counter2++;
        }
        if (_counter2 == _timeToRegenMP)
        {
            RegenerationMP();
        }
        if (_counter2 >= _timeToRegenMP)
        {
            _counter2 = 0;
        }

    }
}