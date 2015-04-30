using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task<List<string>> GetCodesAsync()
        {
            return HasCodeFile ? await ReadAllLinesAsync(CodeAddress.FilePath, Encoding.UTF8, true) : null;
        }



        public static async Task<List<string>> ReadAllLinesAsync(string path, Encoding encoding, bool byLineNo = false)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(path, encoding))
            {
                string line;
                var index = 0;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(string.Format("{0}{1}",
                        byLineNo ? ++index + "\t" : "", line));
                }
            }

            return lines;
        }

        #endregion
    }
}
