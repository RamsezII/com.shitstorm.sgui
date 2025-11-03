using UnityEngine;
using UnityEngine.Video;

namespace _SGUI_
{
    public partial class SguiMediaPlayer : SguiWindow1
    {
        [HideInInspector] public VideoPlayer video_player;
        [HideInInspector] public AudioSource audio_source;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            OSView.instance.AddOrGetSoftwareButton<SguiMediaPlayer>();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            Transform video_rt = transform.Find("rT/body/video_player");
            video_player = video_rt.GetComponent<VideoPlayer>();
            audio_source = video_rt.GetComponent<AudioSource>();

            base.Awake();
        }
    }
}