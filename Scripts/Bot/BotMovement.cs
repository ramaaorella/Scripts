
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class BotMovement : NetworkBehaviour
{
    [SerializeField] private List<Vector3> waypoints;

    NavMeshAgent agent;
    Animator animator;

    [SyncVar] private bool startMovement = false;

    private string pathName;
    private List<Vector3> pathFromOfficeToConferenceRoom = new List<Vector3>() {
        new Vector3(-4.38f, 0, 16.257f),                // Sala Oficina IN
        new Vector3(-4.38f, 0, 15.32f),                 // Sala Oficina OUT
        new Vector3(-10.22f, 0, 18.24f),                // Teleport Oficina OUT
        new Vector3(-10.22f, 0, 15.61f),                // Teleport Oficina IN
        new Vector3(-10.22f, 0, 11.65f),                // Teleport Conferencias IN
        new Vector3(-10.22f, 0, 9.31f),                 // Teleport Conferencias OUT
        new Vector3(5.03f, 0, 14.97f),                  // Sala Conferencias OUT
        new Vector3(5.03f, 0, 16.16f),                  // Sala Conferencias IN
        new Vector3(3.62f, 0, 24.54f)                   // Posicion en Sala Conferencias
    };

    private List<Vector3> pathFromConferenceRoomToOffice = new List<Vector3>() {
        new Vector3(5.03f, 0, 16.16f),                  // Sala Conferencias IN
        new Vector3(5.03f, 0, 14.97f),                  // Sala Conferencias OUT
        new Vector3(-10.22f, 0, 9.31f),                 // Teleport Conferencias OUT
        new Vector3(-10.22f, 0, 11.65f),                // Teleport Conferencias IN
        new Vector3(-10.22f, 0, 15.61f),                // Teleport Oficina IN
        new Vector3(-10.22f, 0, 18.24f),                // Teleport Oficina OUT
        new Vector3(-4.38f, 0, 15.32f),                 // Sala Oficina OUT
        new Vector3(-4.38f, 0, 16.16f),                 // Sala Oficina IN
        new Vector3(-4.373f, 0, 17.98f)                 // Silla Oficina
    };

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        waypoints = pathFromOfficeToConferenceRoom;
        pathName = "Office-ConferenceRoom";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) StartMovement();

        if (!agent.pathPending && agent.remainingDistance < 0.1f && startMovement)
        {
            GoToNextDestination(pathName);
        }

        animator.SetFloat("VelX", agent.velocity.magnitude / agent.speed);
        animator.SetFloat("VelY", agent.velocity.magnitude / agent.speed);
    }

    private IEnumerator PlayAnimation(string animationName)
    {
        Debug.LogError("animation: " + animationName + " is starting");

        if (animationName.Equals("isStartingToSit")) {
            Debug.LogError("se tiene que girar");
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, new Quaternion(0, 230f, 0, 0), 1.8f * Time.deltaTime);
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }
        
        animator.SetTrigger(animationName);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        if (animationName.Equals("isStartingToWalk"))
        {
            animator.SetTrigger("isWalking");
            startMovement = true;
        }
        else if (animationName.Equals("isStartingToSit"))
        {
            animator.SetTrigger("isSitting");
        }
    }


    public void StartMovement()
    {
        CmdSendStartMovementToServer(this.startMovement);
    }

    [Command(ignoreAuthority = true)]
    void CmdSendStartMovementToServer(bool start)
    {
        RcpStartMovement(this.startMovement);
    }

    [ClientRpc]
    void RcpStartMovement(bool startMovement)
    {
        //if(pathName.Equals("Office-ConferenceRoom"))
            StartCoroutine(PlayAnimation("isStartingToWalk"));

        /*while (destinations.Count != 0 && startMovement)
        {
            if (agent.velocity.magnitude == 0 && agent.remainingDistance <= 0.5f)
            {
                GoToNextDestination();
            }
        }
        if (destinations.Count == 0)
        {
            CmdSendStartMovementToServer(false);
        }*/
    }

    private void GoToNextDestination(string pathName)
    {
        Debug.LogError("GOGOGO");
        if (waypoints.Count == 0)
        {
            Debug.LogError("Se terminó maestro");

            startMovement = false;

            if(pathName.Equals("Office-ConferenceRoom")) 
            {
                waypoints = pathFromConferenceRoomToOffice;
                this.pathName = "ConferenceRoom-Office";
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, new Quaternion(0, -226.63f, 0, 0), 0.2f * Time.deltaTime);
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
            }
            else if(pathName.Equals("ConferenceRoom-Office"))
            {
                waypoints = pathFromOfficeToConferenceRoom;
                this.pathName = "Office-ConferenceRoom";
                StartCoroutine(PlayAnimation("isStartingToSit"));
            }   
  
            return;
        }

        agent.SetDestination(waypoints[0]);
        waypoints.RemoveAt(0);
    }
}

