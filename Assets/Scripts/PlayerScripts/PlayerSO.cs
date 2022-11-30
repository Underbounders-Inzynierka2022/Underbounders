using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSO : ScriptableObject
{
    public float speed = 1.4f;
    public float attack = 1.4f;
    public float attackSpeed = 1.4f;
    public float defence = 1.4f;

    public List<ItemSO> equipment;
}
