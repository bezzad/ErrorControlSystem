using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ErrorControlSystem.Shared
{
    public delegate void CodeMapEventHandler(Object sender, CodeMapEventArgs e);


    [Serializable]
    public class CodeMapEventArgs : EventArgs
    {
        public CodeMapEventArgs(CodeScope address)
        {
            CodeAddress = address;
        }


        #region Properties

        public CodeScope CodeAddress { get; set; }

        public bool HasCodeFile
        {
            get
            {
                return CodeAddress != null &&
                       !String.IsNullOrEmpty(CodeAddress.FilePath) &&
                       File.Exists(CodeAddress.FilePath);
            }
        }

        #endregion

        #region Methods

        public async Task<string[]> GetCodesAsync()
        {
            return HasCodeFile ? await ReadAllLinesAsync(CodeAddress.FilePath, Encoding.UTF8) : null;
        }



        public static async Task<string[]> ReadAllLinesAsync(string path, Encoding encoding)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(path, encoding))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }

        #endregion
    }
}
