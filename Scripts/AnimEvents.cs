using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
	public PlayerEvent _playerEvent;

	void TakeStep()
	{
		if(GameState._isInside)
			_playerEvent.PlayerStep(new PlayerEvent.PlayerStepEventArgs { });
		else
			_playerEvent.PlayerStepInside(new PlayerEvent.PlayerStepInsideEventArgs { });
	}

}