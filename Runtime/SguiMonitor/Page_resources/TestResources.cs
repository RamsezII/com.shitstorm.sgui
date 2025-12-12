//using _ARK_;
//using _SGUI_.Monitor;
//using _SGUI_.Monitor.Resources;
//using System;
//using UnityEngine;

//namespace _SGUI_
//{
//    static class TestResources
//    {
//        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//        static void OnAfterSceneLoad()
//        {
//            Action<float> onValue = null;

//            NUCLEOR.instance.heartbeat_unscaled.AddOperation(new(.4f, true, () =>
//            {
//                onValue?.Invoke(UnityEngine.Random.Range(-5, 5));
//            }));

//            for (int i = 0; i < 5; i++)
//                Page.AddPopulator<ResourcesPage>(OnPage);

//            void OnPage(Page page)
//            {
//                var section = page.AddSection<ResourcesSection>();
//                section.trad.SetTrad("Test");

//                var graphs = new TimeGraph[UnityEngine.Random.Range(1, 4)];
//                for (int i = 0; i < graphs.Length; i++)
//                {
//                    var graph = graphs[i] = section.AddElement<TimeGraph>();
//                    graph.renderStep = .1f;
//                    graph.renderer.timeLate = graph.renderer.timeLate = 2;
//                    graph.renderer.timeSpan = graph.renderer.timeSpan = 15;

//                    var curve = graph.renderer.AddCurve();
//                    curve.options = curve.options = new()
//                    {
//                        drawLine = true,
//                        lineColor = Color.orange,
//                        lineThickness = 1,

//                        drawFillBottom = true,
//                        fillColorBottom = .5f * Color.orange,
//                    };

//                    onValue += OnValue;
//                }

//                void OnValue(float value)
//                {
//                    if (section.page.monitor.oblivionized)
//                    {
//                        onValue -= OnValue;
//                        return;
//                    }
//                    for (int i = 0; i < graphs.Length; ++i)
//                        graphs[i].renderer.curves[0].AddPoint(value);
//                }
//            }
//        }
//    }
//}