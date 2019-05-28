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
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Net;
using System.Threading;

namespace FaceCat {
    /// <summary>
    /// ������׽���
    /// </summary>
    public class FCServerSocket {
        /// <summary>
        /// ͬ������
        /// </summary>
        private SocketAsyncEventArgs m_args;
        /// <summary>
        /// ���ݼ���
        /// </summary>
        public ArrayList<SOCKDATA> m_datas = new ArrayList<SOCKDATA>();
        /// <summary>
        /// �׽���ID
        /// </summary>
        public int m_hSocket;
        /// <summary>
        /// �˿�
        /// </summary>
        private int m_port;
        /// <summary>
        /// �׽��ֶ���
        /// </summary>
        private Socket m_socket = null;

        /// <summary>
        /// ����TCP���߳�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private unsafe void acceptHandleTCP(object sender, SocketAsyncEventArgs e) {
            try {
                Socket socket = e.AcceptSocket;
                e.AcceptSocket = null;
                SOCKDATA data = new SOCKDATA();
                data.m_socket = socket;
                int socketID = (int)socket.Handle;
                data.m_hSocket = socketID;
                m_datas[socketID] = data;
                String remoteIP = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
                int remotePort = ((IPEndPoint)socket.RemoteEndPoint).Port;
                String szAccept = String.Format("accept:{0}:{1}", remoteIP, remotePort);
                FCServerSockets.writeServerLog((int)socketID, m_hSocket, 1, szAccept);
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(onIoCompleted);
                args.SetBuffer(new byte[1024], 0, 1024);
                args.AcceptSocket = socket;
                if (!socket.ReceiveAsync(args)) {
                    processReceive(args);
                }
                beginAccept(e);
            } catch (Exception ex) {
            }
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <param name="e">����</param>
        private void beginAccept(SocketAsyncEventArgs e) {
            e.AcceptSocket = null;
            //�첽������ɣ�����false
            if (!m_socket.AcceptAsync(e)) {
                acceptHandleTCP(m_socket, e);
            }
        }

        /// <summary>
        /// �ر�
        /// </summary>
        /// <returns>״̬</returns>
        public int close() {
            if (m_socket != null) {
                try {
                    m_socket.Close();
                    return 1;
                } catch (Exception ex) {

                }
            }
            return -1;
        }

        /// <summary>
        /// �رտͻ���
        /// </summary>
        /// <param name="socketID">�׽���ID</param>
        /// <returns>״̬</returns>
        public int closeClient(int socketID) {
            try {
                if (m_datas[socketID] != null) {
                    m_datas[socketID].m_socket.Close();
                    return 1;
                }
            } catch (Exception ex) {
            }
            return -1;
        }

