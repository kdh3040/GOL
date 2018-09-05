#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("MPHEw16XnO3S54YxBMezy4725Pzvd5OJ5O/1IsvJfhIBYEdEvoG+/1spxg8z1ePWt9EKavDkQ1sNCJYBSDLeRyuF8U5gKGYWnEHEkwoPTjo3gdkEKDg0FGw7KWuocDxtk5QdoDjdz2ATqe9A1GSY+zi1Mhczaj/3G6kqCRsmLSIBrWOt3CYqKiouKyin8UlcPXP63IM5Z7oBW+8MeTQtGQDC2zBnBfU+LX2Dyu/vsglGVo4v3FwIg2EJsGzpzaUxH2aqCxHBRZepKiQrG6kqISmpKiorrOeUDyuPfrrBm0BWT7ZYsZ7bjAo1ld+uAC28QH9xlXU4rlC4oJ8lHSGYyanSAlOmqIk700zyks2f0jdkZVqIzFaaTqsxQb/3G990bCkoKisq");
        private static int[] order = new int[] { 5,6,9,7,9,12,12,11,8,13,12,11,13,13,14 };
        private static int key = 43;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
