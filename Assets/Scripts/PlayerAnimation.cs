using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private string idletowalk_param = "param_idletowalk";
    private string toidle_param = "param_toidle";
    private string idletorunning_param = "param_idletorunning";
    private string idletohittwo_param = "param_idletohit02";
    public Transform legLeft;
    public GameObject skillTwoTrail;
    public GameObject skillTwoGlobe;
    public GameObject skillTwoParticle;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetHitTwo()
    {
        animator.SetTrigger(idletohittwo_param);
    }

    public void SetRunning(bool _state)
    {
        animator.SetBool(idletorunning_param, _state);
    }

    public void SetIdle(bool _state)
    {
        animator.SetBool(toidle_param, _state);
    }

    public void SetWalking(bool _state)
    {
        animator.SetBool(idletowalk_param, _state);
    }
    private void Update()
    {
        
    }
    public void ReturnToIdle()
    {
        animator.SetBool(idletowalk_param, false);
        animator.SetBool(idletorunning_param, false);
        animator.SetBool(idletohittwo_param, false);
        animator.SetBool(toidle_param, true);
    }
}
