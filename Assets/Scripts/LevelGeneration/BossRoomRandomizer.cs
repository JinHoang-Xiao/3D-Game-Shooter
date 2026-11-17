using UnityEngine;

public class BossRoomRandomizer : MonoBehaviour
{
    [SerializeField] private Enemy[] bossCandidates;

    public Enemy SelectedBoss { get; private set; }

    private void Awake()
    {
        if (bossCandidates == null || bossCandidates.Length == 0)
        {
            Debug.LogError("Boss Room Randomizer chưa được gán boss.", this);
            return;
        }

        int index = Random.Range(0, bossCandidates.Length);
        SelectedBoss = bossCandidates[index];

        // Giữ tất cả boss tắt cho đến khi LevelGenerator build xong NavMesh.
        foreach (Enemy boss in bossCandidates)
        {
            if (boss != null)
                boss.gameObject.SetActive(false);
        }
    }

    public bool IsCandidate(Enemy enemy)
    {
        foreach (Enemy boss in bossCandidates)
        {
            if (boss == enemy)
                return true;
        }

        return false;
    }
}