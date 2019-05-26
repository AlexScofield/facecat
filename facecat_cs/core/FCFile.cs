/*����èFaceCat��� v1.0
 1.��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 3.���ϴ�ʼ��-�ӱ�˼����ҵ������ѯ���޹�˾�ϻ���-Ф����(΢�ź�:xiaotianlong_luu);
 4.���Ͽ�����-������(΢�ź�:chenxiaoyangzxy)������-���(΢�ź�:cnnic_zhu);
 5.�ÿ�ܿ�ԴЭ��ΪBSD����ӭ�����ǵĴ�ҵ����и���֧�֣���ӭ���࿪���߼��롣
 ����C/C++,Java,C#,iOS,MacOS,Linux�����汾��ͼ�κ�ͨѶ�����ܡ�
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace FaceCat
{
    /// <summary>
    /// �ļ�������
    /// </summary>
    public class FCFile
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(String lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;

        /// <summary>
        /// ���ļ���׷������
        /// </summary>
        /// <param name="file">�ļ�</param>
        /// <param name="content">����</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool append(String file, String content)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(content);
                sw.Close();
                fs.Dispose();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="dir">�ļ���</param>
        public static void createDirectory(String dir)
        {
            Directory.CreateDirectory(dir);
        }

        /// <summary>
        /// ��ȡ�ļ����е��ļ���
        /// </summary>
        /// <param name="dir">�ļ���</param>
        /// <param name="dirs">�ļ��м���</param>
        /// <returns></returns>
        public static bool getDirectories(String dir, ArrayList<String> dirs)
        {
            if (Directory.Exists(dir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                DirectoryInfo[] lstDir = dirInfo.GetDirectories();
                int lstDirSize = lstDir.Length;
                if (lstDirSize > 0)
                {
                    for (int i = 0; i < lstDirSize; i++)
                    {
                        dirs.add(lstDir[i].FullName);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��ȡ�ļ����е��ļ�
        /// </summary>
        /// <param name="dir">�ļ���</param>
        /// <param name="files">�ļ�����</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool getFiles(String dir, ArrayList<String> files)
        {
            if (Directory.Exists(dir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                FileInfo[] lstFile = dirInfo.GetFiles();
                int lstFileSize = lstFile.Length;
                if (lstFileSize > 0)
                {
                    for (int i = 0; i < lstFileSize; i++)
                    {
                        files.add(lstFile[i].FullName);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// �ж��ļ����Ƿ����
        /// </summary>
        /// <param name="dir">�ļ���</param>
        /// <returns>�Ƿ����</returns>
        public static bool isDirectoryExist(String dir)
        {
            return Directory.Exists(dir);
        }

        /// <summary>
        /// �ж��ļ��Ƿ����
        /// </summary>
        /// <param name="file">�ļ�</param>
        /// <returns>�Ƿ����</returns>
        public static bool isFileExist(String file)
        {
            return File.Exists(file);
        }

        /// <summary>
        /// ���ļ��ж�ȡ����
        /// </summary>
        /// <param name="file">�ļ�</param>
        /// <param name="content">��������</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool read(String file, ref String content)
        {
            try
            {
                if (File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Open);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    content = sr.ReadToEnd();
                    sr.Close();
                    fs.Dispose();
                    return true;
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// �Ƴ��ļ�
        /// </summary>
        /// <param name="file">�ļ�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool removeFile(String file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
                return true;
            }
            return false;
        }

        /// <summary>
        /// ���ļ���д������
        /// </summary>
        /// <param name="file">�ļ�</param>
        /// <param name="content">����</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool write(String file, String content)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(content);
                sw.Close();
                fs.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}