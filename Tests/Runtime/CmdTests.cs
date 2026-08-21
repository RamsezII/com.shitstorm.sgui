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
            TermServer.root_commands.AddCommand(new(
                name: "test_progress",
                owner: null,
                execution: (reader, context) =>
                {
                    while (reader.TryRead(out string read))
                        if (float.TryParse(read, out float time) && time > 0)
                            context.list_args.Add(time);

                    return (CmdExecution)ERoutine;
                    static IEnumerator<CmdStep> ERoutine(CmdContext context)
                    {
                        for (int i = 0; i < context.list_args.Count; i++)
                        {
                            float value = (float)context.list_args[i];
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
                }
            ));
        }
    }
}
#endif