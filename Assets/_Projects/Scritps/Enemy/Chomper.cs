using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : EnemyController
{
    public override void InitializeAnimations()
    {   
        base.InitializeAnimations();   
        m_HashAnimations[EnemyStates.Idle] = Animator.StringToHash("Chomper_Idle");
        m_HashAnimations[EnemyStates.Walk] = Animator.StringToHash("Chomper_Walk");
        m_HashAnimations[EnemyStates.Chase] = Animator.StringToHash("Chomper_Run");
        m_HashAnimations[EnemyStates.Attack] = Animator.StringToHash("Chomper_Attack");
        m_HashAnimations[EnemyStates.Death] = Animator.StringToHash("Chomper_Death");
        m_HashAnimations[EnemyStates.Hit] = Animator.StringToHash("Chomper_Hit");
    }
}
