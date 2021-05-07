using BottomhalfCore.IFactoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.FactoryContext
{
    public class NameSpaceHandler : INameSpaceHandler
    {
        public List<string> ResolveNamespace(List<string> QualifiedNamelist)
        {
            int index = 0;
            List<string> Collection = new List<string>();
            string Name = null;
            string NextName = string.Empty;
            string CollectionNewName = null;
            while (index < QualifiedNamelist.Count)
            {
                Name = QualifiedNamelist[index];
                if (Name.IndexOf("System.Collections") == -1)
                {
                    if (Name.IndexOf("]]") == -1)
                        Collection.Add(Name);
                    else
                        Collection.Add(Name.Split(new[] { ',' })[0]);
                }
                else
                {
                    if (Name.IndexOf("]]") == -1)
                    {
                        if (string.IsNullOrEmpty(CollectionNewName))
                            CollectionNewName += Name + "],[";
                        else
                            CollectionNewName += Name + "],[";
                    }
                    else
                    {

                        if (string.IsNullOrEmpty(CollectionNewName))
                            CollectionNewName = Name + "]";
                        else
                            CollectionNewName += Name + "]";
                        Collection.Add(CollectionNewName);
                        CollectionNewName = "";
                    }
                }

                index++;
            }

            return Collection;
        }

        public List<string> ConvertNameSpaceToList(string SplittedName)
        {
            List<string> NameCollection = new List<string>();
            var CollectionData = SplittedName.Split(new string[] { "[[" }, StringSplitOptions.RemoveEmptyEntries);
            if (CollectionData.ToList<string>().Count > 0)
            {
                if (CollectionData[0].IndexOf("System.Collections") == -1)
                {
                    if (SplittedName.IndexOf("System.Collections") != -1)
                    {
                        var Data = SplittedName.Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in Data)
                        {
                            if (item.Split(new string[] { "[[" }, StringSplitOptions.RemoveEmptyEntries)[0].IndexOf("System.Collections") != -1)
                            {
                                NameCollection.Add(item);
                            }
                            else
                            {
                                if (item.IndexOf("[[") != -1)
                                {
                                    var FirstPart = item.Substring(0, item.IndexOf("[["));
                                    NameCollection.Add(FirstPart);
                                    string LastPart = item.Substring(item.IndexOf("[[") + 2, item.Length - item.IndexOf("[[") - 2);
                                    var PartedCollection = ConvertNameSpaceToList(LastPart);
                                    if (PartedCollection.Count > 0)
                                        NameCollection.AddRange(PartedCollection);
                                }
                                else
                                {
                                    var LastItem = item.Split(new[] { ',' });
                                    if (LastItem.Count() > 0)
                                        NameCollection.Add(LastItem[0]);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (CollectionData.Count() > 0)
                            NameCollection.AddRange(CollectionData);
                        return NameCollection;
                    }
                }
                else
                {
                    NameCollection.Add(SplittedName);
                    return NameCollection;
                }
            }

            return NameCollection;
        }

        public string ConvertToImplemented(string Name)
        {
            string ReturningName = "";
            string[] Names = null;
            if (Name != null)
            {
                Names = Name.Split(new string[] { "[[" }, StringSplitOptions.RemoveEmptyEntries);
                if (Names.Count() > 1)
                {
                    if (Names[0].IndexOf("System.Collections") != -1)
                    {
                        ReturningName = GetImplementedName(Names[0]);
                    }
                    ReturningName = ReturningName + "[[" + Names[1];
                }
                else
                {
                    ReturningName = Name;
                }
            }
            return ReturningName;
        }

        public string GetImplementedName(string GenericName)
        {
            string ModifiedName = null;
            string Element = null;
            Element = GenericName.Split(new char[] { '`' })[0];
            if (Element != null)
            {
                if (Element.IndexOf("IDictionary") != -1)
                {
                    Element = Element.Replace("IDictionary", "Dictionary");
                }
                else if (Element.IndexOf("IList") != -1)
                {
                    Element = Element.Replace("IList", "List");
                }
                else if (Element.IndexOf("IEnumerable") != -1)
                {
                    Element = Element.Replace("IEnumerable", "Enumerable");
                }
            }
            ModifiedName = Element + GenericName.Substring(GenericName.IndexOf("`"), GenericName.Length - GenericName.IndexOf("`"));
            return ModifiedName;
        }

        public List<string> CreateStringNameSpace(List<string> SplittedData)
        {
            List<string> NewNameSpace = new List<string>();
            foreach (var Item in SplittedData)
            {
                if (Item.IndexOf("],[") != -1)
                {
                    var DataPart = CreateStringNameSpace(Item.Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries).ToList<string>());
                    if (DataPart != null)
                    {
                        NewNameSpace.AddRange(DataPart);
                    }
                }
                else if (Item.IndexOf('`') != -1)
                {
                    NewNameSpace.Add(Item);
                }
                else
                {
                    var SingleItem = Item.Split(new char[] { ',' });
                    if (SingleItem.Length > 0)
                    {
                        NewNameSpace.Add(SingleItem[0]);
                    }
                }
            }

            return NewNameSpace;
        }
    }
}
