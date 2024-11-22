using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExplode += GrenadeProjectile_OnAnyGrenadeExploded;
    }

    private void ShootAction_OnAnyShoot(object sender, System.EventArgs e)
    {
        ScreenShake.Instance.Shake(1);
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, System.EventArgs e)
    {
        ScreenShake.Instance.Shake(4);
    }
}
