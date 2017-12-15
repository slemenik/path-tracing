using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
   public class Lambertian : BxDF
  {
    private Spectrum kd;
    public Lambertian(Spectrum r)
    {
      kd = r;
    }

    public override Spectrum f(Vector3 wo, Vector3 wi)
    {
//      return Spectrum.ZeroSpectrum;
      return kd / Math.PI; // '/' operation is defined in Spectrum.cs
    }

    public override (Spectrum, Vector3, double) Sample_f(Vector3 wo)
    {
      /* Implement */
//      return (Spectrum.ZeroSpectrum, Vector3.ZeroVector, 0);
      var rand1 = Samplers.ThreadSafeRandom.NextDouble();
      var rand2 = Samplers.ThreadSafeRandom.NextDouble();

      var theta = Math.Asin(Math.Sqrt(rand1));
      var phi = 2 * Math.PI * rand2;
      Vector3 wi = Vector3.ZeroVector;
      wi.x = Math.Cos(phi) * Math.Sin(theta);
      wi.y = Math.Sin(phi) * Math.Sin(theta);
      wi.z = Math.Cos(theta);
      Spectrum f = this.f(wo, wi);
      double pr = Math.Cos(theta)/Math.PI;

      return (f, wi, pr);

    }

    public override double Pdf(Vector3 wo, Vector3 wi)
    {
      return Utils.CosTheta(wi) / Math.PI;
    }
  }
}
