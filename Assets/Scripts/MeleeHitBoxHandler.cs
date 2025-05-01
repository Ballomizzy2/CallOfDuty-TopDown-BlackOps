using System;
using UnityEngine;

public class MeleeHitBoxHandler : MonoBehaviour
{
    public event EventHandler<MeleeHitEventArgs> OnMeleeContact;
    private const string ZOMBIE_TAG = "Zombie";
    //custom event thing to pass gameobject data
    public class MeleeHitEventArgs : EventArgs
    {
        public GameObject hitObject;
    }
    public static MeleeHitBoxHandler Instance {  get; private set; }

    //temporary- so player does'nt damage themselves
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject nose;

    private void OnTriggerEnter(Collider other)
    {
        //detects object w/rigid bodies
        //todo- change this to only detect zombies (layers?)
        if(other.GetComponent<Collider>().CompareTag(ZOMBIE_TAG))
        {
            OnMeleeContact?.Invoke(this, new MeleeHitEventArgs { hitObject= other.gameObject});
        }
            
    }
    private void Awake()
    {
        Instance = this;
    }
}
