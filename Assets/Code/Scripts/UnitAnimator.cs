using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;

    private void Awake()
    {
        if (TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
        if (TryGetComponent(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
        }
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        //animator.SetTrigger("SwordSlash");
        Debug.Log("Sword Slash");
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        Debug.Log("Sword Slash Completed");
    }
    
    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        //animator.SetBool(IsWalking, true);
        Debug.Log("StartMoving");
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        //animator.SetBool(IsWalking, false);
        Debug.Log("StopMoving");
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShotEventArgs e)
    {
        //animator.SetTrigger("Shoot");
        Debug.Log("Shooting anim");
        Transform bulletProjectileTransform =
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootPosition.y = shootPointTransform.position.y;
        
        bulletProjectile.SetUp(targetUnitShootPosition);
    }
}
