using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float impactForce;

    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;


    [SerializeField] private GameObject bulletImpactFX;


    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisabled;

    protected virtual void Awake()
    {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void BulletSetup(float flyDistance = 100, float impactForce = 100)
    {
        this.impactForce = impactForce;

        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.Clear();
        trailRenderer.time = .25f;
        startPosition = transform.position;
        this.flyDistance = flyDistance + .5f; // .5f được cộng vào flyDistance để tính thêm độ dài phần đầu tia laser ( xem method UpdateAimVisuals() o PlayerAim.cs)
    }

    protected virtual void Update()
    {
        FadeTrailIfNeeded();
        DisablledBulletIfNeeded();
        ReturnToPoolIfNeeded();
    }

    protected void ReturnToPoolIfNeeded()
    {
        if (trailRenderer.time < 0)
        {
            ReturnBulletToPool();
        }
    }
    protected void DisablledBulletIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }
    protected void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
        {
            trailRenderer.time -= 2 * Time.deltaTime; // 2 là hệ số dùng để giảm trailRenderer.time theo thời gian
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        CreateImpactFx();
        ReturnBulletToPool();

        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();

        Enemy_Shield shield = collision.gameObject.GetComponent<Enemy_Shield>();

        if (shield != null)
        {
            shield.ReduceDuarbility();
            return;
        }

        if (enemy != null)
        {
            Vector3 force = rb.linearVelocity.normalized * impactForce;
            Rigidbody hitRigidbody = collision.collider.attachedRigidbody;


            enemy.GetHit();
            enemy.DeathImpact(force, collision.contacts[0].point, hitRigidbody);
        }
    }

    protected void ReturnBulletToPool() => ObjectPool.instance.ReturnObject(gameObject);

    protected void CreateImpactFx()
    {
        GameObject newImpactFx = ObjectPool.instance.GetObject(bulletImpactFX, transform);
        ObjectPool.instance.ReturnObject(newImpactFx, 1);
    }
}
