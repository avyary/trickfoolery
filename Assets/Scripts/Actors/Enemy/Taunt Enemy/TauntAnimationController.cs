using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntAnimationController : MonoBehaviour
{
    public bool doneRolling;
    public bool doneAttacking;
    public bool doneDancing;

    [SerializeField] private AnimationClip tauntAttack;
    [SerializeField] private Animator animator;

    [SerializeField]
    private TauntEnemySounds tauntEnemySounds;

    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        doneRolling = true;
        doneAttacking = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StartAttackANIM()
    {
        tauntEnemySounds.StartAttackSFX();
        Debug.Log("startattack");
    }

    private void FinishAttackANIM()
    {
        tauntEnemySounds.FinishAttackSFX();
        Debug.Log("finishattack");
    }

    public void FinishRolling()
    {
        doneRolling = true;
    }

    public void FinishAttacking()
    {
        doneAttacking = true;
    }

    public void moveEnemy()
    {
    }
}
