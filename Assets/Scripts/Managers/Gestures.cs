using UnityEngine;

/*
 *	This script detect gestures on screen if the current platform is android o iOS.
 */

public enum SwipeState {Null, Up, Down, Right, Left}

public class Gestures: MonoBehaviour{
	#if UNITY_IOS || UNITY_ANDROID

	//	Singleton
	public static Gestures instance;

	//	Variables for calibration.
	[SerializeField]
	private int minSwipeHeight = 100;	//	Minimum size of the swipe in Y.
	[SerializeField]
	private int minSwipeWidth = 100;	//	Minimum size of the swipe in X.


	private Touch fingerToch;			//	Structure info of a finger touching the screen.
	public SwipeState swipeState;		//	Current state of swipe.

	private Vector2 touchDirection;		//	Direction of the swipe.
	private Vector2 startTouchPosition;	//	Initial position of the swipe.

	private int wriggle = 0;

	//	Initialization for the singleton.
	void Awake(){
		instance = this;
	}
	//	Initialization
	void Start(){
		swipeState = SwipeState.Null;
	}

	public int GetWriggleCount(){
		return wriggle;
	}

	//	Reading Gestures.
	public void ReadGestures(){

		//	If there are any finger touching the screen...
		if (Input.touchCount > 0) {

			//	Get the first finger.
			fingerToch = Input.touches[0];

			//	Detect the state.
			switch (fingerToch.phase) {

			//	If the swipe began...
			case TouchPhase.Began:
				//	Record the start position.
				startTouchPosition = fingerToch.position;
				wriggle = 0;
				break;
			
			//	If the finger is moving...
			case TouchPhase.Moved:

				//	Get the direction.
				touchDirection = fingerToch.position - startTouchPosition;

				//	Defining the direction and sense of the swipe.
				if (			Vector2.Angle (Vector2.up    , touchDirection) <= 45	&& swipeState != SwipeState.Up		&& (fingerToch.position.y-startTouchPosition.y) > minSwipeHeight 	) {
					swipeState = SwipeState.Up;
					startTouchPosition = fingerToch.position;
					wriggle++;

				} else if (		Vector2.Angle (Vector2.right , touchDirection) < 45		&& swipeState != SwipeState.Right	&& (fingerToch.position.x-startTouchPosition.x) > minSwipeWidth		) {
					swipeState = SwipeState.Right;
					startTouchPosition = fingerToch.position;
					wriggle++;

				} else if (		Vector2.Angle (-Vector2.up   , touchDirection) <= 45	&& swipeState != SwipeState.Down	&& Mathf.Abs(fingerToch.position.y-startTouchPosition.y) > minSwipeHeight	) {
					swipeState = SwipeState.Down;
					startTouchPosition = fingerToch.position;
					wriggle++;

				} else if(		Vector2.Angle (-Vector2.right, touchDirection) < 45		&& swipeState != SwipeState.Left	&& Mathf.Abs(fingerToch.position.x-startTouchPosition.x) > minSwipeWidth	) {
					swipeState = SwipeState.Left;
					startTouchPosition = fingerToch.position;
					wriggle++;
				}
				//	Make the signal Swipe active just for one frame.
				else if(swipeState == SwipeState.Up){
					swipeState = SwipeState.Null;
				} else if(swipeState == SwipeState.Right){
					swipeState = SwipeState.Null;
				} else if(swipeState == SwipeState.Down){
					swipeState = SwipeState.Null;
				} else if(swipeState == SwipeState.Left){
					swipeState = SwipeState.Null;
				}
				break;

			//	If the finger is ended...
			case TouchPhase.Ended:
				//	change to null :P
				swipeState = SwipeState.Null;
				wriggle = 0;
				break;
			}
		}
	}
	#endif
}