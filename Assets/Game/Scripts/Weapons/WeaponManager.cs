using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    private string currentWeapon = "Pistol";

    /// <summary>
    /// For giving current weapon to newcomer
    /// </summary>
    public void GiveWeapon(Survivor survivor)
    {
        survivor.PickUpWeapon(currentWeapon);
    }

    public void UpdateWeapons(string weaponName)
    {
        currentWeapon = weaponName;
        var survivors = HiveManager.Instance.Survivors;
        survivors.ForEach(x => x.ChangeWeapon(weaponName));
    }
}
