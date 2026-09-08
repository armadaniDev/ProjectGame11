using UnityEngine;

public class BossZombie : MonoBehaviour
{
    [Header("Referensi Config")]
    public ZombieConfig config;   // seret aset BossConfig ke sini di Inspector

    private int hpSekarang;
    private Transform player;

    void Awake()
    {
        // Ambil nilai awal dari config
        hpSekarang = config.hp;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float jarak = Vector2.Distance(transform.position, player.position);

        if (jarak <= config.jarakSerang)
        {
            Serang();
        }
        else if (jarak <= config.jarakDeteksi)
        {
            Kejar();
        }
    }

    void Kejar()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            config.kecepatan * Time.deltaTime
        );
    }

    void Serang()
    {
        // contoh: panggil fungsi damage ke player
        // player.GetComponent<PlayerHealth>().TerimaDamage(config.damageSerang);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // player.GetComponent<PlayerHealth>().TerimaDamage(config.damageSaatTabrakan);
        }
    }

    public void TerkenaDamage(int dmg)
    {
        hpSekarang -= dmg;
        Debug.Log("Boss HP sekarang: " + hpSekarang);

        if (hpSekarang <= 0)
        {
            Mati();
        }
    }

    void Mati()
    {
        Destroy(gameObject);
    }
}
