using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MidtermProject
{
    class SoundManager
    {
        private AudioEngine ThisAudioEngine {get; set; }
        private WaveBank ThisWaveBank { get; set; }
        private static SoundBank ThisSoundBank { get; set; }

        public SoundManager(String filePath) 
        {
            ThisAudioEngine = new AudioEngine(filePath + ".xgs");
            ThisWaveBank = new WaveBank(ThisAudioEngine, @"Content/Sounds/Wave Bank.xwb");
            ThisSoundBank = new SoundBank(ThisAudioEngine, @"Content/Sounds/Sound Bank.xsb");
        }

        public static void PlaySound(String fileName)
        {
            try
            {
                ThisSoundBank.PlayCue(fileName);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
