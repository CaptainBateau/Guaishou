using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
	public PlayerEvent _playerEvent;

	void TakeStep()
	{
		if (GameState._isInside)
			_playerEvent.PlayerStepInside(new PlayerEvent.PlayerStepInsideEventArgs { });
		else if (GameState._isOnMetal)
			_playerEvent.PlayerStepMetal(new PlayerEvent.PlayerStepMetalEventArgs { });
		else
			_playerEvent.PlayerStep(new PlayerEvent.PlayerStepEventArgs { });
	}

}