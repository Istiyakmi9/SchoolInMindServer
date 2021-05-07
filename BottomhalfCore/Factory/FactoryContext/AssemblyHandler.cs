using BottomhalfCore.IFactoryContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.FactoryContext
{
    public class AssemblyHandler : IAssemblyHandler
    {
        public List<Assembly> LoadNamedAssemblies(string AsmName, ref string Bindir)
        {
            string Name = null;
            Assembly asm = null;
            List<Assembly> Assemblies = new List<Assembly>();
            var AsmFiles = Directory.GetFiles(Bindir, "*.dll", SearchOption.AllDirectories);
            var AsmExeFiles = Directory.GetFiles(Bindir, "*.exe", SearchOption.AllDirectories);
            if (AsmName != null)
            {
                foreach (string dll in AsmFiles)
                {
                    Name = dll.Substring(dll.LastIndexOf(@"\"), dll.Length - dll.LastIndexOf(@"\")).Replace(@"\", "").Replace(".dll", "");
                    if (AsmName == Name)
                    {
                        asm = null;
                        asm = Assembly.LoadFrom(dll);
                        Assemblies.Add(asm);
                        break;
                    }
                }

                if (AsmExeFiles.Count() > 0)
                {
                    foreach (string ExeFile in AsmExeFiles)
                    {
                        Name = ExeFile.Substring(ExeFile.LastIndexOf(@"\"), ExeFile.Length - ExeFile.LastIndexOf(@"\")).Replace(@"\", "").Replace(".exe", "");
                        if (Name == AsmName)
                        {
                            asm = null;
                            asm = Assembly.LoadFile(ExeFile);
                            Assemblies.Add(asm);
                            break;
                        }
                    }
                }
            }

            return Assemblies;
        }
    }
}
