using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class MobBrain : MobBase
{
    public List<DroppedItem> itemDrops;
    public float movementSpeed = 20f;
    public float rotationSpeed = 100f;

    private Transform attacker;
    public bool startFollowing = false;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isWalking = false;
    private bool isRotating = false;
    private bool isAttacking = false;

    private float followSpeed = .1f;

    public MobAnimation anim;

    Rigidbody rb;
    private void Start()
    {
        base.brainChild = this;
        rb = GetComponent<Rigidbody>();
        isRotatingLeft = Random.Range(1, 2) == 1 ? true : false;
    }

    private void Update()
    {
        if(!isWandering)
        {
            StartCoroutine(Wander());
        }

        if (isRotatingLeft && !isRotating && attacker == null)
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
        else if(!isRotatingLeft && !isRotating && attacker == null)
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);

        if(isWalking && attacker == null)
            rb.AddForce(transform.forward * movementSpeed);

        if (startFollowing && attacker != null)
            FollowAttacker();

        if(isTargetted)
            UIManager.Instance.ShowEnemyBar(mobName, NumToPercent(health, maxHealth, minHealth));
    }

    IEnumerator Wander()
    {
        if (startFollowing || isAttacking)
            yield break;
        int rotationTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 3);
        int rotateDirection = Random.Range(1, 2);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 3);

        isWandering = true;
        yield return new WaitForSeconds(walkWait);

        isWalking = true;
        isRotating = true;
        yield return new WaitForSeconds(walkTime);

        isWalking = false;
        yield return new WaitForSeconds(rotateWait);

        isRotatingLeft = rotateDirection == 1 ? true : false;
        isRotating = false;
        yield return new WaitForSeconds(rotationTime);


        isWandering = false;
    }

    public void StartFollowingAttacker(Transform _attacker)
    {
        attacker = _attacker;
        startFollowing = true;
    }

    private void FollowAttacker()
    {
        Vector3 _attackerPos = new Vector3(attacker.position.x, transform.position.y, attacker.position.z);
        transform.LookAt(_attackerPos);
        
        if (Vector3.Distance(transform.position, attacker.transform.position) > 5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, attacker.transform.position, followSpeed);
        }
        else if(!isAttacking)
        {
            isAttacking = true;
            startFollowing = false;
            Invoke(nameof(Attack), attackDelay);
        }
    }

    public override void Attack()
    {
        isAttacking = false;
        startFollowing = true;
        anim.Attack();
    }

    public void SpawnItem()
    {
        int _itemCount = Random.Range(0, itemDrops.Count);
        Debug.Log(_itemCount);
        for(int i = 0; i < _itemCount; i++)
        {
            int _itemIndex = Random.Range(0, itemDrops.Count);
            DroppedItem _droppedItem = Instantiate(itemDrops[_itemIndex]);
            _droppedItem.transform.position = transform.position;
        }
    }
}
