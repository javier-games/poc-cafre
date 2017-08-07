using System.Collections.Generic;
using System.Collections;
using UnityEngine;

<<<<<<< HEAD
public class CoinFactory : MonoBehaviour {

=======
/// <summary>
/// 
/// Coin factory.
/// Container of coins that make a toss each time that
/// TossCoins is called.
///  
/// </summary>

public class CoinFactory : MonoBehaviour {

	//	Structure of coins
>>>>>>> Optimization
	[System.Serializable]
	public struct CoinPrefab{
		public GameObject prefab;
		public int	amountInBuffer;
	};

<<<<<<< HEAD
	[SerializeField]
	private float regenerationTime = 0.1f;
	[SerializeField]
	private CoinPrefab[] coinPrefab;
	private List<GameObject>[] coins;
	private List<GameObject>[] coinsTemp;

	void Awake(){

		coins = new List<GameObject>[coinPrefab.Length];
		coinsTemp = new List<GameObject>[coinPrefab.Length];

		for (int i = 0; i < coinPrefab.Length; i++) {
			coins [i] = new List<GameObject> ();
			coinsTemp [i] = new List<GameObject> ();
=======
	//	Serialize Field Variables.
	[SerializeField]
	private float regenerationTime = 0.1f;		//	Time to regenerate a coin.
	[SerializeField]
	private CoinPrefab[] coinPrefab;			//	Array of kind of prefabs.

	//	List of coins
	private List<GameObject>[] coins;			//	List of available coins.
	private List<GameObject>[] coinsTemp;		//	List of no available coins. 



	//	Initializing
	void Start(){

		//	Initializing the array of lists.
		coins 		= new List<GameObject>[coinPrefab.Length];
		coinsTemp 	= new List<GameObject>[coinPrefab.Length];

		//	Creating the coins.
		for (int i = 0; i < coinPrefab.Length; i++) {
			
			//	Initializing the list.
			coins [i] = new List<GameObject> ();
			coinsTemp [i] = new List<GameObject> ();
			//	Instantiating the prefab coin and adding to the available list of coins.
>>>>>>> Optimization
			for (int j = 0; j < coinPrefab[i].amountInBuffer; j++) {
				GameObject temp = Instantiate (coinPrefab[i].prefab, transform);
				temp.name = coinPrefab[i].prefab.name;
				coins [i].Add (temp);
			}
		}
	}

<<<<<<< HEAD
	public void TossCoins(int kind,int totalCoins,float offsetTime){
		if (kind >= 0 && kind < coinPrefab.Length && totalCoins>0){
			if (coins[kind].Count > 0) {
				StartCoroutine (PlayAnimator (offsetTime*totalCoins, coins [kind] [0].GetComponent<Animator> (),kind));
				coinsTemp [kind].Add (coins[kind][0]);
				coins [kind].RemoveAt (0);
				TossCoins (kind,totalCoins-1,offsetTime);
			}else if(kind>0){
				TossCoins (kind-1,totalCoins,offsetTime);
			}else{
				StartCoroutine (Wait(1f,kind,totalCoins,offsetTime));
=======
	//	Toss a coins.
	public void TossCoins(int kind,int amountOfCoins,float offsetTime){
		//	If the amount of coins and the kind are correct
		if (kind >= 0 && kind < coinPrefab.Length && amountOfCoins>0){
			//	If coins of the kind are available.
			if (coins[kind].Count > 0) {
				//	Start a coroutine to animate a toss.
				StartCoroutine (PlayToss (offsetTime*amountOfCoins, coins [kind] [0].GetComponent<Animator> (),kind));
				//	Make unavaileable the coin.
				coinsTemp [kind].Add (coins[kind][0]);
				coins [kind].RemoveAt (0);
				//	Make a toss for the leftover amount fo coins recursively.
				TossCoins (kind,amountOfCoins-1,offsetTime);
			}
			//	Else, if the kind of coin is not the inferior one try with an one less kind.
			else if(kind>0){
				TossCoins (kind-1,amountOfCoins,offsetTime);
			}
			//	Else wait for an available coin.
			else{
				StartCoroutine (Wait(1f,kind,amountOfCoins,offsetTime));
>>>>>>> Optimization
			}
		}
	}

<<<<<<< HEAD
	IEnumerator PlayAnimator(float timeToWait, Animator coinAnimator, int kind){
		yield return new WaitForSeconds (timeToWait);
		coinAnimator.SetTrigger ("Jump");
		StartCoroutine (GetBack(regenerationTime,kind));
	}
	IEnumerator Wait(float timeToWait,int kind,int totalCoins,float offsetTime){
		yield return new WaitForSeconds (timeToWait);
		TossCoins (kind,totalCoins-1,offsetTime);
	}
	IEnumerator GetBack (float timeToWait, int kind)
	{
=======
	//	Playing animation
	IEnumerator PlayToss(float timeToWait, Animator coinAnimator, int kind){
		yield return new WaitForSeconds (timeToWait);
		coinAnimator.SetTrigger ("Jump");
		//	Start coroutine to return available the coin.
		StartCoroutine (GetBack(regenerationTime,kind));
	}
	//	Wait for a while.
	IEnumerator Wait(float timeToWait,int kind,int totalCoins,float offsetTime){
		yield return new WaitForSeconds (timeToWait);
		//	Re-call the Toss Method.
		TossCoins (kind,totalCoins-1,offsetTime);
	}
	//	Return the coin to the available list.
	IEnumerator GetBack (float timeToWait, int kind){
>>>>>>> Optimization
		yield return new WaitForSeconds (timeToWait);
		coins [kind].Add (coinsTemp [kind] [0]);
		coinsTemp [kind].RemoveAt (0);
	}
}