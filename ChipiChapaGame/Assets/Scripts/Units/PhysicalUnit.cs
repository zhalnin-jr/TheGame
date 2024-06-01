using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalUnit : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void PlayAttack()
    {
        _animator.SetTrigger("Attack");
    }

    public void Die()
    {

    }

    public void Destroy()
    {
        Debug.Log("TRYING TO DESTROY");
        Destroy(this.gameObject);
    }
}