namespace TopDownShooter
{
    public class AxisTags
    {
        public const string VERTICAL = "Vertical";
        public const string HORIZONTAL = "Horizontal";
    }

    public class TagTags
    {
        public const string BULLET = "Bullet";
        public const string PLAYER = "Player";
        public const string AIM = "Aim";
    }

    public class AnimationTags
    {
        //PLAYER
        public const string FLOAT_X_VELOCITY = "xVelocity";
        public const string FLOAT_Z_VELOCITY = "zVelocity";
        public const string BOOL_IS_RUNNING = "isRunning";
        public const string TRIGGER_FIRE = "Fire";
        public const string TRIGGER_RELOAD = "Reload";
        public const string BOOL_BUSY_GRABBING_WEAPON = "BusyGrabbingWeapon";
        public const string TRIGGER_WEAPON_GRAB = "WeaponGrab";
        public const string FLOAT_WEAPON_GRAB_TYPE = "WeaponGrabType";
    }

    public class LayerTags
    {
        public const string GROUND = "Ground";
        public const string OBSTACLES = "Obstacles";
        public const string PLAYER = "Player";
        public const string BULLET = "Bullet";
        public const string AIM = "Aim";
        public const string ENEMY = "Enemy";
    }
}