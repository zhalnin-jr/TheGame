using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для анимаций на Unity.
/// </summary>
public class PhysicalUnit : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    /// <summary>
    /// Движение при атаке.
    /// </summary>
    public void PlayAttack()
    {
        _animator.SetTrigger("Attack");
    }

    /// <summary>
    /// Смерть юнита.
    /// </summary>
    public void Die()
    {

    }

    /// <summary>
    /// Удаление объекта.
    /// </summary>
    public void Destroy()
    {
        Debug.Log("TRYING TO DESTROY");
        Destroy(this.gameObject);
    }
}
