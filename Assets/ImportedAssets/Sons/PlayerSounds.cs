using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSounds : MonoBehaviour
{
    // Use this for initialization
    CharacterController characterController;
    public AudioClip walkSound;
    public AudioClip jumpSound;
    public AudioClip fallSound;

    bool onGround;

    float timer;

    [SerializeField] float intervaloTempoEntreSons;
    [SerializeField] float volume = 1f;
    void Start()
    {
        characterController = GetComponent<CharacterController>();


        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //Debug.Log(characterController.velocity.magnitude);

        if (characterController.isGrounded == true && characterController.velocity.magnitude > 0f && timer >= intervaloTempoEntreSons /*&& this.GetComponent<AudioSource>().isPlaying == false*/)
        {
            this.GetComponent<AudioSource>().PlayOneShot(walkSound, volume);
            timer = 0f;
        }
        if(characterController.isGrounded == false && characterController.velocity.magnitude > 0f)
        {
            onGround = true;      
        }
        if(onGround && characterController.velocity.magnitude == 0f /*&& this.GetComponent<AudioSource>().isPlaying == false*/)
        {    
            this.GetComponent<AudioSource>().PlayOneShot(fallSound, volume);
            onGround = false;
        }
        
    }
}
