using UnityEngine;

public class VelocityRenderer : MonoBehaviour {
   [SerializeField] private Gradient _colourGradientSources;
   [SerializeField] private Gradient _colourGradient;
   [SerializeField] private PlayerController _playerController;
   [SerializeField] private ParticleSystem.ColorOverLifetimeModule _colorOverLifeTime;

   public void Awake() {
       _colorOverLifeTime = GetComponent<ParticleSystem>().colorOverLifetime;
       _playerController = GetComponent<PlayerController>();
   }

   private void FixedUpdate() {
       _colourGradient.colorKeys[0].color = _colourGradientSources.Evaluate(_playerController.SpeedPercent);
       _colorOverLifeTime.color = _colourGradient;
   }
}