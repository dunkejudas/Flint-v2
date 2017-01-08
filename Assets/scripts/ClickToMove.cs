using UnityEngine;
using System.Collections;

namespace CompleteProject
{

	public class ClickToMove : MonoBehaviour {

		public float shootDistance = .01f;
		public float shootRate = .5f;
		//public PlayerShooting shootingScript;

		private Animator anim;
		private UnityEngine.AI.NavMeshAgent navMeshAgent;
		private Transform targetedEnemy;
		private Ray shootRay;
		private RaycastHit shootHit;
		private bool walking;
		private bool enemyClicked;
		private float nextFire;

		// Use this for initialization
		void Awake () 
		{
			anim = GetComponent<Animator> ();
			navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		}

		// Update is called once per frame
		void Update () 
		{
            if (enemyClicked)
            {
                MoveAndShoot();
            }

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || Mathf.Abs(navMeshAgent.velocity.sqrMagnitude) < float.Epsilon)
                    walking = false;
            }
            else
            {
                walking = true;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Input.GetButtonDown("Fire1")) 
			{
				if (Physics.Raycast(ray, out hit, 100))
				{
					if (hit.collider.CompareTag("Lantern"))
					{
						targetedEnemy = hit.transform;
						enemyClicked = true;
					}

					else
					{
						walking = true;
						enemyClicked = false;
						navMeshAgent.destination = hit.point;
						navMeshAgent.Resume();
					}
				}
			}

			

			//anim.SetBool ("IsWalking", walking);
		}

		private void MoveAndShoot()
		{
			if (targetedEnemy == null)
				return;
            
			navMeshAgent.destination = targetedEnemy.position;
			if (navMeshAgent.remainingDistance >= shootDistance) {

				navMeshAgent.Resume();
				walking = true;
			}

			if (navMeshAgent.remainingDistance <= shootDistance && navMeshAgent.pathPending == false) {
				transform.LookAt(targetedEnemy);
				Vector3 dirToShoot = targetedEnemy.transform.position - transform.position;
				if (Time.time > nextFire)
				{
					nextFire = Time.time + shootRate;
                    targetedEnemy.GetComponent<lanternMain>().activate();
				}
				navMeshAgent.Stop();
				walking = false;
                enemyClicked = false;
            }
		}

	}

}