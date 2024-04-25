using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 min, max;
    [SerializeField] private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        RandomDestination();

        //Iniciamos Corrutina
        StartCoroutine(Patroll());

    }

    private void RandomDestination()
    {
        destination = new Vector3(Random.Range(min.x, max.x), 0, Random.Range(max.z, min.z));
        GetComponent<NavMeshAgent>().SetDestination(destination);
    }
    IEnumerator Patroll()
    {
        while (true)
        {
            if(Vector3.Distance(transform.position,destination) < 1.5f) //si la distancia es menor de metro y medio
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 3f));
                RandomDestination();
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
