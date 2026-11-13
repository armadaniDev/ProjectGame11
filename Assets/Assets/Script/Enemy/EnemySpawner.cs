using System;
using System.Reflection;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    public struct AreaSpawn
    {
        public Transform titikSpawn;
        public Transform titikPatrolA;
        public Transform titikPatrolB;
        [HideInInspector] public Enemy musuhAktif;
    }

    [Header("Pengaturan Spawn")]
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float intervalSpawn = 3f;
    [SerializeField] private bool spawnSemuaDiAwal = true;

    [Header("Daftar Area Spawn (1 Musuh per Area)")]
    [SerializeField] private AreaSpawn[] daftarAreaSpawn;

    private float timerSpawn;

    void Start()
    {
        if (spawnSemuaDiAwal && enemyPrefab != null && daftarAreaSpawn != null)
        {
            for (int i = 0; i < daftarAreaSpawn.Length; i++)
            {
                SpawnDiArea(i);
            }
        }
    }

    void Update()
    {
        if (enemyPrefab == null || daftarAreaSpawn == null) return;

        int indexAreaKosong = CariAreaKosong();
        if (indexAreaKosong == -1)
        {
            timerSpawn = 0f;
            return;
        }

        timerSpawn += Time.deltaTime;
        if (timerSpawn >= intervalSpawn)
        {
            SpawnDiArea(indexAreaKosong);
            timerSpawn = 0f;
        }
    }

    int CariAreaKosong()
    {
        for (int i = 0; i < daftarAreaSpawn.Length; i++)
        {
            if (daftarAreaSpawn[i].musuhAktif == null)
            {
                return i;
            }
        }
        return -1;
    }

    void SpawnDiArea(int index)
    {
        Vector3 posisiSpawn = daftarAreaSpawn[index].titikSpawn != null
            ? daftarAreaSpawn[index].titikSpawn.position
            : transform.position;

        Enemy musuhBaru = Instantiate(enemyPrefab, posisiSpawn, Quaternion.identity);
        daftarAreaSpawn[index].musuhAktif = musuhBaru;

        if (daftarAreaSpawn[index].titikPatrolA != null && daftarAreaSpawn[index].titikPatrolB != null)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            typeof(Enemy).GetField("titikA", flags)?.SetValue(musuhBaru, daftarAreaSpawn[index].titikPatrolA);
            typeof(Enemy).GetField("titikB", flags)?.SetValue(musuhBaru, daftarAreaSpawn[index].titikPatrolB);
        }
    }
}