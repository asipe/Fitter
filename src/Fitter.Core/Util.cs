namespace Fitter.Core {
  public static class Util {
     public static string ConvertToString(object value) {
       return (value == null) ? null : value.ToString();
     }
  }
}