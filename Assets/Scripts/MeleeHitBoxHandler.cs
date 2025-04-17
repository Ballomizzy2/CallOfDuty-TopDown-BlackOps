using System;
using UnityEngine;

public class MeleeHitBoxHandler : MonoBehaviour
{
    public event EventHandler OnMeleeContact;
    public static MeleeHitBoxHandler Instance {  get; private set; }
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject nose;

    private void OnTriggerEnter(Collider other)
    {
        //OnMeleeContact?.Invoke(this,);
        if(other.gameObject!= player && other.gameObject != nose)
        {
            OnMeleeContact?.Invoke(this, EventArgs.Empty);
        }
            
    }
    private void Awake()
    {
        Instance = this;
    }
}
