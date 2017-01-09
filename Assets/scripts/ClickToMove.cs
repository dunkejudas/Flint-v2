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
                    anim.SetBool("startWalking", false);
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
                        StartCoroutine("rotateToTarget", hit.point);


						enemyClicked = false;

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

                StartCoroutine("rotateToTarget", targetedEnemy.position);
                //navMeshAgent.Resume();
                //walking = true;
            }

			if (navMeshAgent.remainingDistance <= shootDistance && navMeshAgent.pathPending == false) {
				transform.LookAt(targetedEnemy);
				Vector3 dirToShoot = targetedEnemy.transform.position - transform.position;
				if (Time.time > nextFire)
				{
					nextFire = Time.time + shootRate;
                    targetedEnemy.GetComponent<lanternMain>().activate();
                    anim.SetBool("startWalking", false);    
                    anim.SetBool("startInteracting", true);
				}
				navMeshAgent.Stop();
				walking = false;
                enemyClicked = false;
            }
		}

        public IEnumerator rotateToTarget(Vector3 point)
        {
            navMeshAgent.updateRotation = false;
            Quaternion lookDirection = Quaternion.LookRotation((point - this.transform.position).normalized);
            Quaternion.EulerAngles(new Vector3(0, transform.eulerAngles.y, 0));

            int loops = 0;

            while (true)
            {

                this.gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, 30.0f);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                if (Quaternion.Angle(lookDirection,transform.rotation) < 30f | loops++ > 100)
                {
                    anim.SetBool("startWalking", true);
                    walking = true;
                    navMeshAgent.destination = point;
                    navMeshAgent.updateRotation = true;
                    navMeshAgent.Resume();

                    yield break;
                }
                yield return null;
            }
        }

        public void interactionFrame()
        {
            anim.SetBool("startInteracting", false);
        }

    }

}