using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ��� �������� �� Unity.
/// </summary>
public class PhysicalUnit : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    /// <summary>
    /// �������� ��� �����.
    /// </summary>
    public void PlayAttack()
    {
        _animator.SetTrigger("Attack");
    }

    /// <summary>
    /// ������ �����.
    /// </summary>
    public void Die()
    {

    }
    
    /// <summary>
    /// �������� �������.
    /// </summary>
    public void Destroy()
    {
        Debug.Log("TRYING TO DESTROY");
        Destroy(this.gameObject);
    }
}
