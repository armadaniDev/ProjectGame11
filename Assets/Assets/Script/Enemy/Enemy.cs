using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int hp = 100;
    public float ms = 2f;
    [SerializeField] private int damageSaatTabrakan = 50;
    protected Transform player;

    [Header("Pengaturan State Machine")]
    [SerializeField] private float jarakDeteksi = 6f;
    [SerializeField] private float jarakSerang = 1.2f;
    [SerializeField] private float jedaSerang = 1f;
    [SerializeField] private int damageSerang = 15;

    [Header("Pengaturan Patrol")]
    [SerializeField] private Transform titikA;
    [SerializeField] private Transform titikB;
    [SerializeField] private float kecepatanPatroli = 1f;
    [SerializeField] private float waktuIdle = 1.5f;

    [Header("Event Visual (opsional, lewat Inspector)")]
    [SerializeField] private UnityEvent onZombieMatiVisual;

    private Transform tujuanSaatIni;
    private float waktuMulaiIdle;
    private StateZombie state = StateZombie.IDLE;
    private float waktuSerangTerakhir;
    public static event Action<Enemy> OnZombieMati;

    // Cari referensi Player lewat tag, dan tentukan titik patrol awal.
    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            // Mulai patroli menuju titik B duluan (kalau ada)
            if (titikB != null)
            {
                tujuanSaatIni = titikB; // Set tujuan awal patrol ke titikB
            }
        }
    }

        // Dipanggil tiap frame. Cek state saat ini, lalu jalankan perilaku yang sesuai.
        void Update()
        {
            PeriksaTransisi();

            switch (state)
            {
                case StateZombie.IDLE: PerilakuIdle(); break;
                case StateZombie.PATROL: PerilakuPatrol(); break;
                case StateZombie.CHASE: PerilakuChase(); break;
                case StateZombie.ATTACK: PerilakuAttack(); break;
            }
        }

    // Menghitung jarak lurus antara Enemy dan Player.
    // Kalau player belum ditemukan, anggap jaraknya tak terhingga (supaya enemy tidak salah bereaksi).
    public float JarakKePlayer()
    {
        if (player == null) return Mathf.Infinity;
        return Vector2.Distance(transform.position, player.position);
    }

    // Menentukan state Enemy berikutnya berdasarkan jarak ke Player.
    // State cuma diganti dan di-log kalau memang berubah, supaya Console tidak spam.
    void PeriksaTransisi()
    {
        float jarak = JarakKePlayer();
        StateZombie stateBaru;

        if (jarak <= jarakSerang)
        {
            stateBaru = StateZombie.ATTACK;
        }
        else if (jarak <= jarakDeteksi)
        {
            stateBaru = StateZombie.CHASE;
        }
        else
        {
            // Kalau lagi IDLE, biarkan IDLE jalan sampai selesai — jangan dipaksa balik ke PATROL
            stateBaru = (state == StateZombie.IDLE) ? StateZombie.IDLE : StateZombie.PATROL;
        }

        // Cuma log kalau state BERUBAH (baru masuk state itu)
        if (stateBaru != state)
        {
            state = stateBaru;
            Debug.Log($"{gameObject.name} state berubah ke: {state}");
        }
    }

    // Menggerakkan Enemy mendekati posisi Player, dipakai saat state CHASE.
    public void Kejar()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
    }

    // Logic serangan dasar Enemy ke Player.
    public virtual void Serang()
    {
        Debug.Log("Enemy menyerang!");

        if (player == null) return;

        IDamageable playerDamageable = player.GetComponent<IDamageable>();
        if (playerDamageable != null)
        {
            playerDamageable.KenaDamage(damageSerang);
        }
    }

    // Dipanggil otomatis oleh Unity saat collider Enemy bersentuhan fisik dengan collider lain.
    // Kalau yang ditabrak adalah Player, Enemy kena damage balik (bukan Player yang diserang di sini).
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KenaDamage(damageSaatTabrakan);
        }
    }

    public void KenaDamage(int jumlah)
    {
        hp -= jumlah;
        Debug.Log($"{gameObject.name} kena damage {jumlah}, HP sisa: {hp}");

        if (hp <= 0)
        {
            Mati();
        }
    }

    protected virtual void Mati()
    {
        Debug.Log($"{gameObject.name} mati!");

        // '?.Invoke' -> "panggil HANYA JIKA ada yang subscribe/dengar".
        // Kalau belum ada yang subscribe, OnZombieMati bernilai null,
        // jadi tanpa '?' akan error (NullReferenceException).
        // 'this' -> kirim info "siapa yang mati" ke semua pendengar (mis. GameManager).
        OnZombieMati?.Invoke(this);
        onZombieMatiVisual?.Invoke();

        Destroy(gameObject);
    }

    // State IDLE: Enemy diam sejenak (durasi waktuIdle) sebelum lanjut patroli lagi.
    // Dipakai sebagai jeda di tiap titik patrol, biar gerakan tidak terus-menerus tanpa henti.
    void PerilakuIdle()
    {
        if (Time.time >= waktuMulaiIdle + waktuIdle)
        {
            state = StateZombie.PATROL;
        }
    }

    // State PATROL: Enemy jalan menuju titik tujuan saat ini (titikA atau titikB).
    // Begitu sampai, Enemy berhenti dulu (masuk IDLE) sebelum lanjut ke titik satunya.
    void PerilakuPatrol()
    {
        if (titikA == null || titikB == null || tujuanSaatIni == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            tujuanSaatIni.position,
            kecepatanPatroli * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, tujuanSaatIni.position) < 0.1f)
        {
            tujuanSaatIni = (tujuanSaatIni == titikA) ? titikB : titikA;
            waktuMulaiIdle = Time.time;
            state = StateZombie.IDLE;
        }
    }

    void PerilakuChase()
    {
        Kejar();
    }

    void PerilakuAttack()
    {
        if (Time.time >= waktuSerangTerakhir + jedaSerang)
        {
            Serang();
            waktuSerangTerakhir = Time.time;
        }
    }
    }