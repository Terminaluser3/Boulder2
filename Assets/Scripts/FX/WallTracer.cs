using Unity.VisualScripting;
using UnityEngine;

public class WallTracer : MonoBehaviour
{
    [Tooltip("Objects with trail.")]
    [SerializeField] private Transform[] tracerObjects;
    [Tooltip("Corner definers.")]
    [SerializeField] private Transform[] waypoints;
    [Tooltip("Train speed.")]
    [SerializeField] private float speed = 10f;

    private int[] currentWPIndexArr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (waypoints.Length == 0 || tracerObjects.Length == 0) return;

        // init waypoint tracking
        currentWPIndexArr = new int[tracerObjects.Length];

        for (int i = 0; i < tracerObjects.Length; i++)
        {
            //spread out
            int startWP = i % waypoints.Length;
            tracerObjects[i].position = waypoints[startWP].position;
            currentWPIndexArr[i] = (startWP + 1) % waypoints.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Length == 0 || tracerObjects == null) return;

        for (int i = 0; i < tracerObjects.Length; i++)
        {
            Vector3 targetPos = waypoints[currentWPIndexArr[i]].position;
            tracerObjects[i].position = Vector3.MoveTowards(tracerObjects[i].position, targetPos, speed * Time.deltaTime);

            // next wp
            if (Vector3.Distance(tracerObjects[i].position, targetPos) < 0.1f)
            {
                currentWPIndexArr[i] = (currentWPIndexArr[i] + 1) % waypoints.Length;
            }
        }
    }
}
