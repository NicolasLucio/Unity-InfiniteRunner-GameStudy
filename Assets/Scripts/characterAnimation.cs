using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class characterAnimation : MonoBehaviour
{
    public float animationVelocity = 1.0f;    
    private Rigidbody characterBody;
    private Vector3 characterQuat;
    private Vector3 characterPosition;
    public bool onFloor;
    private bool dead;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI deateText;
    private int timerInSeconds;

    public PostProcessVolume characterPPVolume;
    
    void Start()
    {        
        characterBody = GetComponent<Rigidbody>();
        StartCoroutine(MatchScoreTimer());
        timerInSeconds = 0;
        Cursor.visible = false;
        dead = false;
        characterPPVolume.enabled = false;         
    }
    
    void Update()
    {
        //Check rotation
        characterQuat = this.transform.eulerAngles;
        if (characterQuat.z > 10.5f)
        {
            characterQuat.z = 10.0f;
        }
        else if (characterQuat.z < -10.5f)
        {
            characterQuat.z = -10.0f;
        }        
        this.transform.eulerAngles = characterQuat;

        //Check Game Over
        if (characterPosition.y > 8.0f || characterPosition.y < -4.0f)
        {
            DeathScene();
        }
    }

    void FixedUpdate()
    {
        characterPosition = this.transform.position;

        //Side Movement
        if (Input.GetKey(KeyCode.A))
        {
            if (onFloor == true)
            {
                characterBody.AddForce(-0.5f, 0.0f, 0.0f, ForceMode.Impulse);
            }
            else if (onFloor == false)
            {
                characterBody.AddForce(-0.25f, 0.0f, 0.0f, ForceMode.Impulse);
            }            
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (onFloor == true)
            {
                characterBody.AddForce(0.5f, 0.0f, 0.0f, ForceMode.Impulse);
            }
            else if (onFloor == false)
            {
                characterBody.AddForce(0.25f, 0.0f, 0.0f, ForceMode.Impulse);
            }            
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onFloor == true)
            {
                onFloor = false;
                characterBody.AddForce(0.0f, 7.0f, 0.0f, ForceMode.Impulse);
            }            
        }        

        //Foward
        if (Input.GetKey(KeyCode.W) && characterPosition.z <= -7.0f)
        {
            if (onFloor == true)
            {
                characterBody.AddForce(0.0f, 0.0f, 0.5f, ForceMode.Impulse);
            }
            else if (onFloor == false)
            {
                characterBody.AddForce(0.0f, 0.0f, 0.25f, ForceMode.Impulse);
            }
        }
        else if (Input.GetKey(KeyCode.S) && characterPosition.z >= -8.0f)
        {
            if (onFloor == true)
            {
                characterBody.AddForce(0.0f, 0.0f, -0.5f, ForceMode.Impulse);
            }
            else if (onFloor == false)
            {
                characterBody.AddForce(0.0f, 0.0f, -0.25f, ForceMode.Impulse);
            }
        }

        //Reset Velocity
        if (onFloor == true && !Input.anyKey)
        {
            characterBody.velocity = Vector3.zero;
            characterBody.angularVelocity = Vector3.zero;
        }
    }

    void DeathScene()
    {
        if (dead == false)
        {
            StopAllCoroutines();
            deateText.text = "Wasted";
            characterPPVolume.enabled = true;
            StartCoroutine(PosDeath());
            dead = true;
        }        
    }

    void OnCollisionEnter (Collision info)
    {
        if (info.gameObject.name == "Floor")
        {
            onFloor = true;
        }        

        if (info.gameObject.tag == "Enemy")
        {
            characterBody.AddExplosionForce(1000.0f, new Vector3 (characterPosition.x, characterPosition.y, characterPosition.z + 1) , 10.0f, 1000.0f, ForceMode.Impulse);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            onFloor = false;
        }
    }

    IEnumerator MatchScoreTimer()
    {
        scoreText.text = "Score: 0";
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            timerInSeconds++;
            scoreText.text = "Score: " + timerInSeconds.ToString();
        }
    }

    IEnumerator PosDeath()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Game");
    }
}
