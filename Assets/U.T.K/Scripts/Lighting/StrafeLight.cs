using UnityEngine;
using System.Collections;

public class StrafeLight : MonoBehaviour {

    public Color lightColor;

    public float strafeSpeed = 2;

    public float divisionsPerMeter = 2;

    public GameObject start, stop;

    GameObject lightObject;

    Light lightSource;

	// Use this for initialization
	void Start () {
        lightObject = new GameObject("Light");
        lightObject.transform.parent = transform;
        lightObject.transform.position = start.transform.position;
        lightSource = lightObject.AddComponent<Light>();

        lightSource.type = LightType.Point;
        lightSource.shadows = LightShadows.Soft;
        lightSource.range = 2;
    }


    float currentTime = 0;
    public int currentTick = 0;
	// Update is called once per frame
	void Update () {

        lightSource.color = lightColor;

        float distance = Vector3.Distance(stop.transform.position, start.transform.position);

        int tickMarks = Mathf.CeilToInt(distance * divisionsPerMeter);

        float totalTime = distance / strafeSpeed;

        currentTick = Mathf.RoundToInt((currentTime / totalTime) * (tickMarks));

        if(currentTime > totalTime)
        {
            currentTime = 0;
        }

        lightObject.transform.position = Vector3.Lerp(start.transform.position, stop.transform.position, (currentTick / (float)tickMarks));

        currentTime += Time.deltaTime;
    }
}
