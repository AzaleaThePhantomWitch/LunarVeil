using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Terraria.Utilities;
using System.Linq;
using ReLogic.Graphics;
using LunarVeil.Systems.Players;

namespace LunarVeil.Systems.Primitives
{

    public static class Helper
    {
        

        public static Vector2 TileAdj => (Lighting.Mode == Terraria.Graphics.Light.LightMode.Retro || Lighting.Mode == Terraria.Graphics.Light.LightMode.Trippy) ? Vector2.Zero : Vector2.One * 12;

        public static Vector2 ScreenSize => new(Main.screenWidth, Main.screenHeight);

        public static Rectangle ScreenTiles => new((int)Main.screenPosition.X / 16, (int)Main.screenPosition.Y / 16, Main.screenWidth / 16, Main.screenHeight / 16);

        
        public static Rectangle ToRectangle(this Vector2 vector)
        {
            return new Rectangle(0, 0, (int)vector.X, (int)vector.Y);
        }

        public static Vector2 Round(this Vector2 vector)
        {
            return new Vector2((float)Math.Round(vector.X), (float)Math.Round(vector.Y));
        }

        /// <summary>
        /// Runs math.min on both the X and Y seperately, returns the smaller value for each
        /// </summary>
        public static Vector2 TwoValueMin(this Vector2 vector, Vector2 vector2)
        {
            return new Vector2(Math.Min(vector.X, vector2.X), Math.Min(vector.Y, vector2.Y));
        }

        /// <summary>
        /// Runs math.max on both the X and Y seperately, returns the largest value for each
        /// </summary>
        public static Vector2 TwoValueMax(this Vector2 vector, Vector2 vector2)
        {
            return new Vector2(Math.Max(vector.X, vector2.X), Math.Max(vector.Y, vector2.Y));
        }

        public static Player Owner(this Projectile proj)
        {
            return Main.player[proj.owner];
        }

        /// <summary>
        /// Seperates all flags stored in a enum out into an array
        /// </summary>
        public static IEnumerable<Enum> GetFlags(this Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
            {
                if (input.HasFlag(value))
                    yield return value;
            }
        }

        /// <summary>
        /// Updates the value used for flipping rotation on the Player. Should be reset to 0 when not in use.
        /// </summary>
        /// <param name="Player"></param>
        /// <param name="value"></param>


        public static Vector3 Vec3(this Vector2 vector)
        {
            return new Vector3(vector.X, vector.Y, 0);
        }

        public static Vector3 ScreenCoord(this Vector3 vector)
        {
            return new Vector3(-1 + vector.X / Main.screenWidth * 2, (-1 + vector.Y / Main.screenHeight * 2f) * -1, 0);
        }



        public static Color MoltenVitricGlow(float time)
        {
            Color MoltenGlowc = Color.White;
            if (time > 30 && time < 60)
                MoltenGlowc = Color.Lerp(Color.White, Color.Orange, Math.Min((time - 30f) / 20f, 1f));
            else if (time >= 60)
                MoltenGlowc = Color.Lerp(Color.Orange, Color.Lerp(Color.Red, Color.Transparent, Math.Min((time - 60f) / 50f, 1f)), Math.Min((time - 60f) / 30f, 1f));
            return MoltenGlowc;
        }

        public static float RotationDifference(float rotTo, float rotFrom)
        {
            return ((rotTo - rotFrom) % 6.28f + 9.42f) % 6.28f - 3.14f;
        }

        /// <summary>
        /// determines if an NPC is "fleshy" based on it's hit sound
        /// </summary>
        /// <param name="NPC"></param>
        /// <returns></returns>
        
        public static Vector2 Centeroid(List<NPC> input) //Helper overload for NPCs for support NPCs
        {
            var centers = new List<Vector2>();

            for (int k = 0; k < input.Count; k++)
                centers.Add(input[k].Center);

            return Centeroid(centers);
        }

        public static Vector2 Centeroid(List<Vector2> input) //this gets the centeroid of the points. see: https://math.stackexchange.com/questions/1801867/finding-the-centre-of-an-abritary-set-of-points-in-two-dimensions
        {
            float sumX = 0;
            float sumY = 0;

            for (int k = 0; k < input.Count; k++)
            {
                sumX += input[k].X;
                sumY += input[k].Y;
            }

            return new Vector2(sumX / input.Count, sumY / input.Count);
        }

