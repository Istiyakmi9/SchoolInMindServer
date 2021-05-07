using BottomhalfCore.FactoryContext;
using BottomhalfCore.IFactoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Exceptions
{
    [Serializable]
    public class CircularReference : Exception
    {
        public new string Message;
        public CircularReference(List<string> TypeRefCollection, Type CurrentClass)
        {
            string Name = null;
            this.Message = CurrentClass.Name + " is having Circular reference. Below following chain found.\n";
            StringBuilder ErrorLink = new StringBuilder();
            if (TypeRefCollection.Count() > 0 && CurrentClass != null)
            {
                int index = 0;
                int TotalCount = TypeRefCollection.Count();
                ErrorLink.Append("[ ");
                while (index < TotalCount)
                {
                    Name = TypeRefCollection[index];
                    if ((index + 1) == TotalCount)
                        ErrorLink.Append(Name + " ]\n\n");
                    else
                        ErrorLink.Append(Name + " -> ");
                    index++;
                }
            }
            this.Message += ErrorLink.ToString();
        }
    }
}
