// Recompile at 2025/7/3 22:27:13
#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.Playables;
using System;

namespace PixelCrushers.DialogueSystem
{

    [Serializable]
    public class SequencerMessageBehaviour : PlayableBehaviour
    {

        [Tooltip("Sequencer message to send to Dialogue System's sequencer.")]
        public string message;

    }
}
#endif
#endif
