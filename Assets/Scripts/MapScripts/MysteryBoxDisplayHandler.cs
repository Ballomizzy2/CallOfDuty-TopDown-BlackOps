using System.Collections;
using Unity.Hierarchy;
using UnityEngine;
/// <summary>
/// this script makes it easier to access the children of this prefab
/// when box spawns in room call enableBox
/// when box leaves room call disableBox
/// since this is parent, it can hold the animation of the warp.
/// </summary>

public class MysteryBoxDisplayHandler : MonoBehaviour
{
    [SerializeField] private GameObject toy;
    [SerializeField] private Transform snapPointBox;
    [SerializeField] private Vector3 origninalSnapPosition;//set during start
    [SerializeField] private Vector3 currentSnapPostion;//updates before disable

    [SerializeField] private GameObject box;
    [SerializeField] private MysteryBox boxScript;
    [SerializeField] private Transform snapPointFrame;

    [Header("box warp stuff")]
    [SerializeField] private float riseHeight =10f;
    [SerializeField] private float riseDuration = 1f;
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private ParticleSystem poof;

    private Vector3 savedBoxPosition;
    private Quaternion savedBoxRotation;


    private void Start()
    {
        origninalSnapPosition = snapPointBox.transform.position;
        currentSnapPostion = origninalSnapPosition;

        savedBoxPosition = box.transform.position;
        savedBoxRotation = box.transform.rotation;
    }
    public void EnableBox()
    {

        box.SetActive(true);
        toy.SetActive(false);

        boxScript.enabled = true;
        Debug.Log(Aligned());
        if (!Aligned())
        {
            
            //Vector3 angles = box.transform.eulerAngles;
            //box.transform.rotation = Quaternion.Euler(0f, box.transform.eulerAngles.y, box.transform.eulerAngles.z);
            //StartCoroutine(AnimationSetDownBox());
            boxScript.ToggleLid();
            box.transform.position = savedBoxPosition;
            box.transform.rotation = savedBoxRotation;
        }
       
       
            

        

    }
    public void DisableBox() 
    {
       
        currentSnapPostion = snapPointBox.transform.position;
        box.SetActive(false); 
        toy.SetActive(true); 
    }
    public void WarpBox()
    {
        //diable the box MysteryBoxScript
        boxScript.enabled = false;
        //play laugh/byebye
        
        //do an animation go up x seconds, then disappear

        //swap the activity of siblings
        DisableBox();
        MysteryBoxLocation.Instance.ChooseRoom();
    }
    public bool BoxStatus()
    {
        return box.activeSelf;
    }
    private bool Aligned()
    {
        return Vector3.Distance(currentSnapPostion, origninalSnapPosition) < 0.001f;
    }
    public IEnumerator AnimationBoxWarp()
    {
        boxScript.enabled=false;
        SoundMng.Instance.PlayBoxLaugh();
        //get a wrrrrrrrr noise 

        Vector3 startPosition = box.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, riseHeight, 0);

        // Move up
        float elapsed = 0f;
        while (elapsed < riseDuration)
        {
            float t = elapsed / riseDuration;
            box.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        box.transform.position = endPosition;

        // Rotate rapidly for 2 seconds
        elapsed = 0f;
        while (elapsed < spinDuration)
        {
            box.transform.Rotate(Vector3.right * 1000 * Time.deltaTime); // fast x-axis spin
            elapsed += Time.deltaTime;
            yield return null;
        }


        poof?.Play();
        //get a "pop" noise
        
        DisableBox();
        MysteryBoxLocation.Instance.ChooseRoom();
        yield return null;
    }

    public IEnumerator AnimationSetDownBox()
    {
        //idk how to se this up rn, broken.
        Vector3 startPosition = box.transform.position;
        Vector3 endPosition = snapPointBox.position;

        float elapsed = 0f;
        while (elapsed < riseDuration)
        {
            float t = elapsed / riseDuration;
            box.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        box.transform.position = endPosition; // snap to final pos
        currentSnapPostion = endPosition;

        boxScript.enabled = true;
        boxScript.ToggleLid();
    }

}
