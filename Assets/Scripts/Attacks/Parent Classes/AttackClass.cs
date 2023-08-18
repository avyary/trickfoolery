using UnityEngine;

//*******************************************************************************************
// Attack
//*******************************************************************************************
/// <summary>
/// Abstract class that handles the hit box logic of an attack to damage players and
/// enemies alike.
/// </summary>
public abstract class Attack : MonoBehaviour
{
    [SerializeField]
    float _startupTime;
    [SerializeField]
    float _activeTime;
    [SerializeField]
    float _recoveryTime;
    [SerializeField]
    int _damage;
    [SerializeField]
    float _stunTime;
    [SerializeField]
    float _range;

    public float startupTime { get; set; }
    public float activeTime { get; set; }
    public float recoveryTime { get; set; }
    public int damage { get; set; }
    public float stunTime { get; set; }
    public float range { get; set; }

    protected Collider _collider;
    public MeshRenderer _renderer;

    private string[] collisionTags = {"Player", "Enemy"};

    /// <summary>
    /// Initializes all the class bookkeeping data associated with an Attack such as:
    /// <p> <b> StartupTime </b> to delay an Enemy before unleashing an Attack. </p>
    /// <p> <b> ActiveTime </b> to denote how long the Attack lasts. </p>
    /// <p> <b> RecoveryTime </b> to denote how long the associated Enemy must wait before returning to its regular
    /// behaviour after an Attack. </p>
    /// <p> The <b> damage </b> this Attack deals. </p>
    /// <p> The <b> stunTime </b> duration to freeze actors in place when hit. </p>
    /// <p> The <b> range </b> of this Attack. </p>
    /// </summary>
    protected virtual void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _renderer.enabled = false;
        _collider.enabled = false;

        startupTime = _startupTime;
        activeTime = _activeTime;
        recoveryTime = _recoveryTime;
        damage = _damage;
        stunTime = _stunTime;
        range = _range;
    }

    /// <summary>
    /// Enables the collider associated with the Attack.
    /// </summary>
    public virtual void Activate()
    {
        _collider.enabled = true;
    }

    /// <summary>
    /// Disables the collider associated with the Attack.
    /// </summary>
    public virtual void Deactivate()
    {
        _collider.enabled = false;
    }

    /// <summary>
    /// Damages and stuns any Enemies and the player that come in contact with this Attack by the <i>
    /// damage </i> and <i> stunTime </i> attributes.
    /// </summary>
    /// <param name="other"> The Collider of the other GameObject that fired this trigger collider. </param>
    public void OnTriggerEnter(Collider other) {
        if (other.gameObject == transform.parent.gameObject) {
            return;
        }
        else if (other.gameObject.tag == "Enemy")
        {
            print(other.gameObject.name);
            other.gameObject.GetComponent<Enemy>().TakeHit(transform.parent.GetComponent<Enemy>(), damage, stunTime);
        }
        else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeHit(damage, transform.parent.GetComponent<Enemy>());
        }
    }
}
