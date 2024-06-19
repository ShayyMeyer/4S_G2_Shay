using System.Collections;
using UnityEngine;

public class LineOfSightCheck : MonoBehaviour
{
    [SerializeField] private LayerMask layersTopCover;
    [SerializeField] private SimpleEnemyBehaviour seb;
    [SerializeField] private float viewAngle = 60f;
   
    private GameObject Target;
    private Coroutine detectPlayerCoroutine;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Target = other.gameObject;
            Debug.Log(message:"Player entered Awerenessarea");
            detectPlayerCoroutine = StartCoroutine(routine: DetectPlayer());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Target = null;
            Debug.Log(message:"Player exit area");
            StopCoroutine(detectPlayerCoroutine);
            seb.hasTarget = false;
        }
    }
    
    IEnumerator DetectPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log(message:"Detect Player...");
         
            float distanceToPlayer = Vector3.Distance(this.transform.position, Target.transform.position);
            Vector3 directionToPlayer = Target.transform.position - this.transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
         
            bool isCovered = IsPlayerCovered(directionToPlayer, distanceToPlayer);
            Debug.Log("Player Covered?" + isCovered);
            if (!isCovered && angleToPlayer < viewAngle)
            {
                seb.hasTarget = true;
            }
            else
            {
                seb.hasTarget = false;
            }
        }
      
    }
    
    bool IsPlayerCovered(Vector3 direction, float distanceToTarget)
    {
        RaycastHit[] hits = Physics.RaycastAll(this.transform.position, direction, distanceToTarget, layersTopCover);

        foreach (RaycastHit hit in hits)
        {
            Debug.DrawRay(this.transform.position, direction.normalized * distanceToTarget, Color.green, 0.5f);
            return true;
        }

        Debug.DrawRay(this.transform.position, direction.normalized * distanceToTarget, Color.red, 0.5f);
        return false;

    }
}