        public static float LerpFloat(float min, float max, float val)
        {
            float difference = max - min;
            return min + difference * val;
        }

        

        //algorithm taken from http://web.archive.org/web/20060911055655/http://local.wasp.uwa.edu.au/~pbourke/geometry/lineline2d/
        public static bool LinesIntersect(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4, out Vector2 intersectPoint)
        {
            intersectPoint = Vector2.Zero;

            float denominator = (point4.Y - point3.Y) * (point2.X - point1.X) - (point4.X - point3.X) * (point2.Y - point1.Y);

            float a = (point4.X - point3.X) * (point1.Y - point3.Y) - (point4.Y - point3.Y) * (point1.X - point3.X);
            float b = (point2.X - point1.X) * (point1.Y - point3.Y) - (point2.Y - point1.Y) * (point1.X - point3.X);

            if (denominator == 0)
            {
                if (a == 0 || b == 0) //lines are coincident
                {
                    intersectPoint = point3; //possibly not the best fallback?
                    return true;
                }
                else
                {
                    return false; //lines are parallel
                }
            }

            float ua = a / denominator;
            float ub = b / denominator;

            if (ua > 0 && ua < 1 && ub > 0 && ub < 1)
            {
                intersectPoint = new Vector2(point1.X + ua * (point2.X - point1.X), point1.Y + ua * (point2.Y - point1.Y));
                return true;
            }

            return false;
        }

        public static bool CheckCircularCollision(Vector2 center, int radius, Rectangle hitbox)
        {
            if (Vector2.Distance(center, hitbox.TopLeft()) <= radius)
                return true;

            if (Vector2.Distance(center, hitbox.TopRight()) <= radius)
                return true;

            if (Vector2.Distance(center, hitbox.BottomLeft()) <= radius)
                return true;

            return Vector2.Distance(center, hitbox.BottomRight()) <= radius;
        }

        public static bool CheckConicalCollision(Vector2 center, int radius, float angle, float width, Rectangle hitbox)
        {
            if (CheckPoint(center, radius, hitbox.TopLeft(), angle, width))
                return true;

            if (CheckPoint(center, radius, hitbox.TopRight(), angle, width))
                return true;

            if (CheckPoint(center, radius, hitbox.BottomLeft(), angle, width))
                return true;

            return CheckPoint(center, radius, hitbox.BottomRight(), angle, width);
        }

        private static bool CheckPoint(Vector2 center, int radius, Vector2 check, float angle, float width)
        {
            float thisAngle = (center - check).ToRotation() % 6.28f;
            return Vector2.Distance(center, check) <= radius && thisAngle > angle - width && thisAngle < angle + width;
        }

        public static bool PointInTile(Vector2 point)
        {
            var startCoords = new Point16((int)point.X / 16, (int)point.Y / 16);
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point16 thisPoint = startCoords + new Point16(x, y);

                    if (!WorldGen.InWorld(thisPoint.X, thisPoint.Y))
                        return false;

                    Tile tile = Framing.GetTileSafely(thisPoint);

                    if (Main.tileSolid[tile.TileType] && tile.HasTile && !Main.tileSolidTop[tile.TileType])
                    {
                        var rect = new Rectangle(thisPoint.X * 16, thisPoint.Y * 16, 16, 16);

                        if (rect.Contains(point.ToPoint()))
                            return true;
                    }
                }
            }

