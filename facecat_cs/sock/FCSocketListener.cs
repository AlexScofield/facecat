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
    /// �׽��ּ���
    /// </summary>
    public interface FCSocketListener {
        /// <summary>
        /// ���ݻص�
        /// </summary>
        /// <param name="socketID">�׽���ID</param>
        /// <param name="localSID">����ID</param>
        /// <param name="str">����</param>
        /// <param name="len">����</param>
        void callBack(int socketID, int localSID, byte[] str, int len);
        /// <summary>
        /// ��־�ص�
        /// </summary>
        /// <param name="socketID">�׽���ID</param>
        /// <param name="localSID">����ID</param>
        /// <param name="state">״̬</param>
        /// <param name="log">��־u</param>
        void writeLog(int socketID, int localSID, int state, String log);
    }
}
