using UnityEngine;
using _SGUI_.composer;
using UnityEngine.Video;

namespace _SGUI_
{
    public partial class SguiMediaPlayer : SguiFrame
    {
        public VideoPlayer video_player;
        public AudioSource audio_source;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            OSView.instance.AddSoftwareButton<SguiMediaPlayer>(new()
            {
                french = "Lecteur multimédia",
                english = "Médiaplayer",
            });
        }
    }
}