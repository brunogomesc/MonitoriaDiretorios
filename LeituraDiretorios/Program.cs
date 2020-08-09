using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace LeituraDiretorios
{
    class Program
    {

        private static FileSystemWatcher _monitorar;
        private static StreamWriter escreverLog;
        private static string caminhoArqLog = $@"Z:\DEVTEC\LOG\{DateTime.Now.ToString("yyyyMMdd")}.txt";

        static void Main(string[] caminho)
        {

            string path = @"Z:\DEVTEC\FTP\";
            string filtro = "*.*";


            escreverLog = File.AppendText(caminhoArqLog);
            escreverLog.WriteLine($"{DateTime.Now} - ========================================");
            escreverLog.WriteLine($"{DateTime.Now} - ======== INICIANDO GRAVACAO LOG ========");
            escreverLog.WriteLine($"{DateTime.Now} - ========================================\n");
            escreverLog.Close();

            Console.WriteLine(DateTime.Now + " - Monitorando a pasta de FTP do Servidor SRV_DEVTEC com FileSystemWatcher");
            criarLog(DateTime.Now + " - Monitorando a pasta de FTP do Servidor SRV_DEVTEC com FileSystemWatcher");

            MonitorarArquivos(path, filtro);
            Console.ReadKey();

            escreverLog = File.AppendText(caminhoArqLog);
            escreverLog.WriteLine($"\n{DateTime.Now} - =========================================");
            escreverLog.WriteLine($"{DateTime.Now} - ======== ENCERRANDO GRAVACAO LOG ========");
            escreverLog.WriteLine($"{DateTime.Now} - =========================================\n");
            escreverLog.Close();

        }

        public static void MonitorarArquivos(string path, string filtro)
        {

            _monitorar = new FileSystemWatcher(path, filtro)
            {
                IncludeSubdirectories = true
            };

            _monitorar.Created += OnFileChanged;
            _monitorar.Changed += OnFileChanged;
            _monitorar.Deleted += OnFileChanged;
            _monitorar.Renamed += OnFileRenamed;

            _monitorar.EnableRaisingEvents = true;

            Console.WriteLine(DateTime.Now + $" - Monitorando arquivos do tipo: {filtro} \n");
            criarLog(DateTime.Now + $" - Monitorando arquivos do tipo: {filtro}");

        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {

            string tipo = e.ChangeType.ToString();
            
            if (tipo.Equals("Created"))
            {

                Console.WriteLine(DateTime.Now + $" - O Arquivo {e.Name} {e.ChangeType}");
                criarLog(DateTime.Now + $" - O Arquivo {e.Name} {e.ChangeType}");
                criarPastaBkp(e.Name.ToString());

            }
            else
            {

                Console.WriteLine(DateTime.Now + $" - O Arquivo {e.Name} {e.ChangeType}");
                criarLog(DateTime.Now + $" - O Arquivo {e.Name} {e.ChangeType}");

            }

            

        }

        private static void OnFileRenamed(object sender, RenamedEventArgs e)
        {

            Console.WriteLine(DateTime.Now + $" - O Arquivo {e.OldName} {e.ChangeType} para {e.Name}");
            criarLog(DateTime.Now + $" - O Arquivo {e.OldName} {e.ChangeType} para {e.Name}");

        }

        private static void criarLog(String log) {

            escreverLog = File.AppendText(caminhoArqLog);

            escreverLog.WriteLine(log);

            escreverLog.Close();

        }

        private static void criarPastaBkp(String arquivo)
        {

            String data = DateTime.Now.ToString("yyyyMMdd");

            try
            {

                if (Directory.Exists($@"Z:\DEVTEC\BKP\{data}"))
                {

                    Console.WriteLine(DateTime.Now + $@" - Diretório já existe em: Z:\DEVTEC\BKP\{data}");
                    criarLog(DateTime.Now + $@" - Diretório já existe em: Z:\DEVTEC\BKP\{data}");
                    criarBkp($@"Z:\DEVTEC\BKP\{data}", arquivo);

                }
                else
                {

                    System.IO.Directory.CreateDirectory($@"Z:\DEVTEC\BKP\{data}");
                    Console.WriteLine(DateTime.Now + $@" - Diretório criado com sucesso em: Z:\DEVTEC\BKP\{data}");
                    criarLog(DateTime.Now + $@" - Diretório criado com sucesso em: Z:\DEVTEC\BKP\{data}");
                    criarBkp($@"Z:\DEVTEC\BKP\{data}", arquivo);

                }
                
            }
            catch (Exception e)
            {

                Console.WriteLine(DateTime.Now + " - " + e.ToString());
                criarLog(DateTime.Now + " - " + e.ToString());

            }

        }

        private static void criarBkp(String caminhoBkp, String arq)
        {

            try
            {
                string file = Path.GetFileName(arq);

                System.IO.File.Copy($@"Z:\DEVTEC\FTP\{arq}", System.IO.Path.Combine(caminhoBkp, file), true);
                Console.WriteLine(DateTime.Now + $" - O arquivo {file} foi copiado com sucesso para a basta {caminhoBkp}.");
                criarLog(DateTime.Now + $" - O arquivo {file} foi copiado com sucesso para a basta {caminhoBkp}.");

            }
            catch(Exception e)
            {

                Console.WriteLine(DateTime.Now + " - " + e.ToString());
                criarLog(DateTime.Now + " - " + e.ToString());

            }

        }

    }

}
