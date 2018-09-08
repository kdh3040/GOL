#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("gWR22aoQVvlt3SFCgQyLrorThk4fETCCavVLK3Qma47d3OMxde8j9xCTnZKiEJOYkBCTk5IVXi22kjbHA3gi+e/2D+EIJ2I1s4wsZhe5lAWiEJOwop+Um7gU2hRln5OTk5eSkVbOKjBdVkybcnDHq7jZ/v0HOAdGjjhgvZGBja3VgpDSEcmF1CotpBkeSPDlhMpDZTqA3gO44la1wI2UoPnGyCzMgRfpARkmnKSYIXAQa7vqiUh9eucuJVRrXj+IvX4KcjdPXUXikH+2imxabw5os9NJXfritLEvuLl7YonevEyHlMQ6c1ZWC7D/7zeWZeWxOtiwCdVQdByIpt8Tsqh4/C7xi2f+kjxI99mR368l+H0qs7b3gxKI+AZOombN1ZCRk5KT");
        private static int[] order = new int[] { 12,10,10,3,12,6,10,11,12,10,10,12,13,13,14 };
        private static int key = 146;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
