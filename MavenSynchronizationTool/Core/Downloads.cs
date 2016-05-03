using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MavenSynchronizationTool.Core
{
    public class Downloads
    {
        public bool HttpDownloadFile(string url, string saveLocalFolder, string proxyHost = "", int proxyPort = 0, string proxyUser = "", string prxoyPwd = "")
        {
            bool result = false;

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(saveLocalFolder)) return result;
            string fileName = url.Substring(url.LastIndexOf("/") + 1);
            if (!Directory.Exists(saveLocalFolder)) Directory.CreateDirectory(saveLocalFolder);
            string filePath = Path.Combine(saveLocalFolder, fileName);
            if (File.Exists(filePath)) return true;

            Stream responseStream = null;
            Stream stream = null;
            try
            {
                //设置WebRequest参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)";
                if (!string.IsNullOrEmpty(proxyHost) && Spider.IsIP(proxyHost))
                {
                    //设置HTTP代理
                    request.Proxy = Spider.CreateHttpProxy(proxyHost, proxyPort, proxyUser, prxoyPwd);
                }

                //获取请求响应
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //获取请求发挥的流
                responseStream = response.GetResponseStream();
                //创建本地文件写入流
                stream = new FileStream(filePath, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                result = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (stream != null) stream.Close();
                if (responseStream != null) responseStream.Close();
            }

            return result;
        }
    }
}