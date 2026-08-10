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
            TermServer.root_commands.AddCommandNode(new CmdCommand(
                name: "test_progress",
                parse: (reader, context) =>
                {
                    while (reader.TryRead(out string read))
                        if (float.TryParse(read, out float time) && time > 0)
                            context.args.Add(time);
                },
                routine: static context =>
                {
                    return ERoutine(context);
                    static IEnumerator<CmdStep> ERoutine(CmdContext context)
                    {
                        for (int i = 0; i < context.args.Count; i++)
                        {
                            float value = (float)context.args[i];
                            yield return CmdStep.Status($"{i}: {value} seconds.");

                            float timer = 0;
                            while (timer < value)
                                yield return CmdStep.Status(null, progress: timer / value);
                        }
                        yield return CmdStep.Result($"Done.");
                    }
                }
            ));
        }
    }
}
#endif