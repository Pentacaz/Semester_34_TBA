using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BossFightBehavior : MonoBehaviour

{
    public enum BossState
    {
        DEFEND,
        IDLE,
        ATTACK,
        SPAWNENEMIES,
        VULNERABLE
    }

    public BossState currentState;

    public List<GameObject> attackObjects;
    public List<Transform> spawnPoints;
    public float attackDelay = 1f;
    public float idleDuration = 1f;
    public GameObject playerCam;
    public GameObject blendcam;

    private bool isVulnerable = false;
    private bool enemiesDefeated = false;
    private bool hasShields = false;
    public bool hasSpawnedEnemies = false;
    private BossState previousState;

    private SpawnEnemies _spawnEnemies;
    private EnemyStatus _enemyStatus;
    private EnemyReciever _enemyReciever;
    private CamBehavior _camBehavior;
    private PlayerBaseController _playerBaseController;
    
    private void Awake()
    {
        _spawnEnemies = GetComponent<SpawnEnemies>();
        _enemyStatus = GetComponent<EnemyStatus>();
        _enemyReciever = GetComponent<EnemyReciever>();
        _camBehavior = GetComponent<CamBehavior>();
        _playerBaseController = FindObjectOfType<PlayerBaseController>();

        _enemyReciever.IsBoss = true;
    }

    void Update()
    {
        _enemyReciever.canGetDamage = isVulnerable;
        enemiesDefeated = _spawnEnemies.noMoreEnemies;
        hasShields = _enemyReciever.hasShields;
        //hasSpawnedEnemies = !_spawnEnemies.noMoreEnemies;

        Debug.Log($"The Boss is currently in {currentState}");
    }

    void TransitionToState(BossState newState)
    {
        currentState = newState;
        OnStateEnter(newState);
    }

    void OnStateEnter(BossState state)
    {
        switch (state)
        {
            case BossState.DEFEND:
                StartCoroutine(DefendRoutine());
                break;
            case BossState.IDLE:
                StartCoroutine(IdleRoutine(previousState));
                break;
            case BossState.ATTACK:
                StartCoroutine(AttackRoutine());
                break;
            case BossState.SPAWNENEMIES:
                StartCoroutine(SpawnEnemiesRoutine());
                break;
            case BossState.VULNERABLE:
                StartCoroutine(VulnerableRoutine());
                break;
        }
    }

    BossState GetPreviousState()
    {
        switch (currentState)
        {
            case BossState.IDLE:
                if (hasShields)
                    return BossState.DEFEND;
                else if (attackObjects.Count > 0)
                    return BossState.ATTACK;
                else if (!enemiesDefeated)
                    return BossState.SPAWNENEMIES;
                else
                    return BossState.VULNERABLE;
            default:
                return BossState.IDLE;
        }
    }

    IEnumerator DefendRoutine()
    {
        SetShields(true);

        yield return new WaitForSeconds(idleDuration);
        TransitionToState(BossState.IDLE);
    }

    IEnumerator IdleRoutine(BossState previousState)
    {
        yield return new WaitForSeconds(idleDuration);

        switch (previousState)
        {
            case BossState.DEFEND:
                TransitionToState(BossState.ATTACK);
                break;
            case BossState.ATTACK:
                TransitionToState(BossState.SPAWNENEMIES);
                break;
            case BossState.SPAWNENEMIES:
                if (enemiesDefeated)
                {
                    TransitionToState(BossState.VULNERABLE);
                }
                else
                {

                    TransitionToState(BossState.SPAWNENEMIES);
                }

                break;
            case BossState.VULNERABLE:
                TransitionToState(BossState.DEFEND);
                break;
        }
    }

    IEnumerator AttackRoutine()
    {

        foreach (var attackObject in attackObjects)
        {
            attackObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(attackDelay);
        
        foreach (var attackObject in attackObjects)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            attackObject.transform.position = spawnPoints[randomIndex].position;
            attackObject.transform.GetChild(1).gameObject.GetComponent<VisualEffect>().Play();
            attackObject.SetActive(true);
        }

        yield return new WaitForSeconds(idleDuration);
        previousState = BossState.ATTACK;
        TransitionToState(BossState.IDLE);
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        SpawnEnemies();
        //yield return new WaitForSeconds(2);
        yield return new WaitUntil(() => enemiesDefeated);
        previousState = BossState.SPAWNENEMIES;
        TransitionToState(BossState.IDLE);
    }

    IEnumerator VulnerableRoutine()
    {
        isVulnerable = true;
        SetShields(false);
        yield return new WaitUntil(() => _enemyReciever.currentHp <= _enemyStatus.enemyMaxHp * 2 / 3);
        isVulnerable = false;
        previousState = BossState.VULNERABLE;
        TransitionToState(BossState.IDLE);
    }

    void SetShields(bool active)
    {
        _enemyReciever.SetShields();
    }

    void SpawnEnemies()
    {
        if (!hasSpawnedEnemies)
        {
            _spawnEnemies.StartNewRound();
            hasSpawnedEnemies = true;
        }
    }
    
    public void StartCutscene()
    {
        StartCoroutine(nameof(StartBossCutscene));
    }

    public IEnumerator StartBossCutscene()
    {
        _playerBaseController.DisableInput();
        _camBehavior.shakeDuration = 3;
        _camBehavior.shakeStrength = 0.9f;
        _camBehavior.CamShake();
        playerCam.SetActive(false);
        blendcam.SetActive(true);
        yield return new WaitForSeconds(_camBehavior.shakeDuration);
        yield return new WaitForSeconds(2f);
        blendcam.SetActive(false);
        playerCam.SetActive(true);
        _camBehavior.enabled = false;
        _playerBaseController.EnableInput();
        yield return new WaitForSeconds(3f);
        TransitionToState(BossState.DEFEND);

    }
    
}

