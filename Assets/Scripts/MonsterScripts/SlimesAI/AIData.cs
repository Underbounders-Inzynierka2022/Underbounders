using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData: MonoBehaviour
{

    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;

    public Transform currentTarget;

    public int GetTargetsCount() => targets is null ? 0 : targets.Count;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
