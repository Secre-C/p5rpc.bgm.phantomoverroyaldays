using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Memory.Sources;
using System.Diagnostics;
using System.Collections;
using System;
using p5rpc.bgm.phantomoverroyaldays.Configuration;

namespace p5rpc.noholdupmusic
{
    public unsafe class Phantom
    {
        public Phantom(IReloadedHooks hooks, ILogger logger, IModLoader modLoader, Config configuration)
        {
            var memory = Memory.Instance;

            using var thisProcess = Process.GetCurrentProcess();
            long baseAddress = thisProcess.MainModule.BaseAddress.ToInt64();

            modLoader.GetController<IStartupScanner>().TryGetTarget(out var startupScanner);

            startupScanner.AddMainModuleScan("B9 85 03 00 00 66 89 56 ??", (result) =>
            {
                long holdupMusicStart = result.Offset + baseAddress;
                if (result.Found)
                {
                    memory.SafeWrite(holdupMusicStart + 1, configuration.cue);
                }
            });
        }
    }
}