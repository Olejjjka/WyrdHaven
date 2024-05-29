using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Sprite[] healthSprites; // ћассив спрайтов здоровь€
    private Image healthImage;
    private Player player;

    private void Start()
    {
        healthImage = GetComponent<Image>();
        player = Player.Instance;
    }

    private void Update()
    {
        if (player != null)
        {
            UpdateHealthBar(player.GetCurrentHealth());
        }
    }


    private void UpdateHealthBar(float currentHealth)
    {
        // ¬ычисл€ем индекс спрайта здоровь€ на основе текущего здоровь€ игрока
        int healthIndex = Mathf.Clamp(Mathf.FloorToInt(currentHealth), 0, 25);

        // ќбновл€ем изображение здоровь€
        healthImage.sprite = healthSprites[healthIndex];
    }
}
