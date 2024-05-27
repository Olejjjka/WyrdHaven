using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PolygonCollider2D))]

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 10;
    private int _currentHealth;

    private PolygonCollider2D _polygonCollider2D;


    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }


    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }
}
