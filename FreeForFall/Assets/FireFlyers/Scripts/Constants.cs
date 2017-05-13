using System;

namespace AssemblyCSharp
{
	public static class Constants
	{
		public const string GAME_VERSION = "0.2";

		public const float MOVEMENT_SPEED = 20f;

		public const float SPRINT_SPEED = 4f;

		public const float JUMP_CD = 2f;

		public const float JUMP_FORCE = 150000f;

		public const float DROP_DELAY = 0.2f;

		public const float DROP_FORCE = 300f;

		public const float POWERUP_SPAWN_CD = 10f;

		public const float BAZOOKA_CD = 3f;

		public const float GRIP_CD = 3f;

		public const float PROJECTILE_FORCE = 10000f;

		public const float NETWORK_SMOOTHING = 0.1f;

		public const float WHEEL_SPEED = 200f;

		public const int SPEED_BOOST_POWERUP_MULT = 20;

		public const float VISION_IMPAIRED_POWERUP_DURATION = 5f;

		public const float BAZOOKA_EXPLOSION_FORCE = 15f;

		public const float GRIP_PULL_FORCE = 300000f;

		public enum MAPS_IDS
		{
			SPACE_MAP,
			BASIC_MAP,
			TEST_MAP
		}

		/*
		 WATCH OUT
		 The order of the values here has to match the order of the values in MAPS_IDS
		*/
		public static string[] MAPS_NAMES = new string[] {
			"Map",
			"Map2",
			"TestMap"
		};

		public enum POWERUP_IDS
		{
			VISION_IMPAIRED_POWERUP,
			SPEED_BOOST_POWERUP,
			COOLDOWN_REFRESH_POWERUP
		}

		public enum EVENT_IDS
		{
			LOAD_MAP,
			REMOVE_WALLS,
			PLAYER_LOST,
			VISION_IMPAIRED,
			SPEED_BOOST,
			COOLDOWN_REFRESH,
			SPAWN_POWERUP,
			GRAPPLING_HOOK,
			BAZOOKA_SHOT,
			END_GAME
		}
	}
}

