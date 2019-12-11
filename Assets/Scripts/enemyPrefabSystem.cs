using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPrefabSystem : MonoBehaviour
{
    public GameObject spawnSystemObj;
    public spawnSystem spawnSystemScript;
    public bool onFloor;
    private Rigidbody enemyRigidbody;
    public Vector3 thisObjectPosition; 
    void Start()
    {
        spawnSystemObj = GameObject.Find("Spawn");
        spawnSystemScript = spawnSystemObj.GetComponent<spawnSystem>();
        enemyRigidbody = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        thisObjectPosition = this.transform.position;
        if (thisObjectPosition.y < -4.0f)
        {
            GameObject auxGameObject = this.gameObject;
            Destroy(auxGameObject);
        }
        if (onFloor == true)
        {
            enemyRigidbody.AddForce(new Vector3(0.0f, 0.0f, -(spawnSystemScript.enemyVelocityVector)), ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision info)
    {
        if (info.gameObject.name == "Floor")
        {
            onFloor = true;            
        }       
    }
}
