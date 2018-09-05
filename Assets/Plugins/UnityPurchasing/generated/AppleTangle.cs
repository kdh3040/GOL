#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("NxYBBFIDDRQNRnd3a2InTmlkKTZuYW5kZnNuaGknRnJzb2h1bnN+Nt4xeMaAUt6gnr41Rfzf0naZeaZVGIKEghyeOkAw9a6cR4kr07aXFd8rJ2RidXNuYW5kZnNiJ3doa25kfgiaOvQsTi8dz/nJsr4J3lkb0cw6kpl9C6NAjFzTETA0zMMISskTbtZgiA+zJ/DMqysnaHexOAY3i7BEyH4nZnR0cmpidCdmZGRid3NmaWRiMjU2Mzc0MV0QCjQyNzU3PjU2MzcBNwgBBFIaFAYG+AMCNwQGBvg3GndrYidVaGhzJ0RGNxkQCjcxNzM1I+Xs1rB32AhC5iDN9mp/6uCyEBAhNyMBBFIDDBQaRnd3a2InRGJ1c84edfJaCdJ4WJz1IgS9UohKWgr2KUeh8EBKeA9ZNxgBBFIaJAMfNxGFBgcBDi2BT4HwZGMCBjeG9TctAWMyJBJMEl4atJPw8ZuZyFe9xl9Xd2tiJ0RidXNuYW5kZnNuaGknRnIA63o+hIxUJ9Q/w7a4nUgNbPgs+07fcZg0E2KmcJPOKgUEBgcGpIUGsBy6lEUjFS3ACBqxSptZZM9MhxCspHaVQFRSxqgoRrT//OR3yuGkSydmaWMnZGJ1c25hbmRmc25oaSd3aWMnZGhpY25zbmhpdCdoYSdydGIoN4bEAQ8sAQYCAgAFBTeGsR2GtDQxXTdlNgw3DgEEUgMBFAVSVDYULYFPgfAKBgYCAgc3ZTYMNw4BBFIRNxMBBFIDBBQKRnd3a2InVWhoc2tiJ05pZCk2ITcjAQRSAwwUGkZ3cHApZnd3a2IpZGhqKGZ3d2tiZGZzbmFuZGZzYidlfidmaX4nd2Z1c8dkNHDwPQArUezdCCYJ3b10HkiyVWJrbmZpZGInaGknc29udCdkYnV9N4UGcTcJAQRSGggGBvgDAwQFBg9ZN4UGFgEEUhonA4UGDzeFBgM3OiFgJ400bfAKhcjZ7KQo/lRtXGNCeRhLbFeRRo7Dc2UMF4RGgDSNhjeFA7w3hQSkpwQFBgUFBgU3CgEOAgcEhQYIBzeFBg0FhQYGB+OWrg4DARQFUlQ2FDcWAQRSAw0UDUZ3d4wejtn+TGvyAKwlNwXvHzn/Vw7UJ0RGN4UGJTcKAQ4tgU+B8AoGBgYKAQ4tgU+B8AoGBgICBwSFBgYHWzGeSyp/sOqLnNv0cJz1cdVwN0jGr9t5JTLNItLeCNFs06UjJBbwpqteoAIOexBHURYZc9SwjCQ8QKTSaGVrYid0c2ZpY2Z1YydzYnVqdCdmiHSGZ8EcXA4olbX/Q0/3Zz+ZEvKyParzCAkHlQy2JhEpc9I7CtxlERiW3BlAV+wC6ll+gyrsMaVQS1LrufN0nOnVYwjMfkgz36U5/n/4bM91ZmRzbmRiJ3RzZnNiamJpc3QpN3hGr5/+1s1hmyNsFtekvOMcLcQYtjdf610DNYtvtIga2WJ0+GBZYrsPLAEGAgIABQYRGW9zc3d0PSgocIcTLNduQJNxDvnzbIopR6HwQEp4J2hhJ3NvYidzb2JpJ2Z3d2tuZGZzb2h1bnN+NhE3EwEEUgMEFApGdwEEUhoJAxEDEyzXbkCTcQ7582yKV62N0t3j+9cOADC3cnIm");
        private static int[] order = new int[] { 9,11,58,49,44,22,18,21,40,21,29,18,53,29,19,31,34,20,44,51,27,59,24,45,43,37,40,44,53,49,36,39,57,38,53,38,56,37,59,51,58,53,49,47,48,54,52,49,55,54,51,51,52,55,58,59,57,59,58,59,60 };
        private static int key = 7;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
