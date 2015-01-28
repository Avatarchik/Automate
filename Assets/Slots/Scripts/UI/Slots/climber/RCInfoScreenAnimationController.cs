using UnityEngine;
using System.Collections;

public class RCInfoScreenAnimationController : MonoBehaviour
{
    public float snowingSpeed;
    public GameObject snowPrefab;   
    public Transform snowParent;
    public SpriteRenderer info_screen_sprite_05;
    
    SpriteRenderer yetti_eyes_sprite;

    bool _snowing;
    bool createSnow;

    void OnEnable()
    {
        StartSnowing();
    }

    void OnDisable()
    {
        StopSnowing();
    }

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        YettiEyes.enabled = info_screen_sprite_05.enabled;

        for (int i = 0; i < snowParent.transform.childCount; i++)
        {
            snowParent.transform.GetChild(i).Translate(0, -snowingSpeed * Time.deltaTime, 0);
        }

	}

    SpriteRenderer YettiEyes
    {
        get {
            if (yetti_eyes_sprite == null)
            {
                 yetti_eyes_sprite = info_screen_sprite_05.gameObject.transform.FindChild("YettiEyes").gameObject.GetComponent<SpriteRenderer>();
                 return yetti_eyes_sprite;
            }
            else
                return yetti_eyes_sprite;
            ;}
    }


    void StartSnowing()
    {
        createSnow = true;
        _snowing = true;

       StartCoroutine("StartSingleSnow");      
    }

    void StopSnowing()
    {
        _snowing = false;
        createSnow = false;
        
    }

    IEnumerator StartSingleSnow()
    {
        while (createSnow)
        {
            float scaleFactor = UnityEngine.Random.Range(0.5f, 1.5f);

            float posX = UnityEngine.Random.Range(-3.0f, 3.0f);
            float posY = UnityEngine.Random.Range(2.5f, 3.0f);

            GameObject snow = Instantiate(snowPrefab) as GameObject;
            snow.transform.parent = snowParent.transform;
            snow.transform.localPosition = new Vector3(posX, posY, -0.5f);
            snow.transform.localScale = scaleFactor * snow.transform.localScale;

            Destroy(snow, 12.0f);

            yield return new WaitForSeconds(2.0f);
        }
    }


}
