using System;
using Framework.Railways.Runtime.Interactable.Carriage;
using UnityEngine;
using UnityEngine.Splines;

namespace Framework.Railways.Runtime.Paths.PathBehaviour
{
    [Serializable]
    public class RailwayPathForceContinue: RailwayPath
    {
        [field: SerializeField] public override RailwayPath PreviousPath { get; protected set; }
        [field: SerializeField] public override RailwayPath NextPath { get; protected set; }
        
        public override Action<Carriage> ReachStart => carriage => carriage.SetRailwayPath(PreviousPath, 1);
        public override Action<Carriage> ReachEnd => carriage => carriage.SetRailwayPath(NextPath, 0);
    }
}