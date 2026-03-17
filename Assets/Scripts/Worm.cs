using UnityEngine;

public class Worm : MonoBehaviour
{
    [Header("Movement")]
    public Transform upPoint;
    public Transform downPoint;
    public float speed = 2f;
    public float stayUpTime = 0.4f;
    public float stayDownTime = 0.6f;

    private bool _active;
    private float _timer;
    private bool _goingUp = true;

    void Start()
    {
        // Start down
        if (downPoint) transform.position = downPoint.position;
    }

    void Update()
    {
        if (!_active) return;
        if (!upPoint || !downPoint) return;

        // simple state machine: move up -> wait -> move down -> wait
        Vector3 target = _goingUp ? upPoint.position : downPoint.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _goingUp = !_goingUp;
                _timer = _goingUp ? stayDownTime : stayUpTime;
            }
        }
    }

    public void SetActiveForRound(bool active)
    {
        _active = active;
        if (!_active && downPoint)
            transform.position = downPoint.position;

        _timer = stayDownTime;
        _goingUp = true;
    }

    public void OnHit()
    {
        // prevent farming same worm instantly
        if (!_active) return;

        _active = false;
        if (downPoint) transform.position = downPoint.position;

        // tell game manager (event-based flow)
        FindFirstObjectByType<GameManager>()?.RegisterHit();

        // respawn delay
        Invoke(nameof(Respawn), 0.5f);
    }

    private void Respawn()
    {
        // only respawn if game still running
        var gm = FindFirstObjectByType<GameManager>();
        if (gm != null && gm.State == GameManager.GameState.Running)
        {
            _active = true;
        }
    }
}
