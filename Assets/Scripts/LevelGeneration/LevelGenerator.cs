using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Level parts
    [SerializeField] private Transform lastLevelPart;
    [SerializeField] private List<Transform> levelParts;
    private List<Transform> currentLevelParts;
    private List<Transform> generatedLevelParts = new List<Transform>();

    // Snap points
    [SerializeField] private SnapPoint nextSnapPoint;
    //private SnapPoint defaultSnapPoint;


    // Cooldown
    [Space]
    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver;

    private void Start()
    {
        //enemyList = new List<Enemy>();
        //defaultSnapPoint = nextSnapPoint;
        //InitializeGeneration();
        currentLevelParts = new List<Transform>(levelParts);
    }


    private void Update()
    {
        if (generationOver)
            return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0)
        {
            if (currentLevelParts.Count > 0)
            {
                cooldownTimer = generationCooldown;
                GenerateNextLevelPart();
            }
            else if (generationOver == false)
            {
                FinishGeneration();
            }
        }
    }

    //[ContextMenu("Restart generation")]
    //private void InitializeGeneration()
    //{
    //    nextSnapPoint = defaultSnapPoint;
    //    generationOver = false;
    //    currentLevelParts = new List<Transform>(levelParts);

    //    DestroyOldLevelPartsAndEnemies();
    //}

    //private void DestroyOldLevelPartsAndEnemies()
    //{
    //    foreach (Enemy enemy in enemyList)
    //    {
    //        Destroy(enemy.gameObject);
    //    }


    //    foreach (Transform t in generatedLevelParts)
    //    {
    //        Destroy(t.gameObject);
    //    }

    //    generatedLevelParts = new List<Transform>();
    //    enemyList = new List<Enemy>();
    //}

    private void FinishGeneration()
    {
        generationOver = true;
        Transform levelPart = Instantiate(lastLevelPart);
        LevelPart levelPartScript = levelPart.GetComponent<LevelPart>();

        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);
    }

    [ContextMenu("Create next level part")]
    private void GenerateNextLevelPart()
    {
        Transform newPart = Instantiate(ChooseRandomPart());
        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();

        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);
        nextSnapPoint = levelPartScript.GetExitPoint();

    }

    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, currentLevelParts.Count);

        Transform choosenPart = currentLevelParts[randomIndex];

        currentLevelParts.RemoveAt(randomIndex);

        return choosenPart;
    }
}
