using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rythin.src
{
    internal class Collector
    {
        /*[DllImport("GarbageCollector.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GC_AddObject(IntPtr obj);
        [DllImport("GarbageCollector.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GC_Collect();*/

        private IntPtr obj;
        public Collector() {
            //obj = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
        }

        public void collect()
        {
            //GC_AddObject(obj);
            //GC_Collect();
            //Marshal.FreeHGlobal(obj);
            
        }
    }
}
