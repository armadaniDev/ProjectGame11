using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalKoin;
    private int koinTerkumpul = 0;
    private int skor = 0;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;

    // Dipanggil otomatis oleh Unity saat GameManager aktif (mis. saat scene dimuat).
    // Di sinilah GameManager mulai "berlangganan" (subscribe) ke event kematian zombie.
    void OnEnable()
    {
        Enemy.OnZombieMati += TambahSkorSaatZombieMati;
    }

    // Dipanggil otomatis oleh Unity saat GameManager nonaktif/dihancurkan.
    // WAJIB unsubscribe di sini, pasangan dari OnEnable, supaya tidak terjadi memory leak.
    void OnDisable()
    {
        Enemy.OnZombieMati -= TambahSkorSaatZombieMati;
    }

    // Method ini otomatis terpanggil setiap ada Enemy mana pun yang memicu OnZombieMati.
    // Parameter 'zombieYangMati' -> data Enemy yang dikirim lewat OnZombieMati?.Invoke(this) di Enemy.cs.
    void TambahSkorSaatZombieMati(Enemy zombieYangMati)
    {
        skor += 10;
        Debug.Log("Skor: " + skor);
    }

    // Dipanggil sekali saat GameManager pertama kali muncul di scene.
    void Start()
    {
        totalKoin = GameObject.FindGameObjectsWithTag("coin").Length;
    }

    // Dipanggil dari script Koin/Player setiap kali 1 koin berhasil diambil.
    public void AmbilKoin()
    {
        koinTerkumpul++;
        if (koinTerkumpul == totalKoin)
            Menang();
    }

    // Dipanggil saat kondisi menang terpenuhi (semua koin terkumpul).
    void Menang()
    {
        Debug.Log("KAMU MENANG!");
    }

    // dipanggil saat Player mati (dari PlayerMovement)
    public void GameOver()
    {
        Debug.Log("GAME OVER");
        Time.timeScale = 0f;
        gameOverPanel?.SetActive(true);
    }

    // dipanggil dari tombol Restart
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}