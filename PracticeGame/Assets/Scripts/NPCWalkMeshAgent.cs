using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWalkMeshAgent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    NavMeshAgent _agent;

    [SerializeField]
    List<GameObject> _pointsToWalk;
    [SerializeField]
    Transform _fakeHead;
    [SerializeField]
    bool _hunterPlayer;

    [SerializeField]
    Transform _player;

    private int _indexOfPosition;


    void Start()
    {
        _hunterPlayer = false;
        _indexOfPosition = 0;
        _agent = gameObject.GetComponent<NavMeshAgent>();
        StartCoroutine(IntialWalk());
    }

    // Update is called once per frame
    void Update()
    {
    }


    IEnumerator IntialWalk()
    {
        yield return new WaitForSeconds(1);
        _agent.SetDestination(_pointsToWalk[_indexOfPosition].transform.position);
        //_indexOfPosition = (_indexOfPosition + 1) % 3;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals(_pointsToWalk[_indexOfPosition].name) && _hunterPlayer==false)
        {
            Debug.LogError("LLego al point " + _indexOfPosition);
            _indexOfPosition = (_indexOfPosition + 1) % 3;
            StartCoroutine(IntialWalk());
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = new Ray(_fakeHead.position, _fakeHead.forward);


        if (Physics.Raycast(ray, out hit, 1000) && _hunterPlayer == false)
        {
            Debug.DrawLine(_fakeHead.position, hit.point, Color.red);
            if (hit.transform.tag.Equals("Player"))
            {
                _player = hit.transform;
                StopCoroutine(IntialWalk());
                _hunterPlayer = true;
                StartCoroutine(GoToPlayer());

            }
        }

        


    }


    IEnumerator GoToPlayer()
    {
        while (_hunterPlayer) {
            _agent.SetDestination(_player.position);
            yield return new WaitForSeconds(0.2f);
        }
    }








}
