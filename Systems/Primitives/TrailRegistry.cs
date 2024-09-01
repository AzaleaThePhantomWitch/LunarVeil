using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace LunarVeil.Systems.Primitives
{
    static class TrailRegistry
    {
        public static MiscShaderData LaserShader => GameShaders.Misc["LunarVeil:LaserShader"];
        public static Asset<Texture2D> BeamTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/Beam");
        public static Asset<Texture2D> BeamTrail2 => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/BeamTrail");
        public static Asset<Texture2D> BulbTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/BulbTrail");

        public static Asset<Texture2D> SlicingTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/Slice");

        public static Asset<Texture2D> STARTRAIL => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/BeamTrail");
        public static Asset<Texture2D> STARTRAIL2 => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/Beamlight");
        public static MiscShaderData GenericLaserVertexShader => GameShaders.Misc["VampKnives:GenericLaserShader"];
        public static MiscShaderData LightBeamVertexShader => GameShaders.Misc["VampKnives:LightBeamVertexShader"];
        public static MiscShaderData ArtemisLaserShader => GameShaders.Misc["VampKnives:ArtemisLaserShader"];
        public static MiscShaderData FireVertexShader => GameShaders.Misc["VampKnives:Fire"];
        public static MiscShaderData FireWhiteVertexShader => GameShaders.Misc["VampKnives:FireWhite"];


        public static Asset<Texture2D> StarTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/StarTrail");
        public static Asset<Texture2D> StringTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/StringTrail");
        public static Asset<Texture2D> SmallWhispyTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/SmallWhispyTrail");
        public static Asset<Texture2D> WaveTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/WaveTrail");
        public static Asset<Texture2D> WaterTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/WaterTrail");

        public static Asset<Texture2D> CrystalTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/CrystalTrail");
        public static Asset<Texture2D> VortexTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/VortexTrail");
        public static Asset<Texture2D> WhispyTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/WhispyTrail");
        public static Asset<Texture2D> CorkscrewTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/CorkscrewTrail");
        public static Asset<Texture2D> TwistingTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/TwistingTrail");
        public static Asset<Texture2D> TwistingTrailSmall => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/TwistingTrailSmall");
        public static Asset<Texture2D> FadedStreak => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/FadedStreak");
        public static Asset<Texture2D> TerraTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/TerraTrail");
        public static Asset<Texture2D> DNATrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/DNAHelixTrail");
        public static Asset<Texture2D> SpikyTrail1 => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/SpikyTrail1");
        public static Asset<Texture2D> SpikyTrail2 => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/SpikyTrail2");
        public static Asset<Texture2D> LightningTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/LightningTrail");
        public static Asset<Texture2D> SimpleTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/SimpleTrail");
        public static Asset<Texture2D> SmoothTrailInverted => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/SmoothTrailInverted");


        public static Asset<Texture2D> CrystalNoise => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/Crystals");
        public static Asset<Texture2D> CausticTrail => ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/CausticTrail");


    }
}
