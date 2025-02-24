using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BossState
{
    ATTACK,
    DEFEND,
    VULNERABLE,
    IDLE
}

public class BossBehavior : MonoBehaviour
{
    public BossState currentState;
    public List<GameObject> damageObjects;
    public List<GameObject> activeDamageObjects;

    public GameObject playerCam;
    public GameObject blendcam;

    public List<Transform> damageAreaSpawnPositions;
    public List<GameObject> damageAreaIndicator;
    public int damageAreaAmount;

    public float vulnerableTimer;
    private float _vulnerableTimerValue;

    private float _attackCooldownvalue;
    private float _idleTimerValue;
    public float idleDuration = 2f;

    public bool hasSpawnedEnemies;
    public bool isDefending;
    public bool isAttacking;
    public bool lockedIn;

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
    }

    void Start()
    {
        _vulnerableTimerValue = vulnerableTimer;
        _attackCooldownvalue = _enemyStatus.enemyAttackCooldown;
        _idleTimerValue = idleDuration;
        currentState = BossState.IDLE;
        _enemyReciever.IsBoss = true;
    }

    void Update()
    {
        switch (currentState)
        {
            case BossState.ATTACK:
                AttackBehavior();
                break;
            case BossState.DEFEND:
                DefendBehavior();
                break;
            case BossState.VULNERABLE:
                VulnerableBehavior();
                break;
            case BossState.IDLE:
                IdleBehavior();
                break;
        }

        CheckForShields();

        if (currentState == BossState.VULNERABLE)
        {
            StartVulnerableTimer();
        }

        if (isDefending)
        {
            _enemyReciever.SetShields();
        }

        if (_spawnEnemies.noMoreEnemies)
        {
            ChangeState(BossState.VULNERABLE);
        }

        AttackCooldown();
    }

    void ChangeState(BossState newState)
    {
        currentState = newState;
    }

    public void AttackBehavior()
    {
        for (int i = 0; i < damageAreaAmount; i++)
        {
            GameObject gameobject = GetInactiveGameobject();

            if (gameobject != null)
            {
                Transform spawnPosition = damageAreaSpawnPositions[Random.Range(0, damageAreaSpawnPositions.Count)];
                gameobject.transform.position = spawnPosition.position;

                gameobject.SetActive(true);

                activeDamageObjects.Add(gameobject);
            }
        }

        ChangeState(BossState.DEFEND);
    }

    private GameObject GetInactiveGameobject()
    {
        foreach (GameObject dmgObject in damageObjects)
        {
            if (!dmgObject.activeInHierarchy)
            {
                return dmgObject;
            }
        }

        return null;
    }

    public void DefendBehavior()
    {
        _enemyReciever.canGetDamage = false;
        isDefending = true;
        isDefending = false;

        if (!hasSpawnedEnemies)
        {
            _spawnEnemies.StartNewRound();
            hasSpawnedEnemies = true;
        }

      
    }

    public void CheckForShields()
    {
        if (currentState != BossState.IDLE && _enemyReciever.currentShields <= 0)
        {
            ChangeState(BossState.VULNERABLE);
            hasSpawnedEnemies = false;
        }
    }

    public void VulnerableBehavior()
    {
        _enemyReciever.canGetDamage = true;
        //play vfx
    }

    private void StartVulnerableTimer()
    {
        vulnerableTimer -= Time.deltaTime;

        if (vulnerableTimer <= 0)
        {
            vulnerableTimer = _vulnerableTimerValue;
            ChangeState(BossState.IDLE);
        }
    }

    public void IdleBehavior()
    {
        _idleTimerValue -= Time.deltaTime;

        if (_idleTimerValue <= 0)
        {
            _idleTimerValue = idleDuration;
            ChangeState(BossState.ATTACK);
        }
    }

    public void AttackCooldown()
    {
        if (currentState == BossState.ATTACK)
        {
            _attackCooldownvalue -= Time.deltaTime;

            if (_attackCooldownvalue <= _enemyStatus.enemyAttackCooldown / 2)
            {
                foreach (var indicator in damageAreaIndicator)
                {
                    indicator.SetActive(true);
                }
            }
            else
            {
                foreach (var indicator in damageAreaIndicator)
                {
                    indicator.SetActive(false);
                }
            }

            if (_attackCooldownvalue <= _enemyStatus.enemyAttackCooldown / 4)
            {
                lockedIn = true;
            }
            else
            {
                lockedIn = false;
            }

            if (_attackCooldownvalue <= 0)
            {
                isAttacking = true;
                _attackCooldownvalue = _enemyStatus.enemyAttackCooldown;
                AttackBehavior();
            }
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
        ChangeState(BossState.ATTACK);

    }

}

