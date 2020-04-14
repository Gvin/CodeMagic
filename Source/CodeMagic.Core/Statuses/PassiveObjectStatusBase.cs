using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Statuses
{
    public abstract class PassiveObjectStatusBase : IObjectStatus
    {
        private readonly int maxLifeTime;
        private int lifeTime;

        protected PassiveObjectStatusBase(int maxLifeTime)
        {
            this.maxLifeTime = maxLifeTime;
            lifeTime = 0;
        }

        public bool Update(IDestroyableObject owner, Point position)
        {
            if (lifeTime >= maxLifeTime)
            {
                return false;
            }

            lifeTime++;
            return true;
        }

        public abstract string Type { get; }
    }
}