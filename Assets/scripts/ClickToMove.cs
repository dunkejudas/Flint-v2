using UnityEngine;
using System.Collections;

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
    public bool _isAlive;
    public bool _isRotating = false;

    private Vector3 _currentMoveTarget;
    public ParticleSystem _flintSparkSystem;

        public bool _isAwake = false;

		// Use this for initialization
		void Awake () 
		{
			anim = GetComponent<Animator> ();
			navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
            this.GetComponent<Rigidbody>().isKinematic = true;
            _isAlive = true;
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
                if (_isAwake)
                {
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        if (hit.collider.CompareTag("Lantern"))
                        {
                            targetedEnemy = hit.transform;
                            _currentMoveTarget = hit.transform.position;
                            enemyClicked = true;
                        }

                        else
                        {
                            _currentMoveTarget = hit.point;
                            StartCoroutine("rotateToTarget", hit.point);


                            enemyClicked = false;

                        }
                    }
                } else if (_isAlive)
                {
                    anim.SetBool("startWakeUp", true);
                }

			}

			

			//anim.SetBool ("IsWalking", walking);
		}

		private void MoveAndShoot()
		{
			if (targetedEnemy == null)
				return;
            
			navMeshAgent.destination = targetedEnemy.position;
			if (navMeshAgent.remainingDistance >= shootDistance && !_isRotating) {

                StartCoroutine("rotateToTarget", targetedEnemy.position);
                //navMeshAgent.Resume();
                //walking = true;
            }

			if (navMeshAgent.remainingDistance <= shootDistance && navMeshAgent.pathPending == false) {

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
            _isRotating = true;
            navMeshAgent.updateRotation = false;
            Quaternion lookDirection = Quaternion.LookRotation((point - this.transform.position).normalized);
            Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));

            int loops = 0;
            float rotationSpeed = 1000.0f;

            while (true)
            {
            if (!_isAlive | point != _currentMoveTarget)
                yield break;

            if (Quaternion.Angle(lookDirection, transform.rotation) < 30f | loops++ > 100)
            {
                anim.SetBool("startWalking", true);
                walking = true;
                _isRotating = false;
                navMeshAgent.destination = point;
                navMeshAgent.updateRotation = true;
                navMeshAgent.Resume();

                yield break;
            }

                this.gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, rotationSpeed * Time.deltaTime);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

                yield return null;
            }
        }

        public void interactionFrame()
        {
            anim.SetBool("startInteracting", false);
        }

        public void characterAwoken()
        {
            _isAwake = true;
        }

        public void flintSparkTrigger()
        {
        _flintSparkSystem.Emit(15);
        }

        public void bounceCharacter()
        {
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        _isAlive = false;

        float bounceRotationSpeed = 5f;

        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Rigidbody>().detectCollisions = false;
        this.GetComponent<Rigidbody>().AddForce(new Vector3(0,205, 0));
        this.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(-bounceRotationSpeed, bounceRotationSpeed), Random.Range(-bounceRotationSpeed, bounceRotationSpeed), Random.Range(-bounceRotationSpeed, bounceRotationSpeed)));
    }

    }