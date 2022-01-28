using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum EnemyStates
{
    Idle    = 0,
    Walk    = 1,
    Chase   = 2,     // Run
    Attack  = 3,
    Death   = 4,
    Hit     = 5
}

public class EnemyController : MonoBehaviour
{
    [Header("Vision Settings")]
    [Tooltip("The angle of the forward of the view cone. 0 is forward of the sprite, 90 is up, 180 behind etc.")]
    [Range(0.0f,360.0f)]
    public float visionDirection = 0;
    [Range(0.0f,360.0f)]
    public float visionFov = 0;     // Angle Range in which enemy can attack
    public float visionDistance;    // Range in which enemy will Chase player
  
    [Header("Attack Settings")]
    public float attackRange = 0.2f;    // Range in which Attack is possible 
//    public float attackForce = 10;      // Force of attack KnockBack
    public float timeForNextAttack = 2f;

    [Header("Properties")]
    public int health = 5;              // Number of hits the enemy can withstand
    public float speed;
    public float runSpeed;
    public float hitFlickerDuration;
    public bool isPatroling = false;
    public float patrolingDistance;

    protected SpriteRenderer m_SpriteRenderer;
    protected Rigidbody2D m_RigidBody;
    protected Collider2D m_Collider;
    protected Animator m_Animator;
    protected EnemyStates m_State;  
    protected Dictionary<EnemyStates,int> m_HashAnimations;
    protected bool m_InVision, m_InAttackRange, m_InAnimation = false;
    private float m_AttackTimer;
    private Vector3 m_StartPatrolPoint , m_EndPatrolPoint , m_NextPatrolPoint;

    void Awake()
    {
        InitializeAnimations();
        InitializeComponents();   
        InitializePositions(); 
        Idle();
    }
    public virtual void InitializeAnimations()
    {
        m_HashAnimations = new Dictionary<EnemyStates, int>();
    }

    private void InitializeComponents()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<BoxCollider2D>();
        m_Animator = GetComponent<Animator>();
    }
    private void InitializePositions()
    {
        Vector3 pos = transform.localPosition;
        if(!isPatroling)
        {
            m_StartPatrolPoint = Vector3.right * (pos.x -1);
            m_EndPatrolPoint = Vector3.zero;
            return;
        }
        m_StartPatrolPoint = Vector3.right *(pos.x - patrolingDistance);
        m_EndPatrolPoint = Vector3.right *(pos.x + patrolingDistance);
        m_NextPatrolPoint = m_StartPatrolPoint;
    }


    void Update()
    {
        m_RigidBody.velocity = Vector2.zero;
        SetState();
        IncreaseTimer();
    }

    private void IncreaseTimer()
    {
        m_AttackTimer += Time.deltaTime;
    }

#region  Checking
    private bool CheckAttackTimer()
    {
        return m_AttackTimer > timeForNextAttack;
    }

    private bool CheckPlayerinRange(float range)
    {
        float distance = Vector3.Distance(PlayerController.playerInstance.GetLocation(), transform.position);
        if(distance > range)
            return false;

        return true;
    }
#endregion

#region Movement And Rotation

    private void MoveEnemy(Vector3 target)
    {
        Vector3 pos = transform.localPosition;
        target.y = pos.y;
        target.z = pos.z;
        SetRotation(target);
        transform.localPosition =  Vector3.MoveTowards(transform.position,target,((m_State == EnemyStates.Chase)?runSpeed:speed) * Time.deltaTime);
    }

    private void SetRotation(Vector3 target)
    {
        if((target.x - transform.position.x) < 0)
            transform.rotation = Quaternion.Euler(0,180,0);
        else
            transform.rotation = Quaternion.Euler(0,0,0);
    }
#endregion

#region   States
    private void SetState()
    {
        if(m_InAnimation)
            return;

        m_InVision = CheckPlayerinRange(visionDistance);
        m_InAttackRange = CheckPlayerinRange(attackRange);

        if (m_InVision)
        {
            if (m_InAttackRange)
            {    
                Attack();
                return;
            }
            
            Chase();      
            MoveEnemy(PlayerController.playerInstance.GetLocation());
            return;
        }

        ContinueNormalState();
    }

    private void ContinueNormalState()
    {
        if(!isPatroling)
        {
            if (InSameLocation(m_StartPatrolPoint))
                Idle();
            else
            {
                Walk();
                MoveEnemy(m_StartPatrolPoint);
            }
            return;
        }

        Walk();
        if(InSameLocation(m_StartPatrolPoint))
            m_NextPatrolPoint = m_EndPatrolPoint;
        else if(InSameLocation(m_EndPatrolPoint))
            m_NextPatrolPoint = m_StartPatrolPoint;

        MoveEnemy(m_NextPatrolPoint);
    }

    private bool InSameLocation(Vector3 location)
    {
        return Mathf.Abs(transform.position.x - location.x) < 0.3;
    }

    private void Idle()
    {
        m_State = EnemyStates.Idle;
        m_RigidBody.velocity = Vector2.zero;
        PlayAnimation();
    }


    private void Walk()
    {
        m_State = EnemyStates.Walk;
        PlayAnimation();
    }

    public void GetHit()
    {
        health -= 1;
        if(health <= 0)
        {
            Death();
        }
        StartCoroutine(Flicker());
    }
    IEnumerator Flicker()
    {
        m_InAnimation = true;
        m_State = EnemyStates.Hit;
        SetSimulation(false);
        PlayAnimation();
        m_InAnimation = true;
        yield return new WaitForSeconds(hitFlickerDuration);
        SetSimulation(true);
        m_InAnimation = false;
        m_InAnimation = true;
    }

    private void SetSimulation(bool value)
    {
        m_RigidBody.simulated = value;
    }

    private void Chase()
    {
        m_State = EnemyStates.Chase;
        PlayAnimation();
    }
    public virtual void Attack()
    {
        m_State = EnemyStates.Attack;
        m_AttackTimer = 0;
        StartCoroutine(Attaching());
    }

    IEnumerator Attaching()
    {
        PlayAnimation();
        m_InAnimation = true;
        yield return new WaitForSeconds(timeForNextAttack);
        m_InAnimation = false;
        Idle();
    }

    private void Death()
    {
        m_State = EnemyStates.Death;
        PlayAnimation();
        Destroy(this.gameObject,GetAnimationTime());
    }

#endregion

#region   Animations
    public void PlayAnimation()
    {
        m_Animator.Play(m_HashAnimations[m_State]);
    }
   
    public float GetAnimationTime()
    {
        return m_Animator.GetCurrentAnimatorStateInfo(0).length;
    }
#endregion

#region Editor
#if UNITY_EDITOR
    void OnDrawGizmosSelected() {
        
        Vector3 forward = Quaternion.Euler(0, 0, visionDirection) * Vector2.right;
        Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, visionFov * 0.5f) * forward);

        // Vision Range
        Handles.color = new Color(0, 1.0f, 0, 0.4f);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, visionFov, visionDistance);

        //Draw attack range
        Handles.color = new Color(1.0f, 0,0, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.back, attackRange);

        //Patrol Distance
        if(!isPatroling)
            return;
        Handles.color = Color.blue;
        Vector3 start = transform.position + Vector3.left * patrolingDistance + Vector3.up * 0.5f;
        Vector2 end = transform.position + Vector3.right * patrolingDistance + Vector3.up * 0.5f;
        Handles.DrawLine(start,end);

    }
#endif
#endregion
   
}
