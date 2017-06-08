﻿using System;

namespace AssemblyCSharp
{
    public static class Constants
    {
        public const int PHYSICS_GRAVITY = 60;

        public const int ROUND_COUNT = 2;

        public const string GAME_VERSION = "0.2";

        public const float MOVEMENT_SPEED = 50f;

        public const float SPRINT_SPEED = 1.5f;

        public const float JUMP_CD = 1f;

        public const float JUMP_FORCE = 150000f;

        public const float DROP_DELAY = 0.2f;

        public const float DROP_FORCE = 300f;

        public const float POWERUP_SPAWN_CD = 10f;

        public const float BAZOOKA_CD = 1f;

        public const float GRIP_CD = 2f;

        public const float PROJECTILE_FORCE = 10000f;

        public const float NETWORK_SMOOTHING = 0.1f;

        public const float WHEEL_SPEED = 300f;

        public const int SPEED_BOOST_POWERUP_MULT = 2;

        public const float VISION_IMPAIRED_POWERUP_DURATION = 5f;

        public const float BAZOOKA_EXPLOSION_FORCE = 500000f;

        public const float GRIP_PULL_FORCE = 1000000f;

        public const int NUMBER_OF_AI = 0;

        public const float START_GAME_DELAY = 5f;

        public const float SPEC_CAMERA_TRAVEL_TIME = 5f;

        public const float JUMP_CD_AI = 2f;

        public const float FORCE_FIELD = 100f;

        public static string[] ROBOT_NAMES = new string[]
        {
            "Robot1",
            "Robot2"
        };

        public enum ROBOT_IDS
        {
            ROBOT_1,
            ROBOT_2
        }

        public enum MAPS_IDS
        {
            SPACE_MAP,
            BASIC_MAP,
            TEST_MAP,
            VOLCANO_MAP
        }

        /*
		 WATCH OUT
		 The order of the values here has to match the order of the values in MAPS_IDS
		*/
        public static string[] MAPS_NAMES = new string[]
        {
            "Map",
            "Map2",
            "TestMap",
            "Map-Volcano"
        };

        /*
		Same as MAPS_NAMES
		*/
        public static string[] MAP_SCENES_NAMES = new string[]
        {
            "SpaceArena",
            "BasicArena",
            "TestScene",
            "Volcano"
        };

        public enum POWERUP_IDS
        {
            VISION_IMPAIRED_POWERUP,
            SPEED_BOOST_POWERUP,
            COOLDOWN_REFRESH_POWERUP,
            SWAP_POWERUP
        }

        public enum EVENT_IDS
        {
            LOAD_SCENE,
            SCENE_LOADED,
            SPAWN_PLAYER,
            PLAYER_SPAWNED,
            REMOVE_WALLS,

            SPAWN_POWERUP,
            IMPAIR_VISION_EFFECT,

            BAZOOKA_SHOT,
            SWAP_PARTICLES,

            CHAT_MESSAGE,

            END_GAME
        }

        public static string[] AI_NAMES = new string[]
        {
            "Barry",
            "Larry",
            "John",
            "Bashar",
            "Donald",
            "Manuel",
            "Alex",
            "Corentin",
            "Nicolas",
            "Stan",
            "Spood",
            "Beast",
            "Frog",
            "Damon"

        };
    }
}
