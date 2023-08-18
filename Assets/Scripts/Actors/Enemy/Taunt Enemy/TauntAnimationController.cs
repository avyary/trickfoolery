using UnityEngine;

//*******************************************************************************************
// TauntAnimationController
//*******************************************************************************************
/// <summary>
/// Handles the animations and plays SFX for the taunt enemy. Contains various
/// methods for the different states such as beginning and finishing the Attacking and
/// Rolling actions.
/// </summary>
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

    /// <summary>
    /// Plays the beginning attack SFX and logs the beginning of this Enemy's attack state. Invoked by the
    /// TauntAttack animation event.
    /// </summary>
    private void StartAttackANIM()
    {
        tauntEnemySounds.StartAttackSFX();
        Debug.Log("startattack");
    }

    /// <summary>
    /// Plays the end attack SFX and logs the end of this Enemy's attack state. Invoked by the TauntAttack
    /// animation event.
    /// </summary>
    private void FinishAttackANIM()
    {
        tauntEnemySounds.FinishAttackSFX();
        Debug.Log("finishattack");
    }

    /// <summary>
    /// Toggles the associated flag to mark the end of the rolling state. Invoked by the TauntRoll animation event.
    /// </summary>
    public void FinishRolling()
    {
        doneRolling = true;
    }

    /// <summary>
    /// Toggles the associated flag to mark the end of the attacking state. Invoked by the TauntAttack animation event.
    /// </summary>
    public void FinishAttacking()
    {
        doneAttacking = true;
    }

    public void moveEnemy()
    {
    }
}
