using UnityEngine;
using UnityEngine.InputSystem; // WAJIB untuk Input System

public class PlayerMovement : MonoBehaviour, IDamageable
{
    [Header("Pengaturan HP")]
    [SerializeField] private int hp = 100;
    [SerializeField] private int hpMax = 100;
    public int Hp => hp;
    public int HpMax => hpMax;

    public int skor = 0;
    public float kecepatan = 5f;
    private Vector2 arahGerak; // nilai dari action "Move"
    public GameManager gameManager;

    private bool sudahMati = false; // cegah Mati() jalan berkali-kali

    void OnMove(InputValue value)
    {
        arahGerak = value.Get<Vector2>();
    }

    void Update()
    {
        if (sudahMati) return; // stop gerak kalau sudah mati

        Vector3 arah = new Vector3(arahGerak.x, arahGerak.y, 0);
        transform.position += arah * kecepatan * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("coin"))
        {
            Destroy(other.gameObject);
            skor++;
            Debug.Log("Total Skor : " + skor);
            gameManager.AmbilKoin();
        }
    }

    public void KenaDamage(int jumlah)
    {
        hp -= jumlah;
        hp = Mathf.Clamp(hp, 0, hpMax);
        Debug.Log($"Player kena damage {jumlah}, HP sisa: {hp}");

        if (hp <= 0)
        {
            Mati();
        }
    }

    private void Mati()
    {
        if (sudahMati) return; // cegah dipanggil dobel

        sudahMati = true;
        Debug.Log("Player mati!");
        gameManager.GameOver();
    }
}