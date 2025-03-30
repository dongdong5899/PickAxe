using UnityEngine;

namespace RPG.Animators
{
    [CreateAssetMenu(fileName = "AnimParamSO", menuName = "SO/Anim/ParamSO")]
    public class AnimParamSO : ScriptableObject
    {
        public enum ParamType
        {
            Boolean, Float, Integer, Trigger
        }

        public string paramName;
        public ParamType paramType;
        public int hashValue;

        private void OnValidate()
        {
            hashValue = Animator.StringToHash(paramName);
        }
    }
}
