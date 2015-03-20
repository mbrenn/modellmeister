using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runtime
{
    public interface ICompositeModelType : IModelType
    {
        object GetBlock(string name);
    }
}
