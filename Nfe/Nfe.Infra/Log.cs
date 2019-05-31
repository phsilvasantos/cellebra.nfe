using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Nfe.Infra.Configuracao;
namespace Nfe.Infra
{
    public static class Log
    {
       

       public static void Save(string msgLog)
        {
            ConfiguracaoApp app = new ConfiguracaoApp();
            string Path = app.PastaXmlErro+"\\"+ DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
            using (FileStream fs = new FileStream(Path, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(msgLog);
                }
            }
        }
    }
}
