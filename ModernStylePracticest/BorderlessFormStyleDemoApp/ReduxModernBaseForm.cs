using ChromFXUI;
using ReduxCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorderlessFormStyleDemoApp
{
    public class ReduxModernBaseForm<S> : ChromFXBaseForm where S : struct
    {
        private Package<S> store;
        public virtual Package<S> Store
        {
            private set
            {
                store = value;
            }
            get
            {
                return store;
            }
        }
        public ReduxModernBaseForm(Package<S> store, string initialUrl)
            :base(initialUrl)
        {
            Store = store;
        }

        public ReduxModernBaseForm()
            :base(null)
        {

        }

        
    }
}
