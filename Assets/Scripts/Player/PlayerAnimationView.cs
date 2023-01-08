using UnityEngine;

namespace Player
{
    public class PlayerAnimationView 
    {
        
        private Animator animator;
        private int zoneCount = 0;
        public bool IsWalking = false;

        public PlayerAnimationView(Animator animator)
        {
            this.animator = animator;
        }

        public void AnimateWalk(float curVel,float maxVelocity)
        {
            if (!IsWalking)
            {
                animator.SetBool("WalkAttack",false);
                animator.SetBool("Walk",false);
                return;
            }
        
            if (zoneCount>0)
            {
                animator.SetBool("WalkAttack",true);
                animator.SetBool("Walk",false);
            }
            else
            {
                animator.speed = curVel / (float) maxVelocity;
                animator.SetBool("Walk",true);
                animator.SetBool("WalkAttack",false);
            }
        
        }

        public void AnimateAttack()
        {
            if (zoneCount>0)
            {
                animator.SetBool("Attack",true);
            }
            else
            {
                animator.SetBool("Attack",false);
            }
        }

        public void DecZoneCount()
        {
            --zoneCount;
        }
        public void IncZoneCount()
        {
            ++zoneCount;
        }

        public int GetZoneCount()
        {
            return zoneCount;
        }
    }
}