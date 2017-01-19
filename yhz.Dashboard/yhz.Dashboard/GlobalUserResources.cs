using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace yhz.Dashboard
{
    public static class GlobalResources
    {
        public const string CONST_USER_IMAGE_PATH = @"Content\UserData\image";
        public const string CONST_USER_AUDIO_PATH = @"Content\UserData\audio";
        public const string CONST_USER_VIDEO_PATH = @"Content\UserData\video";
        const string CONST_THEMES_PATH = @"Content\themes";

        static GlobalResources()
        {

        }

        /// <summary>
        /// 所有的用户图片
        /// key 文件名
        /// value 路径（相对）
        /// </summary>
        public static List<string> UserImages
        {
            get
            {
                return Directory.GetFiles(
                    Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    CONST_USER_IMAGE_PATH
                    ), "*", SearchOption.AllDirectories)
                    .Select(s =>
                        s.Substring(s.LastIndexOf("\\") + 1, s.Length - s.LastIndexOf("\\") - 1)
                        )
                    .ToList();
            }
        }

        public static List<string> UserAudios
        {
            get
            {
                return Directory.GetFiles(
                        Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        CONST_USER_AUDIO_PATH
                        ), "*", SearchOption.AllDirectories)
                        .Select(s =>
                            s.Substring(s.LastIndexOf("\\") + 1, s.Length - s.LastIndexOf("\\") - 1)
                            )
                        .ToList();
            }
        }

        public static List<string> UserVideos
        {
            get
            {
                return Directory.GetFiles(
                    Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    CONST_USER_VIDEO_PATH
                    ), "*", SearchOption.AllDirectories)
                    .Select(s =>
                        s.Substring(s.LastIndexOf("\\") + 1, s.Length - s.LastIndexOf("\\") - 1)
                        )
                    .ToList();
            }
        }

        public static List<string> Themes
        {
            get {
                return Directory.GetDirectories(
                    Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    CONST_THEMES_PATH
                    ), "*", SearchOption.TopDirectoryOnly)
                    .Select(s =>
                        s.Substring(s.LastIndexOf("\\") + 1, s.Length - s.LastIndexOf("\\") - 1)
                        )
                    .Where(s=>!s.Contains("fonts"))
                    .ToList();
            }
        }
    }
}