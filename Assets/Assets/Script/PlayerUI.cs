using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Referensi")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Slider hpBar;
    [SerializeField] private TMP_Text hpText;      // ganti Text jadi TMP_Text
    [SerializeField] private TMP_Text speedText; 
    [SerializeField] private Image fillImage; // drag child "Fill" ke sini

    void Update()
    {
        if (player == null) return;

        if (hpBar != null)
        {
            hpBar.maxValue = player.HpMax;
            hpBar.value = player.Hp;
        }

        if (hpText != null)
        {
            hpText.text = $"{player.Hp} / {player.HpMax}";
        }

        if (speedText != null)
        {
            speedText.text = $"Speed: {player.kecepatan}";
        }

        // Warna Fill berubah sesuai persentase HP
        if (fillImage != null && player.HpMax > 0)
        {
            float persen = (float)player.Hp / player.HpMax;

            if (persen > 0.5f)
                fillImage.color = Color.green;
            else if (persen > 0.25f)
                fillImage.color = Color.yellow;
            else
                fillImage.color = Color.red;
        }
    }
}