using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Role
{
    Player,
    Enemy,
}


/// <summary>
/// 攻击判定
/// </summary>
public class AttackDef : MonoBehaviour
{
    Character parent;

    public Role role = Role.Player;
    void Start()
    {
        parent = GetComponentInParent<Character>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!parent.isDeath && parent.isAttack && other.gameObject.tag == role.ToString())
        {
            Character character = other.gameObject.GetComponent<Character>();
            character.characterStats.TakeDamage(parent.characterStats);
            character.BeHurt();
            character.DeathCheck();
            character.UpdateHealthBar();
            if (SoundsManager.Instance != null)
            {
                SoundsManager.Instance.HurtAudio();
            }
        }
    }
}
