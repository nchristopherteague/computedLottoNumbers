using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LotteryStatistics.Console
{
    public class OutputManager : IDisposable
    {

        #region Properties

        private readonly bool isFileMode = false;
        private FileStream fs = null;
        private TextWriter tw = null;
        private StreamWriter sw = null;

        public string FilePath { get; private set; }

        #endregion

        public OutputManager (bool outputFile, string filePath="")
        {
            this.isFileMode = outputFile;
            this.FilePath = filePath;

            if(isFileMode)
            {
                CreateFileMode();
            }

        }
        private void CreateFileMode()
        {
            var fileName = this.FilePath + "statistics_" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".txt";
            if (File.Exists(FilePath + fileName))
            {
                fileName = this.FilePath + "statistics_" + DateTime.Now.AddTicks(100).ToString("MMddyyyy-HHmmss") + ".txt";
            }

            fs = new FileStream(fileName, FileMode.Create);
            tw = System.Console.Out;
            sw = new StreamWriter(fs) {AutoFlush = true};
            System.Console.SetOut(sw);
            System.Console.SetError(sw);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Output() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            if(isFileMode && sw != null)
            {
                sw.Close();
                fs.Close();

                System.Console.SetOut(tw);
            }           

            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