            return false;
        }

        public static string TicksToTime(int ticks)
        {
            int sec = ticks / 60;
            return sec / 60 + ":" + (sec % 60 < 10 ? "0" + sec % 60 : "" + sec % 60);
        }

        public static bool ScanForTypeDown(int startX, int startY, int type, int maxDown = 50)
        {
            for (int k = 0; k <= maxDown && k + startY < Main.maxTilesY; k++)
            {
                Tile tile = Framing.GetTileSafely(startX, startY + k);

                if (tile.HasTile && tile.TileType == type)
                    return true;
            }

            return false;
        }

        public static float CompareAngle(float baseAngle, float targetAngle)
        {
            return (baseAngle - targetAngle + (float)Math.PI * 3) % MathHelper.TwoPi - (float)Math.PI;
        }

        public static float ConvertAngle(float angleIn)
        {
            return CompareAngle(0, angleIn) + (float)Math.PI;
        }

        public static string WrapString(string input, int length, DynamicSpriteFont font, float scale)
        {
            string output = "";
            string[] words = input.Split();

            string line = "";
            foreach (string str in words)
            {
                if (str == "NEWBLOCK")
                {
                    output += "\n\n";
                    line = "";
                    continue;
                }

                if (font.MeasureString(line).X * scale < length)
                {
                    output += " " + str;
                    line += " " + str;
                }
                else
                {
                    output += "\n" + str;
                    line = str;
                }
            }

            return output[1..];
        }

        public static List<T> RandomizeList<T>(List<T> input)
        {
            int n = input.Count();

            while (n > 1)
            {
                n--;
                int k = Main.rand.Next(n + 1);
                (input[n], input[k]) = (input[k], input[n]);
            }

            return input;
        }

        public static List<T> RandomizeList<T>(List<T> input, UnifiedRandom rand)
        {
            int n = input.Count();

            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                (input[n], input[k]) = (input[k], input[n]);
            }

            return input;
        }

        public static Player FindNearestPlayer(Vector2 position)
        {
            Player Player = null;

            for (int k = 0; k < Main.maxPlayers; k++)
            {
                if (Main.player[k] != null && Main.player[k].active && (Player == null || Vector2.DistanceSquared(position, Main.player[k].Center) < Vector2.DistanceSquared(position, Player.Center)))
                    Player = Main.player[k];
            }

            return Player;
        }

        public static float BezierEase(float time)
        {
            return time * time / (2f * (time * time - time) + 1f);
        }

        public static float SwoopEase(float time)
        {
            return 3.75f * (float)Math.Pow(time, 3) - 8.5f * (float)Math.Pow(time, 2) + 5.75f * time;
        }

        public static float Lerp(float a, float b, float f)
        {
            return a * (1.0f - f) + b * f;
        }

        /// <summary>
        /// <para>Animations are interpolated with a cubic bezier. You will define the bezier using the p1 and p2 parameters.</para>
        /// <para>This function serves as a constructor for the real interpolation function</para>
        /// <para>Use https://cubic-bezier.com/ to find appropriate parameters.</para>
        /// </summary>
        public static Func<float, float> CubicBezier(float p1x, float p1y, float p2x, float p2y)
        {
            if (p1x < 0 || p1x > 1 || p2x < 0 || p2x > 1)
            {
                throw new ArgumentException("X point parameters of cubic bezier timing function should be between values 0 and 1!");
            }

            Vector2 p0 = Vector2.Zero;
            var p1 = new Vector2(p1x, p1y);
            var p2 = new Vector2(p2x, p2y);
            Vector2 p3 = Vector2.One;

            float timing(float t)
            {
                return (float)(Math.Pow(1 - t, 3) * p0.X + 3 * Math.Pow(1 - t, 2) * t * p1.X + 3 * (1 - t) * Math.Pow(t, 2) * p2.X + Math.Pow(t, 3) * p3.X);
            }

            float progression(float x)
            {
                float time;
                if (x <= 0)
                    time = 0;
                else if (x >= 1)
                    time = 1;
                else
                    time = BinarySolve(timing, x, 0.001f);

                return (float)(Math.Pow(1 - time, 3) * p0.Y + 3 * Math.Pow(1 - time, 2) * time * p1.Y + 3 * (1 - time) * Math.Pow(time, 2) * p2.Y + Math.Pow(time, 3) * p3.Y);
            }

            return progression;
        }

        // Binary solver for cubic bezier
        private static float BinarySolve(in Func<float, float> function, in float target, in float precision, float start = 0, float end = 1, int iteration = 0)
        {
            if (iteration > 1000)
            {
                throw new ArgumentException("Could not converge to an answer in over 1000 iterations.");
            }

            float halfway = (start + end) / 2;
            float res = function(halfway);

            if (Math.Abs(res - target) < precision)
                return halfway;
            else if (target < res)
                return BinarySolve(function, target, precision, start, halfway, iteration + 1);
            else
                return BinarySolve(function, target, precision, halfway, end, iteration + 1);
        }

        public static T[] FastUnion<T>(this T[] front, T[] back)
        {
            var combined = new T[front.Length + back.Length];

            Array.Copy(front, combined, front.Length);
            Array.Copy(back, 0, combined, front.Length, back.Length);

            return combined;
        }


       



        public static Point16 FindTile(Point16 start, Func<Tile, bool> condition, int radius = 30, int w = 1, int h = 1)
        {
            Point16 output = Point16.Zero;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    Point16 check1 = start + new Point16(x, y);
                    Point16 attempt1 = CheckTiles(check1, condition, w, h);
                    if (attempt1 != Point16.Zero)
                        return attempt1;

                    Point16 check2 = start + new Point16(-x, y);
                    Point16 attempt2 = CheckTiles(check2, condition, w, h);
                    if (attempt2 != Point16.Zero)
                        return attempt2;

                    Point16 check3 = start + new Point16(x, -y);
                    Point16 attempt3 = CheckTiles(check3, condition, w, h);
                    if (attempt3 != Point16.Zero)
                        return attempt3;

                    Point16 check4 = start + new Point16(-x, -y);
                    Point16 attempt4 = CheckTiles(check4, condition, w, h);
                    if (attempt4 != Point16.Zero)
                        return attempt4;
                }
            }

            return output;
        }

        private static Point16 CheckTiles(Point16 check, Func<Tile, bool> condition, int w, int h)
        {
            if (WorldGen.InWorld(check.X, check.Y))
            {
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        Tile checkTile = Framing.GetTileSafely(check.X + x, check.Y + y);

                        if (!condition(checkTile))
                            return Point16.Zero;
                    }
                }

                return check;
            }

            return Point16.Zero;
        }

        public static bool ClearPath(Vector2 point1, Vector2 point2)
        {
            Vector2 direction = point2 - point1;

            for (int i = 0; i < direction.Length(); i += 4)
            {
                Vector2 toLookAt = point1 + Vector2.Normalize(direction) * i;

                if (Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).HasTile && Main.tileSolid[Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).TileType])
                    return false;
            }

            return true;
        }
    }




    public class PrimitiveTrailingTypes : IDisposable
    {
        private DynamicVertexBuffer vertexBuffer;
        private DynamicIndexBuffer indexBuffer;

        private readonly GraphicsDevice device;

        public bool IsDisposed { get; private set; }

        public PrimitiveTrailingTypes(GraphicsDevice device, int maxVertices, int maxIndices)
        {
            this.device = device;

            if (device != null)
            {
                Main.QueueMainThreadAction(() =>
                {
                    vertexBuffer = new DynamicVertexBuffer(device, typeof(VertexPositionColorTexture), maxVertices, BufferUsage.WriteOnly);
                    indexBuffer = new DynamicIndexBuffer(device, IndexElementSize.SixteenBits, maxIndices, BufferUsage.WriteOnly);
                });
            }
        }

        public void Render(Effect effect)
        {
            if (vertexBuffer is null || indexBuffer is null)
                return;

            device.SetVertexBuffer(vertexBuffer);
            device.Indices = indexBuffer;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
            }
        }

        public void SetVertices(VertexPositionColorTexture[] vertices)
        {
            vertexBuffer?.SetData(0, vertices, 0, vertices.Length, VertexPositionColorTexture.VertexDeclaration.VertexStride, SetDataOptions.Discard);
        }

        public void SetIndices(short[] indices)
        {
            indexBuffer?.SetData(0, indices, 0, indices.Length, SetDataOptions.Discard);
        }

        public void Dispose()
        {
            IsDisposed = true;

            vertexBuffer?.Dispose();
            indexBuffer?.Dispose();
        }
    }

    public interface ITrailTip
    {
        int ExtraVertices { get; }

        int ExtraIndices { get; }

        void GenerateMesh(Vector2 trailTipPosition, Vector2 trailTipNormal, int startFromIndex, out VertexPositionColorTexture[] vertices, out short[] indices, TrailWidthFunction trailWidthFunction, TrailColorFunction trailColorFunction);
    }

    public delegate float TrailWidthFunction(float factorAlongTrail);

    public delegate Color TrailColorFunction(Vector2 textureCoordinates);

    public class Trail : IDisposable
    {
        public int stayAlive = 10;

        private readonly PrimitiveTrailingTypes primitives;

        private readonly int maxPointCount;

        private readonly ITrailTip tip;

        private readonly TrailWidthFunction trailWidthFunction;

        private readonly TrailColorFunction trailColorFunction;

        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Array of positions that define the trail. NOTE: Positions[Positions.Length - 1] is assumed to be the start (e.g. Projectile.Center) and Positions[0] is assumed to be the end.
        /// </summary>
        public Vector2[] Positions
        {
            get => positions;
            set
            {
                if (value.Length != maxPointCount)
                {
                    throw new ArgumentException("Array of positions was a different length than the expected result!");
                }

                positions = value;
            }
        }

        private Vector2[] positions;

        /// <summary>
        /// Used in order to calculate the normal from the frontmost position, because there isn't a point after it in the original list.
        /// </summary>
        public Vector2 NextPosition { get; set; }

        private const float defaultWidth = 16;

        public Trail(GraphicsDevice device, int maxPointCount, ITrailTip tip, TrailWidthFunction trailWidthFunction, TrailColorFunction trailColorFunction)
        {
            this.tip = tip ?? new NoTip();

            this.maxPointCount = maxPointCount;

            this.trailWidthFunction = trailWidthFunction;

            this.trailColorFunction = trailColorFunction;

            /* A---B---C
             * |  /|  /|
             * D / E / F
             * |/  |/  |
             * G---H---I
             * 
             * Let D, E, F, etc. be the set of n points that define the trail.
             * Since each point generates 2 vertices, there are 2n vertices, plus the tip's count.
             * 
             * As for indices - in the region between 2 defining points there are 2 triangles.
             * The amount of regions in the whole trail are given by n - 1, so there are 2(n - 1) triangles for n points.
             * Finally, since each triangle is defined by 3 indices, there are 6(n - 1) indices, plus the tip's count.
             */

            primitives = new PrimitiveTrailingTypes(device, maxPointCount * 2 + this.tip.ExtraVertices, 6 * (maxPointCount - 1) + this.tip.ExtraIndices);

            ModContent.GetInstance<TrailManager>().managed.Add(this);
        }

        private void GenerateMesh(out VertexPositionColorTexture[] vertices, out short[] indices, out int nextAvailableIndex)
        {
            var verticesTemp = new VertexPositionColorTexture[maxPointCount * 2];

            short[] indicesTemp = new short[maxPointCount * 6 - 6];

            // k = 0 indicates starting at the end of the trail (furthest from the origin of it).
            for (int k = 0; k < Positions.Length; k++)
            {
                // 1 at k = Positions.Length - 1 (start) and 0 at k = 0 (end).
                float factorAlongTrail = (float)k / (Positions.Length - 1);

                // Uses the trail width function to decide the width of the trail at this point (if no function, use 
                float width = trailWidthFunction?.Invoke(factorAlongTrail) ?? defaultWidth;

                Vector2 current = Positions[k];
                Vector2 next = k == Positions.Length - 1 ? Positions[^1] + (Positions[^1] - Positions[^2]) : Positions[k + 1];

                Vector2 normalToNext = (next - current).SafeNormalize(Vector2.Zero);
                Vector2 normalPerp = normalToNext.RotatedBy(MathHelper.PiOver2);

                /* A
                 * |
                 * B---D
                 * |
                 * C
                 * 
                 * Let B be the current point and D be the next one.
                 * A and C are calculated based on the perpendicular vector to the normal from B to D, scaled by the desired width calculated earlier.
                 */

                Vector2 a = current + normalPerp * width;
                Vector2 c = current - normalPerp * width;

                /* Texture coordinates are calculated such that the top-left is (0, 0) and the bottom-right is (1, 1).
                 * To achieve this, we consider the Y-coordinate of A to be 0 and that of C to be 1, while the X-coordinate is just the factor along the trail.
                 * This results in the point last in the trail having an X-coordinate of 0, and the first one having a Y-coordinate of 1.
                 */
                var texCoordA = new Vector2(factorAlongTrail, 0);
                var texCoordC = new Vector2(factorAlongTrail, 1);

                // Calculates the color for each vertex based on its texture coordinates. This acts like a very simple shader (for more complex effects you can use the actual shader).
                Color colorA = trailColorFunction?.Invoke(texCoordA) ?? Color.White;
                Color colorC = trailColorFunction?.Invoke(texCoordC) ?? Color.White;

                /* 0---1---2
                 * |  /|  /|
                 * A / B / C
                 * |/  |/  |
                 * 3---4---5
                 * 
                 * Assuming we want vertices to be indexed in this format, where A, B, C, etc. are defining points and numbers are indices of mesh points:
                 * For a given point that is k positions along the chain, we want to find its indices.
                 * These indices are given by k for the above point and k + n for the below point.
                 */

                verticesTemp[k] = new VertexPositionColorTexture(a.Vec3(), colorA, texCoordA);
                verticesTemp[k + maxPointCount] = new VertexPositionColorTexture(c.Vec3(), colorC, texCoordC);
            }

            /* Now, we have to loop through the indices to generate triangles.
             * Looping to maxPointCount - 1 brings us halfway to the end; it covers the top row (excluding the last point on the top row).
             */
            for (short k = 0; k < maxPointCount - 1; k++)
            {
                /* 0---1
                 * |  /|
                 * A / B
                 * |/  |
                 * 2---3
                 * 
                 * This illustration is the most basic set of points (where n = 2).
                 * In this, we want to make triangles (2, 3, 1) and (1, 0, 2).
                 * Generalising this, if we consider A to be k = 0 and B to be k = 1, then the indices we want are going to be (k + n, k + n + 1, k + 1) and (k + 1, k, k + n)
                 */

                indicesTemp[k * 6] = (short)(k + maxPointCount);
                indicesTemp[k * 6 + 1] = (short)(k + maxPointCount + 1);
                indicesTemp[k * 6 + 2] = (short)(k + 1);
                indicesTemp[k * 6 + 3] = (short)(k + 1);
                indicesTemp[k * 6 + 4] = k;
                indicesTemp[k * 6 + 5] = (short)(k + maxPointCount);
            }

            // The next available index will be the next value after the count of points (starting at 0).
            nextAvailableIndex = verticesTemp.Length;

            vertices = verticesTemp;

            // Maybe we could use an array instead of a list for the indices, if someone figures out how to add indices to an array properly.
            indices = indicesTemp;
        }




        public void Render(Effect effect)
        {
            if (Positions == null || (primitives?.IsDisposed ?? true) || IsDisposed)
                return;

            SetupMeshes();

            primitives.Render(effect);

            stayAlive = 10; //Set stayalive to 10 frames as we render again, we will dispose this trail if it fails to render for 10 frames
        }

        private void SetupMeshes()
        {
            GenerateMesh(out VertexPositionColorTexture[] mainVertices, out short[] mainIndices, out int nextAvailableIndex);

            Vector2 toNext = (NextPosition - Positions[^1]).SafeNormalize(Vector2.Zero);

            tip.GenerateMesh(Positions[^1], toNext, nextAvailableIndex, out VertexPositionColorTexture[] tipVertices, out short[] tipIndices, trailWidthFunction, trailColorFunction);

            primitives.SetVertices(mainVertices.FastUnion(tipVertices));
            primitives.SetIndices(mainIndices.FastUnion(tipIndices));
        }

        public void Dispose()
        {
            primitives?.Dispose();
            IsDisposed = true;
        }
    }

    public class NoTip : ITrailTip
    {
        public int ExtraVertices => 0;

        public int ExtraIndices => 0;

        public void GenerateMesh(Vector2 trailTipPosition, Vector2 trailTipNormal, int startFromIndex, out VertexPositionColorTexture[] vertices, out short[] indices, TrailWidthFunction trailWidthFunction, TrailColorFunction trailColorFunction)
        {
            vertices = Array.Empty<VertexPositionColorTexture>();
            indices = Array.Empty<short>();
        }
    }

    public class TriangularTip : ITrailTip
    {
        private readonly float length;

        public int ExtraVertices => 3;

        public int ExtraIndices => 3;

        public TriangularTip(float length)
        {
            this.length = length;
        }

        public void GenerateMesh(Vector2 trailTipPosition, Vector2 trailTipNormal, int startFromIndex, out VertexPositionColorTexture[] vertices, out short[] indices, TrailWidthFunction trailWidthFunction, TrailColorFunction trailColorFunction)
        {
            /*     C
             *    / \
             *   /   \
             *  /     \
             * A-------B
             * 
             * This tip is arranged as the above shows.
             * Consists of a single triangle with indices (0, 1, 2) offset by the next available index.
             */

            Vector2 normalPerp = trailTipNormal.RotatedBy(MathHelper.PiOver2);

            float width = trailWidthFunction?.Invoke(1) ?? 1;
            Vector2 a = trailTipPosition + normalPerp * width;
            Vector2 b = trailTipPosition - normalPerp * width;
            Vector2 c = trailTipPosition + trailTipNormal * length;

            Vector2 texCoordA = Vector2.UnitX;
            Vector2 texCoordB = Vector2.One;
            var texCoordC = new Vector2(1, 0.5f);//this fixes the texture being skewed off to the side

            Color colorA = trailColorFunction?.Invoke(texCoordA) ?? Color.White;
            Color colorB = trailColorFunction?.Invoke(texCoordB) ?? Color.White;
            Color colorC = trailColorFunction?.Invoke(texCoordC) ?? Color.White;

            vertices = new VertexPositionColorTexture[]
            {
                new(a.Vec3(), colorA, texCoordA),
                new(b.Vec3(), colorB, texCoordB),
                new(c.Vec3(), colorC, texCoordC)
            };

            indices = new short[]
            {
                (short)startFromIndex,
                (short)(startFromIndex + 1),
                (short)(startFromIndex + 2)
            };
        }
    }

    // Note: Every vertex in this tip is drawn twice, but the performance impact from this would be very little
    public class RoundedTip : ITrailTip
    {
        // TriCount is the amount of tris the curve should have, higher means a better circle approximation. (Keep in mind each tri is drawn twice)
        private readonly int triCount;

        // The edge vextex count is count * 2 + 1, but one extra is added for the center, and there is one extra hidden vertex.
        public int ExtraVertices => triCount * 2 + 3;

        public int ExtraIndices => triCount * 2 * 3 + 5;

        public RoundedTip(int triCount = 2)//amount of tris
        {
            this.triCount = triCount;

            if (triCount < 2)
                throw new ArgumentException($"Parameter {nameof(triCount)} cannot be less than 2.");
        }

        public void GenerateMesh(Vector2 trailTipPosition, Vector2 trailTipNormal, int startFromIndex, out VertexPositionColorTexture[] vertices, out short[] indices, TrailWidthFunction trailWidthFunction, TrailColorFunction trailColorFunction)
        {
            /*   C---D
             *  / \ / \
             * B---A---E (first layer)
             * 
             *   H---G
             *  / \ / \
             * I---A---F (second layer)
             * 
             * This tip attempts to approximate a semicircle as shown.
             * Consists of a fan of triangles which share a common center (A).
             * The higher the tri count, the more points there are.
             * Point E and F are ontop of eachother to prevent a visual seam.
             */

            /// We want an array of vertices the size of the accuracy amount plus the center.
            vertices = new VertexPositionColorTexture[ExtraVertices];

            var fanCenterTexCoord = new Vector2(1, 0.5f);

            vertices[0] = new VertexPositionColorTexture(trailTipPosition.Vec3(), (trailColorFunction?.Invoke(fanCenterTexCoord) ?? Color.White) * 0.75f, fanCenterTexCoord);

            var indicesTemp = new List<short>();

            for (int k = 0; k <= triCount; k++)
            {
                // Referring to the illustration: 0 is point B, 1 is point E, any other value represent the rotation factor of points in between.
                float rotationFactor = k / (float)triCount;

                // Rotates by pi/2 - (factor * pi) so that when the factor is 0 we get B and when it is 1 we get E.
                float angle = MathHelper.PiOver2 - rotationFactor * MathHelper.Pi;

                Vector2 circlePoint = trailTipPosition + trailTipNormal.RotatedBy(angle) * (trailWidthFunction?.Invoke(1) ?? 1);

                // Handily, the rotation factor can also be used as a texture coordinate because it is a measure of how far around the tip a point is.
                var circleTexCoord = new Vector2(rotationFactor, 1);

                // The transparency must be changed a bit so it looks right when overlapped
                Color circlePointColor = (trailColorFunction?.Invoke(new Vector2(1, 0)) ?? Color.White) * rotationFactor * 0.85f;

                vertices[k + 1] = new VertexPositionColorTexture(circlePoint.Vec3(), circlePointColor, circleTexCoord);

                //if (k == triCount)//leftover and not needed
                //{
                //    continue;
                //}

                short[] tri = new short[]
                {
                    /* Because this is a fan, we want all triangles to share a common point. This is represented by index 0 offset to the next available index.
                     * The other indices are just pairs of points around the fan. The vertex k points along the circle is just index k + 1, followed by k + 2 at the next one along.
                     * The reason these are offset by 1 is because index 0 is taken by the fan center.
                     */

                    //before the fix, I believe these being in the wrong order was what prevented it from drawing
                    (short)startFromIndex,
                    (short)(startFromIndex + k + 2),
                    (short)(startFromIndex + k + 1)
                };

                indicesTemp.AddRange(tri);
            }

            // These 2 forloops overlap so that 2 points share the same location, this hidden point hides a tri that acts as a transition from one UV to another
            for (int k = triCount + 1; k <= triCount * 2 + 1; k++)
            {
                // Referring to the illustration: triCount + 1 is point F, 1 is point I, any other value represent the rotation factor of points in between.
                float rotationFactor = (k - 1) / (float)triCount - 1;

                // Rotates by pi/2 - (factor * pi) so that when the factor is 0 we get B and when it is 1 we get E.
                float angle = MathHelper.PiOver2 - rotationFactor * MathHelper.Pi;

                Vector2 circlePoint = trailTipPosition + trailTipNormal.RotatedBy(-angle) * (trailWidthFunction?.Invoke(1) ?? 1);

                // Handily, the rotation factor can also be used as a texture coordinate because it is a measure of how far around the tip a point is.
                var circleTexCoord = new Vector2(rotationFactor, 0);

                // The transparency must be changed a bit so it looks right when overlapped
                Color circlePointColor = (trailColorFunction?.Invoke(new Vector2(1, 0)) ?? Color.White) * rotationFactor * 0.75f;

                vertices[k + 1] = new VertexPositionColorTexture(circlePoint.Vec3(), circlePointColor, circleTexCoord);

                // Skip last point, since there is no point to pair with it.
                if (k == triCount * 2 + 1)
                    continue;

                short[] tri = new short[]
                {
                    /* Because this is a fan, we want all triangles to share a common point. This is represented by index 0 offset to the next available index.
                     * The other indices are just pairs of points around the fan. The vertex k points along the circle is just index k + 1, followed by k + 2 at the next one along.
                     * The reason these are offset by 1 is because index 0 is taken by the fan center.
                     */

                    //The order of the indices is reversed since the direction is backwards
                    (short)startFromIndex,
                    (short)(startFromIndex + k + 1),
                    (short)(startFromIndex + k + 2)
                };

                indicesTemp.AddRange(tri);
            }

            indices = indicesTemp.ToArray();
        }
    }

    public class TrailManager : ModSystem
    {
        public List<Trail> managed = new();

        public override void PostUpdateEverything()
        {
            foreach (Trail trail in managed)
            {
                trail.stayAlive--;

                if (trail.stayAlive <= 0)
                    trail.Dispose();
            }

            managed.RemoveAll(n => n.IsDisposed);
        }
    }

 
        public class PrimitiveDrawing : HookGroup
        {
            // Should not interfere with anything.
            public override void Load()
            {
                if (Main.dedServ)
                    return;

                On_Main.DrawDust += DrawPrimitives;
            }

            private void DrawPrimitives(On_Main.orig_DrawDust orig, Main self)
            {
                orig(self);

                if (Main.gameMenu)
                    return;

                Main.graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                for (int k = 0; k < Main.maxProjectiles; k++) // Projectiles.
                {
                    if (Main.projectile[k].active && Main.projectile[k].ModProjectile is IDrawPrimitive)
                        (Main.projectile[k].ModProjectile as IDrawPrimitive).DrawPrimitives();
                }

                for (int k = 0; k < Main.maxNPCs; k++) // NPCs.
                {
                    if (Main.npc[k].active && Main.npc[k].ModNPC is IDrawPrimitive)
                        (Main.npc[k].ModNPC as IDrawPrimitive).DrawPrimitives();
                }
            }
        }
    }

