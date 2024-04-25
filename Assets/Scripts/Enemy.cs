using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 min, max;
    [SerializeField] private Vector3 destination;
    [SerializeField] private bool playerDetected=false;

    private GameObject player;
    [SerializeField] private float playerDistanceDetection=30;
    [SerializeField] private float visionAngle=45;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");


        RandomDestination();

        //Iniciamos Corrutina
        StartCoroutine(Patroll());
        StartCoroutine(Alert());

    }

    private void Update()
    {
        if (playerDetected)
        {
            destination=player.transform.position;
        }
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
                Vector3 vectorPlayer= player.transform.position - transform.position;
                if (Vector3.Angle(vectorPlayer.normalized, transform.forward)<visionAngle)
                {
                    playerDetected = true;
                    StopCoroutine(Patroll());
                    GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
                }
                else
                {
                    playerDetected = false;
                    StartCoroutine(Patroll());
                }

            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Alert()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < playerDistanceDetection) //si la distancia es menor de metro y medio
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 3f));
                RandomDestination();
            }
            yield return new WaitForEndOfFrame();
        }
    }


    /*
    //Detección por trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            playerDetected = true;
            StopCoroutine(Patroll());
            transform.LookAt(other.transform);
            GetComponent<NavMeshAgent>().SetDestination(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerDetected = false;
            StartCoroutine(Patroll());
        }
    }*/
}
