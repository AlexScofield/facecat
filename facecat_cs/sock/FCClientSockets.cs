/*����èFaceCat��� v1.0
 1.��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 3.���ϴ�ʼ��-Ф����(΢�ź�:xiaotianlong_luu);
 4.���Ͽ�����-������(΢�ź�:chenxiaoyangzxy)������-���(΢�ź�:cnnic_zhu);
 5.�ÿ�ܿ�ԴЭ��ΪBSD����ӭ�����ǵĴ�ҵ����и���֧�֣���ӭ���࿪���߼��롣
 ����C/C++,Java,C#,iOS,MacOS,Linux�����汾��ͼ�κ�ͨѶ�����ܡ�
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace FaceCat {
    /// <summary>
    /// �ͻ����׽��ֹ���
    /// </summary>
    public class FCClientSockets {
        /// <summary>
        /// �ͻ����׽��ּ���
        /// </summary>
        private static HashMap<int, FCClientSocket> m_clients = new HashMap<int, FCClientSocket>();

        /// <summary>
        /// ������
        /// </summary>
        private static FCSocketListener m_listener;

        /// <summary>
        /// �׽���ID
        /// </summary>
        private static int m_socketID;

        /// <summary>
        /// �رշ���
        /// </summary>
        /// <param name="socketID">�׽���ID</param>
        /// <returns>״̬</returns>
        public static int close(int socketID) {
            int ret = -1;
            if (m_clients.containsKey(socketID)) {
                FCClientSocket client = m_clients.get(socketID);
                ret = client.close();
                m_clients.remove(socketID);
            }
            return ret;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="proxyType">��������</param>
        /// <param name="ip">IP��ַ</param>
        /// <param name="port">�˿�</param>
        /// <param name="proxyIp">����IP</param>
        /// <param name="proxyPort">����˿�</param>
        /// <param name="proxyUserName">�û���</param>
        /// <param name="proxyUserPwd">����</param>
        /// <param name="proxyDomain">��</param>
        /// <param name="timeout">��ʱ</param>
        /// <returns>״̬</returns>
        public static int connect(int type, int proxyType, String ip, int port, String proxyIp, int proxyPort, String proxyUserName, String proxyUserPwd, String proxyDomain, int timeout) {
            FCClientSocket client = new FCClientSocket(type, (long)proxyType, ip, port, proxyIp, proxyPort, proxyUserName, proxyUserPwd, proxyDomain, timeout);
            ConnectStatus ret = client.connect();
            if (ret != ConnectStatus.CONNECT_SERVER_FAIL) {
                m_socketID++;
                int socketID = m_socketID;
                client.m_hSocket = m_socketID;
                m_clients.put(socketID, client);
                return socketID;
            }
            else {
                client.delete();
                return -1;
            }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="socketID">�׽���ID</param>
        /// <param name="localSID">����ID</param>
        /// <param name="str">����</param>
        /// <param name="len">����</param>
        public static void recvClientMsg(int socketID, int localSID, byte[] str, int len) {
            m_listener.callBack(socketID, localSID, str, len);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="socketID">�׽���ID</param>
        /// <param name="str">����</param>
        /// <param name="len">����</param>
        /// <returns>״̬</returns>
        public static int send(int socketID, byte[] str, int len) {
            int ret = -1;
            if (m_clients.containsKey(socketID)) {
                FCClientSocket client = m_clients.get(socketID);
                ret = client.send(str, len);
            }
            return ret;
        }

        /// <summary>
        /// ����UDP����
        /// </summary>
        /// <param name="socketID">�׽���ID</param>
        /// <param name="str">����</param>
        /// <param name="len">����</param>
        /// <returns>״̬</returns>
        public static int sendTo(int socketID, byte[] str, int len) {
            int ret = -1;
            if (m_clients.containsKey(socketID)) {
                FCClientSocket client = m_clients.get(socketID);
                ret = client.sendTo(str, len);
            }
            return ret;
        }

        /// <summary>
        /// ���ü�����
        /// </summary>
        /// <param name="listener">������</param>
        /// <returns>״̬</returns>
        public static int setListener(FCSocketListener listener) {
            m_listener = listener;
            return 1;
        }

        /// <summary>
        /// д��־�Ļص�
        /// </summary>
        /// <param name="socketID">�׽���ID</param>
        /// <param name="localSID">����ID</param>
        /// <param name="state">״̬</param>
        /// <param name="log">��־</param>
        public static void writeClientLog(int socketID, int localSID, int state, String log) {
            m_listener.writeLog(socketID, localSID, state, log);
        }
    }
}
