global using System;
global using StereoKit;

SKSettings settings = new SKSettings
{
    appName = "Feesh",
    assetsFolder = "add",
    // displayPreference = DisplayMode.Flatscreen
};
if (!SK.Initialize(settings))
    Environment.Exit(1);

Model fishForest = Model.FromFile("fishforest.glb");
Model chair = Model.FromFile("chair.glb");

bool gotBounds = false;
float rotate = 0;
Matrix bounds = Matrix.T(0, -1.7f, 0);
Vec3 origin = Vec3.Zero;
SK.Run(() =>
{
    if (!gotBounds && World.HasBounds)
    {
        gotBounds = true;
        bounds = World.BoundsPose.ToMatrix();
        Console.WriteLine(World.BoundsPose.position);
    }
    float delta = Input.Controller(Handed.Right).stick.x * 90 * Time.Elapsedf;
    rotate += delta;
    Vec3 headPos = (Input.Head.position + Input.Head.orientation * -Vec3.Forward * 6 * U.cm).X0Z;
    origin -= headPos;
    origin = Quat.FromAngles(0, delta, 0) * origin;
    origin += headPos;
    
    origin += Input.Controller(Handed.Left).stick.X0Y * Time.Elapsedf;
    Renderer.CameraRoot = Matrix.TR(origin, Quat.FromAngles(0, rotate, 0)) * bounds.Inverse;


    fishForest.Draw(Matrix.Identity);
    chair.Draw(Matrix.S(1f / 50f));
});
// SK.Shutdown();
