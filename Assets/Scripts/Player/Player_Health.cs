using UnityEngine;

public class Player_Health : HealthController
{
    private Player player;

    public bool isDead { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public override void ReduceHealth(int damage)
    {
        base.ReduceHealth(damage);

        if (ShouldDie())
            Die();

        UI.instance.inGameUI.UpdateHealthUI(currentHealth, maxHealth);
    }
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UI.instance.inGameUI.UpdateHealthUI(currentHealth, maxHealth);
    }
    private void Die()
    {
        if (isDead)
            return;

        Debug.Log("Player was killed at " + Time.time);
        isDead = true;
        player.anim.enabled = false;
        player.ragdoll.RagdollActive(true);

        GameManager.instance.GameOver();
    }

}
