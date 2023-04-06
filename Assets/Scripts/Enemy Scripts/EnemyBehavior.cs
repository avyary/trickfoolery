using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    bool isCharging = false;
    bool isAggro = false;
    bool isAttacking = false;
    public float chargeTime = 3f;
    public float attackTime = 1f;
    float flashInterval;
    Color originalColor;
    public LineRenderer lineRend;
    public GameObject player;
    Vector3 direction;
    public float attackSpeed = 2f;
    public GameObject obj1;
public GameObject obj2;
public GameObject obj3;
public GameObject obj4;
private GameObject[] objs;

    // Start is called before the first frame update
    void Start()
    {
    obj1.SetActive(false);
    obj2.SetActive(false);
    obj3.SetActive(false);
    obj4.SetActive(false); 
        originalColor = GetComponent<MeshRenderer>().material.color;
        flashInterval = chargeTime / 6f;
        lineRend.positionCount = 2;
        player = GameObject.Find("Player");
            objs = new GameObject[] { obj1, obj2, obj3, obj4 };
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            gameObject.transform.position += attackSpeed * direction * Time.deltaTime; 
        }
    }

    IEnumerator aggro() {
        if (transform.position.y <= 2.5f)
        {
            isAggro = true;
            StartCoroutine(charge());
            yield return new WaitForSeconds(chargeTime);
            StartCoroutine(attack());
        }
    }

    IEnumerator attack()
    {
        print("starting attack");
        isAttacking = true;
        yield return new WaitForSeconds(attackTime);
        GetComponent<MeshRenderer>().material.color = originalColor;
        isAttacking = false;
        print("finishing attack");
        isAggro = false;
    }

    IEnumerator charge()
    {   
        lineRend.SetPosition(0, gameObject.transform.position);
        direction = player.transform.position - gameObject.transform.position;
        direction[1] = 0;
        lineRend.SetPosition(1, gameObject.transform.position + direction);
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
        lineRend.enabled = true;
        isCharging = true;
        print("starting charge");
        GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(flashInterval);
        GetComponent<MeshRenderer>().material.color = originalColor;
        yield return new WaitForSeconds(flashInterval);
        GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(flashInterval);
        GetComponent<MeshRenderer>().material.color = originalColor;
        yield return new WaitForSeconds(flashInterval);
        GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(flashInterval * 2f);
        GetComponent<MeshRenderer>().material.color = Color.yellow;
        isCharging = false;
        lineRend.enabled = false;
        print("finishing charge");
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!isAggro )
        {
            StartCoroutine(aggro());
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (isAttacking && collision.collider.GetType() == typeof(CapsuleCollider))
        {   Vector3 position = collision.transform.position; 
        
            Destroy(collision.gameObject);
              int randomIndex = Random.Range(0, objs.Length);
        GameObject selectedObj = objs[randomIndex];
        selectedObj.SetActive(true);
        selectedObj.transform.position = position; 
  
        }
    }
}