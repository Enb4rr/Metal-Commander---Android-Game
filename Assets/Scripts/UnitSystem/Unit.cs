using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    #region Stats and Constructor
    [SerializeField] public string unitName, className, unitSide;
    [SerializeField] public int hitPoints, maxHP, weaponPower, attack, defense;
    [SerializeField] public float movement;
    [HideInInspector] [SerializeField] public bool hasMoved, hasAttacked;
    [SerializeField] public bool isDead;
    [SerializeField] public Sprite portrait;

    [SerializeField] private GameObject parent;

    [SerializeField] public GameObject path;

    //UI for healthbar

    [SerializeField] public HealthBarBehaviour healthBarBehaviour;
    
    //From here audio System

    [SerializeField] private AudioManager source;
    
    //Animator

    [SerializeField] public Animator anim;
    [SerializeField] public RuntimeAnimatorController controller;
    
    [SerializeField] private TurnSystem.TurnSystem turnSystem;

    [SerializeField] private SpriteRenderer renderer;
    
    //enemy range

    [HideInInspector] [SerializeField] public GameObject range;
    
    //Other

    [HideInInspector] [SerializeField] public bool foundRival = false;
    
    //Enemy IA

    [SerializeField] public bool aggressive, inRange, passive;
    
    //Shaders

    [SerializeField] public Material mat;
    public Material instancedMat;
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int Fade = Shader.PropertyToID("_fade");
    private float _time;
    private static readonly int Thickness = Shader.PropertyToID("_thickness");

    public Unit(int hitPoints, int maxHP, int attack, int defense, int movement, int weaponPower)
    {
        this.hitPoints = hitPoints;
        this.maxHP = maxHP;
        this.attack = attack;
        this.defense = defense;
        this.movement = movement;
        this.weaponPower = weaponPower;
    }
    #endregion

    private void Start()
    {
        renderer = GetComponentInParent<SpriteRenderer>();
        source = FindObjectOfType<AudioManager>();
        turnSystem = FindObjectOfType<TurnSystem.TurnSystem>(); 
        instancedMat = gameObject.GetComponent<SpriteRenderer>().material = new Material(mat);
        
        instancedMat.SetFloat(Thickness, 0);
    }

    private void Update()
    {
        if (isDead)
        {
            StartCoroutine(DeadAnim());
        }

        if (hasMoved)
        {
            renderer.color = Color.gray;
        }

        if (!hasMoved)
        {
            renderer.color = Color.white;
        }
    }

    #region Combat Methods

    public void Attack(Unit attacked, Unit attacker)
    {
        attacked.hitPoints -= (attacker.weaponPower + attacker.attack) - attacked.defense;

        if (attacked.hitPoints > 0 && attacker.className != "Sniper" && attacker.CompareTag("Ally"))
        {
            attacker.hitPoints -= (attacked.weaponPower + attacked.attack) - attacker.defense;
        }
        else if (attacked.hitPoints > 0 && attacker.className != "Mage" && attacker.CompareTag("Enemy"))
        {
            attacker.hitPoints -= (attacked.weaponPower + attacked.attack) - attacker.defense;
        }

        if (attacked.hitPoints <= 0)
        {
            if (attacked.CompareTag("Ally"))
            {
                turnSystem.playerCount++;
            }
            if (attacked.CompareTag("Enemy"))
            {
                turnSystem.enemyCount++;
            }
            
            attacked.isDead = true;
        }

        if (attacker.hitPoints <= 0)
        {
            if (attacker.CompareTag("Ally"))
            {
                turnSystem.playerCount++;
            }
            if (attacker.CompareTag("Enemy"))
            {
                turnSystem.enemyCount++;
            }
            
            isDead = true;
        }
    }

    public void Heal(Unit healed)
    {
        if (healed.hitPoints > 0)
        {
            healed.hitPoints += weaponPower + attack;
        
            if (healed.hitPoints > healed.maxHP)
            {
                healed.hitPoints = healed.maxHP;
            }
        }
    }
    #endregion

    private void Deactivate()
    {
        parent.SetActive(false);
    }

    private IEnumerator DeadAnim()
    {
        yield return new WaitForSeconds(5);

        _time += 0.02f;
        instancedMat.SetFloat(Fade, Mathf.Lerp(1, 0, _time));
            
        Invoke(nameof(Deactivate), 0.8f);
    }
}
