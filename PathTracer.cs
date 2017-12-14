using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Spatial.Units;
using static PathTracer.Samplers;

namespace PathTracer
{
  class PathTracer
  {
    public Spectrum Li(Ray r, Scene s)
    {
      var L = Spectrum.ZeroSpectrum;
      var beta =  Spectrum.Create(1.0);
      var nbounces = 0;

      while (nbounces < 20)
      {
        var p1 = r.o;
        var p2 = r.d + p1;
        (double? distance, SurfaceInteraction si) = s.Intersect(r);
        if (!distance.HasValue || (p2 - si.Point).Length() < Renderer.Epsilon)//no elements between
        {
          break;
        }
        var wo = new Ray(r.d, r.o);//inverse r
        if (si.Obj is Light)
        {
          if (nbounces == 0)
          {
            L = beta * si.Le(r.o); // '*' is defined in Spectrum.cs
          }
          break;
        }
        var Ld = Samplers.UniformSampleDisk();
//        L = L.AddTo(beta * Ld);

        (Spectrum f, Vector3 wi, double pr) = ((Lambertian) ((Shape) si.Obj).BSDF).Sample_f(r.o);
        
//        if (nbounces>3)
//          var q = 1-
     
        nbounces++;
      }
      
//      r <- random ray from camera
//      L <- 0, 
//      β <- 1,
//      nbounces<-0
//      repeat while nbounces <20     
//        isect <-intersect r with scene 
//        if isect == null // no hit 
//          break
//        wo < -r
//      if isect isect isect == light // light hit
//        if nbounces == 0 // direct light hit 
//          L< -β*Le(wo)  // add light emitted
//        break 
//      Ld <-sample light from isect
//      L < -L+ β*Ld
//      (f, wi , pr) < -sample_bsdf (wo, isect)
//      β <- β*f*|cosθ|/ pr
//      r < -wi
//      if nbounces >3
//        q <- 1 - max(β)
//        if random() < q
//          break 
//        β <-β/(1 -q)
//       nbounces<-nbounces+1
      
      return L;
    }

    private int maxDepth = 20;

    public Spectrum Li2(Ray ray, Scene scene)
    {
      Spectrum L = Spectrum.Create(0.0);
      Spectrum beta = Spectrum.Create(1.0);
      bool specularBounce = false;
      for (int bounces = 0; ; ++bounces)
      {
        //Intersect ray with scene and store intersection in isect
        (double? distance, SurfaceInteraction isect) = scene.Intersect(ray);
        bool foundIntersection = distance.HasValue;
        
        //Possibly add emitted light at intersection 
        if (bounces == 0 || specularBounce)
        {
          //Add emitted light at path vertex or from the environment
          if (foundIntersection)
          {
            L = L.AddTo(beta * isect.Le(-ray.d));
          }         
          else
          {
            foreach (Light light in scene.Lights)
            {
              L = L.AddTo(beta * Spectrum.Create(0.0));
            }
          }
        }
        //Terminate path if ray escaped or maxDepth was reached
        if (!foundIntersection || bounces >= maxDepth)
        {
          break;
        }
        
        //Compute scattering functions and skip over medium boundaries
        //spustimo?//TODO
            
        //Sample illumination from lights to find path contribution//TODO
        Spectrum uniformSample = isect.Le();
        L = L.AddTo(beta * UniformSampleDisk());
        
        //Sample BSDF to get new path direction
        Vector3 wo = -ray.d;
        (Spectrum f, Vector3 wi, double pdf) = ((Lambertian) ((Shape) isect.Obj).BSDF).Sample_f(ray.o);//TODO
        if (f.IsBlack() || pdf == 0)
        {
          break;
        }
        beta *= f * Math.Abs(Vector3.Dot(wi, isect.Normal));//TODO vprašaj
        ray = isect.SpawnRay(wi);

        //Possibly terminate the path with Russian roulette
        if (bounces > 3)
        {
          double q = 1 - Math.Max(beta.c[0], beta.c[1]);//TODO vprašaj
          if (ThreadSafeRandom.NextDouble() < q)
          {
            break;
          }
          beta /= 1 - q;
        }
        
      }
      return L;
    }

  }
}
