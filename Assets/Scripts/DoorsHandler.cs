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
    //door A move -90 deg y
    //door B move +90 deg y
    //disable collider
    //
    public int GetPrice()
    {
        return price;
    }
}
