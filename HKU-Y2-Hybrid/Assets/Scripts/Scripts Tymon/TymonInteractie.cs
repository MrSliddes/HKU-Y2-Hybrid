using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Video;

public class TymonInteractie : MonoBehaviour
{
    [Header("0")]
    public Deur deurTrigger;
    public RuntimeAnimatorController animConIdle;
    public RuntimeAnimatorController animConWalking;
    public Animator animator;
    public Transform player;

    public Transform[] waypoints;
    private int currentWaypoint;

    [Header("Video")]
    public VideoPlayer videoPlayer;
    public VideoClip clipIntro;
    public VideoClip clipDance;

    private int state = 0;

    private bool isIntro = false;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStates();
    }

    private void UpdateStates()
    {
        switch(state)
        {
            case 0:
                // Wacht tot deur opengaat, speel dan video af
                if(!isIntro && deurTrigger.isOpen)
                {
                    StartCoroutine(Intro());
                }

                // Roteer naar speler                
                Vector3 tarDir = player.position - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, tarDir, 10 * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
                break;
            case 1:
                animator.runtimeAnimatorController = animConWalking;

                // walk to waypoint
                transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, 1 * Time.deltaTime);

                // Check next
                if(Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 0.5f)
                {
                    currentWaypoint++;

                    if(currentWaypoint == waypoints.Length)
                    {
                        state = 2;
                        return;
                    }                

                }

                // Rotation
                Vector3 tarDir1 = waypoints[currentWaypoint].transform.position - transform.position;
                Vector3 newDir1 = Vector3.RotateTowards(transform.forward, tarDir1, 2 * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir1);

                break;
            case 2:
                videoPlayer.clip = clipDance;
                videoPlayer.Play();
                videoPlayer.isLooping = true;
                animator.runtimeAnimatorController = animConIdle;
                videoPlayer.transform.GetComponent<AudioSource>().mute = true;
                break;

            default:
                break;
        }
    }

    private IEnumerator Intro()
    {
        // wacht totdat tymon is uitgepraat
        animator.runtimeAnimatorController = animConIdle;
        isIntro = true;
        videoPlayer.clip = clipIntro;
        videoPlayer.Play();

        yield return new WaitForSeconds(8f);

        state = 1;
        yield break;
    }
}