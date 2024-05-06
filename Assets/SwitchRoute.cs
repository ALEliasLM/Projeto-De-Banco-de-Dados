using UnityEngine;

public class SwitchRoute : MonoBehaviour
{

    public GameObject Search, Route;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
        {
            Search.SetActive(false);
            Route.SetActive(true);
        }
    }
}
