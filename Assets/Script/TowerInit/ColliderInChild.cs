using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInChild : MonoBehaviour
{ 
    private Collider _thisCollider;
    private Turret _tur;
    private void Awake()
    {
        _thisCollider = GetComponent<Collider>();
        _tur = transform.parent.GetComponent<Turret>();
        if (_tur == null)
        {
            _tur = transform.parent.parent.parent.GetComponent<Turret>();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // 确保只处理具有 "Enemy" 标签的敌人
        {
            _tur._onTriggerEnter(other.transform);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _tur._onTriggerExit(other.transform);
        }
    }
    
}
