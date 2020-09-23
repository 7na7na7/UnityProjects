#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("/VgDSt88j3ftYlO4xP6b4K9wLHxUPY00EcR4Dx3PDwCuXkKuroEeBHv49vnJe/jz+3v4+PlTy/MALjvKVOiNAh5S3MFKAJmAoL+UVj4S1g2KfcS74lqaSOJT8VODzQavOeCo5bziIZBVyPv6sNnxcOfVqvF2bm9vDnvI42qI1xdeTTk5wX0nfl5VdWgP5soSp7kPdZio5SAJIHE3zzuUR+Pk1yOcTxWI+wx0EdGG7npzJt7pMRHLyfpv6tDzpurv6titM1T54k+ykMweiev+uR5oRbW22042rEP3jjA/TZ4fKpjYBS1AV0L672dhNF1gyXv428n0//DTf7F/DvT4+Pj8+frEuZIN302Z+vfo0H2uOAE7qsr3mgcf2phOPndrbPv6+Pn4");
        private static int[] order = new int[] { 13,5,5,9,11,7,8,10,8,11,11,12,13,13,14 };
        private static int key = 249;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
