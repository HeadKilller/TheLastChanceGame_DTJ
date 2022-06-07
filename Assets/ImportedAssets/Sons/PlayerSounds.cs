using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSounds : MonoBehaviour
{
    // Use this for initialization
    CharacterController cc;
    public AudioClip walkSound;
    public AudioClip jumpSound;
    public AudioClip fallSound;

    bool onGround;
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
       
        Debug.Log(cc.velocity.magnitude);
        if (cc.isGrounded == true && cc.velocity.magnitude > 0f && this.GetComponent<AudioSource>().isPlaying == false)
        {
            this.GetComponent<AudioSource>().PlayOneShot(walkSound,1f);
        }
        if(cc.isGrounded == false && cc.velocity.magnitude > 0f)
        {
            onGround = true;      
        }
        if(onGround && cc.velocity.magnitude == 0f && this.GetComponent<AudioSource>().isPlaying == false)
        {    
            this.GetComponent<AudioSource>().PlayOneShot(fallSound, 1f);
            onGround = false;
        }
        
    }
}
