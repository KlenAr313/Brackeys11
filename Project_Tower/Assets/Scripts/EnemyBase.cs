using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private int posX;
    [SerializeField] private int posY;

    [SerializeField] private int health;
    [SerializeField] private int baseDamage;
    [SerializeField] private int speed;

    protected int PosX { get => posX; set => posX = value; }
    protected int PosY { get => posY; set => posY = value; }
    protected int Health { get => health; set => health = value; }
    protected int BaseDamage { get => baseDamage; set => baseDamage = value; }
    protected int Speed { get => speed; set => speed = value; }
}
