using UnityEngine;

public class MatchRunnerX : MonoBehaviour
{
    public Transform runner;
    public bool useLocalPosition = false;

    private void LateUpdate()
    {
        if (runner == null)
        {
            var found = GameObject.Find("Runner");
            if (found != null) runner = found.transform;
            else return;
        }

        if (useLocalPosition)
        {
            var pos = transform.localPosition;
            pos.x = runner.localPosition.x;
            transform.localPosition = pos;
        }
        else
        {
            var pos = transform.position;
            pos.x = runner.position.x;
            transform.position = pos;
        }
    }
}
