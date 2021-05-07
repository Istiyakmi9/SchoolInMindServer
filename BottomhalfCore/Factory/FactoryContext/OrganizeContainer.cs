using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BottomhalfCore.BottomhalfModel;
using System.Collections.Concurrent;
using IFactoryContext.IFactoryContext;

namespace BottomhalfCore.FactoryContext
{
    public class OrganizeContainer : IOrganizeContainer
    {
        public ConcurrentDictionary<string, GraphContainerModal> TypeContainerDetail = null;
        public ConcurrentDictionary<string, GraphContainerModal> ReOrganizeContainer(IDictionary<string, TypeRefCollection> ClassTypeCollection)
        {
            string[] Modules = null;
            string NodeName = null;
            string TypeName = null;
            GraphContainerModal ObjGraphContainerModal = null;
            foreach (KeyValuePair<string, TypeRefCollection> TypeDetail in ClassTypeCollection)
            {
                Modules = null;
                Modules = TypeDetail.Key.Split(new char[] { '.' });
                if (Modules != null && Modules.Length > 0)
                {
                    if (TypeDetail.Key.IndexOf('.') != -1)
                    {
                        NodeName = TypeDetail.Key.Substring(0, TypeDetail.Key.LastIndexOf('.'));
                        TypeName = TypeDetail.Key.Substring(TypeDetail.Key.LastIndexOf('.') + 1, TypeDetail.Key.Length - TypeDetail.Key.LastIndexOf('.') - 1);
                        if (TypeContainerDetail == null)
                            TypeContainerDetail = new ConcurrentDictionary<string, GraphContainerModal>();
                        if (TypeContainerDetail.ContainsKey(NodeName))
                        {
                            TypeContainerDetail.TryGetValue(NodeName, out ObjGraphContainerModal);
                            if (ObjGraphContainerModal != null)
                            {
                                ObjGraphContainerModal.TypeDetail.TryAdd(TypeName, TypeDetail.Value);
                            }
                            else
                            {
                                ObjGraphContainerModal = new GraphContainerModal();
                                ObjGraphContainerModal.TypeDetail.TryAdd(TypeName, TypeDetail.Value);
                            }
                            TypeContainerDetail[NodeName] = ObjGraphContainerModal;
                        }
                        else
                        {
                            ObjGraphContainerModal = new GraphContainerModal();
                            ObjGraphContainerModal.TypeDetail.TryAdd(TypeName, TypeDetail.Value);
                            TypeContainerDetail.TryAdd(NodeName, ObjGraphContainerModal);
                        }
                    }
                    else
                    {
                        ObjGraphContainerModal = new GraphContainerModal();
                        ObjGraphContainerModal.TypeDetail.TryAdd(TypeDetail.Key, TypeDetail.Value);
                        TypeContainerDetail.TryAdd(TypeDetail.Key, ObjGraphContainerModal);
                    }
                }
            }
            return TypeContainerDetail;
        }
    }
}
