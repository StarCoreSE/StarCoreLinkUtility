using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.Game.Components;

namespace playerHUD
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class playerhud : MySessionComponentBase
    {
        bool shown = false;
        bool waiting = false;
        DateTime startTime;

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
        }

        public override void BeforeStart()
        {
            MyVisualScriptLogicProvider.PlayerSpawned += PlayerSpawned;
            MyVisualScriptLogicProvider.RemoveQuestlogDetails();
            MyVisualScriptLogicProvider.SetQuestlog(false);
            shown = false;
            waiting = false;
        }

        private void PlayerSpawned(long playerId)
        {
            if (!shown && !waiting)
            {
                MyVisualScriptLogicProvider.SetQuestlog(true, "Keybinds");
                MyVisualScriptLogicProvider.AddQuestlogObjective("ALT+P = Starcore Pointsheet", false, true);
                MyVisualScriptLogicProvider.AddQuestlogObjective("SHIFT+F2 = Report Bug", false, true);
                MyVisualScriptLogicProvider.AddQuestlogObjective("/harmdist NUMBER to change the visibility distance of the harmsphere hexes.", false, true);
                waiting = true;
                startTime = DateTime.Now;
            }
        }

        public override void UpdateAfterSimulation()
        {
            if (waiting && DateTime.Now - startTime >= TimeSpan.FromSeconds(10))
            {
                MyVisualScriptLogicProvider.RemoveQuestlogDetails();
                MyVisualScriptLogicProvider.SetQuestlog(false);
                waiting = false;
                shown = false;
            }
        }

        protected override void UnloadData()
        {
            MyVisualScriptLogicProvider.PlayerSpawned -= PlayerSpawned;
        }
    }
}
