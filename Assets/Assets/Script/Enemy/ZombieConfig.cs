using UnityEngine;

// Atribut ini membuat menu "Create > PvZ > Zombie Config" muncul
// saat klik kanan di folder Project — supaya bisa membuat ASET dari class ini.
[CreateAssetMenu(fileName = "ZombieConfig", menuName = "PvZ/Zombie Config")]
public class ZombieConfig : ScriptableObject
{
    [Header("Data Dasar")]
    public int hp = 100;              
    public float kecepatan = 2f;      

    [Header("Data State Machine")]
    public float jarakDeteksi = 6f;
    public float jarakSerang = 1.2f;
    public int damageSerang = 15;
    public int damageSaatTabrakan = 20;
}
