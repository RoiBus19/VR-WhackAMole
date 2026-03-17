using UnityEngine;
public class Worm : MonoBehaviour
{
   public Transform upPoint;
   public Transform downPoint;
   public float moveSpeed = 2f;
   public float waitAtTop = 0.5f;
   public float waitAtBottom = 0.7f;
   private bool activeForRound = false;
   private bool movingUp = true;
   private float waitTimer = 0f;
   private bool waiting = false;
   void Start()
   {
       if (downPoint != null)
           transform.position = downPoint.position;
   }
   void Update()
   {
       if (!activeForRound || upPoint == null || downPoint == null) return;
       if (waiting)
       {
           waitTimer -= Time.deltaTime;
           if (waitTimer <= 0f)
           {
               waiting = false;
               movingUp = !movingUp;
           }
           return;
       }
       Transform target = movingUp ? upPoint : downPoint;
       transform.position = Vector3.MoveTowards(
           transform.position,
           target.position,
           moveSpeed * Time.deltaTime
       );
       if (Vector3.Distance(transform.position, target.position) < 0.001f)
       {
           waiting = true;
           waitTimer = movingUp ? waitAtTop : waitAtBottom;
       }
   }
   public void SetActiveForRound(bool active)
   {
       activeForRound = active;
       waiting = false;
       movingUp = true;
       if (!active && downPoint != null)
       {
           transform.position = downPoint.position;
       }
   }
   public void OnHit()
   {
       if (!activeForRound) return;
       GameManager gm = FindFirstObjectByType<GameManager>();
       if (gm == null || gm.State != GameManager.GameState.Running) return;
       gm.RegisterHit();
       activeForRound = false;
       waiting = false;
       if (downPoint != null)
           transform.position = downPoint.position;
       Invoke(nameof(Respawn), 0.8f);
   }
   private void Respawn()
   {
       GameManager gm = FindFirstObjectByType<GameManager>();
       if (gm != null && gm.State == GameManager.GameState.Running)
       {
           activeForRound = true;
           movingUp = true;
           waiting = false;
       }
   }
}