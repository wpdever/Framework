﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 * 
 * Document: http://catlib.io/
 */

using CatLib.API.Config;
using CatLib.API.Time;
using CatLib.API.Timer;
using CatLib.Config;
using CatLib.Time;
using CatLib.Timer;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Timer
{
    [TestClass]
    public class TimerProviderTests
    {
        /// <summary>
        /// 这是一个调试的时间
        /// </summary>
        public class TimerTestTime : ITime
        {
            /// <summary>
            /// 从游戏开始到现在所用的时间(秒)
            /// </summary>
            public float Time { get; private set; }

            /// <summary>
            /// 上一帧到当前帧的时间(秒)
            /// </summary>
            public float DeltaTime { get; private set; }

            /// <summary>
            /// 从游戏开始到现在的时间（秒）使用固定时间来更新
            /// </summary>
            public float FixedTime { get; private set; }

            /// <summary>
            /// 从当前scene开始到目前为止的时间(秒)
            /// </summary>
            public float TimeSinceLevelLoad { get; private set; }

            /// <summary>
            /// 固定的上一帧到当前帧的时间(秒)
            /// </summary>
            public float FixedDeltaTime { get; set; }

            /// <summary>
            /// 能获取最大的上一帧到当前帧的时间(秒)
            /// </summary>
            public float MaximumDeltaTime { get; private set; }

            /// <summary>
            /// 平稳的上一帧到当前帧的时间(秒)，根据前N帧的加权平均值
            /// </summary>
            public float SmoothDeltaTime { get; private set; }

            /// <summary>
            /// 时间缩放系数
            /// </summary>
            public float TimeScale { get; set; }

            /// <summary>
            /// 总帧数
            /// </summary>
            public float FrameCount { get; private set; }

            /// <summary>
            /// 自游戏开始后的总时间（暂停也会增加）
            /// </summary>
            public float RealtimeSinceStartup { get; private set; }

            /// <summary>
            /// 每秒的帧率
            /// </summary>
            public int CaptureFramerate { get; set; }

            /// <summary>
            /// 不考虑时间缩放上一帧到当前帧的时间(秒)
            /// </summary>
            public float UnscaledDeltaTime { get; private set; }

            /// <summary>
            /// 不考虑时间缩放的从游戏开始到现在的时间
            /// </summary>
            public float UnscaledTime { get; private set; }

            public TimerTestTime()
            {
                Time = 0.25f;
                DeltaTime = 0.25f;
                FixedTime = 0.25f;
                TimeSinceLevelLoad = 0;
                FixedDeltaTime = 0;
                MaximumDeltaTime = 0;
                SmoothDeltaTime = 0;
                TimeScale = 0;
                FrameCount = 0;
                RealtimeSinceStartup = 0;
                CaptureFramerate = 0;
                UnscaledDeltaTime = 0;
                UnscaledTime = 0;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var app = new Application().Bootstrap();
            app.Register(typeof(TimeProvider));
            app.Register(typeof(ConfigProvider));
            app.Register(typeof(TimerProvider));
            app.Init();

            var manager = app.Make<ITimeManager>();
            manager.Extend(() => new TimerTestTime(), "test");

            var config = app.Make<IConfigManager>();
            config.Default.Set("times.default", "test");
        }

        [TestMethod]
        public void TestTimerSimpleDelay()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = false;
            timer.Create(() =>
            {
                statu = !statu;
            }).Delay(1);

            var app = App.Instance as Application;

            //0.25
            Assert.AreEqual(false, statu);
            app.Update();

            //0.5
            Assert.AreEqual(false, statu);
            app.Update();

            //0.75
            Assert.AreEqual(false, statu);
            app.Update();

            //1.0
            Assert.AreEqual(false, statu);
            app.Update();

            Assert.AreEqual(true, statu);
            app.Update();

            Assert.AreEqual(true, statu);
        }
    }
}
