using UnityEngine;

/// <summary>
///
/// Collision detector.
/// Set of methods related to the physics collision and collider.
///
/// </summary>

[RequireComponent(typeof(RunnerSoundFX))]
[RequireComponent(typeof(CoinFactory))]
public class CollisionDetector : MonoBehaviour {

	//	Serialize Field Variables
	[SerializeField]
	private int timesToCollide = 50;		//	Times that the veicle can collide.
	[SerializeField]
	private ParticleSystem explotion;	//	Particle system to exploit.
	[SerializeField]
	private float locationToPay = 1;
	[SerializeField]
	private bool insurances = true;

	private CoinFactory factory;		//	System to animate a toss.
	private RunnerSoundFX sound;

	private int initialTimesToCollide;

	//	Initializing
	void Start(){
		factory = GetComponent<CoinFactory> ();
		sound = GetComponent<RunnerSoundFX> ();
		initialTimesToCollide = timesToCollide;
	}

	//	Trigger Methods
	void OnTriggerEnter(Collider other){

		//	If it is a coin.
		if (other.transform.CompareTag ("Coin")) {
			//	Return the coin to the pool.
			ObjectPool.instance.PoolGameObject(other.gameObject);
			//	TODO Increase the amount of Money.
			//	Make a toss.
			if(other.transform.GetComponent<ID>().GetID() == "Silver Coin")
				factory.TossCoins (0,1,0.1f);
			else
				factory.TossCoins (1,1,0.1f);
		}
	}

	//	Collision Methods
	void OnCollisionEnter(Collision other){

		//	Is it its a Vehicle.
		if (other.transform.CompareTag ("Vehicle")) {
			//	If it strikes behind.
			if (transform.InverseTransformPoint (other.transform.position).z > locationToPay || !insurances) {
				timesToCollide--;
				HUDManager.instance.UpdateCrashesAmount ((timesToCollide*1.0f)/(initialTimesToCollide*1f));
				//	If it collide to the limit.
				if (timesToCollide <= 0) {
					//	Stop tracking the path.
					GetComponent<RunnerController> ().StopTrackingPath ();
					// Play Explotion FX.
					explotion.Play ();
					sound.ExplotionFX ();
					GameManager.instance.ChangeToNewState (GameState.GAME_OVER);
				}
			}
			// Else it crashes across.
			else {
				//	 Make a toss
				// TODO Add Money to the bag.
				factory.TossCoins (1,1,0.1f);
				//	Add a foce to impulse the other car.
				//other.transform.GetComponent<Rigidbody> ().AddForce(-other.contacts[0].normal*10000f,ForceMode.Impulse);
			}
		}
	}
}
