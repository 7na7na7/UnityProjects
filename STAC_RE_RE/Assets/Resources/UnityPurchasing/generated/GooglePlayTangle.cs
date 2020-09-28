#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("NUhj/C68aAsGGSGMX8nwyls7Bmv+FzvjVkj+hGlZFNH40YDGPspltkNhPe94Gg9I75m0REcqv8ddsgZ/pcx8xeA1if7sPv7xX6+zX19w7/WKCQcIOIoJAgqKCQkIojoC8d/KO6UZfPPvoy0wu/FocVFOZafP4yf8TRPQYaQ5CgtBKACBFiRbAIefnp4SFSbSbb7keQr9heAgdx+LgtcvGHuMNUoTq2u5E6IAonI8917IEVkUDKnyuy7NfoYck6JJNQ9qEV6B3Y3A4Do4C54bIQJXGx4bKVzCpQgTvjiKCSo4BQ4BIo5Ajv8FCQkJDQgLwc68b+7baSn03LGmswselpDFrJH/ijkSm3km5q+8yMgwjNaPr6SEmfbuK2m/z4aanQoLCQgJ");
        private static int[] order = new int[] { 2,10,3,5,10,9,7,7,11,13,10,13,12,13,14 };
        private static int key = 8;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
