using UnityEngine;

/// <summary>
/// Hộp hồi máu tương tác được. Khi người chơi nhặt, hộp hồi máu rồi được trả
/// về ObjectPool hiện có trong project.
/// </summary>
public class Pickup_Health : Interactable
{
    [SerializeField, Min(1)] private int healAmount = 25;

    private Player_Health playerHealth;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (playerHealth == null)
            playerHealth = other.GetComponent<Player_Health>();
    }

    public override void Interaction()
    {
        // Nếu không tìm được Player_Health hoặc máu đã đầy thì giữ hộp lại.
        if (playerHealth == null || playerHealth.currentHealth >= playerHealth.maxHealth)
            return;

        playerHealth.currentHealth = Mathf.Min(
            playerHealth.currentHealth + healAmount,
            playerHealth.maxHealth);

        ObjectPool.instance.ReturnObject(gameObject);
    }
}
