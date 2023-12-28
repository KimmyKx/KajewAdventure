using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerBase
{
    public GameObject targetPrefab;
    public Transform _transform { get; private set; }

    private float attackRadius = 15f;
    private MobBrain target;
    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        _transform = transform;
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Collider[] _colliders = Physics.OverlapSphere(_transform.position, attackRadius);
            MobBrain _enemyTarget = GetTargettedEnemy<MobBrain>(_colliders);
            GameObject _lastTarget = GameObject.FindGameObjectWithTag("Target");
            if(_lastTarget != null)
            {
                if(_lastTarget.transform.parent.gameObject != _enemyTarget?.gameObject)
                {
                    _lastTarget.transform.parent.GetComponent<MobBrain>().isTargetted = false;
                    Debug.Log("Target changed");
                }
                Destroy(_lastTarget);
            }
            if (_enemyTarget != null && _enemyTarget.isTargetted && !onBattleMode && target != null)
            {
                SkillTwo();
                AttackEnemy();
            }
            if(_enemyTarget != null && !_enemyTarget.isTargetted)
            {
                _enemyTarget.isTargetted = true;
                target = _enemyTarget;
                Instantiate(targetPrefab, _enemyTarget.transform);
            }
        }
    }

    private void SkillTwo()
    {
        playerAnimation.ReturnToIdle();
        playerAnimation.SetHitTwo();
        onBattleMode = true;
        float healthLeft = target.TakeDamage(skillTwoDamage);
        if (healthLeft <= target.minHealth)
        {
            target.isTargetted = false;
            target = null;
            UIManager.Instance.HideEnemyBar();
        }
        GameObject _trail = Instantiate(playerAnimation.skillTwoTrail, playerAnimation.legLeft);
        GameObject _globe = Instantiate(playerAnimation.skillTwoGlobe, playerAnimation.legLeft);
        GameObject _particle = Instantiate(playerAnimation.skillTwoParticle, playerAnimation.legLeft);
        StartCoroutine(DestroyTrail(_trail, _globe, _particle));
        StartCoroutine(toIdleDelayed(skillTwoDelay));
    }

    private IEnumerator DestroyTrail(GameObject _trail, GameObject _globe, GameObject _particle)
    {
        yield return new WaitForSeconds(3);
        Destroy(_trail);
        Destroy(_globe);
        Destroy(_particle);
    }

    private IEnumerator toIdleDelayed(float _time)
    {
        yield return new WaitForSeconds(_time);
        onBattleMode = false;
    }

    private void AttackEnemy()
    {
        target?.StartFollowingAttacker(transform);
    }

    T GetTargettedEnemy<T>(Collider[] _colliders)
    {
        T _result = default;
        List<EnemyTarget<T>> _targets = new();
        foreach(Collider _collider in _colliders)
        {
            T _target = _collider.gameObject.GetComponent<T>();
            if(_target != null)
            {
                float _distance = Vector3.Distance(_collider.transform.position, _transform.position);
                _targets.Add( new() { Component = _target, Distance = _distance } );
            }
        }
        _targets.Sort(delegate (EnemyTarget<T> a, EnemyTarget<T> b)
        {
            if (a.Distance < b.Distance)
                return -1;
            else if (a.Distance > b.Distance)
                return 1;
            else
                return 0;
        });
        if(_targets.Count > 0)
            _result = _targets[0].Component;
        return _result;
    }

    struct EnemyTarget<T>
    {
        public T Component;
        public float Distance;
    }
}
