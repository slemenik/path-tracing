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
    public Spectrum Li(Ray ray, Scene scene)
    {
      Spectrum L = Spectrum.ZeroSpectrum;
      Spectrum beta = Spectrum.Create(1.0);
      int bounces = 0;
      while (bounces < 20)
      {
        //Intersect ray with scene and store intersection in isect
        (double? distance, SurfaceInteraction isect) = scene.Intersect(ray);
        if (!distance.HasValue)
        {
          break;
        }
        Vector3 wo = -ray.d;

        //Possibly add emitted light at intersection 
        if (isect.Obj is Light)
        {
          //Add emitted light at path vertex or from the environment
          if (bounces == 0)
          {
            L = L.AddTo(beta * isect.Le(wo));
          }
          break;
        }

        //Sample illumination from lights to find path contribution
        L = L.AddTo(beta * Light.UniformSampleOneLight(isect, scene));

        //Sample BSDF to get new path direction
        (Spectrum f, Vector3 wi, double pdf, bool isSpecular) = ((Shape) isect.Obj).BSDF.Sample_f(wo, isect);
        if (f.IsBlack() || pdf == 0.0000)
        {
          break;
        }
        beta *= f * Math.Abs(Vector3.Dot(wi, isect.Normal)) / pdf;
        ray = isect.SpawnRay(wi);

        //Possibly terminate the path with Russian roulette
        if (bounces > 3)
        {
          double q = Math.Max(0.05, 1 - Math.Max(beta.c[0], Math.Max(beta.c[1],beta.c[2])));
          if (ThreadSafeRandom.NextDouble() < q)
          {
            break;
          }
          beta /= 1 - q;
        }
        bounces++;

      }
      return L;
    }
  }
}
