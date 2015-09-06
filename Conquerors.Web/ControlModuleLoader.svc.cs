using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Conquerors.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ControlModuleLoader
    {
        [OperationContract]
        public List<string> GetModuleList()
        {
            List<string> listOfFiles = new List<string>();
            string path = "C:/Users/lugerovac/Documents/visual studio 2013/Projects/Conquerors/Conquerors.Web/ClientBin/";
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                string editedFileName;
                editedFileName = file.Replace(path, string.Empty);
                if (!string.Equals(editedFileName, "Conquerors.xap")) listOfFiles.Add(editedFileName);
            }
            return listOfFiles;
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
