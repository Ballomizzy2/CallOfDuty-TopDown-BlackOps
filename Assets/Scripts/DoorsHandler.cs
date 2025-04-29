using UnityEngine;

public class DoorsHandler : MonoBehaviour
{
    [SerializeField] int price;
    [SerializeField] GameObject doorA;
    [SerializeField] GameObject doorB;
    [SerializeField] BoxCollider boxCollider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //listen for a successful purchase from GM then:
    //disable collider
    //
    public int GetPrice()
    {
        return price;
    }
    public void AnimateDoors()
    {
        boxCollider.enabled = false;
        // Instantly rotate Door A -90° around Y
        doorA.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);

        // Instantly rotate Door B +90° around Y
        doorB.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);

    }
}
