using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Enemy", menuName ="Enemy/Create new enemy")]
public class EnemyBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite sprite;

    [SerializeField] EnemyType type;

    //base stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }
    public Sprite Sprite
    {
        get { return sprite; }
    }
    public EnemyType Type
    {
        get { return type; }
    }
    public int MaxHp
    {
        get { return maxHp; }
    }
    public int Attack
    {
        get { return attack; }
    }
    public int Defense
    {
        get { return defense; }
    }
}

public enum EnemyType
{
    None,
    Dummy,
    Wolf,
    Boss
}