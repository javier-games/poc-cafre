using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CoinFactory : MonoBehaviour {

	[System.Serializable]
	public struct CoinPrefab{
		public GameObject prefab;
		public int	amountInBuffer;
	};

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
			for (int j = 0; j < coinPrefab[i].amountInBuffer; j++) {
				GameObject temp = Instantiate (coinPrefab[i].prefab, transform);
				temp.name = coinPrefab[i].prefab.name;
				coins [i].Add (temp);
			}
		}
	}

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
			}
		}
	}

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
		yield return new WaitForSeconds (timeToWait);
		coins [kind].Add (coinsTemp [kind] [0]);
		coinsTemp [kind].RemoveAt (0);
	}
}