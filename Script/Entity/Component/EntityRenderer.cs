using System;
using System.Collections.Generic;
using DG.Tweening;
using RPG.Animators;
using RPG.Players;
using UnityEngine;

public enum TriggertType
{
    End,
    Attack,
}

namespace RPG.Entities
{
    public class EntityRenderer : MonoBehaviour, IEntityComponent
    {
        public float FacingDirection { get; private set; } = 1;

        public event Action<TriggertType> OnAnimationTriggerEvent;

        [SerializeField] private bool _isStaticFacing;
        [SerializeField] private bool _isMouseFacing;
        private List<SpriteRenderer> _rendererList = new List<SpriteRenderer>();

        private readonly int _ShaderBlinkHash = Shader.PropertyToID("_Blink");
        private readonly int _ShaderDissolveHash = Shader.PropertyToID("_Dissolve");
        
        private Entity _entity;
        private Animator _animator;
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _animator = GetComponent<Animator>();
            GetComponentsInChildren(_rendererList);
        }

        public void Update()
        {
            if (_isMouseFacing)
            {
                Vector3 mosuePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                bool isFlip = transform.position.x - mosuePos.x > 0;

                if (FacingDirection < 1 ^ isFlip)
                    transform.eulerAngles = new Vector3(0, isFlip ? 180f : 0f, 0);
                    FacingDirection = isFlip ? -1 : 1;
            }
        }

        public void SetParam(AnimParamSO param, bool value) => _animator.SetBool(param.hashValue, value);
        public void SetParam(AnimParamSO param, float value) => _animator.SetFloat(param.hashValue, value);
        public void SetParam(AnimParamSO param, int value) => _animator.SetInteger(param.hashValue, value);
        public void SetParam(AnimParamSO param) => _animator.SetTrigger(param.hashValue);

        #region FlipControl

        public void Flip()
        {
            if (_isStaticFacing || _isMouseFacing) return;
            FacingDirection *= -1;
            transform.Rotate(0, 180f, 0);
        }

        public void FlipController(float xMove)
        {
            if (Mathf.Abs(FacingDirection + xMove) < 0.5f)
                Flip();
        }


        #endregion

        public void Blink(float time)
        {
            _rendererList.ForEach(renderer => renderer.material.SetFloat(_ShaderBlinkHash, 1));
            _rendererList.ForEach(renderer => renderer.material.DOFloat(0, _ShaderBlinkHash, time).SetEase(Ease.InQuad));
        }

        public void Dissolve(float time)
        {
            _rendererList.ForEach(renderer => renderer.material.DOFloat(1, _ShaderDissolveHash, time).SetEase(Ease.Linear));
        }

        private void AnimationTrigger(TriggertType triggrtType)
        {
            OnAnimationTriggerEvent?.Invoke(triggrtType);
        }
    }
}
