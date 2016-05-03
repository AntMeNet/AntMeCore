using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Extension.Test
{
    internal class DebugEngine : IEngine
    {
        public int Round
        {
            get { throw new NotImplementedException(); }
        }

        public EngineState State
        {
            get { throw new NotImplementedException(); }
        }

        public Map Map
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<Item> Items
        {
            get { throw new NotImplementedException(); }
        }

        public void InsertItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void RemoveItem(Item item)
        {
            throw new NotImplementedException();
        }

        public event ChangeItem OnRemoveItem;

        public event ChangeItem OnInsertItem;

        public event ValueUpdate<int> OnNextRound;

        public ITypeResolver TypeResolver
        {
            get { throw new NotImplementedException(); }
        }
    }
}
