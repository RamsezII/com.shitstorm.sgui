#if HAS_TERM
using _TERM_;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUIS_.Tests
{
    static class CmdTests
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            TermServer.root_namespace.AddCommand("test_progress", static context =>
            {
                while (context.Reader.TryRead(out string read))
                    if (float.TryParse(read, out float time) && time > 0)
                        context.queue_args.Enqueue(time);

                return new(ERoutine);
                static IEnumerator<CmdStep> ERoutine(CmdContext context)
                {
                    for (int i = 0; i < context.queue_args.Count; i++)
                    {
                        float value = (float)context.queue_args.Dequeue();
                        yield return CmdStep.Status($"{i}: {value} seconds.");

                        float timer = 0;
                        while (timer < value)
                        {
                            timer += Time.unscaledDeltaTime;
                            yield return default;
                        }
                    }
                    yield return CmdStep.Result($"Done.");
                }
            });
        }
    }
}
#endif