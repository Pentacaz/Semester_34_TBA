using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using WaitForSeconds = UnityEngine.WaitForSeconds;

public class EnemyReciever : MonoBehaviour
{
    public  VisualEffect hitVFX;
   
    public bool IsBoss = false;

    #region takeDMG

  
    public float currentHp;

    public bool hasShields;
    public float currentShields;

    #region Damage Indicator

    public CinemachineFreeLook vcam;
    public bool tookDamage;
    public bool canGetDamage = true;
    public float invincibilityTimer = 1.5f;
    private float _invincibilityTimerValue;
    public float knockback;
   
    public Image damageIndicator;
    public Image shieldIndicator;
    public GameObject shieldGameObject;
    public TextMeshProUGUI damageText;
    public GameObject damageTextObject;

    #endregion

    #endregion


    #region References
    private SpawnEnemies[] _spawnEnemies;
    private CamBehavior _camBehavior;
    private EnemyStatus _enemyStatus;
    private Rigidbody _rigidbody;
    public Animator _animator;
    public List<GameObject> loot;
    public float noSpawnChance = 0.1f;
    #endregion

    private void Awake()
    {
        _enemyStatus = GetComponent<EnemyStatus>();
        _camBehavior = GetComponent<CamBehavior>();
        _rigidbody = GetComponent<Rigidbody>();
        _spawnEnemies = FindObjectsOfType<SpawnEnemies>();

    }

    private void Start()
    {
        Debug.Log("enemyreciever");
        currentHp = _enemyStatus.enemyMaxHp;
        _invincibilityTimerValue = invincibilityTimer;
        //DamageIndication(damageIndicator, _enemyStatus.enemyMaxHp, currentHp, 0, false);
        if (!IsBoss)
        {
            SetShields();
        }

        
    }

    private void Update()
    {
        if (tookDamage && hasShields)
        {
            
        }
        Invincibility(tookDamage);

    }
//applies damage to enemy if possible atm and updates Healthbar Ui. enemies have thier own little health and shields attacked to them.
    public void GetDmg(int dmg, bool crit)
    {
        if (canGetDamage)
        {
            tookDamage = true;
            if (hasShields)
            {
                currentShields -= dmg / 2f;
                
            }
            else
            {
                currentHp -= dmg;
            }

            if (currentHp > _enemyStatus.enemyMaxHp)
            {
                currentHp = _enemyStatus.enemyMaxHp;
            }

            DamageIndication(damageIndicator, _enemyStatus.enemyMaxHp, currentHp, dmg, crit);
        }

    }

    public void DamageIndication(Image damageind, float maxHpval, float currentHpval, int damagevalue, bool crit)
    {

        if (hasShields)
        {
            shieldIndicator.fillAmount = currentShields / _enemyStatus.enemyDefense;
        }

        if (tookDamage)
        {
            _camBehavior.CamShake();
            damageText.enabled = true;
            StartCoroutine(DamageDisplay(damagevalue, crit));
            hitVFX.Play();
            Pushback();

            Debug.Log("TOOK DAMAGE ENEMY");
        }

        if (currentShields <= 0)
        {
            hasShields = false;
            damageind.fillAmount = currentHpval / maxHpval;
            shieldIndicator.enabled = false;
            
        }

        if (currentHp <= 0)
        {
            if (_animator != null)
            {
                _animator.SetTrigger("ActionTrigger");
                _animator.SetInteger("ActionId", 1);
            }


            StartCoroutine(nameof(Enemydeath));
        }


    }



    public void Invincibility(bool damage)
    {
        if (!IsBoss)
        {
            if (damage)
            {
                canGetDamage = false;
                invincibilityTimer -= Time.deltaTime;
            }

            if (invincibilityTimer <= 0)
            {
                tookDamage = false;
                canGetDamage = true;
                invincibilityTimer = _invincibilityTimerValue;
            }
        }

    }

    public void Pushback()
    {
        print("PUSHED!!!!");
        _rigidbody.AddForce(-this.gameObject.transform.position * knockback, ForceMode.Impulse);
    }



    // Showcases dmg numbers and moves them upwards a bit - purely visual
    public IEnumerator DamageDisplay(int dmg, bool crit)
    {


        damageText.enableVertexGradient = crit;
        float moveDistance = 1f;
        Vector3 targetPosition = damageTextObject.transform.position + Vector3.up * moveDistance;
        Vector3 originalPosition = damageTextObject.transform.position;


        float moveDurationTimer = 0f;
        float moveDuration = 1f;


        damageTextObject.SetActive(true);
        damageText.SetText($"{dmg}");

        while (moveDurationTimer < moveDuration)
        {

            damageTextObject.transform.position =
                Vector3.Lerp(originalPosition, targetPosition, moveDurationTimer / moveDuration);
            moveDurationTimer += Time.deltaTime;
            yield return null;
        }

        damageTextObject.SetActive(false);
        damageTextObject.transform.position = originalPosition;

    }

    public void SetShields()
    {

        if (!IsBoss && Random.value < 0.2f)
        {
            hasShields = true;
            shieldGameObject.SetActive(true);
            shieldIndicator.enabled = true;
            currentShields = _enemyStatus.enemyDefense;
        }
        else if (IsBoss)
        {
            hasShields = true;
            shieldIndicator.enabled = true;
            shieldGameObject.SetActive(true);
            currentShields = _enemyStatus.enemyDefense;
        }
    }
    
    
    public void SpawnRandomLoot()
    {
        if (Random.value < noSpawnChance)
        {
            Debug.Log("Nothing this time!...How unfortunate");
            return;
        }
        
        int Index = Random.Range(0, loot.Count);
        GameObject enemyLoot = loot[Index];

     
        enemyLoot.transform.position = gameObject.transform.position;
    
        enemyLoot.SetActive(true);

    }

    IEnumerator Enemydeath()
    {
        
        if (_animator != null)
        {
            _animator.SetTrigger("ActionTrigger");
            _animator.SetInteger("ActionId", 1);
        }


        foreach (var obj in _spawnEnemies)
        {
            obj.RemoveDefeatedEnemy(this.gameObject);
        }
        
        Debug.Log("DEATH ENEMY");
        SpawnRandomLoot();
        yield return new WaitForSeconds(0.2f);
        this.gameObject.SetActive(false);
        
        Destroy(transform.parent.gameObject, 3f);
    }
    
}
