using Nancy;
using DoctorOffice.Objects;

namespace DoctorOffice
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] View["index.cshtml"];
    }
  }

}
