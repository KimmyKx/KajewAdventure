using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAnimation : MonoBehaviour
{
    public MobBrain brain;
    public SkinnedMeshRenderer mobRenderer;
    MeshRenderer mesh;
    Animator animator;
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Die()
    {
        animator.Play("Die");
        Invoke(nameof(Disappear), 2);
    }

    private void Disappear()
    {
        float _a = 255;
        Material _mat = mobRenderer.materials[0] ?? default;
        Color _color = _mat.color;
        if (_mat == default)
            return;
        while(_a > 0)
        {
            _a -= 2 * Time.deltaTime;
            _color.a = _a;
            _mat.color = _color;
        }
        Destroy(transform.parent.gameObject);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void StartFading()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float _a = 1;
        float fadeRate = .1f;
        while(_a > 0)
        {
            _a -= fadeRate * Time.deltaTime;
            mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, _a);
            yield return null;
        }
        yield return null;
        Destroy(transform.parent.gameObject);
    }
}
