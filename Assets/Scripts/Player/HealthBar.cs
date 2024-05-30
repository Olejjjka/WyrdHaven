using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Sprite[] healthSprites;
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
        int healthIndex = Mathf.Clamp(Mathf.FloorToInt(currentHealth), 0, 25);
        healthImage.sprite = healthSprites[healthIndex];
    }
}
