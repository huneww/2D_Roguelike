using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxcollider;
    private Rigidbody2D rigid;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxcollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxcollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        // 오브젝트의 현재 위치에서 이동 위치간의 차이를 구한다.
        // Vector3.Distance 보다 sqrMagnitude가 연산 속도가 빠르다.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        // float형이 0이 아닌 가질수있는 가장 작은 수
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rigid.position, end, inverseMoveTime * Time.deltaTime);
            rigid.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected abstract void OnCantMove<T>(T component) where T : Component;

}