        /// <summary>
        /// ��ɶ˿�����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void onIoCompleted(object sender, SocketAsyncEventArgs e) {
            switch (e.LastOperation) {
                case SocketAsyncOperation.Receive:
                    processReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    processSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="e">����</param>
        private void processReceive(SocketAsyncEventArgs e) {
            try {
                if (e.BytesTransferred > 0) {
                    if (e.SocketError == SocketError.Success) {
                        byte[] buffer = e.Buffer;
                        int socketID = (int)e.AcceptSocket.Handle;
                        SOCKDATA data = m_datas[socketID];
                        if (data != null) {
                            data.m_buffer = e.Buffer;
                            data.m_len = e.BytesTransferred;
                            int state = recv(data);
                            if (state == -1) {
                                data.m_socket.Close();
                                FCServerSockets.writeServerLog((int)socketID, m_hSocket, 2, "socket exit");
                                m_datas[socketID] = null;
                                return;
                            }
                        }
                        if (!e.AcceptSocket.ReceiveAsync(e)) {
                            processReceive(e);
                        }
                    }
                } else {
                    int socketID2 = (int)e.AcceptSocket.Handle;
                    SOCKDATA data2 = m_datas[socketID2];
                    data2.m_socket.Close();
                    m_datas[socketID2] = null;
                    FCServerSockets.writeServerLog(socketID2, m_hSocket, 2, "socket exit");
                }
            } catch (Exception ex) {
            }
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="e">����</param>
        private void processSend(SocketAsyncEventArgs e) {
            if (e.SocketError == SocketError.Success) {
                //������Ͽ�ʼ�´ν���
                if (!e.AcceptSocket.ReceiveAsync(e)) {
                    this.processReceive(e);
                }
            } else {

            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="data">����</param>
        /// <returns>״̬</returns>
        public int recv(SOCKDATA data) {
            if (!data.m_submit) {
                if (data.m_len == 1024 && data.m_buffer[0] == 'm' && data.m_buffer[1] == 'i' && data.m_buffer[2] == 'a' && data.m_buffer[3] == 'o') {
                    data.m_submit = true;
                    return 1;
                } else {
                    return -1;
                }
            }
            int intSize = 4;
            data.m_index = 0;
            while (data.m_index < data.m_len) {
                int diffSize = 0;
                if (!data.m_get) {
                    diffSize = intSize - data.m_headSize;
                    if (diffSize == 0) {
                        data.m_head = (((data.m_buffer[data.m_index] | (data.m_buffer[data.m_index + 1] << 8)) | (data.m_buffer[data.m_index + 2] << 0x10)) | (data.m_buffer[data.m_index + 3] << 0x18));
                    } else {
                        for (int i = 0; i < diffSize; i++) {
                            data.m_headStr[data.m_headSize + i] = data.m_buffer[i];
                        }
                        data.m_head = (((data.m_headStr[0] | (data.m_headStr[1] << 8)) | (data.m_headStr[2] << 0x10)) | (data.m_headStr[3] << 0x18));
                    }
                    if (data.m_str != null) {
                        data.m_str = null;
                    }
                    data.m_str = new byte[data.m_head];
                    if (diffSize > 0) {
                        for (int i = 0; i < data.m_headSize; i++) {
                            data.m_str[i] = data.m_headStr[i];
                        }
                        data.m_pos += data.m_headSize;
                        data.m_headSize = intSize;
                    }
                }
                data.m_bufferRemain = data.m_len - data.m_index;
                data.m_strRemain = data.m_head - data.m_pos;
                data.m_get = data.m_strRemain > data.m_bufferRemain;
                int remain = Math.Min(data.m_strRemain, data.m_bufferRemain);
                Array.Copy(data.m_buffer, data.m_index, data.m_str, data.m_pos, remain);
                data.m_pos += remain;
                data.m_index += remain;
                if (!data.m_get) {
                    FCServerSockets.recvClientMsg(data.m_hSocket, m_hSocket, data.m_str, data.m_head);
                    data.m_head = 0;
                    data.m_pos = 0;
                    if (data.m_len - data.m_index == 0 || data.m_len - data.m_index >= intSize) {
                        data.m_headSize = intSize;
                    } else {
                        data.m_headSize = data.m_bufferRemain - data.m_strRemain;
                        for (int j = 0; j < data.m_headSize; j++) {
                            data.m_headStr[j] = data.m_buffer[data.m_index + j];
                        }
                        break;
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="socketID">�׽���ID</param>
        /// <param name="str">�ַ���</param>
        /// <param name="len">����</param>
        /// <returns>״̬</returns>
        public int send(int socketID, byte[] str, int len) {
            lock (this) {
                if (m_socket == null) {
                    return -1;
                }
                try {
                    return m_datas[socketID].m_socket.Send(str);
                } catch (Exception ex) {
                    return -1;
                }
            }
        }

        /// <summary>
        /// ��ʼTCP����
        /// </summary>
        /// <param name="port">�˿�</param>
        /// <returns>״̬</returns>
        public unsafe int startTCP(int port) {
            m_port = port;
            try {
                IPEndPoint ipe = new IPEndPoint(IPAddress.Any, m_port);
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_socket.Bind(ipe);
                m_socket.Listen(0);
                m_hSocket = (int)m_socket.Handle;
                m_args = new SocketAsyncEventArgs();
                m_args.Completed += new EventHandler<SocketAsyncEventArgs>(acceptHandleTCP);
                m_datas = new ArrayList<SOCKDATA>();
                for (int i = 0; i < 100000; i++) {
                    m_datas.add(null);
                }
                m_socket.AcceptAsync(m_args);
                return 1;
            } catch (Exception ex) {
            }
            return -1;
        }
    }
}
