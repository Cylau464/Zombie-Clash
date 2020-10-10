using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Boss : EnemySoldier
{
    [Header("Health Bar")]
    [SerializeField] private TextMeshPro _healthText = null;
    [SerializeField] private Image _healthBar = null;

    [Header("Attack Properties")]
    [SerializeField] private float _rangeAttack = 1f;

    private new void OnEnable()
    {
        base.OnEnable();

        _healthText.text = _health.ToString();
        _healthBar.fillAmount = (float)_health / _maxHealth;
    }

    public override void GiveDamage()
    {
        Collider[] targets = Physics.OverlapSphere(_target.position, _rangeAttack, _friendlyLayer);

        foreach (Collider sold in targets)
        {
            sold.GetComponent<Soldier>().GetDamage(_damage);
        }
    }

    public override bool GetDamage(int damage)
    {
        base.GetDamage(damage);

        _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
        _healthText.text = _health.ToString();
        _healthBar.fillAmount = (float)_health / _maxHealth;

        if (_health <= 0)
        {
            Dead(true);
            return true;
        }

        return false;
    }
}