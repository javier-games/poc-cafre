using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeState {Null, Up, Down, Right, Left}

public class Gestures : MonoBehaviour {

	[SerializeField]
	private int minSwipeHeight = 100;
	[SerializeField]
	private int minSwipeWidth = 100;


	private Touch fingerToch;
	public SwipeState swipeState;

	private Vector2 touchDirection;
	private Vector2 startTouchPosition;

	void Start(){
		swipeState = SwipeState.Null;
	}


	public void ReadGestures(){

		if (Input.touchCount > 0) {

			fingerToch = Input.touches[0];

			switch (fingerToch.phase) {

			case TouchPhase.Began:
				startTouchPosition = fingerToch.position;
				break;

			case TouchPhase.Moved:
				
				touchDirection = fingerToch.position - startTouchPosition;

				if (			Vector2.Angle (Vector2.up    , touchDirection) <= 45	&& swipeState != SwipeState.Up		&& (fingerToch.position.y-startTouchPosition.y) > minSwipeHeight 	) {
					swipeState = SwipeState.Up;
					startTouchPosition = fingerToch.position;

				} else if (		Vector2.Angle (Vector2.right , touchDirection) < 45		&& swipeState != SwipeState.Right	&& (fingerToch.position.x-startTouchPosition.x) > minSwipeWidth		) {
					swipeState = SwipeState.Right;
					startTouchPosition = fingerToch.position;

				} else if (		Vector2.Angle (-Vector2.up   , touchDirection) <= 45	&& swipeState != SwipeState.Down	&& Mathf.Abs(fingerToch.position.y-startTouchPosition.y) > minSwipeHeight	) {
					swipeState = SwipeState.Down;
					startTouchPosition = fingerToch.position;

				} else if(		Vector2.Angle (-Vector2.right, touchDirection) < 45		&& swipeState != SwipeState.Left	&& Mathf.Abs(fingerToch.position.x-startTouchPosition.x) > minSwipeWidth	) {
					swipeState = SwipeState.Left;
					startTouchPosition = fingerToch.position;

				} else if(swipeState == SwipeState.Up){
					swipeState = SwipeState.Null;
				} else if(swipeState == SwipeState.Right){
					swipeState = SwipeState.Null;
				} else if(swipeState == SwipeState.Down){
					swipeState = SwipeState.Null;
				} else if(swipeState == SwipeState.Left){
					swipeState = SwipeState.Null;
				}

				break;


			case TouchPhase.Ended:
				swipeState = SwipeState.Null;
				break;


			}


		}
	}

}