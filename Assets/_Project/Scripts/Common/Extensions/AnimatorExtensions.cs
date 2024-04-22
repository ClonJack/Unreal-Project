using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnrealTeam.SB.Common.Extensions
{
    public static class AnimatorExtensions
    {
        public static float GetAnimationLength(this Animator animator, int animationHash)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
                if (Animator.StringToHash(clip.name) == animationHash)
                    return clip.length;

            return -1f;
        }        
        
        public static Dictionary<int, float> GetAnimationsLengths(this Animator animator, int[] animationsHashes)
        {
            var animationsLengths = new Dictionary<int, float>();
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                var index = Array.IndexOf(animationsHashes, Animator.StringToHash(clip.name));
                if (index < 0)
                    continue;

                animationsLengths[animationsHashes[index]] = clip.length;
            }

            return animationsLengths;
        }
    }
}