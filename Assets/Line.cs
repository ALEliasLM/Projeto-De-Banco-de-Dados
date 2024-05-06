using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    RectTransform pointA, pointB;
    RectTransform rt;
    bool created = false;
    IEnumerator Start()
    {

        rt = GetComponent<RectTransform>();
        rt.localEulerAngles = new Vector3(0, 0, Vector3.Angle(transform.right, (pointB.position - pointA.position)));

        var direction = new Vector2(Vector3.Distance(pointB.position, pointA.position), 5f);

        rt.sizeDelta = new Vector2(0, 5);

        float t = 0;
        while (true)
        {
            t += Time.deltaTime;
            var perc = Mathf.Clamp((t / .75f), 0f, 1f);

            if (perc >= 1)
            {
                rt.sizeDelta = direction;
                created = true;
                yield break;
            }

            rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, direction, perc);
            yield return null;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!created) return;
        rt.sizeDelta = new Vector2(Vector3.Distance(pointB.position, pointA.position), 5);
        rt.localEulerAngles = new Vector3(0, 0, Vector3.Angle(transform.right, (pointB.position - pointA.position)));
    }
}
