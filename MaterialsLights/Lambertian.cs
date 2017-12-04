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
      /* Implement */
      return Spectrum.ZeroSpectrum;
    }

    public override (Spectrum, Vector3, double) Sample_f(Vector3 wo)
    {
      /* Implement */
      return (Spectrum.ZeroSpectrum, Vector3.ZeroVector, 0);
    }

    public override double Pdf(Vector3 wo, Vector3 wi)
    {
      /* Implement */
      return 0;
    }
  }
}
