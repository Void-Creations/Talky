using System;
using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers;
using RemoteAdmin;

namespace Talky.LabAPI
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class EmotionCommand : ParentCommand
    {

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender playerSender)
            {
                response = "Bu komut sadece oyuncular tarafından kullanılabilir.";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = $"Geçersiz duygu. Geçerli duygular şunlardır: Angry, AwkwardSmile, Chad, Happy, Neutral, Ogre, Scared";
                return false;
            }

            string emotion = arguments.At(0).ToLower();
            Player player = Player.Get(sender);
            if (EmotionPresetType.TryParse(emotion, out EmotionPresetType preset))
            {
                player.ReferenceHub.ServerSetEmotionPreset(preset);
                if (player.ReferenceHub.TryGetComponent(out SpeechTracker tracker))
                {
                    tracker.DefaultPreset = preset;
                }
                response = $"Duygularınız {preset} olarak ayarlandı.";
                return true;
            }
            else
            {
                response = $"Geçersiz duygu. Geçerli duygular şunlardır: Angry, AwkwardSmile, Chad, Happy, Neutral, Ogre, Scared";
                return false;
            }
            response = $"Duygularınız {emotion} olarak ayarlandı.";
            return true;
        }

        public override string Command { get; } = "duygular";
        public override string[] Aliases { get; } = { "duygu" };
        public override string Description { get; } = "Karakterinizin yüz ifadesini belirler. Geçerli duygular şunlardır: Angry, AwkwardSmile, Chad, Happy, Neutral, Ogre, Scared";

        public override void LoadGeneratedCommands()
        {
            
        }
    }
}
