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
}