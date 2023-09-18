using System.Collections.Generic;
using Framework.Railways.Runtime.ScriptableObjects.Channels;
using UnityEngine;

namespace Framework.Railways.Runtime.Interactable.Carriage
{
    public class CarriageVisualEffects: MonoBehaviour
    {
        [SerializeField] private PuzzleEventChannel puzzleEventChannel;
        
        /// <summary>
        /// The <see cref="ParticleSystem"/> array to play on <see cref="PuzzleEventChannel.OnPuzzleSolved"/>
        /// </summary>
        [SerializeField] private ParticleSystem[] puzzleSolvedVFX;

        private void OnEnable()
        {
            puzzleEventChannel.OnPuzzleSolved += PlayPuzzleSolvedVFX;
        }
        
        private void OnDisable()
        {
            puzzleEventChannel.OnPuzzleSolved -= PlayPuzzleSolvedVFX;
        }

        /// <summary>
        /// Plays all the particle systems.
        /// </summary>
        private static void PlayParticleSystems(IEnumerable<ParticleSystem> particleSystems)
        {
            foreach (var system in particleSystems) system.Play();
        }

        private void PlayPuzzleSolvedVFX() => PlayParticleSystems(puzzleSolvedVFX);
    }
}